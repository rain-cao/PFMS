using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;
using System.Text;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_EditUserLendPowerStatus 的摘要说明
    /// </summary>
    public class SMM_EditUserLendPowerStatus : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string user_idStr = context.Request.Form["user_id"].ToString();
            string create_byStr = context.Session["user_id"].ToString();
            string create_timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string update_byStr = create_byStr;
            string update_timeStr = create_timeStr;
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            string msgStr = "";
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append("if not exists(select user_id from SteelMesh_LendAndReturnPower where user_id='" + user_idStr + "')\n");
                sb.Append("begin\n");
                sb.Append("insert into SteelMesh_LendAndReturnPower(user_id,power_status,create_by,create_time,update_by,update_time)\n");
                sb.Append("values('" + user_idStr + "','1','" + create_byStr + "','" + create_timeStr + "','" + update_byStr + "','" + update_timeStr + "')\n");
                sb.Append("end\n");
                sb.Append("else\n");
                sb.Append("begin\n");
                sb.Append("update SteelMesh_LendAndReturnPower set power_status=\n");
                sb.Append("case power_status\n");
                sb.Append("when '0' then '1'\n");
                sb.Append("when '1' then '0'\n");
                sb.Append("else '0' end,update_by='" + update_byStr + "',update_time='" + update_timeStr + "'\n");
                sb.Append("end");
                mycom.CommandText = sb.ToString();
                int ret = mycom.ExecuteNonQuery();
                if (ret > 0)
                    msgStr = "OK";
                else
                    msgStr = "更新操作影响行数为0";
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