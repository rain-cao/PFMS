using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PFMS_S.BM
{
    /// <summary>
    /// BM_DelPCB 的摘要说明
    /// </summary>
    public class BM_DelPCB : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            string idStr = context.Request.Form["id"].ToString();
            string msgStr = "";
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "delete FROM PCB_PN where id='" + idStr + "'";
                int ret = mycom.ExecuteNonQuery();
                if (ret > 0)
                    msgStr = "OK";
                else
                    msgStr = "删除操作影响行数0";
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