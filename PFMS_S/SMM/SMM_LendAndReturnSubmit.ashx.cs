using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using System.Web.SessionState;
using System.IO;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_LendAndReturnSubmit 的摘要说明
    /// </summary>
    public class SMM_LendAndReturnSubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.Write("Hello World");
            string Lend_return = "";
            string SteelMesh_ID = "";
            string pcb_pn_id = "";
            string wo = "";
            string wo_count = "";
            string productLine = "";
            string owner_person_id = "";
            string operate_person_id = "";
            if (context.Session.Count > 0)
            {
                context.Response.ContentType = "text/plain";
                Lend_return = context.Request.Form["Lend_return"].ToString();
                SteelMesh_ID = context.Request.Form["sn"].ToString().Trim().ToUpper();
                pcb_pn_id = context.Request.Form["pcb_pn"].ToString().Trim().ToUpper();
                wo = context.Request.Form["wo"].ToString().Trim().ToUpper();
                wo_count = context.Request.Form["wo_count"].ToString().Trim();
                productLine = context.Request.Form["productLine"].ToString();
                owner_person_id = context.Request.Form["opid"].ToString().Trim().ToUpper();
                operate_person_id = context.Session["user_id"].ToString();
            }
            else
            {
                context.Response.ContentType = "application/x-www-form-urlencoded";
                Stream stream = context.Request.InputStream;
                if (stream.Length != 0)
                {
                    StreamReader streamreader = new StreamReader(stream);
                    string json = streamreader.ReadToEnd();
                    streamreader.Close();
                    string[] paraStr = json.Split('&');
                    foreach (string ss in paraStr)
                    {
                        switch (ss.Substring(0, ss.IndexOf("=")))
                        {
                            case "Lend_return":
                                Lend_return = ss.Substring(ss.IndexOf("=") + 1).Trim();
                                break;
                            case "sn":
                                SteelMesh_ID = ss.Substring(ss.IndexOf("=") + 1).Trim().ToUpper();
                                break;
                            case "pcb_pn":
                                pcb_pn_id = ss.Substring(ss.IndexOf("=") + 1).Trim().ToUpper();
                                break;
                            case "wo":
                                wo = ss.Substring(ss.IndexOf("=") + 1).Trim().ToUpper();
                                break;
                            case "wo_count":
                                wo_count = ss.Substring(ss.IndexOf("=") + 1).Trim();
                                break;
                            case "productLine":
                                productLine = ss.Substring(ss.IndexOf("=") + 1).Trim();
                                break;
                            case "opid":
                                owner_person_id = ss.Substring(ss.IndexOf("=") + 1).Trim().ToUpper();
                                break;
                            case "OperaterID":
                                operate_person_id = ss.Substring(ss.IndexOf("=") + 1).Trim();
                                break;
                        }
                    }
                }
                stream.Close();
            }
            string operate_dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string msgStr = "";
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            StringBuilder sb = new StringBuilder();
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                //1.判断钢网编号是否存在，是否在启用状态，是否在库里或在库外
                if (Lend_return == "2")  //借用
                {
                    mycom.CommandText = "select id from SteelMesh_List where sn='" + SteelMesh_ID + "' and enabled='1' and in_storage='1'";
                }
                else if (Lend_return == "3")  //归还，不仅要确认钢网不在库里，还要确认钢网未处于待验收状态
                {
                    mycom.CommandText = "select id from SteelMesh_List where sn='" + SteelMesh_ID + "' and enabled='1' and in_storage='0' and now_status<>-1";
                }
                dr = mycom.ExecuteReader();
                bool flag = false;
                int recordCount = 0;
                while (dr.Read())
                {
                    SteelMesh_ID = dr[0].ToString();
                    flag = true;
                }
                dr.Close();
                dr = null;
                //2.针对借出操作，判断PCB料号是否存在，是否绑定了该钢网
                if (flag)
                {
                    if (Lend_return == "2")  //借用
                    {
                        if (pcb_pn_id != "")
                        {
                            flag = false;
                            //2.1 判断PCB料号是否存在
                            mycom.CommandText = "select id from PCB_PN where pcb_pn='" + pcb_pn_id + "'";
                            dr = mycom.ExecuteReader();
                            while (dr.Read())
                            {
                                pcb_pn_id = dr[0].ToString();
                                flag = true;
                            }
                            dr.Close();
                            dr = null;
                            //2.2 若PCB料号存在再判断是否与钢网绑定关系，并确认关系是否正确
                            if (flag)
                            {
                                flag = false;
                                mycom.CommandText = "select smlist_id from SteelMesh_BindPCB where pcb_id='" + pcb_pn_id + "' and status='1'";
                                dr = mycom.ExecuteReader();
                                recordCount = 0;
                                while (dr.Read())
                                {
                                    recordCount++;
                                    if (dr[0].ToString() == SteelMesh_ID)
                                        flag = true;
                                }
                                dr.Close();
                                dr = null;
                                if (flag == false && recordCount == 0)
                                    flag = true;
                                if (!flag)
                                    msgStr = "钢网与PCB不匹配!";
                            }
                            else
                            {
                                msgStr = "PCB料号不存在!";
                            }
                        }
                    }
                }
                else
                {
                    if (Lend_return == "2")  //借用
                        msgStr = "钢网不存在或未启用或不在库里!";
                    else if (Lend_return == "3")  //归还
                        msgStr = "钢网不存在或未启用或在库里或未验收!";
                }
                //3.判断人员是否存在以及是否有权限借用归还
                if (flag)
                {
                    flag = false;
                    //3.1 人员是否存在
                    mycom.CommandText = "select user_id from User_Manage where opid='" + owner_person_id + "' and user_state='1'";
                    dr = mycom.ExecuteReader();
                    recordCount = 0;
                    while (dr.Read())
                    {
                        owner_person_id = dr[0].ToString();
                        recordCount++;
                    }
                    dr.Close();
                    dr = null;
                    //3.2 若存在是否有权限
                    if (recordCount > 0)
                    {
                        mycom.CommandText = "select count(id) from SteelMesh_LendAndReturnPower where user_id='" + owner_person_id + "' and power_status='1'";
                        int ret = Convert.ToInt32(mycom.ExecuteScalar().ToString());
                        if (ret > 0)
                            flag = true;
                        else
                        {
                            if (Lend_return == "2")  //借用
                                msgStr = "该工号无权限借用钢网!";
                            else if (Lend_return == "3")  //归还
                                msgStr = "该工号无权限归还钢网!";
                        }
                    }
                    else
                    {
                        msgStr = "该工号不存在或已注销!";
                    }
                }
                //4.针对借出操作，判断借出前是否需要进行张力测试，若需要，则判断是否已测试
                if (flag)
                {
                    if (Lend_return == "2")  //借用
                    {
                        flag = false;
                        //4.1 首先判断是否需要进行钢网张力测试
                        mycom.CommandText = "select verify_info from SteelMesh_VerifyForm_Info_List where status='1' and required='1'";
                        dr = mycom.ExecuteReader();
                        recordCount = 0;
                        while (dr.Read())
                        {
                            switch(dr[0].ToString().Trim())
                            {
                                case "钢网张力测试A点":
                                    recordCount++;
                                    break;
                                case "钢网张力测试B点":
                                    recordCount++;
                                    break;
                                case "钢网张力测试C点":
                                    recordCount++;
                                    break;
                                case "钢网张力测试D点":
                                    recordCount++;
                                    break;
                                case "钢网张力测试E点":
                                    recordCount++;
                                    break;
                                case "钢网张力测试F点":
                                    recordCount++;
                                    break;
                                case "钢网张力测试G点":
                                    recordCount++;
                                    break;
                                case "钢网张力测试H点":
                                    recordCount++;
                                    break;
                                case "钢网张力测试I点":
                                    recordCount++;
                                    break;
                                case "钢网张力测试J点":
                                    recordCount++;
                                    break;
                            }
                        }
                        dr.Close();
                        dr = null;
                        //4.2 若需要测试，再判断是否有进行了测试，即查询该钢网最近5分钟内的最后一条历史状态是否为张力测试
                        if (recordCount > 0)
                        {
                            mycom.CommandText = "select top 1 status_id,operate_dt from SteelMesh_Status_History where smlist_id='" + SteelMesh_ID + "' ORDER BY id DESC";
                            dr = mycom.ExecuteReader();
                            dr.Read();
                            if (dr[0].ToString() != "1")
                            {
                                msgStr = "请先对钢网进行张力测试!";
                            }
                            else
                            {
                                DateTime dbDT = Convert.ToDateTime(dr[1].ToString());
                                DateTime endDT = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                DateTime beginDT = endDT.AddMinutes(-5);
                                if (dbDT >= beginDT && dbDT <= endDT)
                                {
                                    flag = true;
                                }
                                else
                                {
                                    msgStr = "距上一次钢网张力测试已超5分钟,请重新测试!";
                                }
                            }
                            dr.Close();
                            dr = null;
                        }
                        else
                            flag = true;
                    }
                }
                //5.将借用归还状态写入数据库
                if (flag)
                {
                    flag = false;
                    sb.Clear();
                    sb.Append("insert into SteelMesh_Status_History(smlist_id,status_id,test_value,production_line_id,wo,wo_count,pcb_id,owner_person_id,operate_person_id,operate_dt)\n");
                    sb.Append("values('" + SteelMesh_ID + "','" + Lend_return + "','','" + productLine + "','" + wo + "','" + wo_count + "','" + pcb_pn_id + "','" + owner_person_id + "','" + operate_person_id + "','" + operate_dt + "')");
                    mycom.CommandText = sb.ToString();
                    int ret = mycom.ExecuteNonQuery();
                    if (ret > 0)
                    {
                        if (Lend_return == "2")  //借用
                            mycom.CommandText = "update SteelMesh_List set now_status='2',in_storage='0' where id='" + SteelMesh_ID + "'";
                        else if (Lend_return == "3")  //归还
                            mycom.CommandText = "update SteelMesh_List set now_status='3',in_storage='1' where id='" + SteelMesh_ID + "'";
                        ret = mycom.ExecuteNonQuery();
                        if (ret > 0)
                            flag = true;
                        else
                            msgStr = "更新钢网当前状态失败!";
                    }
                    else
                    {
                        msgStr = "状态写入历史记录表失败!";
                    }
                }
                //6.OK后返回结果
                if (flag)
                    msgStr = "OK";
            }
            catch (Exception msg)
            {
                msgStr = msg.Message;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr = null;
                }
                if (mycon.State != System.Data.ConnectionState.Closed)
                    mycon.Close();
                mycon = null;
            }
            context.Response.Write(msgStr);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}