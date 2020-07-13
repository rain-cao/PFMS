using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace PFMS_S.BM
{
    /// <summary>
    /// BM_GetStatusProcessList 的摘要说明
    /// </summary>
    public class BM_GetStatusProcessList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataAdapter da = null;
            DataSet ds = new DataSet();
            int code = 0;
            string msgStr = "";
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select id,status_id,status_name,status_desc from Fixture_ProcessStatus_List";
                da = new SqlDataAdapter(mycom.CommandText, mycon);
                da.Fill(ds);
            }
            catch (Exception msg)
            {
                msgStr = msg.Message;
                code = 1;
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
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(new { code = code, msg = msgStr, count = ds.Tables[0].Rows.Count, data = ds.Tables[0] }));    //返回JSON数据
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