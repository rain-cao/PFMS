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
    /// BM_AddWorkshopSubmit 的摘要说明
    /// </summary>
    public class BM_AddWorkshopSubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string returnStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string ws_nameStr = context.Request.Form["ws_name"];
            ws_nameStr = ws_nameStr.Trim().ToUpper();
            string codeStr = context.Request.Form["code"];
            codeStr = codeStr.ToUpper().Trim();
            string addressStr = context.Request.Form["address"];
            addressStr = addressStr.Trim();
            string factoryidStr = context.Request.Form["factoryid"];
            string owneridStr = context.Request.Form["ownerid"];
            string remarkStr = context.Request.Form["remark"];
            remarkStr = remarkStr.Trim();
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
                sb.Append("if not exists(select id from Workshop_Manage where ws_name='" + ws_nameStr + "')\n");
                sb.Append("begin\n");
                sb.Append("insert into Workshop_Manage(ws_name,code,address,factoryid,ownerid,remark,create_by,create_time,update_by,update_time)\n");
                sb.Append("values(N'" + ws_nameStr + "',N'" + codeStr + "',N'" + addressStr + "','" + factoryidStr + "','" + owneridStr + "',N'" + remarkStr + "','" + context.Session["user_id"] + "','" + create_timeStr + "','" + context.Session["user_id"] + "','" + update_time + "')\n");
                sb.Append("end\n");
                mycom.CommandText = sb.ToString();
                sb.Clear();
                int ret = mycom.ExecuteNonQuery();
                if (ret < 1)
                {
                    returnStr = "车间已存在,请确认!";
                }
                else
                {
                    returnStr = "OK";
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