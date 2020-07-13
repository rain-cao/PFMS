using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;
using System.DirectoryServices;
using System.Data;
using System.Configuration;
using System.Text;

namespace PFMS_S
{
    /// <summary>
    /// UserLogin 的摘要说明
    /// </summary>
    public class UserLogin : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            /*string returnStr = "";
            SqlConnection mycon = null;
            SqlCommand com = null;
            SqlDataReader dr = null;
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string userStr = context.Request.Form["user"];
            userStr = userStr.Trim();
            string pwdStr = context.Request.Form["password"];
            pwdStr = pwdStr.Trim();
            string checkedStr = context.Request.Form["checked"];
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                string SQLText = "select user_id,name,username,opid,dept,telephone,email,weixin_no from User_Manage where (username='" + userStr.ToLower() + "' or email='" + userStr.ToLower() + "' or opid='" + userStr.ToUpper() + "') and password='" + pwdStr + "' and user_state='1'";
                com = mycon.CreateCommand();
                com.CommandText = SQLText;
                dr = com.ExecuteReader();
                int userCount = 0;
                while (dr.Read())
                {
                    userCount++;
                    //将用户信息保存至session中，客户端网页关闭后自动清除
                    context.Session["user_id"] = dr[0].ToString();
                    context.Session["Name"] = dr[1].ToString();
                    context.Session["UserName"] = dr[2].ToString();
                    context.Session["OPID"] = dr[3].ToString();
                    context.Session["Department"] = dr[4].ToString();
                    context.Session["Telephone"] = dr[5].ToString();
                    context.Session["Email"] = dr[6].ToString();
                    context.Session["WeiXin"] = dr[7].ToString();
                    break;
                }
                if (userCount > 0)
                {
                    returnStr = "OK";
                    if (checkedStr.ToLower() == "true" && object.Equals(context.Request.Cookies["UserName"], null) && object.Equals(context.Request.Cookies["Password"], null))
                    {
                        context.Response.Cookies["UserName"].Value = userStr;
                        context.Response.Cookies["Password"].Value= pwdStr;
                        context.Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(1);
                        context.Response.Cookies["Password"].Expires = DateTime.Now.AddDays(1);
                    }
                }
                else
                    returnStr = "登陆账号密码错误或用户不存在!";
            }
            catch(Exception msg)
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
            }*/
            string returnStr = "";
            SqlConnection mycon = null;
            SqlCommand com = null;
            SqlDataReader dr = null;
            context.Response.ContentType = "text/plain";
            string nameStr = "", opidStr = "", deptStr = "", deptDesStr = "", postStr = "", telephoneStr = "", emailStr = "";
            string create_timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string update_timeStr = create_timeStr;
            bool LoginFlag = false;
            //context.Response.Write("Hello World");
            string userStr = context.Request.Form["user"];
            userStr = userStr.Trim().ToLower();
            if (!userStr.Contains("acn\\"))
            {
                userStr = "acn\\" + userStr;
            }
            string pwdStr = context.Request.Form["password"];
            pwdStr = pwdStr.Trim();
            string checkedStr = context.Request.Form["checked"];
            //DA验证登陆
            using (DirectoryEntry adsEntry = new DirectoryEntry(ConfigurationManager.AppSettings["DAVerify"].ToString(), userStr, pwdStr, AuthenticationTypes.Secure))
            {
                using (DirectorySearcher adsSearcher = new DirectorySearcher(adsEntry))
                {
                    adsSearcher.Filter = "(sAMAccountName=" + userStr.Substring(4) + ")";
                    adsSearcher.PropertiesToLoad.Add("msrtcsip-userenabled");
                    adsSearcher.PropertiesToLoad.Add("mobile");
                    adsSearcher.PropertiesToLoad.Add("mail");
                    adsSearcher.PropertiesToLoad.Add("title");
                    try
                    {
                        SearchResult adsSearchResult = adsSearcher.FindOne();
                        ResultPropertyCollection rprops = adsSearchResult.Properties;
                        foreach (string name in rprops.PropertyNames)
                        {
                            foreach (object vl in rprops[name])
                            {
                                switch (name)
                                {
                                    case "msrtcsip-userenabled":
                                        LoginFlag = Convert.ToBoolean(vl.ToString());
                                        break;
                                    case "mobile":
                                        telephoneStr = vl.ToString();
                                        break;
                                    case "mail":
                                        emailStr = vl.ToString().ToLower();
                                        break;
                                    case "title":
                                        postStr = vl.ToString();
                                        break;
                                }
                            }
                        }
                        ServiceReference1.Service1SoapClient getUserClient = new ServiceReference1.Service1SoapClient();
                        DataTable dt = getUserClient.GeteOAUserForDCC(userStr);
                        foreach (DataRow row in dt.Rows)
                        {
                            foreach (DataColumn column in dt.Columns)
                            {
                                switch (column.Caption)
                                {
                                    case "var_hrid":
                                        opidStr = row[column].ToString().ToUpper();
                                        break;
                                    case "var_name":
                                        nameStr = row[column].ToString();
                                        break;
                                    case "var_dept":
                                        deptStr = row[column].ToString().ToUpper();
                                        break;
                                    case "var_deptname":
                                        deptDesStr = row[column].ToString().ToUpper();
                                        break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        returnStr = ex.Message;
                    }
                    finally
                    {
                        adsEntry.Close();
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            if (LoginFlag && returnStr == "")
            {
                mycon = DBConnect.ConnectSQLServer();
                //DA认证成功后，查询数据表里是否有维护上面查询到的部门和职务，若没有则插入对应的数据表并返回部门或职务对应的id号，并将用户信息更新到数据表里,同时初始化权限
                try
                {
                    mycon.Open();
                    com = mycon.CreateCommand();
                    //部门
                    if (deptStr != "")
                    {
                        sb.Clear();
                        sb.Append("if not exists(select id from Department_Manage where dept_code='" + deptStr + "')\n");
                        sb.Append("begin\n");
                        sb.Append("insert into Department_Manage(dept,dept_code,create_by,create_time,update_by,update_time)\n");
                        sb.Append("values('" + deptDesStr + "','" + deptStr + "',(select max(user_id) from User_Manage)+1,'" + create_timeStr + "',(select max(user_id) from User_Manage)+1,'" + update_timeStr + "')\n");
                        sb.Append("end");
                        com.CommandText = sb.ToString();
                        com.ExecuteNonQuery();
                        com.CommandText = "select id from Department_Manage where dept_code='" + deptStr + "'";
                        deptStr = com.ExecuteScalar().ToString();
                    }
                    //职务
                    if (postStr != "")
                    {
                        sb.Clear();
                        sb.Append("if not exists(select id from Job_Manage where post='" + postStr + "')\n");
                        sb.Append("begin\n");
                        sb.Append("insert into Job_Manage(post,create_by,create_time,update_by,update_time)\n");
                        sb.Append("values('" + postStr + "',(select max(user_id) from User_Manage)+1,'" + create_timeStr + "',(select max(user_id) from User_Manage)+1,'" + update_timeStr + "')\n");
                        sb.Append("end");
                        com.CommandText = sb.ToString();
                        com.ExecuteNonQuery();
                        com.CommandText = "select id from Job_Manage where post='" + postStr + "'";
                        postStr = com.ExecuteScalar().ToString();
                    }
                    //将用户插入数据表或更新用户信息
                    userStr = userStr.Substring(4);
                    sb.Clear();
                    sb.Append("if not exists(select user_id from User_Manage where username='" + userStr + "' or opid='" + opidStr + "')\n");
                    sb.Append("begin\n");
                    sb.Append("insert into User_Manage(name,username,opid,password,dept,post,telephone,email,weixin_no,user_state,create_by,create_time,update_by,update_time)\n");
                    sb.Append("values(N'" + nameStr + "','" + userStr + "','" + opidStr + "','" + pwdStr + "','" + deptStr + "','" + postStr + "','" + telephoneStr + "','" + emailStr + "','','1',(select max(user_id) from User_Manage)+1,'" + create_timeStr + "',(select max(user_id) from User_Manage)+1,'" + update_timeStr + "')\n");
                    sb.Append("end\n");
                    sb.Append("else\n");
                    sb.Append("begin\n");
                    sb.Append("update User_Manage set name='" + nameStr + "',username='" + userStr + "',password='" + pwdStr + "',opid='" + opidStr + "',dept='" + deptStr + "',post='" + postStr + "',telephone='" + telephoneStr + "',email='" + emailStr + "',update_time='" + update_timeStr + "' where (username='" + userStr + "' or opid='" + opidStr + "') and user_state='1'\n");
                    sb.Append("end\n");
                    sb.Append("select @@IDENTITY");
                    com.CommandText = sb.ToString();
                    string ret = com.ExecuteScalar().ToString();
                    //初始化用户权限
                    if (ret != "" && ret != null)
                    {
                        sb.Clear();
                        sb.Append("insert into Auth_Manage(user_id,menu_id,power_no,power_desc,create_by,create_time,update_by,update_time)\n");
                        sb.Append("select '" + ret + "',menu_id,init_power,'','" + ret + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + ret + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' from Menu_Manage");
                        com.CommandText = sb.ToString();
                        sb.Clear();
                        com.ExecuteNonQuery();
                    }
                    //将用户信息保存至session中，客户端网页关闭后自动清除
                    string SQLText = "select user_id,name,username,opid,dept,telephone,email,weixin_no from User_Manage where (username='" + userStr + "' or opid='" + opidStr + "') and user_state='1'";
                    com.CommandText = SQLText;
                    dr = com.ExecuteReader();
                    int userCount = 0;
                    while (dr.Read())
                    {
                        userCount++;
                        //将用户信息保存至session中，客户端网页关闭后自动清除
                        context.Session["user_id"] = dr[0].ToString();
                        context.Session["Name"] = dr[1].ToString();
                        context.Session["UserName"] = dr[2].ToString();
                        context.Session["OPID"] = dr[3].ToString();
                        context.Session["Department"] = dr[4].ToString();
                        context.Session["Telephone"] = dr[5].ToString();
                        context.Session["Email"] = dr[6].ToString();
                        context.Session["WeiXin"] = dr[7].ToString();
                        break;
                    }
                    returnStr = "OK";
                }
                catch (Exception msg)
                {
                    returnStr = msg.Message;
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr = null;
                    }
                    if (mycon.State != ConnectionState.Closed)
                        mycon.Close();
                    mycon = null;
                }
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