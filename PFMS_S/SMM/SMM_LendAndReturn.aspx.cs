using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace PFMS_S.SMM
{
    public partial class SMM_LendAndReturn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Count == 0)
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "Information", "<script>alert('登陆已超时,请刷新页面重新登陆!')</script>");
                Response.Write("<script>alert('登陆已超时,请刷新页面重新登陆!')</script>");
                return;
            }
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select id,pl_name,code from ProductLine_Management";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    productLine.Items.Add(new ListItem(dr[1].ToString() + "(" + dr[2].ToString() + ")", dr[0].ToString()));
                }
            }
            catch (Exception msg)
            {

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