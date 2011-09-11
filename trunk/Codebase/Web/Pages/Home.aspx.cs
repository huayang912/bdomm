using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



public partial class Pages_Home : BasePage
{
    
    public string CssClass
    {
        get
        {
            return "HomePage Wide";
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = WebUtil.GetPageTitle("Dashboard");
    }
}
