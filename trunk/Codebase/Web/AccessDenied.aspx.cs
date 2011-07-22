using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AccessDenied : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WebUtil.ShowMessageBox(divMessage, "You are not authorized to access OMM. Please contact with the Administrator to include your User Information in the OMM System.", true);
    }
}
