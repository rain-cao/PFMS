using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace PFMS_S
{
    /// <summary>
    /// BM_AddJobSubmit 的摘要说明
    /// </summary>
    public class BM_AddJobSubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string msgStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string post = context.Request.Form["post"].ToUpper().Trim();
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select id from Job_Manage where post='" + post + "'";
                object retStr = mycom.ExecuteScalar();
                if (retStr != null)
                    msgStr = "职务:" + post + "已存在,请勿重复添加!";
                else
                {
                    mycom.CommandText = "insert into Job_Manage(post,create_by,create_time,update_by,update_time) values(N'" + post + "','" + context.Session["user_id"] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + context.Session["user_id"] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    int ret = mycom.ExecuteNonQuery();
                    if (ret > 0)
                        msgStr = "OK";
                    else
                        msgStr = "数据插入失败!";
                }
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