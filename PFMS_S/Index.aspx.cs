using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PFMS_S
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
            {
                account.Value = Request.Cookies["UserName"].Value.ToString();
                password.Value = Request.Cookies["Password"].Value.ToString();
            }
        }
    }
}