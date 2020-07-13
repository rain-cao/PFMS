using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_GetSignRule 的摘要说明
    /// </summary>
    public class SMM_GetSignRule : IHttpHandler
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
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow drow = null;
            dt.Columns.Add(new DataColumn("id", typeof(string)));
            dt.Columns.Add(new DataColumn("total_use_count", typeof(string)));
            dt.Columns.Add(new DataColumn("status_rule", typeof(string)));
            dt.Columns.Add(new DataColumn("alert_count", typeof(string)));
            dt.Columns.Add(new DataColumn("alert_mailto", typeof(string)));
            dt.Columns.Add(new DataColumn("alert_mailcopy", typeof(string)));
            dt.Columns.Add(new DataColumn("transfinite_mailto", typeof(string)));
            dt.Columns.Add(new DataColumn("transfinite_mailcopy", typeof(string)));
            int code = 0;
            string msgStr = "";
            mycon = DBConnect.ConnectSQLServer();
            myconT = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                myconT.Open();
                mycomT = myconT.CreateCommand();
                mycom.CommandText = "select id,total_use_count,status_rule,alert_count,alert_mailto,alert_mailcopy,transfinite_mailto,transfinite_mailcopy from SteelMesh_ExamineRule";
                dr = mycom.ExecuteReader();
                string[] mailArray = new string[] { };
                string mailStr = "";
                while (dr.Read())
                {
                    drow = dt.NewRow();
                    drow[0] = dr[0].ToString();
                    drow[1] = dr[1].ToString();
                    drow[2] = dr[2].ToString();
                    drow[3] = dr[3].ToString();
                    for (int i = 1; i <= 4; i++)
                    {
                        Array.Clear(mailArray, 0, mailArray.Length);
                        mailArray = dr[i+3].ToString().Split(';');
                        mailStr = "";
                        if (mailArray.Length > 0)
                        {
                            foreach (string userid in mailArray)
                            {
                                if (userid != "" && userid != null)
                                {
                                    mycomT.CommandText = "select username,name from User_Manage where user_id='" + userid + "'";
                                    drT = mycomT.ExecuteReader();
                                    drT.Read();
                                    mailStr = msgStr + drT[0].ToString() + "(" + drT[1].ToString() + ");";
                                    drT.Close();
                                    drT = null;
                                }
                            }
                        }
                        drow[i+3] = mailStr;
                    }
                    dt.Rows.Add(drow);
                }
                ds.Tables.Add(dt);
            }
            catch (Exception msg)
            {
                msgStr = msg.Message;
                code = 1;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr = null;
                }
                if (drT != null)
                {
                    drT.Close();
                    drT = null;
                }
                if (mycon.State != ConnectionState.Closed)
                    mycon.Close();
                mycon = null;
                if (myconT.State != ConnectionState.Closed)
                    myconT.Close();
                myconT = null;
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(new { code = code, msg = msgStr, count = ds.Tables[0].Rows.Count, data = ds.Tables[0] }));   //返回JSON数据
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