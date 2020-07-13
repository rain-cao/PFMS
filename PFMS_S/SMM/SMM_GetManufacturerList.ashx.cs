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
    /// SMM_GetManufacturerList 的摘要说明
    /// </summary>
    public class SMM_GetManufacturerList : IHttpHandler
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
            string condition = context.Request.QueryString["condition"].Trim();
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
                sb.Append("count(id) from SteelMesh_Manufacturer_Management\n");
                if (condition != "")
                    sb.Append("WHERE name LIKE '%" + condition + "%' OR code LIKE '%" + condition + "%'\n");
                mycom.CommandText = sb.ToString();
                sb.Clear();
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    totalCount = Convert.ToInt32(dr[0].ToString());
                }
                dr.Close();
                dr = null;
                sb.Append("SELECT top " + nums.ToString() + "\n");
                sb.Append("id,name,code,responsible,telephone,email,address,status,remark\n");
                sb.Append("FROM\n");
                sb.Append("SteelMesh_Manufacturer_Management\n");
                if (condition != "")
                {
                    sb.Append("WHERE (name LIKE '%" + condition + "%' OR code LIKE '%" + condition + "%')\n");
                    sb.Append("and id not in (select top " + (nums * (curr - 1)).ToString() + " id from SteelMesh_Detail_Storage WHERE name LIKE '%" + condition + "%' OR code LIKE '%" + condition + "%')\n");
                }
                else
                {
                    sb.Append("WHERE id not in (select top " + (nums * (curr - 1)).ToString() + " id from SteelMesh_Detail_Storage)\n");
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