using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace PFMS_S.BM
{
    public partial class BM_UpdateMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string thisMenuIDStr = Request.QueryString["thisMenu"].ToString();
            thisMenuID.Value = thisMenuIDStr;
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select menu_name,path,menu_url,menu_type,icon,remark,init_power from Menu_Manage where menu_id='" + thisMenuIDStr + "'";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    menu_name.Value = dr[0].ToString();
                    path.Value = dr[1].ToString();
                    menu_url.Value = dr[2].ToString();
                    menu_type.Value = dr[3].ToString();
                    icon.Value = dr[4].ToString();
                    remark.Value = dr[5].ToString();
                    string powerDes = "";
                    switch (dr[6].ToString())
                    {
                        case "0":
                            powerDes = "不可显";
                            break;
                        case "1":
                            powerDes = "只读";
                            break;
                        case "2":
                            powerDes = "读写";
                            break;
                    }
                    init_power.SelectedIndex = init_power.Items.IndexOf(new ListItem(powerDes, dr[6].ToString()));
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