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
    public partial class SMM_ProcessSet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Count == 0)
            {
                Response.Write("<script>alert('登陆已超时,请刷新页面重新登陆!')</script>");
                return;
            }
            string powerStr = Request.QueryString["power"];
            inputPower.Value = powerStr;
        }

        public string outhtml()
        {
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            List<string> productLine = new List<string>();
            productLine.Clear();
            string htmlStr = "";
            StringBuilder sb = new StringBuilder();
            StringBuilder sbHTML = new StringBuilder();
            sbHTML.Clear();
            sb.Clear();
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                sb.Append("SELECT productline_id\n");
                sb.Append("from SteelMesh_OnLine_Process\n");
                sb.Append("group by productline_id");
                mycom.CommandText = sb.ToString();
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    productLine.Add(dr[0].ToString());
                }
                dr.Close();
                dr = null;
                for (int i = 0; i < productLine.Count; i++)
                {
                    sb.Clear();
                    sb.Append("select pl_name,code,\n");
                    sb.Append("(select ws_name from Workshop_Manage where id=ProductLine_Management.workshop_id) as workshop\n");
                    sb.Append("from ProductLine_Management where id='" + productLine[i].ToString() + "'");
                    mycom.CommandText = sb.ToString();
                    dr = mycom.ExecuteReader();
                    dr.Read();
                    sbHTML.Append("<br />\n");
                    sbHTML.Append("<div style=\"font-weight:bolder;color:grey;\">" + dr[2].ToString() + "---" + dr[0].ToString() + "(" + dr[1].ToString() + ")" + "</div>\n");
                    sbHTML.Append("<br />\n");
                    dr.Close();
                    dr = null;
                    sb.Clear();
                    sb.Append("select (select status_name from Fixture_ProcessStatus_List where status_id=SteelMesh_OnLine_Process.status_id) as process\n");
                    sb.Append("from SteelMesh_OnLine_Process where productline_id='" + productLine[i].ToString() + "'\n");
                    sb.Append("order by order_number asc");
                    mycom.CommandText = sb.ToString();
                    dr = mycom.ExecuteReader();
                    string strTmp = "";
                    while (dr.Read())
                    {
                        strTmp = strTmp + dr[0].ToString() + "->";
                    }
                    if (strTmp != "")
                        strTmp = strTmp.Substring(0, strTmp.Length - 2);
                    sbHTML.Append("<div><span>线上流程：</span><a href=\"javascript:;\" style=\"cursor:pointer;\" onclick=\"ShowdetailProcess('" + productLine[i].ToString() + "')\">" + strTmp + "</a></div>\n");
                    sbHTML.Append("<hr />\n");
                    dr.Close();
                    dr = null;
                }
                htmlStr = sbHTML.ToString();
            }
            catch (Exception msg)
            {
                htmlStr = "<table style=\"margin:0 auto;border:none;width:100%;text-align:center;\"><tr><td style=\"text-align:center;border:none;\">" + msg.Message + "</td></tr></table>";
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
            return htmlStr;
        }
    }
}