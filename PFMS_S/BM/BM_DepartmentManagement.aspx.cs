using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PFMS_S.BackstageManagement
{
    public partial class BM_DepartmentManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Count == 0)
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "Information", "<script>alert('登陆已超时,请刷新页面重新登陆!')</script>");
                Response.Write("<script>alert('登陆已超时,请刷新页面重新登陆!')</script>");
                return;
            }
            string powerStr = Request.QueryString["power"];
            inputPower.Value = powerStr;
        }
    }
}