using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;
using System.Text;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_AddEditSignRuleSubmit 的摘要说明
    /// </summary>
    public class SMM_AddEditSignRuleSubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string idStr = context.Request.Form["input_id"].ToString();
            string total_use_countStr = context.Request.Form["total_use_count"].ToString().Trim() ;
            string status_ruleStr = context.Request.Form["status_rule"].ToString().Trim();
            string alert_countStr = context.Request.Form["alert_count"].ToString().Trim();
            string alert_mailtoStr = context.Request.Form["alert_mailto"].ToString().Trim().ToLower();
            string alert_mailcopyStr = context.Request.Form["alert_mailcopy"].ToString().Trim().ToLower();
            string transfinite_mailtoStr = context.Request.Form["transfinite_mailto"].ToString().Trim().ToLower();
            string transfinite_mailcopyStr = context.Request.Form["transfinite_mailcopy"].ToString().Trim().ToLower();
            string create_byStr = context.Session["user_id"].ToString();
            string create_timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string update_byStr = create_byStr;
            string update_timeStr = create_timeStr;
            string msgStr = "";
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlConnection myconT = null;
            SqlCommand mycomT = null;
            SqlDataReader drT = null;
            mycon = DBConnect.ConnectSQLServer();
            myconT = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                myconT.Open();
                mycom = mycon.CreateCommand();
                mycomT = myconT.CreateCommand();
                //1.首先查询邮件发送或抄送人员在人员管理表单里的id号
                string[] mailArray = new string[] { };
                //查询预警发送人员id号
                Array.Clear(mailArray, 0, mailArray.Length);
                if (alert_mailtoStr != "")
                {
                    mailArray = alert_mailtoStr.Split(';');
                    alert_mailtoStr = "";
                    foreach (string item in mailArray)
                    {
                        if (item != "")
                        {
                            mycomT.CommandText = "select user_id from User_Manage where name='" + item.Substring(item.IndexOf("(") + 1, item.IndexOf(")") - item.IndexOf("(") - 1) + "' and username='" + item.Substring(0, item.IndexOf("(")) + "'";
                            drT = mycomT.ExecuteReader();
                            drT.Read();
                            alert_mailtoStr = alert_mailtoStr + drT[0].ToString() + ";";
                            drT.Close();
                            drT = null;
                        }
                    }
                }
                //查询预警抄送人员id号
                Array.Clear(mailArray, 0, mailArray.Length);
                if (alert_mailcopyStr != "")
                {
                    mailArray = alert_mailcopyStr.Split(';');
                    alert_mailcopyStr = "";
                    foreach (string item in mailArray)
                    {
                        if (item != "")
                        {
                            mycomT.CommandText = "select user_id from User_Manage where name='" + item.Substring(item.IndexOf("(") + 1, item.IndexOf(")") - item.IndexOf("(") - 1) + "' and username='" + item.Substring(0, item.IndexOf("(")) + "'";
                            drT = mycomT.ExecuteReader();
                            drT.Read();
                            alert_mailcopyStr = alert_mailcopyStr + drT[0].ToString() + ";";
                            drT.Close();
                            drT = null;
                        }
                    }
                }
                //查询超限发送人员id号
                Array.Clear(mailArray, 0, mailArray.Length);
                if (transfinite_mailtoStr != "")
                {
                    mailArray = transfinite_mailtoStr.Split(';');
                    transfinite_mailtoStr = "";
                    foreach (string item in mailArray)
                    {
                        if (item != "")
                        {
                            mycomT.CommandText = "select user_id from User_Manage where name='" + item.Substring(item.IndexOf("(") + 1, item.IndexOf(")") - item.IndexOf("(") - 1) + "' and username='" + item.Substring(0, item.IndexOf("(")) + "'";
                            drT = mycomT.ExecuteReader();
                            drT.Read();
                            transfinite_mailtoStr = transfinite_mailtoStr + drT[0].ToString() + ";";
                            drT.Close();
                            drT = null;
                        }
                    }
                }
                //查询超限抄送人员id号
                Array.Clear(mailArray, 0, mailArray.Length);
                if (transfinite_mailcopyStr != "")
                {
                    mailArray = transfinite_mailcopyStr.Split(';');
                    transfinite_mailcopyStr = "";
                    foreach (string item in mailArray)
                    {
                        if (item != "")
                        {
                            mycomT.CommandText = "select user_id from User_Manage where name='" + item.Substring(item.IndexOf("(") + 1, item.IndexOf(")") - item.IndexOf("(") - 1) + "' and username='" + item.Substring(0, item.IndexOf("(")) + "'";
                            drT = mycomT.ExecuteReader();
                            drT.Read();
                            transfinite_mailcopyStr = transfinite_mailcopyStr + drT[0].ToString() + ";";
                            drT.Close();
                            drT = null;
                        }
                    }
                }
                //2.根据idStr是否为0进行插入数据或更新数据
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                if (idStr == "0")
                {
                    sb.Append("insert into SteelMesh_ExamineRule(total_use_count,status_rule,alert_count,alert_mailto,alert_mailcopy,transfinite_mailto,transfinite_mailcopy,create_by,create_time,update_by,update_time)\n");
                    sb.Append("values('" + total_use_countStr + "',N'" + status_ruleStr + "','" + alert_countStr + "','" + alert_mailtoStr + "','" + alert_mailcopyStr + "','" + transfinite_mailtoStr + "','" + transfinite_mailcopyStr + "','" + create_byStr + "','" + create_timeStr + "','" + update_byStr + "','" + update_timeStr + "')");
                    mycom.CommandText = sb.ToString();
                }
                else
                {
                    sb.Append("update SteelMesh_ExamineRule set total_use_count='" + total_use_countStr + "',status_rule=N'" + status_ruleStr+ "',alert_count='" + alert_countStr + "',alert_mailto='" + alert_mailtoStr + "',alert_mailcopy='" + alert_mailcopyStr + "',\n");
                    sb.Append("transfinite_mailto='" + transfinite_mailtoStr + "',transfinite_mailcopy='" + transfinite_mailcopyStr + "',create_by='" + create_byStr + "',create_time='" + create_timeStr + "',update_by='" + update_byStr + "',update_time='" + update_timeStr + "' where id='" + idStr + "'");
                    mycom.CommandText = sb.ToString();
                }
                int ret = mycom.ExecuteNonQuery();
                if (ret > 0)
                    msgStr = "OK";
                else
                    msgStr = "影响行数为0";
            }
            catch (Exception msg)
            {
                msgStr = msg.Message;
            }
            finally
            {
                if (drT != null)
                {
                    drT.Close();
                    drT = null;
                }
                if (mycon.State != System.Data.ConnectionState.Closed)
                    mycon.Close();
                if (myconT.State != System.Data.ConnectionState.Closed)
                    myconT = null;
                mycon = null;
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