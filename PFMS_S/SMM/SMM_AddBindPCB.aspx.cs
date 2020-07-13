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
    public partial class SMM_AddBindPCB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string idStr = Request.QueryString["id"].ToString();
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
                sb.Clear();
                sb.Append("select id,sn\n");
                sb.Append("from SteelMesh_List\n");
                sb.Append("where id not in(select smlist_id from SteelMesh_BindPCB)\n");
                sb.Append("and now_status > -1 and enabled='1'");
                mycom.CommandText = sb.ToString();
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    smlist_id.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
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