using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Data;
using App.Core.Extensions;

public partial class Pages_PersonnelChange : BasePage
{
    protected int _ID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            CheckPersonnelInfo();
        }
    }
    protected void BindPageInfo()
    {
        _ID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
        if (_ID > 0)
            ltrHeading.Text = "Edit Personnel";

        Page.Title = WebUtil.GetPageTitle(ltrHeading.Text);        
    }
    protected void CheckPersonnelInfo()
    {
        if (_ID > 0)
        {
            OMMDataContext context = new OMMDataContext();
            var personnel = context.Contacts.FirstOrDefault(P => P.ID == _ID);
            if (personnel == null)
            {
                WebUtil.ShowMessageBox(divMessage, "Sorrey! requested personnel was not found.", true);
                pnlFormContainer.Visible = false ;
            }
        }
    }
}
