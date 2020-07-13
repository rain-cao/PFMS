using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_AddBindPCBSubmit 的摘要说明
    /// </summary>
    public class SMM_AddBindPCBSubmit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string smlist_id = context.Request.Form["smlist_id"].ToString();
            string pcb_id = context.Request.Form["input_id"].ToString();
            string msgStr = "";
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                if (pcb_id == "")
                {
                    msgStr = "待绑定的PCB料号为空!";
                }
                else
                {
                    mycon.Open();
                    mycom = mycon.CreateCommand();
                    mycom.CommandText = "insert into SteelMesh_BindPCB(smlist_id,pcb_id,status) values('" + smlist_id + "','" + pcb_id + "','1')";
                    int ret = mycom.ExecuteNonQuery();
                    if (ret > 0)
                        msgStr = "OK";
                    else
                        msgStr = "数据插入影响行数为0";
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