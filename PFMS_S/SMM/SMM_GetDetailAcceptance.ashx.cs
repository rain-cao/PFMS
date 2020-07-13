using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_GetDetailAcceptance 的摘要说明
    /// </summary>
    public class SMM_GetDetailAcceptance : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string msgStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            SqlConnection myconT = null;
            SqlCommand mycomT = null;
            SqlDataReader drT = null;
            mycon = DBConnect.ConnectSQLServer();
            myconT = DBConnect.ConnectSQLServer();
            string idStr = context.Request.Form["id"].ToString();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                myconT.Open();
                mycomT = myconT.CreateCommand();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT\n");
                sb.Append("B.info_name,A.info_value\n");
                sb.Append("FROM SteelMesh_Detail_Info_List A\n");
                sb.Append("INNER JOIN SteelMesh_Info_List B\n");
                sb.Append("ON A.sminfolist_id=B.id\n");
                sb.Append("WHERE A.smlist_id='" + idStr + "'");
                mycom.CommandText = sb.ToString();
                dr = mycom.ExecuteReader();
                sb.Clear();
                sb.Append("<table style=\"margin:0 auto;text-align:center;border:none;width:98%;\">\n");
                int recordCount = 0;
                string strTmp = "";
                while(dr.Read())
                {
                    strTmp = "";
                    recordCount++;
                    if (dr[1] != null && dr[1].ToString().Trim() != "")
                    {
                        switch (dr[0].ToString().Trim())
                        {
                            case "印刷面":
                                if (dr[1].ToString() == "F")
                                    strTmp = "正面";
                                else
                                    strTmp = "背面";
                                break;
                            case "有铅无铅":
                                if (dr[1].ToString() == "Y")
                                    strTmp = "是";
                                else
                                    strTmp = "否";
                                break;
                            case "供应商":
                                mycomT.CommandText = "select name from SteelMesh_Manufacturer_Management where id='" + dr[1].ToString() + "'";
                                drT = mycomT.ExecuteReader();
                                drT.Read();
                                strTmp = drT[0].ToString();
                                drT.Close();
                                drT = null;
                                break;
                            case "审核规则":
                                mycomT.CommandText = "select status_rule from SteelMesh_ExamineRule where id='" + dr[1].ToString() + "'";
                                drT = mycomT.ExecuteReader();
                                drT.Read();
                                strTmp = drT[0].ToString();
                                drT.Close();
                                drT = null;
                                break;
                            case "存放储位":
                                mycomT.CommandText = "SELECT A.storage_name,B.storage_name FROM SteelMesh_Detail_Storage A INNER JOIN SteelMesh_Storage_Management B ON A.storage_id=B.id WHERE A.id='" + dr[1].ToString() + "'";
                                drT = mycomT.ExecuteReader();
                                drT.Read();
                                strTmp = drT[0].ToString() + "(" + drT[1].ToString() + ")";
                                drT.Close();
                                drT = null;
                                break;
                            case "工程人员":
                                mycomT.CommandText = "select name,username from User_Manage where user_id='" + dr[1].ToString() + "'";
                                drT = mycomT.ExecuteReader();
                                drT.Read();
                                strTmp = drT[0].ToString() + "(" + drT[1].ToString() + ")";
                                drT.Close();
                                drT = null;
                                break;
                            case "入库人员":
                                mycomT.CommandText = "select name,username from User_Manage where user_id='" + dr[1].ToString() + "'";
                                drT = mycomT.ExecuteReader();
                                drT.Read();
                                strTmp = drT[0].ToString() + "(" + drT[1].ToString() + ")";
                                drT.Close();
                                drT = null;
                                break;
                            case "验收表单":
                                if(dr[1].ToString() != "0")
                                    strTmp = "<a href=\"SMM_ViewVerifyForm.aspx?smlist_id=" + idStr + "\" target=\"_blank\" style=\"color: blue; text-decoration:underline; cursor: pointer;\">查看验收表单</a>";
                                break;
                            default:
                                strTmp = dr[1].ToString();
                                break;
                        }
                        if (strTmp != "")
                        {
                            sb.Append("<tr>\n");
                            sb.Append("<td style=\"border:none;height:30px;width:120px;text-align:right;\">\n");
                            sb.Append(dr[0].ToString() + "：\n");
                            sb.Append("</td>\n");
                            sb.Append("<td style=\"border:none;height:30px;text-align:center;\">\n");
                            sb.Append(strTmp + "\n");
                            sb.Append("</td>\n");
                            sb.Append("</tr>\n");
                        }
                    }
                }
                if (recordCount == 0)
                {
                    sb.Append("<tr>\n");
                    sb.Append("<td style=\"border:none;height:50px;text-align:center;\">\n");
                    sb.Append("未查询到该钢网的详细信息\n");
                    sb.Append("</td>\n");
                    sb.Append("</tr>\n");
                }
                else
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr = null;
                    }
                    StringBuilder sb1 = new StringBuilder();
                    sb1.Clear();
                    sb1.Append("select B.name,B.username,A.create_time\n");
                    sb1.Append("from SteelMesh_List_CreateSN A\n");
                    sb1.Append("inner join User_Manage B on A.create_by=B.user_id\n");
                    sb1.Append("where A.steelmesh_sn=(select sn from SteelMesh_List where id='" + idStr + "')");
                    mycom.CommandText = sb1.ToString();
                    dr = mycom.ExecuteReader();
                    dr.Read();
                    sb.Append("<tr>\n");
                    sb.Append("<td style=\"border:none;height:30px;width:120px;text-align:right;\">\n");
                    sb.Append("编码人员：\n");
                    sb.Append("</td>\n");
                    sb.Append("<td style=\"border:none;height:30px;text-align:center;\">\n");
                    sb.Append(dr[0].ToString() + "(" + dr[1].ToString() + ")" + "\n");
                    sb.Append("</td>\n");
                    sb.Append("</tr>\n");
                    sb.Append("<tr>\n");
                    sb.Append("<td style=\"border:none;height:30px;width:120px;text-align:right;\">\n");
                    sb.Append("编码时间：\n");
                    sb.Append("</td>\n");
                    sb.Append("<td style=\"border:none;height:30px;text-align:center;\">\n");
                    sb.Append(dr[2].ToString() + "\n");
                    sb.Append("</td>\n");
                    sb.Append("</tr>\n");
                    dr.Close();
                    dr = null;
                    sb1.Clear();
                    sb1.Append("select B.name,B.username,A.update_time\n");
                    sb1.Append("from SteelMesh_List A\n");
                    sb1.Append("inner join User_Manage B on A.update_by=B.user_id\n");
                    sb1.Append("where A.id='" + idStr + "'");
                    mycom.CommandText = sb1.ToString();
                    dr = mycom.ExecuteReader();
                    dr.Read();
                    sb.Append("<tr>\n");
                    sb.Append("<td style=\"border:none;height:30px;width:120px;text-align:right;\">\n");
                    sb.Append("验收人员：\n");
                    sb.Append("</td>\n");
                    sb.Append("<td style=\"border:none;height:30px;text-align:center;\">\n");
                    sb.Append(dr[0].ToString() + "(" + dr[1].ToString() + ")" + "\n");
                    sb.Append("</td>\n");
                    sb.Append("</tr>\n");
                    sb.Append("<tr>\n");
                    sb.Append("<td style=\"border:none;height:30px;width:120px;text-align:right;\">\n");
                    sb.Append("验收时间：\n");
                    sb.Append("</td>\n");
                    sb.Append("<td style=\"border:none;height:30px;text-align:center;\">\n");
                    sb.Append(dr[2].ToString() + "\n");
                    sb.Append("</td>\n");
                    sb.Append("</tr>\n");
                }
                sb.Append("</table>");
                msgStr = "OK:" + sb.ToString(); ;
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
                if (drT != null)
                {
                    drT.Close();
                    drT = null;
                }
                if (myconT.State != System.Data.ConnectionState.Closed)
                    myconT.Close();
                myconT = null;
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