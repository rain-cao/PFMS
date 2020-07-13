using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;

namespace PFMS_S.SMM
{
    public partial class SMM_AddEditEquipment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string idStr = Request.QueryString["id"];
            //根据传入的id设置主旨内容
            if (idStr == "0")
            {
                h2Show.InnerText = "新增清洗设备";
                btnAddEdit.InnerText = "添加";
            }
            else
            {
                h2Show.InnerText = "编辑清洗设备";
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
                mycom.CommandText = "select Equipment_name,ip_address,clean_length from SteelMesh_CleanEquipment where id='" + idStr + "'";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    Equipment_name.Value = dr[0].ToString();
                    ip_address.Value = dr[1].ToString();
                    clean_length.Value = dr[2].ToString();
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