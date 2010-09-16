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
}
