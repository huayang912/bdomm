using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Core.Extensions;
using System.Text;

public partial class Pages_ProjectDetails : System.Web.UI.Page
{
    private int _ID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            BindProjectDetails();
        }
    }
    protected void BindPageInfo()
    {
        _ID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
        Page.Title = WebUtil.GetPageTitle("Project Details");
    }
    protected void BindProjectDetails()
    {
        OMMDataContext context = new OMMDataContext();
        Project project = context.Projects.SingleOrDefault(P => P.ID == _ID);
        if (project == null)
            ShowErrorMessage();
        else
        {
            Page.Title = WebUtil.GetPageTitle(String.Format("Project Details : {0}", project.Name.HtmlEncode()));
            ltrHeading.Text = String.Format("Project Details : {0}", project.Name.HtmlEncode());
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<b>Number:</b> {0}<br/><br/>", project.Number);
            sb.AppendFormat("<b>Quotation Information:</b> <br/>");
            if (project.Quotation != null)
            {
                if (project.Quotation.ClientContact != null)
                {
                    sb.AppendFormat("<b>Client Name:</b> {0}<br/>", project.Quotation.ClientContact.Client.Name.HtmlEncode());
                    sb.AppendFormat("<b>Contact Name: </b> {0}<br/>", project.Quotation.ClientContact.Name.HtmlEncode());
                }
                if(project.Quotation.User != null)
                    sb.AppendFormat("<b>Created By:</b> {0}<br/>", project.Quotation.User.DisplayName.HtmlEncode());
                if(project.Quotation.User1 != null)
                    sb.AppendFormat("<b>Changed By:</b> {0}<br/>", project.Quotation.User1.DisplayName.HtmlEncode());
                
                String mainEquipment = project.Quotation.MainEquipment;
                sb.AppendFormat("<br/><b>Main Equipments:</b> <br/>{0}<br/>", WebUtil.FormatText(mainEquipment.IsNullOrEmpty() ? "NA" : mainEquipment));

                
                sb.AppendFormat("<br/><span style='color:red;'><b>Subcontractor(s):</b> {0}</span><br/>", project.Quotation.Subcontractor);
            }
            sb.AppendFormat("<b>Description:</b> {0}<br/>",  WebUtil.FormatText(project.Description.IsNullOrEmpty() ? "NA" : project.Description));
            sb.AppendFormat("<b>Start Date:</b> {0}<br/>", project.StartDate == DateTime.MinValue ? "NA" : project.StartDate.GetValueOrDefault().ToString(AppConstants.ValueOf.DATE_FROMAT_DISPLAY));
            sb.AppendFormat("<b>End Date:</b> {0}<br/>", project.EndDate == DateTime.MinValue ? "NA" : project.EndDate.GetValueOrDefault().ToString(AppConstants.ValueOf.DATE_FROMAT_DISPLAY));

            if (project.Quotation != null)
            {
                sb.AppendFormat("<b>Quotation No.:</b> <a href='javascript:void(0);'>{0}</a><br/>", project.Quotation.Number);
                if(project.Quotation.Enquiry != null)
                    sb.AppendFormat("<b>Enquiry No.:</b> <a href='javascript:void(0);'>{0}</a><br/>", project.Quotation.Enquiry.Number);
                sb.AppendFormat("<br/><b>Scope of the Work:</b> <br/>{0}<br/>", project.Quotation.ScopeOfWork.IsNullOrEmpty() ? "NA" : WebUtil.FormatText(project.Quotation.ScopeOfWork));
            }

            divDetails.InnerHtml = sb.ToString();
        }
    }
    protected void ShowErrorMessage()
    {
        //pnlDetails.Visible = false;
        WebUtil.ShowMessageBox(divDetails, "Sorry! the requested project was not found.", true);
    }
}
