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
    public partial class SMM_DetailProductLineProcess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string powerStr = Request.QueryString["power"].ToString();
            string PLidStr = Request.QueryString["PLid"].ToString();
            inputPower.Value = powerStr;
            PLid.Value = PLidStr;
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            StringBuilder sb = new StringBuilder();
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                sb.Clear();
                sb.Append("select pl_name,code,\n");
                sb.Append("(select ws_name from Workshop_Manage where id=ProductLine_Management.workshop_id) as workshop\n");
                sb.Append("from ProductLine_Management where id='" + PLidStr + "'");
                mycom.CommandText = sb.ToString();
                dr = mycom.ExecuteReader();
                dr.Read();
                h2.InnerText = dr[2].ToString() + "---" + dr[0].ToString() + "(" + dr[1].ToString() + ")流程明细";
            }
            catch (Exception msg)
            {
                h2.InnerText = "标题获取失败:" + msg.Message;
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