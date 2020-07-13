using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PFMS_S.BM
{
    public partial class BM_EditMenuPowerForUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string user_idStr = Request.QueryString["user_id"].ToString();
            inputUserID.Value = user_idStr;
        }
    }
}