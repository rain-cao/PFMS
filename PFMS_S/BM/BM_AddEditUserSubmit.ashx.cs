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
    /// BM_AddEditUserSubmit 的摘要说明
    /// </summary>
    public class BM_AddEditUserSubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string returnStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string idStr = context.Request.Form["input_id"];
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
                if (idStr == "0")
                {
                    sb.Append("if not exists(select user_id from User_Manage where username='" + usernameStr + "' or opid='" + opidStr + "')\n");
                    sb.Append("begin\n");
                    sb.Append("insert into User_Manage(name,username,opid,password,dept,post,telephone,email,weixin_no,user_state,create_by,create_time,update_by,update_time)\n");
                    sb.Append("values(N'" + nameStr + "',N'" + usernameStr + "','" + opidStr + "',N'" + passwordStr + "','" + deptStr + "','" + postStr + "','" + telephoneStr + "','" + emailStr + "','" + weixin_noStr + "','" + user_stateStr + "','" + context.Session["user_id"] + "','" + create_timeStr + "','" + context.Session["user_id"] + "','" + update_time + "')\n");
                    sb.Append("end\n");
                    sb.Append("select @@IDENTITY");
                    mycom.CommandText = sb.ToString();
                    sb.Clear();
                    string ret = mycom.ExecuteScalar().ToString();
                    if (ret != "" && ret != null)
                    {
                        sb.Append("insert into Auth_Manage(user_id,menu_id,power_no,power_desc,create_by,create_time,update_by,update_time)\n");
                        sb.Append("select '" + ret + "',menu_id,init_power,'','" + context.Session["user_id"] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + context.Session["user_id"] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' from Menu_Manage");
                        mycom.CommandText = sb.ToString();
                        sb.Clear();
                        int retCount = mycom.ExecuteNonQuery();
                        if (retCount > 0)
                        {
                            returnStr = "OK";
                        }
                        else
                        {
                            returnStr = "用户添加成功,菜单初始权限设置失败!";
                        }
                    }
                    else
                    {
                        returnStr = "账号已存在,请确认!";
                    }
                }
                else
                {
                    mycom.CommandText = "update User_Manage set name=N'" + nameStr + "',username=N'" + usernameStr + "',opid='" + opidStr + "',password=N'" + passwordStr + "',dept='" + deptStr + "',post='" + postStr + "',telephone='" + telephoneStr + "',email='" + emailStr + "',weixin_no='" + weixin_noStr + "',update_by='" + context.Session["user_id"] + "',update_time='" + update_time + "' where user_id='" + idStr + "'";
                    int ret = mycom.ExecuteNonQuery();
                    if (ret > 0)
                    {
                        returnStr = "OK";
                    }
                    else
                    {
                        returnStr = "用户编辑更新失败!";
                    }
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