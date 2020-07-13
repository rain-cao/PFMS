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
    public partial class SMM_AddEditVerifyForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string idStr = Request.QueryString["id"];
            //根据传入的id设置主旨内容
            if (idStr == "0")
            {
                h2Show.InnerText = "新增验收表单信息";
                btnAddEdit.InnerText = "添加";
            }
            else
            {
                h2Show.InnerText = "编辑验收表单信息";
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
                mycon.Open();
                mycom = mycon.CreateCommand();
                sb.Append("select verify_info,verify_type,\n");
                sb.Append("case verify_type\n");
                sb.Append("when '0' then '复选框'\n");
                sb.Append("when '1' then '文本输入框'\n");
                sb.Append("when '2' then '下拉列表框'\n");
                sb.Append("end as type_des,\n");
                sb.Append("(select value+';' from SteelMesh_VerifyForm_ValueForSelect where smverifyinfolist_id='" + idStr + "' for xml path('')) as select_value,\n");
                sb.Append("check_type,\n");
                sb.Append("case check_type\n");
                sb.Append("when '0' then '无需校对'\n");
                sb.Append("when '1' then '数值校对'\n");
                sb.Append("when '2' then '文本校对'\n");
                sb.Append("else '无需校对'\n");
                sb.Append("end as check_des,standard_value,required,\n");
                sb.Append("case required\n");
                sb.Append("when '0' then '否'\n");
                sb.Append("when '1' then '是'\n");
                sb.Append("end as required_des,remark\n");
                sb.Append("from SteelMesh_VerifyForm_Info_List where id='" + idStr + "'");
                mycom.CommandText = sb.ToString();
                dr = mycom.ExecuteReader();
                sb.Clear();
                while (dr.Read())
                {
                    verify_info.Value = dr[0].ToString();
                    verify_type.SelectedIndex = verify_type.Items.IndexOf(new ListItem(dr[2].ToString(), dr[1].ToString()));
                    if (dr[1].ToString() == "2")
                    {
                        trSelectValue.Style["display"] = "";
                        select_value.Value = dr[3].ToString();
                    }
                    check_type.SelectedIndex = check_type.Items.IndexOf(new ListItem(dr[5].ToString(), dr[4].ToString()));
                    if (dr[0].ToString() == "钢网张力测试A点" || dr[0].ToString() == "钢网张力测试B点"
                       || dr[0].ToString() == "钢网张力测试C点" || dr[0].ToString() == "钢网张力测试D点"
                       || dr[0].ToString() == "钢网张力测试E点" || dr[0].ToString() == "钢网张力测试F点"
                       || dr[0].ToString() == "钢网张力测试G点" || dr[0].ToString() == "钢网张力测试H点"
                       || dr[0].ToString() == "钢网张力测试I点" || dr[0].ToString() == "钢网张力测试J点")
                    {
                        verify_info.Attributes["readonly"] = "readonly";
                        verify_info.Attributes["title"] = "钢网张力测试点为保留项，不可编辑，您可以在验收表单模块将其开启或关闭,但若删除请谨慎!";
                        verify_type.Attributes["disabled"] = "disabled";
                        check_type.Attributes["disabled"] = "disabled";
                    }
                    standard_value.Value = dr[6].ToString();
                    required.SelectedIndex = required.Items.IndexOf(new ListItem(dr[8].ToString(), dr[7].ToString()));
                    remark.Value = dr[9].ToString();
                }
                dr.Close();
                dr = null;
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