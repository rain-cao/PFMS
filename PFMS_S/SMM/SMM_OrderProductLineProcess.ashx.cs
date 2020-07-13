using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_OrderProductLineProcess 的摘要说明
    /// </summary>
    public class SMM_OrderProductLineProcess : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string msgStr = "";
            string thisIDStr = context.Request.Form["id"].ToString();
            string otherIDStr = "";
            string updownStr = context.Request.Form["updown"].ToString();
            string thisOrdernumStr = context.Request.Form["ordernum"].ToString();
            string otherOrdernumStr = "";
            string PLidStr = context.Request.Form["PLid"].ToString();
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                if (updownStr == "up")
                {
                    mycom.CommandText = "select max(order_number) from SteelMesh_OnLine_Process where productline_id='" + PLidStr + "' and order_number<" + thisOrdernumStr;
                    dr = mycom.ExecuteReader();
                    dr.Read();
                    otherOrdernumStr = dr[0].ToString();
                    dr.Close();
                    dr = null;
                }
                else if (updownStr == "down")
                {
                    mycom.CommandText = "select min(order_number) from SteelMesh_OnLine_Process where productline_id='" + PLidStr + "' and order_number>" + thisOrdernumStr;
                    dr = mycom.ExecuteReader();
                    dr.Read();
                    otherOrdernumStr = dr[0].ToString();
                    dr.Close();
                    dr = null;
                }
                mycom.CommandText = "select id from SteelMesh_OnLine_Process where order_number='" + otherOrdernumStr + "'";
                dr = mycom.ExecuteReader();
                dr.Read();
                otherIDStr = dr[0].ToString();
                dr.Close();
                dr = null;
                mycom.CommandText = "update SteelMesh_OnLine_Process set order_number='" + otherOrdernumStr + "' where id='" + thisIDStr + "'";
                mycom.ExecuteNonQuery();
                mycom.CommandText = "update SteelMesh_OnLine_Process set order_number='" + thisOrdernumStr + "' where id='" + otherIDStr + "'";
                mycom.ExecuteNonQuery();
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