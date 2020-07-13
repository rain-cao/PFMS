using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data.SqlClient;

namespace PFMS_S.BM
{
    /// <summary>
    /// BM_MoveMenu 的摘要说明
    /// </summary>
    public class BM_MoveMenu : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string msgStr = "";
            string thisIDStr = context.Request.Form["menuID"].ToString();
            string otherIDStr = "";
            string updownStr = context.Request.Form["updown"].ToString();
            string parentIDStr = "";
            string thisOrdernumStr = "";
            string otherOrdernumStr = "";
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select parent_id,order_num from Menu_Manage where menu_id='" + thisIDStr + "'";
                dr = mycom.ExecuteReader();
                dr.Read();
                parentIDStr = dr[0].ToString();
                thisOrdernumStr = dr[1].ToString();
                dr.Close();
                dr = null;
                if (updownStr == "up")
                {
                    mycom.CommandText = "select max(order_num) from Menu_Manage where parent_id='" + parentIDStr + "' and order_num<" + thisOrdernumStr;
                    dr = mycom.ExecuteReader();
                    dr.Read();
                    otherOrdernumStr = dr[0].ToString();
                    dr.Close();
                    dr = null;
                }
                else if (updownStr == "down")
                {
                    mycom.CommandText = "select min(order_num) from Menu_Manage where parent_id='" + parentIDStr + "' and order_num>" + thisOrdernumStr;
                    dr = mycom.ExecuteReader();
                    dr.Read();
                    otherOrdernumStr = dr[0].ToString();
                    dr.Close();
                    dr = null;
                }
                if (otherOrdernumStr != "" && otherOrdernumStr != null)
                {
                    mycom.CommandText = "select menu_id from Menu_Manage where parent_id='" + parentIDStr + "' and order_num='" + otherOrdernumStr + "'";
                    dr = mycom.ExecuteReader();
                    dr.Read();
                    otherIDStr = dr[0].ToString();
                    dr.Close();
                    dr = null;
                    mycom.CommandText = "update Menu_Manage set order_num='" + otherOrdernumStr + "' where menu_id='" + thisIDStr + "'";
                    mycom.ExecuteNonQuery();
                    mycom.CommandText = "update Menu_Manage set order_num='" + thisOrdernumStr + "' where menu_id='" + otherIDStr + "'";
                    mycom.ExecuteNonQuery();
                    msgStr = "OK";
                }
                else
                {
                    if (updownStr == "up")
                        msgStr = "已处于最顶端!";
                    else if (updownStr == "down")
                        msgStr = "已处于最低端!";
                }
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