using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace PFMS_S
{
    /// <summary>
    /// BM_AddDepartmentSubmit 的摘要说明
    /// </summary>
    public class BM_AddDepartmentSubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string msgStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string dept = context.Request.Form["dept"].ToUpper().Trim();
            string dept_code = context.Request.Form["dept_code"].ToUpper().Trim();
            string mgr_1 = context.Request.Form["mgr_1"];
            string mgr_2 = context.Request.Form["mgr_2"];
            string mgr_3 = context.Request.Form["mgr_3"];
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select id from Department_Manage where dept='" + dept + "' or dept_code='" + dept_code + "'";
                object retStr = mycom.ExecuteScalar();
                if (retStr != null)
                    msgStr = "部门:" + dept + "(" + dept_code + ")已存在,请勿重复添加!";
                else
                {
                    mycom.CommandText = "insert into Department_Manage(dept,dept_code,mgr_1,mgr_2,mgr_3,create_by,create_time,update_by,update_time) values(N'" + dept + "','" + dept_code + "','" + mgr_1 + "','" + mgr_2 + "','" + mgr_3 + "','" + context.Session["user_id"] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + context.Session["user_id"] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
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