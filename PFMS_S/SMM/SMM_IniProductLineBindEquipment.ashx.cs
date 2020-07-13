using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;
using System.Data;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_IniProductLineBindEquipment 的摘要说明
    /// </summary>
    public class SMM_IniProductLineBindEquipment : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string idStr = context.Request.Form["id"].ToString();
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            SqlDataAdapter da = null;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow drow = null;
            dt.Columns.Add(new DataColumn("value", typeof(string)));
            dt.Columns.Add(new DataColumn("title", typeof(string)));
            DataSet ds1 = new DataSet();
            int code = 0;
            string msgStr = "";
            int count = 0;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select id,pl_name,code from ProductLine_Management";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    drow = dt.NewRow();
                    drow[0] = dr[0].ToString();
                    drow[1] = dr[1].ToString() + "(" + dr[2].ToString() + ")";
                    dt.Rows.Add(drow);
                }
                ds.Tables.Add(dt);
                dr.Close();
                dr = null;
                mycom.CommandText = "select productline_id from SteelMesh_BindEquipAndPLine where equipment_id='" + idStr + "'";
                da = new SqlDataAdapter(mycom.CommandText, mycon);
                da.Fill(ds1);
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
                    da = null;
                }
                if (dr != null)
                {
                    dr.Close();
                    dr = null;
                }
                if (mycon.State != ConnectionState.Closed)
                    mycon.Close();
                mycon = null;
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(new { code = code, msg = msgStr, data = ds.Tables[0], value = ds1.Tables[0] }));    //返回JSON数据
            ds.Dispose();
            ds = null;
            ds1.Dispose();
            ds1 = null;
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