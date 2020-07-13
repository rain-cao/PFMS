using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data.SqlClient;

namespace PFMS_S.BM
{
    /// <summary>
    /// SMM_SetMenuStatus 的摘要说明
    /// </summary>
    public class SMM_SetMenuStatus : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            string msgStr = "";
            string menuID = context.Request.Form["menuID"].ToString();
            string statusNum = context.Request.Form["statusNum"].ToString();
            string updateBy = context.Session["user_id"].ToString();
            string updateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "update Menu_Manage set menu_state='" + statusNum + "',update_by='" + updateBy + "',update_time='" + updateTime + "' where menu_id='" + menuID + "'";
                int ret = mycom.ExecuteNonQuery();
                if (ret > 0)
                    msgStr = "OK";
                else
                    msgStr = "编辑影响行数为0";
            }
            catch (Exception msg)
            {
                msgStr = msg.Message;
            }
            finally
            {
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