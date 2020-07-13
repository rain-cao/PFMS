using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;


namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_EditVerifyInfoRequired 的摘要说明
    /// </summary>
    public class SMM_EditVerifyInfoRequired : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string msgStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            string idStr = context.Request.Form["id"];
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append("update SteelMesh_VerifyForm_Info_List set required=\n");
                sb.Append("case (select required from SteelMesh_VerifyForm_Info_List where id='" + idStr + "')\n");
                sb.Append("when '1' then '0'\n");
                sb.Append("when '0' then '1'\n");
                sb.Append("else '0' end\n");
                sb.Append("where id='" + idStr + "'");
                mycom.CommandText = sb.ToString();
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