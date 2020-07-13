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
    /// BM_UpdateMenuSubmit 的摘要说明
    /// </summary>
    public class BM_UpdateMenuSubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string menu_nameStr = context.Request.Form["menu_name"].ToString().Trim();
            string menu_id = context.Request.Form["thisMenuID"].ToString();
            string pathStr = context.Request.Form["path"].ToString().Trim();
            string menu_urlStr = context.Request.Form["menu_url"].ToString().Trim();
            string menu_typeStr = context.Request.Form["menu_type"].ToString().Trim();
            if (menu_typeStr == "catalogue")
                menu_urlStr = "";
            string iconStr = context.Request.Form["icon"].ToString().Trim();
            string update_byStr = context.Session["user_id"].ToString();
            string update_timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
                mycom.CommandText = "update Menu_Manage set menu_name=N'" + menu_nameStr + "',path='" + pathStr + "',menu_url='" + menu_urlStr + "',icon='" + iconStr + "',remark=N'" + remarkStr + "',init_power='" + init_powerStr + "' where menu_id='" + menu_id + "'";
                int ret = mycom.ExecuteNonQuery();
                if (ret > 0)
                    msgStr = "OK";
                else
                    msgStr = "编辑操作影响行数为0";
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