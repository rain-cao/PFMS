using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_DelAcceptance 的摘要说明
    /// </summary>
    public class SMM_DelAcceptance : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string msgStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string idStr = context.Request.Form["id"];
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "delete from SteelMesh_Detail_Info_List where smlist_id='" + idStr + "'";
                mycom.ExecuteNonQuery();
                mycom.CommandText = "delete from SteelMesh_Detail_VerifyForm_Info_List where smlist_id='" + idStr + "'";
                mycom.ExecuteNonQuery();
                mycom.CommandText = "delete from SteelMesh_List_CreateSN where steelmesh_sn=(select sn from SteelMesh_List where id='" + idStr + "')";
                mycom.ExecuteNonQuery();
                mycom.CommandText = "delete from SteelMesh_List where id='" + idStr + "'";
                int ret = mycom.ExecuteNonQuery();
                if (ret > 0)
                    msgStr = "OK";
                else
                    msgStr = "删除操作影响行数为0";
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