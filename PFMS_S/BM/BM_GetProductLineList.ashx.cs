using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Newtonsoft.Json;

namespace PFMS_S
{
    /// <summary>
    /// BM_GetProductLineList 的摘要说明
    /// </summary>
    public class BM_GetProductLineList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataAdapter da = null;
            SqlDataReader dr = null;
            DataSet ds = new DataSet();
            int code = 0;
            string msgStr = "";
            string condition = context.Request.QueryString["condition"].Trim().ToUpper();
            int curr = Convert.ToInt32(context.Request.QueryString["curr"]);
            int nums = Convert.ToInt32(context.Request.QueryString["nums"]);
            int totalCount = 0;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                StringBuilder sb = new StringBuilder();
                mycon.Open();
                mycom = mycon.CreateCommand();
                sb.Clear();
                sb.Append("SELECT\n");
                sb.Append("count(id) from ProductLine_Management\n");
                if (condition != "")
                    sb.Append("WHERE pl_name LIKE '%" + condition + "%' OR code LIKE '%" + condition + "%' OR workshop_id=(select id from Workshop_Manage where ws_name='" + condition + "')\n");
                mycom.CommandText = sb.ToString();
                sb.Clear();
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    totalCount = Convert.ToInt32(dr[0].ToString());
                }
                dr.Close();
                dr = null;
                sb.Clear();
                sb.Append("SELECT top " + nums.ToString() + "\n");
                sb.Append("id,pl_name,code,address,(SELECT ws_name FROM Workshop_Manage WHERE id=ProductLine_Management.workshop_id) AS workshop,(SELECT name FROM User_Manage WHERE user_id=ProductLine_Management.ownerid) AS owner,remark\n");
                sb.Append("FROM\n");
                sb.Append("ProductLine_Management\n");
                if (condition != "")
                {
                    sb.Append("WHERE (pl_name LIKE '%" + condition + "%' OR code LIKE '%" + condition + "%' OR workshop_id=(select id from Workshop_Manage where ws_name='" + condition + "'))\n");
                    sb.Append("AND id not in (select top " + (nums * (curr - 1)).ToString() + " id from ProductLine_Management\n");
                    sb.Append("WHERE pl_name LIKE '%" + condition + "%' OR code LIKE '%" + condition + "%' OR workshop_id=(select id from Workshop_Manage where ws_name='" + condition + "'))\n");
                }
                else
                {
                    sb.Append("WHERE id not in (select top " + (nums * (curr - 1)).ToString() + " id from ProductLine_Management)\n");
                }
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
                if (dr != null)
                {
                    dr.Close();
                    dr = null;
                }
                if (da != null)
                {
                    da.Dispose();
                }
                if (mycon.State != ConnectionState.Closed)
                    mycon.Close();
                mycon = null;
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(new { code = code, msg = msgStr, count = totalCount, data = ds.Tables[0] }));    //返回JSON数据
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