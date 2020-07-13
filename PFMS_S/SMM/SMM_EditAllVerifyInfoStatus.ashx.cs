using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_EditAllVerifyInfoStatus 的摘要说明
    /// </summary>
    public class SMM_EditAllVerifyInfoStatus : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string msgStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            string statusStr = context.Request.Form["status"];
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "update SteelMesh_VerifyForm_Info_List set status='" + statusStr + "'";
                int ret = mycom.ExecuteNonQuery();
                if (ret > 0)
                    msgStr = "OK";
                else
                    msgStr = "操作影响行数0";
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