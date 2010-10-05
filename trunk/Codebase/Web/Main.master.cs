using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



public partial class Main : System.Web.UI.MasterPage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        string pageCssClass = (Page.GetType().Name + " Loading");
        PropertyInfo p = Page.GetType().GetProperty("CssClass");
        if (null != p)
        {
            string cssClassName = ((string)(p.GetValue(Page, null)));
            if (!(String.IsNullOrEmpty(pageCssClass)))
            	pageCssClass = (pageCssClass + " ");
            pageCssClass = (pageCssClass + cssClassName);
        }
        if (!(pageCssClass.Contains("Wide")))
        	pageCssClass = (pageCssClass + " Standard");
        LiteralControl c = ((LiteralControl)(Page.Form.Controls[0]));
        if ((null != c) && !(String.IsNullOrEmpty(pageCssClass)))
        	c.Text = Regex.Replace(c.Text, "<div>", String.Format("<div class=\"{0}\">", pageCssClass), RegexOptions.Compiled);
    }

    protected void BindPageInfo()
    {
        if (SessionCache.CurrentUser == null)
        {
            hplLogin.Text = "Log In";
            hplLogin.NavigateUrl = AppConstants.Pages.LOG_IN;
            ltrUserName.Text = String.Empty;            
        }
        else
        {
            hplLogin.NavigateUrl = String.Format("{0}?{1}=True", AppConstants.Pages.LOG_IN, AppConstants.QueryString.LOG_OUT);
            hplLogin.Text = "Log Out";
            ltrUserName.Text = String.Format("Welcome {0} | ", SessionCache.CurrentUser.UserNameWeb);
        }
    }
}
