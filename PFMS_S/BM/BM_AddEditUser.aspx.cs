using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;

namespace PFMS_S
{
    public partial class BM_AddEditUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string idStr = Request.QueryString["user_id"];
            //根据传入的id设置主旨内容
            if (idStr == "0")
            {
                h2Show.InnerText = "新增用户";
                btnAddEdit.InnerText = "添加";
            }
            else
            {
                h2Show.InnerText = "编辑用户";
                btnAddEdit.InnerText = "编辑";
            }
            input_id.Value = idStr;

            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            StringBuilder sb = new StringBuilder();
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                //查询部门清单到下拉列表框
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select id,dept,dept_code from Department_Manage";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    dept.Items.Add(new ListItem(dr[2].ToString() + "(" + dr[1].ToString() + ")", dr[0].ToString()));
                }
                dr.Close();
                //查询职务清单到下拉列表框
                mycom.CommandText = "select id,post from Job_Manage";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    post.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                }
                dr.Close();
                //根据传入的id初始化对应的用户信息
                sb.Clear();
                sb.Append("select\n");
                sb.Append("A.user_id,A.name,A.username,A.opid,A.password,B.id as dept_id,B.dept as dept,B.dept_code as dept_code,C.id as post_id,C.post as post,A.telephone,A.email,A.weixin_no\n");
                sb.Append("from User_Manage A\n");
                sb.Append("left join Department_Manage B on A.dept=B.id\n");
                sb.Append("left join Job_Manage C on A.post=C.id\n");
                sb.Append("where A.user_id='" + idStr + "'");
                mycom.CommandText = sb.ToString();
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    name.Value = dr[1].ToString();
                    username.Value = dr[2].ToString();
                    opid.Value = dr[3].ToString();
                    password.Value = dr[4].ToString();
                    rpassword.Value = dr[4].ToString();
                    dept.SelectedIndex = dept.Items.IndexOf(new ListItem(dr[7].ToString() + "(" + dr[6].ToString() + ")", dr[5].ToString()));
                    post.SelectedIndex = post.Items.IndexOf(new ListItem(dr[9].ToString(), dr[8].ToString()));
                    telephone.Value = dr[10].ToString();
                    email.Value = dr[11].ToString();
                    weixin_no.Value = dr[12].ToString();
                }
            }
            catch (Exception msg)
            {
                System.Console.WriteLine(msg.Message);
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr = null;
                }
                if (mycon.State != System.Data.ConnectionState.Closed)
                    mycon.Close();
                mycon = null;
            }
        }
    }
}