using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Newtonsoft.Json;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_GetDetalProductLineProcess 的摘要说明
    /// </summary>
    public class SMM_GetDetalProductLineProcess : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            SqlConnection myconT = null;
            SqlCommand mycomT = null;
            SqlDataReader drT = null;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow drow = null;
            dt.Columns.Add(new DataColumn("id", typeof(string)));
            dt.Columns.Add(new DataColumn("order_number", typeof(string)));
            dt.Columns.Add(new DataColumn("status_id", typeof(string)));
            dt.Columns.Add(new DataColumn("role_define", typeof(string)));
            dt.Columns.Add(new DataColumn("role_force", typeof(string)));
            int code = 0;
            string msgStr = "";
            string condition = context.Request.QueryString["condition"].Trim();
            mycon = DBConnect.ConnectSQLServer();
            myconT = DBConnect.ConnectSQLServer();
            try
            {
                StringBuilder sb = new StringBuilder();
                mycon.Open();
                mycom = mycon.CreateCommand();
                myconT.Open();
                mycomT = myconT.CreateCommand();
                sb.Clear();
                sb.Append("select id,order_number,(select status_name from Fixture_ProcessStatus_List where status_id=SteelMesh_OnLine_Process.status_id) as status_id,role_define,\n");
                sb.Append("case role_force\n");
                sb.Append("when 0 then 'N'\n");
                sb.Append("when 1 then 'Y'\n");
                sb.Append("end as role_force\n");
                sb.Append("from SteelMesh_OnLine_Process where productline_id='" + condition + "'\n");
                sb.Append("order by order_number asc");
                mycom.CommandText = sb.ToString();
                dr = mycom.ExecuteReader();
                string roledefineNum = "";
                string roledefine = "";
                while (dr.Read())
                {
                    roledefine = "";
                    roledefineNum = "";
                    drow = dt.NewRow();
                    drow[0] = dr[0].ToString();
                    drow[1] = dr[1].ToString();
                    drow[2] = dr[2].ToString();
                    roledefineNum = dr[3].ToString().Trim();
                    if (roledefineNum == "")
                        drow[3] = "";
                    else
                    {
                        while (roledefineNum.Contains(";"))
                        {
                            switch (roledefineNum.Substring(0, roledefineNum.IndexOf(";")))
                            {
                                case "1":
                                    roledefine = roledefine + "卡关设备清洗时长<br />";
                                    break;
                                case "2":
                                    roledefine = roledefine + "卡关人工清洗时长<br />";
                                    break;
                                case "3":
                                    roledefine = roledefine + "卡关钢网清洗周期<br />";
                                    break;
                                case "4":
                                    roledefine = roledefine + "归还前卡关钢网总清洗次数<br />";
                                    break;
                                default:
                                    break;
                            }
                            roledefineNum = roledefineNum.Substring(roledefineNum.IndexOf(";") + 1);
                        }
                        roledefine = roledefine.Substring(0, roledefine.Length - 1);
                        drow[3] = roledefine;
                    }
                    drow[4] = dr[4].ToString();
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