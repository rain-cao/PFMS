using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Newtonsoft.Json;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SWM_GetPersonnelByDept 的摘要说明
    /// </summary>
    public class SWM_GetPersonnelByDept : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataAdapter da = null;
            DataSet ds = new DataSet();
            bool flag = false;
            string msgStr = "";
            string idStr = context.Request.Form["id"].ToString();
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select user_id,name,username from User_Manage where user_state='1' and dept='" + idStr + "'";
                da = new SqlDataAdapter(mycom.CommandText, mycon);
                da.Fill(ds);
                flag = true;
            }
            catch (Exception msg)
            {
                msgStr = msg.Message;
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
                if (mycon.State != ConnectionState.Closed)
                    mycon.Close();
                mycon = null;
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(new { flag = flag, msg = msgStr, data = ds.Tables[0] }));    //返回JSON数据
            ds.Dispose();
            ds = null;
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