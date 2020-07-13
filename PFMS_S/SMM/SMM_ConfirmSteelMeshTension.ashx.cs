using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_ConfirmSteelMeshTension 的摘要说明
    /// </summary>
    public class SMM_ConfirmSteelMeshTension : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string idStr = context.Request.Form["id"];
            string msgStr = "";
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                int smID = 0;
                string standardValue = "";
                double maxV = 0, minV = 0;
                string[] tmpV;
                for(int i=0;i<10;i++)
                {
                    mycom.CommandText = "select id,standard_value from SteelMesh_VerifyForm_Info_List where verify_info='钢网张力测试" + Convert.ToChar('A' + i).ToString() + "点' and verify_type='1' and status='1' and required='1'";
                    dr = mycom.ExecuteReader();
                    while(dr.Read())
                    {
                        smID = Convert.ToInt16(dr[0].ToString());
                        standardValue = dr[1].ToString();
                        break;
                    }
                    dr.Close();
                    dr = null;
                    if (smID != 0)
                    {
                        tmpV = standardValue.Split(';');
                        if (tmpV.Length < 3)
                        {
                            msgStr = "'钢网张力测试" + Convert.ToChar('A' + i).ToString() + "点'的标准值不存在或范围格式错误（" + standardValue + "）";
                        }
                        else
                        {
                            if (Convert.ToDouble(tmpV[0]) >= Convert.ToDouble(tmpV[1]))
                            {
                                maxV = Convert.ToDouble(tmpV[0]);
                                minV = Convert.ToDouble(tmpV[1]);
                            }
                            else
                            {
                                minV = Convert.ToDouble(tmpV[0]);
                                maxV = Convert.ToDouble(tmpV[1]);
                            }
                            mycom.CommandText = "select verify_content from SteelMesh_Detail_VerifyForm_Info_List where smlist_id='" + idStr + "' and smverifyinfolist_id='" + smID + "'";
                            bool flag = false;
                            dr = mycom.ExecuteReader();
                            while (dr.Read())
                            {
                                flag = true;
                                if (Convert.ToDouble(dr[0].ToString()) >= minV && Convert.ToDouble(dr[0].ToString()) <= maxV)
                                {
                                    msgStr = "OK";
                                }
                                else
                                {
                                    flag = false;
                                    msgStr = "'钢网张力测试" + Convert.ToChar('A' + i).ToString() + "点'的量测值(" + dr[0].ToString() + ")不在标准值范围内(" + standardValue + ")";
                                }
                                break;
                            }
                            dr.Close();
                            dr = null;
                            if (flag == false)
                            {
                                if (msgStr == "")
                                    msgStr = "'钢网张力测试" + Convert.ToChar('A' + i).ToString() + "点'未量测，请前往客户端进行钢网张力测试";
                                break;
                            }
                        }
                    }
                    else
                        msgStr = "OK";
                }
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