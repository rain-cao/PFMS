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
    /// BM_EditUserStatus 的摘要说明
    /// </summary>
    public class BM_EditUserStatus : IHttpHandler, IRequiresSessionState
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
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append("update User_Manage set update_by='" + context.Session["user_id"] + "',update_time='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', user_state=\n");
                sb.Append("case (select user_state from User_Manage where user_id='" + idStr + "')\n");
                sb.Append("when '1' then '0'\n");
                sb.Append("when '0' then '1'\n");
                sb.Append("else '0' end\n");
                sb.Append("where user_id='" + idStr + "'");
                mycom.CommandText = sb.ToString();
                int ret = mycom.ExecuteNonQuery();
                if (ret > 0)
                {
                    msgStr = "OK";
                    //将用户的启用状态设置为0时，同步更新该用户对钢网的借用归还权限为0
                    sb.Clear();
                    sb.Append("if exists(select id from SteelMesh_LendAndReturnPower where user_id='" + idStr + "')\n");
                    sb.Append("begin\n");
                    sb.Append("update SteelMesh_LendAndReturnPower set power_status=\n");
                    sb.Append("case (select user_state from User_Manage where user_id='" + idStr + "')\n");
                    sb.Append("when '0' then '0'\n");
                    sb.Append("else (select power_status from SteelMesh_LendAndReturnPower where user_id='" + idStr + "')\n");
                    sb.Append("end,update_by='" + context.Session["user_id"].ToString() + "',update_time='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'\n");
                    sb.Append("end");
                    mycom.CommandText = sb.ToString();
                    mycom.ExecuteNonQuery();
                }
                else
                    msgStr = "操作影响行数0";
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