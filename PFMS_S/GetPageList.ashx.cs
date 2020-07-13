using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Web.SessionState;
using System.Diagnostics;

namespace PFMS_S
{
    /// <summary>
    /// GetPageList 的摘要说明
    /// </summary>
    public class GetPageList : IHttpHandler, IRequiresSessionState
    {
        SqlConnection mycon = null;
        SqlCommand mycom = null;
        SqlDataReader dr = null;

        SqlConnection myconT = null;
        SqlCommand mycomT = null;
        SqlDataReader drT = null;

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataRow drow = null;
        StringBuilder sb = new StringBuilder();
        int code = 0;
        string msgStr = "";
        int totalCount = 0;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            dt.Columns.Add(new DataColumn("menu_name", typeof(string)));
            dt.Columns.Add(new DataColumn("parent", typeof(string)));
            dt.Columns.Add(new DataColumn("url", typeof(string)));
            dt.Columns.Add(new DataColumn("type", typeof(string)));
            dt.Columns.Add(new DataColumn("status", typeof(string)));
            dt.Columns.Add(new DataColumn("remark", typeof(string)));
            dt.Columns.Add(new DataColumn("power", typeof(string)));
            mycon = DBConnect.ConnectSQLServer();
            myconT = DBConnect.ConnectSQLServer();
            //首先查询主目录
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select menu_id,menu_name,menu_state,remark,(select power_no from Auth_Manage where user_id='" + context.Session["user_id"].ToString() + "' and menu_id=Menu_Manage.menu_id) as init_power from Menu_Manage where parent_id='0' and menu_state='1' order by order_num asc";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    totalCount++;
                    drow = dt.NewRow();
                    drow[0] = dr[1].ToString();
                    drow[1] = "N/A";
                    drow[2] = "N/A";
                    drow[3] = "目录";
                    drow[4] = dr[2].ToString();
                    drow[5] = dr[3].ToString();
                    if (dr[4].ToString() == "0")
                    {
                        drow[6] = "不可显";
                    }
                    else if (dr[4].ToString() == "1")
                    {
                        drow[6] = "只读";
                    }
                    else if (dr[4].ToString() == "2")
                    {
                        drow[6] = "读写";
                    }
                    dt.Rows.Add(drow);
                    GetNextMenu(dr[0].ToString());
                }
                ds.Tables.Add(dt);
            }
            catch (Exception msg)
            {
                code = 1;
                msgStr = msgStr + msg.Message + "\n";
            }
            finally
            {
                if (drT != null)
                {
                    drT.Close();
                    drT = null;
                }
                if (myconT.State != ConnectionState.Closed)
                    myconT.Close();
                myconT = null;
                if (dr != null)
                {
                    dr.Close();
                    dr = null;
                }
                if (mycon.State != ConnectionState.Closed)
                    mycon.Close();
                mycon = null;
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(new { code = code, msg = msgStr, count = totalCount, data = ds.Tables[0] }));    //返回JSON数据
            ds.Dispose();
            ds = null;
        }

        private void GetNextMenu(string menuID)
        {
            //查询各目录的下一阶目录
            try
            {
                myconT.Open();
                mycomT = myconT.CreateCommand();
                mycomT.CommandText = "select menu_id,menu_name,(select menu_name from Menu_Manage where menu_id='" + menuID + "') as parentName,menu_state,remark,(select power_no from Auth_Manage where user_id='" + HttpContext.Current.Session["user_id"].ToString() + "' and menu_id=Menu_Manage.menu_id) as init_power from Menu_Manage where parent_id='" + menuID + "' and menu_type='catalogue' and menu_state='1' order by order_num asc";
                drT = mycomT.ExecuteReader();
                List<string> strlist = new List<string>();
                strlist.Clear();
                while (drT.Read())
                {
                    strlist.Add(drT[0].ToString() + "|" + drT[1].ToString() + "|" + drT[2].ToString() + "|N/A|catalogue|" + drT[3].ToString() + "|" + drT[4].ToString() + "|" + drT[5].ToString());
                }
                drT.Close();
                drT = null;
                myconT.Close();
                string[] strTmp;
                foreach (string ss in strlist)
                {
                    totalCount++;
                    strTmp = ss.Split('|');
                    drow = dt.NewRow();
                    drow[0] = strTmp[1];
                    drow[1] = strTmp[2];
                    drow[2] = strTmp[3];
                    drow[3] = "目录";
                    drow[4] = strTmp[5];
                    drow[5] = strTmp[6];
                    if (strTmp[7] == "0")
                    {
                        drow[6] = "不可显";
                    }
                    else if (strTmp[7] == "1")
                    {
                        drow[6] = "只读";
                    }
                    else if (strTmp[7] == "2")
                    {
                        drow[6] = "读写";
                    }
                    dt.Rows.Add(drow);
                    GetNextMenu(strTmp[0]);
                }
            }
            catch (Exception msg)
            {
                code = 1;
                msgStr = msgStr +  msg.Message + "\n";
            }
            finally
            {
                if (drT != null)
                {
                    drT.Close();
                    drT = null;
                }
                if (myconT.State != ConnectionState.Closed)
                    myconT.Close();
            }
            //查询各个目录下的菜单
            try
            {
                myconT.Open();
                mycomT = myconT.CreateCommand();
                mycomT.CommandText = "select menu_name,(select menu_name from Menu_Manage where menu_id='" + menuID + "') as parentName,menu_url,menu_state,remark,(select power_no from Auth_Manage where user_id='" + HttpContext.Current.Session["user_id"].ToString() + "' and menu_id=Menu_Manage.menu_id) as init_power from Menu_Manage where parent_id='" + menuID + "' and menu_type='menu' and menu_state='1' order by order_num asc";
                drT = mycomT.ExecuteReader();
                while (drT.Read())
                {
                    totalCount++;
                    drow = dt.NewRow();
                    drow[0] = drT[0].ToString();
                    drow[1] = drT[1].ToString();
                    drow[2] = drT[2].ToString();
                    drow[3] = "菜单";
                    drow[4] = drT[3].ToString();
                    drow[5] = drT[4].ToString();
                    if (drT[5].ToString() == "0")
                    {
                        drow[6] = "不可显";
                    }
                    else if (drT[5].ToString() == "1")
                    {
                        drow[6] = "只读";
                    }
                    else if (drT[5].ToString() == "2")
                    {
                        drow[6] = "读写";
                    }
                    dt.Rows.Add(drow);
                }
                drT.Close();
                drT = null;
                myconT.Close();
            }
            catch (Exception msg)
            {
                code = 1;
                msgStr = msgStr + msg.Message + "\n";
            }
            finally
            {
                if (drT != null)
                {
                    drT.Close();
                    drT = null;
                }
                if (myconT.State != ConnectionState.Closed)
                    myconT.Close();
            }
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