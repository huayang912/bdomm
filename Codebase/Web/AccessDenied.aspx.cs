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
        WebUtil.ShowMessageBox(divMessage, "IIS (Internet Information Services) Is Passing Blank String as User Name. This Indicates that there is some configuration issue in Windows Authentication.", true);
    }
}
