using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using System.Web.SessionState;

namespace PFMS_S.BM
{
    /// <summary>
    /// BM_AddNextMenuSubmit 的摘要说明
    /// </summary>
    public class BM_AddNextMenuSubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string menu_nameStr = context.Request.Form["menu_name"].ToString().Trim();
            string parent_idStr = context.Request.Form["thisMenuID"].ToString();
            string order_numStr = "";
            string pathStr = context.Request.Form["path"].ToString().Trim();
            string menu_urlStr = context.Request.Form["menu_url"].ToString().Trim();
            string menu_typeStr = context.Request.Form["menu_type"].ToString().Trim();
            if (menu_typeStr == "catalogue")
                menu_urlStr = "";
            string menu_stateStr = "1";
            string iconStr = context.Request.Form["icon"].ToString().Trim();
            string create_byStr = context.Session["user_id"].ToString();
            string create_timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string update_byStr = create_byStr;
            string update_timeStr = create_timeStr;
            string remarkStr = context.Request.Form["remark"].ToString().Trim();
            string init_powerStr = context.Request.Form["init_power"].ToString().Trim();
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            string msgStr = "";
            StringBuilder sb = new StringBuilder();
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select max(order_num) from Menu_Manage where parent_id='" + parent_idStr + "'";
                order_numStr = mycom.ExecuteScalar().ToString();
                if (order_numStr != "" && order_numStr != null)
                    order_numStr = (Convert.ToInt32(order_numStr) + 1).ToString();
                else
                    order_numStr = "1";
                sb.Clear();
                sb.Append("insert into Menu_Manage(menu_name,parent_id,order_num,path,menu_url,menu_type,menu_state,icon,create_by,create_time,update_by,update_time,remark,init_power)\n");
                sb.Append("values(N'" + menu_nameStr + "','" + parent_idStr + "','" + order_numStr + "','" + pathStr + "','" + menu_urlStr + "','" + menu_typeStr + "','" + menu_stateStr + "',N'" + iconStr + "','" + create_byStr + "','" + create_timeStr + "','" + update_byStr + "','" + update_timeStr + "',N'" + remarkStr + "','" + init_powerStr + "')");
                mycom.CommandText = sb.ToString();
                int ret = mycom.ExecuteNonQuery();
                if (ret > 0)
                    msgStr = "OK";
                else
                    msgStr = "新增操作影响行数为0";
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