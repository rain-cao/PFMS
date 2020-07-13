using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace PFMS_S
{
    public partial class BM_AddDepartment : System.Web.UI.Page
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
                mycom.CommandText = "select user_id,name,username from User_Manage where user_state='1'";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    mgr_1.Items.Add(new ListItem(dr[2].ToString() + "(" + dr[1].ToString() + ")", dr[0].ToString()));
                    mgr_2.Items.Add(new ListItem(dr[2].ToString() + "(" + dr[1].ToString() + ")", dr[0].ToString()));
                    mgr_3.Items.Add(new ListItem(dr[2].ToString() + "(" + dr[1].ToString() + ")", dr[0].ToString()));
                }
            }
            catch (Exception msg)
            {
                mgr_1.Items.Add(new ListItem(msg.Message, ""));
                mgr_2.Items.Add(new ListItem(msg.Message, ""));
                mgr_3.Items.Add(new ListItem(msg.Message, ""));
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