using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace PFMS_S
{
    /// <summary>
    /// UserPasswordGet 的摘要说明
    /// </summary>
    public class UserPasswordGet : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string returnStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            string MailHost = ConfigurationManager.AppSettings["MailHost"].ToString().Trim();
            int MailPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"].ToString().Trim());
            string MailFromAddress = ConfigurationManager.AppSettings["MailFromAddress"].ToString().Trim();
            string MailPassword = ConfigurationManager.AppSettings["MailPassword"].ToString().Trim();
            string userName = context.Request.Form["userAccount"];
            userName = userName.Trim().ToLower();
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select password,email from User_Manage where username='" + userName + "' or email='" + userName + "' or opid='" + userName + "'";
                dr = mycom.ExecuteReader();
                string emailStr = "", passwordStr = "";
                while (dr.Read())
                {
                    passwordStr = dr[0].ToString();
                    emailStr = dr[1].ToString();
                }
                if (emailStr != "")
                {
                    MailClass.EmailParameterSet emailPara = new MailClass.EmailParameterSet();
                    emailPara.mailServer = MailHost;
                    emailPara.mailPort = MailPort;
                    emailPara.mailFrom = MailFromAddress;
                    emailPara.mailFromPassword = MailPassword;
                    emailPara.mailSubject = "治工具管理系统---密码找回";
                    emailPara.mailBody = "您的登陆密码：" + passwordStr;
                    emailPara.mailTo = new List<string>();
                    emailPara.mailTo.Clear();
                    emailPara.mailTo.Add(emailStr);
                    emailPara.mailCopy = new List<string>();
                    emailPara.mailCopy.Clear();
                    bool mailOK = false;
                    try
                    {
                        MailClass mailFun = new MailClass();
                        mailFun.SendMail(emailPara);
                        mailOK = true;
                    }
                    catch (Exception msg)
                    {
                        Console.WriteLine(msg.Message);
                    }
                    if (mailOK)
                        returnStr = emailStr;
                }
            }
            catch (Exception msg)
            {
                System.Console.WriteLine(msg.Message);
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