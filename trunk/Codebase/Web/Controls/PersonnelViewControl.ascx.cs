using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BUDI2_NS.Data;
using BUDI2_NS.Web;


public partial class Controls_PersonnelViewControl : System.Web.UI.UserControl
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindStartsWith();
        }
    }

    protected void BindStartsWith()
    {
        List<App.CustomModels.StartsWith> chars = new List<App.CustomModels.StartsWith>(); 
        chars = App.CustomModels.StartsWith.GetStartsWith();
        rptStartsWith.DataSource = chars;
        rptStartsWith.DataBind();
    }
    protected void SearchPersonnel(String startsWith)
    {
        List<FieldFilter> filters = new List<FieldFilter>();
        filters.Add(new FieldFilter("LastName", RowFilterOperation.Like, String.Format("{0}%", startsWith)));
        view1Extender.AssignFilter(filters);
        //view1Extender.DataBind();
    }
    protected void rptStartsWith_Command(object sender, RepeaterCommandEventArgs e)
    {
        SearchPersonnel((String)e.CommandArgument);
    }
    protected void rptStartsWith_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        App.CustomModels.StartsWith start = new App.CustomModels.StartsWith();
        start = e.Item.DataItem as App.CustomModels.StartsWith;
        
        LinkButton lkbCommand = e.Item.FindControl("lkbCommand") as LinkButton;
        lkbCommand.CommandArgument = start.Start;
        lkbCommand.Text = start.Start;
    }
}
