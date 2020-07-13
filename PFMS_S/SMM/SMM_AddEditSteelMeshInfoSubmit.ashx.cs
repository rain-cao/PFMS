using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.SessionState;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SWM_AddEditSteelMeshInfoSubmit 的摘要说明
    /// </summary>
    public class SWM_AddEditSteelMeshInfoSubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            string msgStr = "";
            string submitStr = context.Request.Form["submit"].ToString().Trim();
            string idStr = context.Request.Form["input_id"].ToString().Trim();
            string snStr = context.Request.Form["sn"].ToString().Trim().ToUpper();
            string pnStr = context.Request.Form["pn"].ToString().Trim().ToUpper();
            string nameStr = context.Request.Form["name"].ToString().Trim().ToUpper();
            string sizeStr = context.Request.Form["size"].ToString().Trim();
            string thicknessStr = context.Request.Form["thickness"].ToString().Trim();
            string materialStr = context.Request.Form["material"].ToString().Trim();
            string technologyStr = context.Request.Form["technology"].ToString().Trim();
            string ladderStr = context.Request.Form["ladder"].ToString().Trim();
            string impositionStr = context.Request.Form["imposition"].ToString().Trim();
            string treatmentStr = context.Request.Form["treatment"].ToString().Trim();
            string printingStr = context.Request.Form["printing"].ToString().Trim();
            string printcountStr = context.Request.Form["printcount"].ToString().Trim();
            string leadedStr = context.Request.Form["leaded"].ToString().Trim();
            string cleanStr = context.Request.Form["clean"].ToString().Trim();
            string manufacturerStr = context.Request.Form["manufacturer"].ToString().Trim();
            string custormerStr = context.Request.Form["custormer"].ToString().Trim();
            string verifydateStr = context.Request.Form["verifydate"].ToString().Trim();
            string auditStr = context.Request.Form["audit"].ToString().Trim();
            string intervalTStr = context.Request.Form["intervalT"].ToString().Trim();
            string intervalCStr = context.Request.Form["intervalC"].ToString().Trim();
            string storageStr = context.Request.Form["storage"].ToString().Trim();
            string engineerStr = context.Request.Form["engineer"].ToString().Trim();
            string warehousingStr = context.Request.Form["warehousing"].ToString().Trim();
            string verifyformStr = context.Request.Form["verifyform"].ToString().Trim();
            string createByStr = context.Session["user_id"].ToString();
            string updateByStr = createByStr;
            string createDateStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string updateDateStr = createDateStr;
            string smlist_idStr = "0";
            string sminfolist_idStr = "0";
            StringBuilder sb = new StringBuilder();
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                bool continueFlag = true;
                mycon.Open();
                mycom = mycon.CreateCommand();
                //1.首先新建或更新钢网清单表单
                if (idStr == "0")
                {
                    //当待插入的钢网存在时，先删除再插入
                    mycom.CommandText = "select count(*) from SteelMesh_List where sn='" + snStr + "'";
                    dr = mycom.ExecuteReader();
                    dr.Read();
                    if (Convert.ToInt16(dr[0].ToString()) > 0)
                    {
                        continueFlag = false;
                        msgStr = "钢网(" + snStr + ")已存在，请勿重复添加!";
                    }
                    else
                    {
                        dr.Close();
                        dr = null;
                        sb.Append("insert into SteelMesh_List(sn,now_status,in_storage,enabled,user_count,create_by,create_time,update_by,update_time)\n");
                        if (submitStr == "NoSubmit")  //是否暂存
                            sb.Append("values('" + snStr + "','-1','0','1','0','" + createByStr + "','" + createDateStr + "','" + updateByStr + "','" + updateDateStr + "')\n");
                        else
                            sb.Append("values('" + snStr + "','0','1','1','0','" + createByStr + "','" + createDateStr + "','" + updateByStr + "','" + updateDateStr + "')\n");
                        sb.Append("select @@IDENTITY");
                        mycom.CommandText = sb.ToString();
                        smlist_idStr = mycom.ExecuteScalar().ToString();
                    }
                    if (dr != null)
                    {
                        dr.Close();
                        dr = null;
                    }
                }
                else
                {
                    if (submitStr == "NoSubmit")   //是否暂存，用来区分状态为待验收还是入库
                    {
                        sb.Append("update SteelMesh_List set sn='" + snStr + "',now_status='-1',in_storage='0',update_by='" + updateByStr + "',update_time='" + updateDateStr + "' where id='" + idStr + "'");
                    }
                    else
                    {
                        sb.Append("update SteelMesh_List set sn='" + snStr + "',now_status='0',in_storage='1',update_by='" + updateByStr + "',update_time='" + updateDateStr + "' where id='" + idStr + "'");
                    }
                    mycom.CommandText = sb.ToString();
                    smlist_idStr = idStr;
                    mycom.ExecuteNonQuery();
                }
                sb.Clear();
                //2.判断是否为提交状态，若为提交状态，则将入库状态写入SteelMesh_Status_History中
                if (submitStr == "Submit")
                {
                    sb.Append("insert into SteelMesh_Status_History(smlist_id,status_id,wo,wo_count,owner_person_id,operate_person_id,operate_dt)\n");
                    sb.Append("values('" + smlist_idStr + "','0','','0','" + createByStr + "','" + createByStr + "','" + createDateStr + "')");
                    mycom.CommandText = sb.ToString();
                    sb.Clear();
                    mycom.ExecuteNonQuery();
                }
                //3.新增或更新钢网信息详细列表，这里不想再创建一个SQL连接对象，直接写了，虽然冗余，但本人自己写的，我自己看得懂就行，代码多卖的贵啊，看着不爽，自己改去，O(∩_∩)O哈哈~
                //23笔信息插入数据库，写的好累......
                if (continueFlag)
                {
                    string infoDesStr = "";
                    string infoValueStr = "";
                    for (int i = 0; i < 23; i++)
                    {
                        sb.Clear();
                        switch (i)
                        {
                            case 0:
                                infoDesStr = "钢网料号";
                                infoValueStr = pnStr;
                                break;
                            case 1:
                                infoDesStr = "钢网名称";
                                infoValueStr = nameStr;
                                break;
                            case 2:
                                infoDesStr = "钢网尺寸";
                                infoValueStr = sizeStr;
                                break;
                            case 3:
                                infoDesStr = "钢网厚度";
                                infoValueStr = thicknessStr;
                                break;
                            case 4:
                                infoDesStr = "钢网材料";
                                infoValueStr = materialStr;
                                break;
                            case 5:
                                infoDesStr = "钢网工艺";
                                infoValueStr = technologyStr;
                                break;
                            case 6:
                                infoDesStr = "阶梯钢网";
                                infoValueStr = ladderStr;
                                break;
                            case 7:
                                infoDesStr = "拼版数量";
                                infoValueStr = impositionStr;
                                break;
                            case 8:
                                infoDesStr = "表面处理方式";
                                infoValueStr = treatmentStr;
                                break;
                            case 9:
                                infoDesStr = "印刷面";
                                infoValueStr = printingStr;
                                break;
                            case 10:
                                infoDesStr = "单板印刷次数";
                                infoValueStr = printcountStr;
                                break;
                            case 11:
                                infoDesStr = "有铅无铅";
                                infoValueStr = leadedStr;
                                break;
                            case 12:
                                infoDesStr = "清洗类型";
                                infoValueStr = cleanStr;
                                break;
                            case 13:
                                infoDesStr = "供应商";
                                infoValueStr = manufacturerStr;
                                break;
                            case 14:
                                infoDesStr = "客户";
                                infoValueStr = custormerStr;
                                break;
                            case 15:
                                infoDesStr = "验收日期";
                                infoValueStr = verifydateStr;
                                break;
                            case 16:
                                infoDesStr = "审核规则";
                                infoValueStr = auditStr;
                                break;
                            case 17:
                                infoDesStr = "清洗间隔时间";
                                infoValueStr = intervalTStr;
                                break;
                            case 18:
                                infoDesStr = "清洗间隔次数";
                                infoValueStr = intervalCStr;
                                break;
                            case 19:
                                infoDesStr = "存放储位";
                                infoValueStr = storageStr;
                                break;
                            case 20:
                                infoDesStr = "工程人员";
                                infoValueStr = engineerStr;
                                break;
                            case 21:
                                infoDesStr = "入库人员";
                                infoValueStr = warehousingStr;
                                break;
                            case 22:
                                infoDesStr = "验收表单";
                                infoValueStr = verifyformStr;
                                break;
                        }
                        mycom.CommandText = "select id from SteelMesh_Info_List where info_name='" + infoDesStr + "'";
                        dr = mycom.ExecuteReader();
                        dr.Read();
                        sminfolist_idStr = dr[0].ToString().Trim();
                        dr.Close();
                        dr = null;
                        if (idStr == "0")
                        {
                            //当钢网对应的详细信息存在时，则先删除，再插入
                            sb.Append("if exists(select sminfolist_id from SteelMesh_Detail_Info_List where smlist_id='" + smlist_idStr + "' and sminfolist_id='" + sminfolist_idStr + "')\n");
                            sb.Append("begin\n");
                            sb.Append("delete from SteelMesh_Detail_Info_List where smlist_id='" + smlist_idStr + "' and sminfolist_id='" + sminfolist_idStr + "'\n");
                            sb.Append("end\n");
                            sb.Append("insert into SteelMesh_Detail_Info_List(smlist_id,sminfolist_id,info_value,create_by,create_time,update_by,update_time)\n");
                            sb.Append("values('" + smlist_idStr + "','" + sminfolist_idStr + "','" + infoValueStr + "','" + createByStr + "','" + createDateStr + "','" + updateByStr + "','" + updateDateStr + "')");
                        }
                        else
                        {
                            sb.Append("update SteelMesh_Detail_Info_List set info_value='" + infoValueStr + "' where smlist_id='" + smlist_idStr + "' and sminfolist_id='" + sminfolist_idStr + "'");
                        }
                        mycom.CommandText = sb.ToString();
                        mycom.ExecuteNonQuery();
                    }
                    msgStr = "OK:" + smlist_idStr;
                }
            }
            catch(Exception msg)
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
                if (mycon.State != ConnectionState.Closed)
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