using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_DelEquipment 的摘要说明
    /// </summary>
    public class SMM_DelEquipment : IHttpHandler
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
                //首先删除绑定设备的线体
                mycom.CommandText = "delete from SteelMesh_BindEquipAndPLine where equipment_id='" + idStr + "'";
                int ret = mycom.ExecuteNonQuery();
                //if (ret > 0)
                //{
                    mycom.CommandText = "delete from SteelMesh_CleanEquipment where id='" + idStr + "'";
                    ret = mycom.ExecuteNonQuery();
                    if (ret > 0)
                        msgStr = "OK";
                    else
                        msgStr = "操作影响行数0";
                //}
                //else
                    //msgStr = "删除设备绑定的线体失败,操作影响行数0";
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