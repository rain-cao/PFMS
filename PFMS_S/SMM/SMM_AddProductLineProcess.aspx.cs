using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace PFMS_S.SMM
{
    public partial class SMM_AddProductLineProcess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
                    productline_id.Items.Add(new ListItem(dr[2].ToString() + "(" + dr[1].ToString() + ")", dr[0].ToString()));
                }
                dr.Close();
                dr = null;
                mycom.CommandText = "select status_id,status_name from Fixture_ProcessStatus_List where status_id>10";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    status_id.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                }
            }
            catch (Exception msg)
            {
                Response.Write(msg.Message);
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