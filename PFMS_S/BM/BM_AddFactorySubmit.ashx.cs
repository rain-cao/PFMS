using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace PFMS_S
{
    /// <summary>
    /// BM_AddFactorySubmit 的摘要说明
    /// </summary>
    public class BM_AddFactorySubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string msgStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string factory_code = context.Request.Form["factory_code"].ToUpper().Trim();
            string factory_describe = context.Request.Form["factory_describe"].Trim();
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select id from Factory_Manage where factory_code='" + factory_code + "'";
                object retStr = mycom.ExecuteScalar();
                if (retStr != null)
                    msgStr = "厂区:" + factory_code + "已存在,请勿重复添加!";
                else
                {
                    mycom.CommandText = "insert into Factory_Manage(factory_code,factory_describe,create_by,create_time,update_by,update_time) values('" + factory_code + "',N'" + factory_describe + "','" + context.Session["user_id"] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + context.Session["user_id"] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    int ret = mycom.ExecuteNonQuery();
                    if (ret > 0)
                        msgStr = "OK";
                    else
                        msgStr = "数据插入失败!";
                }
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