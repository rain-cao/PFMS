using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;

namespace PFMS_S
{
    /// <summary>
    /// BM_GetFactoryList 的摘要说明
    /// </summary>
    public class BM_GetFactoryList : IHttpHandler
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
            int count = 0;
            string condition = context.Request.QueryString["condition"];
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                StringBuilder sb = new StringBuilder();
                mycon.Open();
                mycom = mycon.CreateCommand();
                sb.Clear();
                sb.Append("SELECT\n");
                sb.Append("id,factory_code,factory_describe\n");
                sb.Append("FROM\n");
                sb.Append("Factory_Manage\n");
                if (condition != "")
                    sb.Append("WHERE factory_code LIKE '%" + condition + "%' OR factory_describe LIKE '%" + condition + "%'\n");
                sb.Append("ORDER BY id ASC\n");
                mycom.CommandText = sb.ToString();
                sb.Clear();
                da = new SqlDataAdapter(mycom.CommandText, mycon);
                da.Fill(ds);
            }
            catch (Exception msg)
            {
                code = 1;
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