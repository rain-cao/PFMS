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
    /// SMM_GetBindPCBList 的摘要说明
    /// </summary>
    public class SMM_GetBindPCBList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string condition = context.Request.QueryString["condition"].Trim().ToUpper();
            int curr = Convert.ToInt32(context.Request.QueryString["curr"]);
            int nums = Convert.ToInt32(context.Request.QueryString["nums"]);
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            SqlDataAdapter da = null;
            DataSet ds = new DataSet();
            mycon = DBConnect.ConnectSQLServer();
            int code = 0;
            int totalCount = 0;
            string msgStr = "";
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append("select count(id)\n");
                sb.Append("from PCB_PN where id not in(select pcb_id from SteelMesh_BindPCB where status='1')\n");
                if (condition != "")
                    sb.Append("and pcb_pn like '%" + condition + "%'");
                mycom.CommandText = sb.ToString();
                dr = mycom.ExecuteReader();
                dr.Read();
                totalCount = Convert.ToInt32(dr[0].ToString());
                dr.Close();
                dr = null;
                sb.Clear();
                sb.Append("select top " + nums.ToString() + " id,pcb_pn\n");
                sb.Append("from PCB_PN\n");
                if (condition != "")
                {
                    sb.Append("where (id not in(select pcb_id from SteelMesh_BindPCB where status='1') and pcb_pn like '%" + condition + "%')\n");
                    sb.Append("and id not in(select top " + (nums * (curr - 1)).ToString() + " id from PCB_PN\n");
                    sb.Append("where (id not in(select pcb_id from SteelMesh_BindPCB where status='1') and pcb_pn like '%" + condition + "%'))");
                }
                else
                {
                    sb.Append("where (id not in(select pcb_id from SteelMesh_BindPCB where status='1'))\n");
                    sb.Append("and id not in(select top " + (nums * (curr - 1)).ToString() + " id from PCB_PN\n");
                    sb.Append("where (id not in(select pcb_id from SteelMesh_BindPCB where status='1')))");
                }
                mycom.CommandText = sb.ToString();
                da = new SqlDataAdapter(mycom.CommandText, mycon);
                da.Fill(ds);
            }
            catch (Exception msg)
            {
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
                    da = null;
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