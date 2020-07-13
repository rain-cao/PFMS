using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_AddEditClientPagePowerSubmit 的摘要说明
    /// </summary>
    public class SMM_AddEditClientPagePowerSubmit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string user_id = context.Request.Form["user_id"].ToString();
            string clintpage_id = context.Request.Form["clintpage_id"].ToString();
            string msgStr = "";
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                //首先删除现有的用户对客户端页面的访问权限
                mycom.CommandText = "delete from SteelMesh_ClientPagePower where user_id='" + user_id + "'";
                mycom.ExecuteNonQuery();
                //再将新的用户对客户端页面的访问权限写入数据库
                while (clintpage_id.Contains(";"))
                {
                    mycom.CommandText = "insert into SteelMesh_ClientPagePower(clintpage_id,user_id) values('" + clintpage_id.Substring(0, clintpage_id.IndexOf(";")) + "','" + user_id + "')";
                    mycom.ExecuteNonQuery();
                    clintpage_id = clintpage_id.Substring(clintpage_id.IndexOf(";") + 1);
                }
                msgStr = "OK";
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