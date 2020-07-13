using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_AddProductLineProcessSubmit 的摘要说明
    /// </summary>
    public class SMM_AddProductLineProcessSubmit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string productline_idStr = context.Request.Form["productline_id"].ToString();
            string status_idStr = context.Request.Form["status_id"].ToString();
            string role_defineStr = context.Request.Form["role_define"].ToString();
            string role_forceStr = context.Request.Form["role_force"].ToString();
            if (role_defineStr == "")
                role_forceStr = "0";
            int order_number = 0;
            string msgStr = "";
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select ((select max(order_number) from SteelMesh_OnLine_Process where productline_id='" + productline_idStr + "')+1)";
                object obj = mycom.ExecuteScalar();
                if (obj != null && obj.ToString() != "")
                    order_number = Convert.ToInt32(obj.ToString());
                else
                    order_number = 1;
                mycom.CommandText = "insert into SteelMesh_OnLine_Process(productline_id,status_id,role_define,role_force,order_number) values('" + productline_idStr + "','" + status_idStr + "','" + role_defineStr + "','" + role_forceStr + "','" + order_number.ToString() + "')";
                int ret = mycom.ExecuteNonQuery();
                if (ret > 0)
                    msgStr = "OK";
                else
                    msgStr = "操作影响行数为0";
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