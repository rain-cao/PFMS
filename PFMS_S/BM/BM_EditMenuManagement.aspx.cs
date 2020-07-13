using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PFMS_S.BM
{
    public partial class BM_EditMenuManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string useridStr = Request.QueryString["user_id"].ToString();
            userid.Value = useridStr;
        }
    }
}