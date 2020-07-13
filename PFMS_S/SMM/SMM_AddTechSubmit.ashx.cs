using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_AddTechSubmit 的摘要说明
    /// </summary>
    public class SMM_AddTechSubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string msgStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string technologyStr = context.Request.Form["technology"].Trim();
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select id from SteelMesh_Technology where technology='" + technologyStr + "'";
                object retStr = mycom.ExecuteScalar();
                if (retStr != null)
                    msgStr = "钢网工艺:" + technologyStr + "已存在,请勿重复添加!";
                else
                {
                    mycom.CommandText = "insert into SteelMesh_Technology(technology,create_by,create_time,update_by,update_time) values(N'" + technologyStr + "','" + context.Session["user_id"] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + context.Session["user_id"] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
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