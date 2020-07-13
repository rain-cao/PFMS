using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace PFMS_S
{
    public partial class UserRegister : System.Web.UI.Page
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
                mycom.CommandText = "select id,dept,dept_code from Department_Manage";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    dept.Items.Add(new ListItem(dr[2].ToString() + "(" + dr[1].ToString() + ")", dr[0].ToString()));
                }
                dr.Close();
                mycom.CommandText = "select id,post from Job_Manage";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    post.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                }
            }
            catch(Exception msg)
            {
                System.Console.WriteLine(msg.Message);
            }
            finally
            {
                if (dr != null)
                    dr.Close();
                if (mycon.State != System.Data.ConnectionState.Closed)
                    mycon.Close();
                mycon = null;
            }
        }
    }
}