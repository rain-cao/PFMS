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
    public partial class SMM_AddEditManufacturer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string idStr = Request.QueryString["id"];
            //根据传入的id设置主旨内容
            if (idStr == "0")
            {
                h2Show.InnerText = "新增厂商";
                btnAddEdit.InnerText = "添加";
            }
            else
            {
                h2Show.InnerText = "编辑厂商";
                btnAddEdit.InnerText = "编辑";
            }
            input_id.Value = idStr;

            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                //查询部门清单到下拉列表框
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select name,code,responsible,telephone,email,address,remark from SteelMesh_Manufacturer_Management where id='" + idStr + "'";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    name.Value = dr[0].ToString();
                    code.Value = dr[1].ToString();
                    responsible.Value = dr[2].ToString();
                    telephone.Value = dr[3].ToString();
                    email.Value = dr[4].ToString();
                    address.Value = dr[5].ToString();
                    remark.Value = dr[6].ToString();
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