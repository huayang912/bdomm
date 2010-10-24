using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Core.Extensions;

public partial class Pages_PersonnelDetails : BasePage
{
    private int _PersonnelID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            BindPersonnelDetails();
        }
    }
    protected void BindPageInfo()
    {
        _PersonnelID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
    }
    protected void BindPersonnelDetails()
    {
        OMMDataContext context = new OMMDataContext();
        EmploymentHistory personnel = context.EmploymentHistories.SingleOrDefault(P => P.ID == _PersonnelID);
        if (personnel != null)
        {
            ltrHeading.Text = String.Format("Personnel Details. First Name: {0} Last Name: {1}", personnel.Contact.FirstNames.HtmlEncode(), personnel.Contact.LastName.HtmlEncode());
            Page.Title = WebUtil.GetPageTitle(ltrHeading.Text);
        }
    }
}
