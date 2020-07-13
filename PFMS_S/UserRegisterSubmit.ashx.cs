using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;

namespace PFMS_S
{
    /// <summary>
    /// UserRegisterSubmit 的摘要说明
    /// </summary>
    public class UserRegisterSubmit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string returnStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string nameStr = context.Request.Form["name"];
            nameStr = nameStr.Trim();
            string usernameStr = context.Request.Form["username"];
            usernameStr = usernameStr.Trim().ToLower();
            string opidStr = context.Request.Form["opid"];
            opidStr = opidStr.Trim().ToUpper();
            string passwordStr = context.Request.Form["password"];
            passwordStr = passwordStr.Trim();
            string deptStr = context.Request.Form["dept"];
            deptStr = deptStr.Trim();
            string postStr = context.Request.Form["post"];
            postStr = postStr.Trim();
            string telephoneStr = context.Request.Form["telephone"];
            telephoneStr = telephoneStr.Trim();
            string emailStr = context.Request.Form["email"];
            emailStr = emailStr.Trim().ToLower();
            string weixin_noStr = context.Request.Form["weixin_no"];
            weixin_noStr = weixin_noStr.Trim();
            string user_stateStr = "1";
            string create_timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string update_time = create_timeStr;
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append("if not exists(select user_id from User_Manage where username='" + usernameStr + "' or opid='" + opidStr + "')\n");
                sb.Append("begin\n");
                sb.Append("insert into User_Manage(name,username,opid,password,dept,post,telephone,email,weixin_no,user_state,create_by,create_time,update_by,update_time)\n");
                sb.Append("values(N'" + nameStr + "',N'" + usernameStr + "','" + opidStr + "',N'" + passwordStr + "','" + deptStr + "','" + postStr + "','" + telephoneStr + "','" + emailStr + "','" + weixin_noStr + "','" + user_stateStr +"',(select max(user_id) from User_Manage)+1,'" + create_timeStr + "',(select max(user_id) from User_Manage)+1,'" + update_time + "')\n");
                sb.Append("end\n");
                sb.Append("select @@IDENTITY");
                mycom.CommandText = sb.ToString();
                sb.Clear();
                string ret = mycom.ExecuteScalar().ToString();
                if (ret != "" && ret != null)
                {
                    sb.Append("insert into Auth_Manage(user_id,menu_id,power_no,power_desc,create_by,create_time,update_by,update_time)\n");
                    sb.Append("select '" + ret + "',menu_id,init_power,'','" + ret + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + ret + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' from Menu_Manage");
                    mycom.CommandText = sb.ToString();
                    sb.Clear();
                    int retCount = mycom.ExecuteNonQuery();
                    if (retCount > 0)
                    {
                        returnStr = "OK";
                    }
                    else
                    {
                        returnStr = "账号注册成功,菜单初始权限设置失败!";
                    }
                }
                else
                {
                    returnStr = "账号已存在,请确认!";
                }
            }
            catch (Exception msg)
            {
                returnStr = msg.Message;
            }
            finally
            {
                if (dr != null)
                    dr.Close();
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