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
    /// BM_AddProductLineSubmit 的摘要说明
    /// </summary>
    public class BM_AddProductLineSubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string returnStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string pl_nameStr = context.Request.Form["pl_name"];
            pl_nameStr = pl_nameStr.Trim().ToUpper();
            string codeStr = context.Request.Form["code"];
            codeStr = codeStr.ToUpper().Trim();
            string addressStr = context.Request.Form["address"];
            addressStr = addressStr.Trim();
            string workshop_idStr = context.Request.Form["workshop_id"];
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
                sb.Append("if not exists(select id from ProductLine_Management where pl_name='" + pl_nameStr + "')\n");
                sb.Append("begin\n");
                sb.Append("insert into ProductLine_Management(pl_name,code,address,workshop_id,ownerid,remark,create_by,create_time,update_by,update_time)\n");
                sb.Append("values(N'" + pl_nameStr + "',N'" + codeStr + "',N'" + addressStr + "','" + workshop_idStr + "','" + owneridStr + "',N'" + remarkStr + "','" + context.Session["user_id"] + "','" + create_timeStr + "','" + context.Session["user_id"] + "','" + update_time + "')\n");
                sb.Append("end\n");
                mycom.CommandText = sb.ToString();
                sb.Clear();
                int ret = mycom.ExecuteNonQuery();
                if (ret < 1)
                {
                    returnStr = "线别已存在,请确认!";
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