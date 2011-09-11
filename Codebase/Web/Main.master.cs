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
    public String _SelectedTabID = String.Empty;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        //string pageCssClass = (Page.GetType().Name + " Loading");
        //PropertyInfo p = Page.GetType().GetProperty("CssClass");
        //if (null != p)
        //{
        //    string cssClassName = ((string)(p.GetValue(Page, null)));
        //    if (!(String.IsNullOrEmpty(pageCssClass)))
        //        pageCssClass = (pageCssClass + " ");
        //    pageCssClass = (pageCssClass + cssClassName);
        //}
        //if (!(pageCssClass.Contains("Wide")))
        //    pageCssClass = (pageCssClass + " Standard");
        //LiteralControl c = ((LiteralControl)(Page.Form.Controls[0]));
        //if ((null != c) && !(String.IsNullOrEmpty(pageCssClass)))
        //    c.Text = Regex.Replace(c.Text, "<div>", String.Format("<div class=\"{0}\">", pageCssClass), RegexOptions.Compiled);
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
            ltrUserName.Text = String.Format("Welcome {0} | ", SessionCache.CurrentUser.UserName);//SessionCache.CurrentUser.UserNameWeb);
        }
        //if (IsPostBack)
        //    Menu1.DataBind();
        _SelectedTabID = GetSelectedTabID();
        Page.DataBind();
    }
    public String GetSelectedTabID()
    {
        if (this.SelectedTab == global::SelectedTab.Client)
            return "1:divRibbonTabItemsClient";        
        else if (this.SelectedTab == global::SelectedTab.Project)
            return "2:divRibbonTabItemsProject";
        else if (this.SelectedTab == global::SelectedTab.Personnel)
            return "3:divRibbonTabItemsPersonnel";
        else if (this.SelectedTab == global::SelectedTab.Option)
            return "4:divRibbonTabItemsOption";
        else if (this.SelectedTab == global::SelectedTab.Report)
            return "5:divRibbonTabItemsReport";
        else
            return "0:divRibbonTabItemsHome";
    }
    public SelectedTab SelectedTab { get; set; }
}

public enum SelectedTab
{
    Home,
    Client,
    Personnel,
    Project,
    Option,
    Report
}
