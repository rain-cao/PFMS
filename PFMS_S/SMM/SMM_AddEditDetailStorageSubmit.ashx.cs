using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using System.Web.SessionState;

namespace PFMS_S
{
    /// <summary>
    /// SMM_AddEditDetailStorageSubmit 的摘要说明
    /// </summary>
    public class SMM_AddEditDetailStorageSubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string returnStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string idStr = context.Request.Form["input_id"];
            string storage_idStr = context.Request.Form["storage_id"];
            string storage_nameStr = context.Request.Form["storage_name"];
            storage_nameStr = storage_nameStr.Trim().ToUpper();
            string statusStr = "1";
            string create_timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string update_time = create_timeStr;
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                if (idStr == "0")
                {
                    sb.Append("if not exists(select id from SteelMesh_Detail_Storage where storage_id='" + storage_idStr + "' and storage_name='" + storage_nameStr + "')\n");
                    sb.Append("begin\n");
                    sb.Append("insert into SteelMesh_Detail_Storage(storage_id,storage_name,status,create_by,create_time,update_by,update_time)\n");
                    sb.Append("values('" + storage_idStr + "',N'" + storage_nameStr + "','" + statusStr + "','" + context.Session["user_id"] + "','" + create_timeStr + "','" + context.Session["user_id"] + "','" + update_time + "')\n");
                    sb.Append("end\n");
                    mycom.CommandText = sb.ToString();
                    sb.Clear();
                    int ret = mycom.ExecuteNonQuery();
                    if (ret < 1)
                    {
                        returnStr = "详细储位已存在,请确认!";
                    }
                    else
                    {
                        returnStr = "OK";
                    }
                }
                else
                {
                    mycom.CommandText = "update SteelMesh_Detail_Storage set storage_id='" + storage_idStr + "',storage_name=N'" + storage_nameStr + "' where id='" + idStr + "'";
                    int ret = mycom.ExecuteNonQuery();
                    if (ret > 0)
                    {
                        returnStr = "OK";
                    }
                    else
                    {
                        returnStr = "详细储位编辑更新失败!";
                    }
                }
            }
            catch (Exception msg)
            {
                returnStr = msg.Message;
            }
            finally
            {
                if (mycon.State != System.Data.ConnectionState.Closed)
                    mycon.Close();
                mycon = null;
            }
            context.Response.Write(returnStr);
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