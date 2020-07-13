using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using System.Text;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_GetEquipmentList 的摘要说明
    /// </summary>
    public class SMM_GetEquipmentList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            SqlConnection myconT = null;
            SqlCommand mycomT = null;
            SqlDataReader drT = null;
            int code = 0;
            string msgStr = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow drow = null;
            dt.Columns.Add(new DataColumn("id", typeof(string)));
            dt.Columns.Add(new DataColumn("Equipment_name", typeof(string)));
            dt.Columns.Add(new DataColumn("ip_address", typeof(string)));
            dt.Columns.Add(new DataColumn("clean_length", typeof(string)));
            dt.Columns.Add(new DataColumn("productline_id", typeof(string)));
            dt.Columns.Add(new DataColumn("status", typeof(string)));
            mycon = DBConnect.ConnectSQLServer();
            myconT = DBConnect.ConnectSQLServer();
            List<string> PLid = new List<string>();
            PLid.Clear();
            try
            {
                StringBuilder sb = new StringBuilder();
                mycon.Open();
                mycom = mycon.CreateCommand();
                myconT.Open();
                mycomT = myconT.CreateCommand();
                mycom.CommandText = "select id,Equipment_name,ip_address,clean_length,status from SteelMesh_CleanEquipment";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    PLid.Clear();
                    drow = dt.NewRow();
                    drow[0] = dr[0].ToString();
                    drow[1] = dr[1].ToString();
                    drow[2] = dr[2].ToString();
                    drow[3] = dr[3].ToString();
                    mycomT.CommandText = "select productline_id from SteelMesh_BindEquipAndPLine where equipment_id='" + dr[0].ToString() + "'";
                    drT = mycomT.ExecuteReader();
                    while (drT.Read())
                    {
                        PLid.Add(drT[0].ToString());
                    }
                    drT.Close();
                    drT = null;
                    string PLInfo = "";
                    foreach (string plid in PLid)
                    {
                        sb.Clear();
                        sb.Append("select pl_name,code,\n");
                        sb.Append("(select ws_name from Workshop_Manage where id=ProductLine_Management.workshop_id) as workshop\n");
                        sb.Append("from ProductLine_Management where id='" + plid + "'");
                        mycomT.CommandText = sb.ToString();
                        drT = mycomT.ExecuteReader();
                        drT.Read();
                        PLInfo = PLInfo + drT[2].ToString() + "---" + drT[0].ToString() + "(" + drT[1].ToString() + ")<br />";
                        drT.Close();
                        drT = null;
                    }
                    drow[4] = PLInfo;
                    drow[5] = dr[4].ToString();
                    dt.Rows.Add(drow);
                }
                ds.Tables.Add(dt);
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
                if (mycon.State != ConnectionState.Closed)
                    mycon.Close();
                mycon = null;

                if (drT != null)
                {
                    drT.Close();
                    drT = null;
                }
                if (myconT.State != ConnectionState.Closed)
                    myconT.Close();
                myconT = null;
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