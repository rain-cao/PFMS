using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PFMS_S
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Count == 0)
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Information", "<script>alert(\"登陆已超时,请重新登陆!\");</script>", false);
                Response.Redirect("~/Index.aspx");
            }
            else
            {
                userShow.InnerText = " " + Session["Name"] + "  欢迎使用!";
            }
        }
    }
}