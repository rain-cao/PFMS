﻿using System;
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
    /// BM_GetDepartmentList 的摘要说明
    /// </summary>
    public class BM_GetDepartmentList : IHttpHandler
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
                sb.Append("id,dept,dept_code,(SELECT name FROM User_Manage WHERE user_id=Department_Manage.mgr_1) AS mgr_1,(SELECT name FROM User_Manage WHERE user_id=Department_Manage.mgr_2) AS mgr_2,(SELECT name FROM User_Manage WHERE user_id=Department_Manage.mgr_3) AS mgr_3\n");
                sb.Append("FROM\n");
                sb.Append("Department_Manage\n");
                if(condition != "")
                    sb.Append("WHERE dept LIKE '%" + condition + "%' OR dept_code LIKE '%" + condition + "%' OR mgr_1 LIKE '%" + condition + "%' OR mgr_2 LIKE '%" + condition + "%' OR mgr_3 LIKE '%" + condition + "%'\n");
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
                if(da != null)
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