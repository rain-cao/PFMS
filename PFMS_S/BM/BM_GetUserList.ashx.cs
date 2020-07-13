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
    /// BM_GetUserList 的摘要说明
    /// </summary>
    public class BM_GetUserList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataAdapter da = null;
            SqlDataReader dr = null;

            SqlConnection myconT = null;
            SqlCommand mycomT = null;
            SqlDataReader drT = null;

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow drow = null;
            int code = 0;
            string msgStr = "";
            string condition = context.Request.QueryString["condition"].Trim();
            string flag = context.Request.QueryString["flag"].Trim();
            int curr = Convert.ToInt32(context.Request.QueryString["curr"]);
            int nums = Convert.ToInt32(context.Request.QueryString["nums"]);
            int totalCount = 0;
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

                //用户管理模块，用于获取所有用户相关信息列表
                //钢网借用权限管理模块，用于获取用户相关信息和借用权限列表
                if (flag == "0") //用户管理模块
                {
                    sb.Append("SELECT\n");
                    sb.Append("count(user_id) from User_Manage\n");
                    if (condition != "")
                        sb.Append("WHERE name LIKE '%" + condition + "%' OR username LIKE '%" + condition + "%' OR dept=(select id from Department_Manage where dept='" + condition.ToUpper() + "' or dept_code='" + condition.ToUpper() + "')\n");
                }
                else if (flag == "1" || flag == "2" || flag == "3")  //钢网借用权限管理模块和用户对目录菜单权限管理模块和用户对客户端页面管理模块
                {
                    sb.Append("SELECT\n");
                    sb.Append("count(user_id) from User_Manage where user_state='1'\n");
                    if (condition != "")
                        sb.Append("and name LIKE '%" + condition + "%' OR username LIKE '%" + condition + "%' OR dept=(select id from Department_Manage where dept='" + condition.ToUpper() + "' or dept_code='" + condition.ToUpper() + "')\n");
                }
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
                if (flag == "0")  //用户管理模块
                {
                    sb.Append("SELECT top " + nums.ToString() + "\n");
                    sb.Append("user_id,name,username,(SELECT dept FROM Department_Manage WHERE id=User_Manage.dept) AS dept,(SELECT post FROM Job_Manage WHERE id=User_Manage.post) AS post,telephone,email,weixin_no,user_state\n");
                    sb.Append("FROM\n");
                    sb.Append("User_Manage\n");
                    if (condition != "")
                    {
                        sb.Append("WHERE (name LIKE '%" + condition + "%' OR username LIKE '%" + condition + "%' OR dept=(select id from Department_Manage where dept='" + condition.ToUpper() + "' or dept_code='" + condition.ToUpper() + "'))\n");
                        sb.Append("AND user_id not in (select top " + (nums * (curr - 1)).ToString() + " user_id from User_Manage WHERE name LIKE '%" + condition + "%' OR username LIKE '%" + condition + "%' OR dept=(select id from Department_Manage where dept='" + condition.ToUpper() + "' or dept_code='" + condition.ToUpper() + "'))\n");
                    }
                    else
                    {
                        sb.Append("where user_id not in (select top " + (nums * (curr - 1)).ToString() + " user_id from User_Manage)\n");
                    }
                    sb.Append("ORDER BY user_id ASC\n");
                }
                else if (flag == "1")  //钢网借用权限管理模块
                {
                    sb.Append("SELECT top " + nums.ToString() + "\n");
                    sb.Append("user_id,name,username,(SELECT dept FROM Department_Manage WHERE id=User_Manage.dept) AS dept,(SELECT post FROM Job_Manage WHERE id=User_Manage.post) AS post,\n");
                    sb.Append("case (select power_status from SteelMesh_LendAndReturnPower where user_id=User_Manage.user_id)\n");
                    sb.Append("when '0' then '0'\n");
                    sb.Append("when '1' then '1'\n");
                    sb.Append("else '0' end as power_status\n");
                    sb.Append("FROM\n");
                    sb.Append("User_Manage\n");
                    if (condition != "")
                    {
                        sb.Append("WHERE user_state='1' and (name LIKE '%" + condition + "%' OR username LIKE '%" + condition + "%' OR dept=(select id from Department_Manage where dept='" + condition.ToUpper() + "' or dept_code='" + condition.ToUpper() + "'))\n");
                        sb.Append("AND user_id not in (select top " + (nums * (curr - 1)).ToString() + " user_id from User_Manage WHERE user_state='1' and name LIKE '%" + condition + "%' OR username LIKE '%" + condition + "%' OR dept=(select id from Department_Manage where dept='" + condition.ToUpper() + "' or dept_code='" + condition.ToUpper() + "'))\n");
                    }
                    else
                    {
                        sb.Append("where user_state='1' and user_id not in (select top " + (nums * (curr - 1)).ToString() + " user_id from User_Manage where user_state='1')\n");
                    }
                    sb.Append("ORDER BY user_id ASC\n");
                }
                else if (flag == "2" || flag == "3")  //用户对目录菜单权限管理模块、用户对客户端页面管理模块也在这里获取用户清单并在下面拼接ds
                {
                    sb.Append("SELECT top " + nums.ToString() + "\n");
                    sb.Append("user_id,name,username,(SELECT dept FROM Department_Manage WHERE id=User_Manage.dept) AS dept,(SELECT post FROM Job_Manage WHERE id=User_Manage.post) AS post\n");
                    sb.Append("FROM\n");
                    sb.Append("User_Manage\n");
                    if (condition != "")
                    {
                        sb.Append("WHERE user_state='1' and (name LIKE '%" + condition + "%' OR username LIKE '%" + condition + "%' OR dept=(select id from Department_Manage where dept='" + condition.ToUpper() + "' or dept_code='" + condition.ToUpper() + "'))\n");
                        sb.Append("AND user_id not in (select top " + (nums * (curr - 1)).ToString() + " user_id from User_Manage WHERE user_state='1' and name LIKE '%" + condition + "%' OR username LIKE '%" + condition + "%' OR dept=(select id from Department_Manage where dept='" + condition.ToUpper() + "' or dept_code='" + condition.ToUpper() + "'))\n");
                    }
                    else
                    {
                        sb.Append("where user_state='1' and user_id not in (select top " + (nums * (curr - 1)).ToString() + " user_id from User_Manage)\n");
                    }
                    sb.Append("ORDER BY user_id ASC\n");
                }
                if (flag == "3")
                {
                    dt.Columns.Add(new DataColumn("user_id", typeof(string)));
                    dt.Columns.Add(new DataColumn("name", typeof(string)));
                    dt.Columns.Add(new DataColumn("username", typeof(string)));
                    dt.Columns.Add(new DataColumn("dept", typeof(string)));
                    dt.Columns.Add(new DataColumn("post", typeof(string)));
                    dt.Columns.Add(new DataColumn("client_page", typeof(string)));
                    string clientPageStr = "";
                    mycom.CommandText = sb.ToString();
                    dr = mycom.ExecuteReader();
                    while (dr.Read())
                    {
                        clientPageStr = "";
                        drow = dt.NewRow();
                        drow[0] = dr[0].ToString();
                        drow[1] = dr[1].ToString();
                        drow[2] = dr[2].ToString();
                        drow[3] = dr[3].ToString();
                        drow[4] = dr[4].ToString();
                        sb.Clear();
                        sb.Append("select A.client_page\n");
                        sb.Append("FROM SteelMesh_ClientPage A\n");
                        sb.Append("inner join SteelMesh_ClientPagePower B on A.id=B.clintpage_id\n");
                        sb.Append("where B.user_id='" + dr[0].ToString() + "'\n");
                        mycomT.CommandText = sb.ToString();
                        drT = mycomT.ExecuteReader();
                        while (drT.Read())
                        {
                            clientPageStr = clientPageStr + drT[0].ToString() + "<br />";
                        }
                        drT.Close();
                        drT = null;
                        drow[5] = clientPageStr;
                        dt.Rows.Add(drow);
                    }
                    dr.Close();
                    dr = null;
                    ds.Tables.Add(dt);
                }
                else
                {
                    mycom.CommandText = sb.ToString();
                    sb.Clear();
                    da = new SqlDataAdapter(mycom.CommandText, mycon);
                    da.Fill(ds);
                }
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

                if (drT != null)
                {
                    drT.Close();
                    drT = null;
                }
                if (myconT.State != ConnectionState.Closed)
                    myconT.Close();
                myconT = null;
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