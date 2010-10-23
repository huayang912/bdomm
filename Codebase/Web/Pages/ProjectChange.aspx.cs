using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_ProjectChange : BasePage
{
    protected int _QuotationID = 0;
    protected int _ProjectID = 0;
    protected bool _IsEditMode = false;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        object user = SessionCache.CurrentUser;
        BindPageInfo();
        if (!IsPostBack)
        {
            BindProjectInfo();
        }
    }

    protected void BindPageInfo()
    {
        _QuotationID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.QUOTATION_ID);
        _ProjectID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
        if (_QuotationID > 0 && _ProjectID > 0)
            _IsEditMode = true;
        
        Page.Title = ltrHeading.Text;
    }
    protected void BindProjectInfo()
    {
        OMMDataContext context = new OMMDataContext();
        Quotation quotation = context.Quotations.SingleOrDefault(Q => Q.ID == _QuotationID);
        if (quotation == null)
        {
            WebUtil.ShowMessageBox(divMessage, "Sorry! Requested Quotation was not found.", true);
            pnlDetails.Visible = false;
        }
        else if (quotation.StatusID != App.CustomModels.QuotationStatus.Successful)
        {
            WebUtil.ShowMessageBox(divMessage, "Sorry! A Project can be created only for a Successful Quotation.", true);
            pnlDetails.Visible = false;
        }
        else
        {
            ltrHeading.Text = String.Format("Create Project Wizard : Quotation {0}", quotation.Number);
            if (_IsEditMode)
            {                
                Project project = context.Projects.SingleOrDefault(P => P.ID == _ProjectID);
                if (project == null)
                {
                    WebUtil.ShowMessageBox(divMessage, "Sorry! Requested Project was not found.", true);
                    pnlDetails.Visible = false;
                }
                else
                {
                    ltrHeading.Text = String.Format("Edit Project Wizard : Quotation {0} Project {1}", quotation.Number, project.Number);
                    txtName.Text = project.Name;
                    txtDescription.Text = project.Description;
                    txtStartDate.Text = project.StartDate.GetValueOrDefault() == DateTime.MinValue ? String.Empty : project.StartDate.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat);
                    txtEndDate.Text = project.EndDate.GetValueOrDefault() == DateTime.MinValue ? String.Empty : project.EndDate.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat);
                }
            }
        }
        ///Bind Default Value for Start Date and End Date
        if (quotation != null && !_IsEditMode)
        {
            txtStartDate.Text = DateTime.Now.ToString(ConfigReader.CSharpCalendarDateFormat);
            txtEndDate.Text = DateTime.Now.AddDays(quotation.ValidityDays).ToString(ConfigReader.CSharpCalendarDateFormat); 
        }
        Page.Title = WebUtil.GetPageTitle(ltrHeading.Text);
    }
}
