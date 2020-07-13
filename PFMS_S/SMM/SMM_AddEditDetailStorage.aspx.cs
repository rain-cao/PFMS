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
    public partial class SMM_AddEditDetailStorage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string idStr = Request.QueryString["id"];
            //根据传入的id设置主旨内容
            if (idStr == "0")
            {
                h2Show.InnerText = "新增详细储位";
                btnAddEdit.InnerText = "添加";
            }
            else
            {
                h2Show.InnerText = "编辑详细储位";
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
                mycom.CommandText = "select id,storage_name from SteelMesh_Storage_Management where status='1'";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    storage_id.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                }
                dr.Close();
                //根据传入的id初始化对应的用户信息
                sb.Clear();
                sb.Append("select\n");
                sb.Append("A.id,B.id,B.storage_name,A.storage_name\n");
                sb.Append("from SteelMesh_Detail_Storage A\n");
                sb.Append("inner join SteelMesh_Storage_Management B on A.storage_id=B.id\n");
                sb.Append("where A.id='" + idStr + "'");
                mycom.CommandText = sb.ToString();
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    storage_name.Value = dr[3].ToString();
                    storage_id.SelectedIndex = storage_id.Items.IndexOf(new ListItem(dr[2].ToString(), dr[1].ToString()));
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