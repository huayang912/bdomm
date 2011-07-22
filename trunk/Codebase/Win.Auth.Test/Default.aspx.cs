using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //User = HttpContext.Current.User;
        //String userName = User.Identity.Name;
        lblMessage.Text = String.Format("The User Name Is: {0}", User.Identity.Name);
    }
}
