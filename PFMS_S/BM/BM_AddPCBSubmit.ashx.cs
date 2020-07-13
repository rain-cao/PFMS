using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PFMS_S.BM
{
    /// <summary>
    /// BM_AddPCBSubmit 的摘要说明
    /// </summary>
    public class BM_AddPCBSubmit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string msgStr = "";
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            string pcn_pnStr = context.Request.Form["pcb_pn"].ToString().Trim().ToUpper();
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "insert into PCB_PN(pcb_pn) values('" + pcn_pnStr + "')";
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