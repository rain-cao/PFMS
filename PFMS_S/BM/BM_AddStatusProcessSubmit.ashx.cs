using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace PFMS_S.BM
{
    /// <summary>
    /// BM_AddStatusProcessSubmit 的摘要说明
    /// </summary>
    public class BM_AddStatusProcessSubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string status_idStr = context.Request.Form["status_id"].ToString().Trim();
            string status_nameStr = context.Request.Form["status_name"].ToString().Trim();
            string status_descStr = context.Request.Form["status_desc"].ToString().Trim();
            string create_byStr = context.Session["user_id"].ToString();
            string create_timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            string msgStr = "";
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "insert into Fixture_ProcessStatus_List(status_id,status_name,status_desc,create_by,create_time) values('" + status_idStr + "','" + status_nameStr + "','" + status_descStr + "','" + create_byStr + "','" + create_timeStr + "')";
                int ret = mycom.ExecuteNonQuery();
                if (ret > 0)
                    msgStr = "OK";
                else
                    msgStr = "插入操作影响行数为0";
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