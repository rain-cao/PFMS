using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace PFMS_S
{
    public partial class BM_AddProductLine : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            bool userFlag = false;
            bool ProductLineFlag = false;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select user_id,name,username from User_Manage where user_state='1'";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    ownerid.Items.Add(new ListItem(dr[2].ToString() + "(" + dr[1].ToString() + ")", dr[0].ToString()));
                }
                dr.Close();
                dr = null;
                userFlag = true;
                mycom.CommandText = "select id,ws_name from Workshop_Manage";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    workshop_id.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                }
                ProductLineFlag = true;
            }
            catch (Exception msg)
            {
                if (userFlag == false)
                    ownerid.Items.Add(new ListItem(msg.Message, ""));
                if (ProductLineFlag == false)
                    workshop_id.Items.Add(new ListItem(msg.Message, ""));
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