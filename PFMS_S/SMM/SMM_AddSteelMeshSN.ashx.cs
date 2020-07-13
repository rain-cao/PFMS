using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_AddSteelMeshSN 的摘要说明
    /// </summary>
    public class SMM_AddSteelMeshSN : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            string steelmesh_snStr = context.Request.Form["sn"].ToString().Trim().ToUpper();
            string create_byStr = context.Session["user_id"].ToString();
            string create_timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string msgStr = "";
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "insert into SteelMesh_List_CreateSN(steelmesh_sn,create_by,create_time) values('" + steelmesh_snStr + "','" + create_byStr + "','" + create_timeStr + "')";
                int ret = mycom.ExecuteNonQuery();
                if (ret > 0)
                    msgStr = "OK";
                else
                    msgStr = "插入钢网编号时影响行数为0";
            }
            catch(Exception msg)
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