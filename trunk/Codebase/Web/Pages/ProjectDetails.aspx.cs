using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using App.Core.Extensions;


public partial class Pages_ProjectDetails : BasePage
{
    private int _ProjectID = 0;

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
        _ProjectID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
        Page.Title = WebUtil.GetPageTitle("Project Details");
    }
    protected void BindProjectDetails()
    {
        OMMDataContext context = new OMMDataContext();
        Project project = context.Projects.SingleOrDefault(P => P.ID == _ProjectID);
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

                
                sb.AppendFormat("<br/><b>Subcontractor(s):</b> {0}<br/>", GetSubContractors(project, context));
                sb.Append(GetQuotationPricingList(project.Quotation));
            }
            sb.Append(GetPersonnelList(context));
            sb.AppendFormat("<b>Status:</b> {0}<br/>", project.StatusID.GetValueOrDefault() > 0 ? project.ProjectStatuse.Name : "NA");
            sb.AppendFormat("<b>Description:</b> {0}<br/>",  WebUtil.FormatText(project.Description.IsNullOrEmpty() ? "NA" : project.Description));
            sb.AppendFormat("<b>Start Date:</b> {0}<br/>", project.StartDate == DateTime.MinValue ? "NA" : project.StartDate.GetValueOrDefault().ToString(AppConstants.ValueOf.DATE_FROMAT_DISPLAY));
            sb.AppendFormat("<b>End Date:</b> {0}<br/>", project.EndDate == DateTime.MinValue ? "NA" : project.EndDate.GetValueOrDefault().ToString(AppConstants.ValueOf.DATE_FROMAT_DISPLAY));

            if (project.Quotation != null)
            {
                sb.AppendFormat("<b>Quotation No:</b> <a href='{0}?{1}={2}'>{3}</a><br/>", AppConstants.Pages.QUOTATION_DETAILS_Archive,
                    AppConstants.QueryString.ID, project.QuotationID + "&_controllerName=Quotations&_commandName=Select&_commandArgument=editForm1", project.Quotation.Number);
              //  sb.AppendFormat("<b>Quotation Create Date:</b> {0}<br/>", project.Quotation.CreatedOn == DateTime.MinValue ? "NA" : project.Quotation.CreatedOn.ToString(AppConstants.ValueOf.DATE_FROMAT_DISPLAY));

                if (project.Quotation.Enquiry != null)
                {
                    sb.AppendFormat("<b>Enquiry No:</b> <a href='{0}?{1}={2}'>{3}</a><br/>", AppConstants.Pages.ENQUIRY_DETAILS_Archive,
                        AppConstants.QueryString.ID, project.Quotation.EnquiryID + "&_controllerName=Enquiry&_commandName=Select&_commandArgument=editForm1", project.Quotation.Enquiry.Number);

                    sb.AppendFormat("<b>Attachments: </b><br/>{0}", GetAttachmentsList(project.Quotation.Enquiry));
                }
                sb.AppendFormat("<br/><b>Scope of the Work:</b> <br/>{0}<br/>", project.Quotation.ScopeOfWork.IsNullOrEmpty() ? "NA" : WebUtil.FormatText(project.Quotation.ScopeOfWork));
                
            }

            divDetails.InnerHtml = sb.ToString();
            ucProjectNotes.ProjectID = _ProjectID;
            ucProjectNotes.DataBind();
        }
    }

    protected String GetAttachmentsList(Enquiry enquiry)
    {
        if (enquiry.EnquiryFiles.Count > 0)
        {
            StringBuilder sb = new StringBuilder(10);
            sb.Append("<ul style='margin:0px 0px 0px 0px;'>");
            foreach (EnquiryFile file in enquiry.EnquiryFiles)
            {
                String fileNameToShow = WebUtil.GetFormattedFileName(file.FileName);
                sb.AppendFormat("<li><a href='..{0}/{1}/{2}' target='_blank'>{3}</a></li>",
                    AppConstants.ENQUIRY_ATTACHMENTS, enquiry.ID, file.FileName, fileNameToShow);
            }
            sb.Append("</ul>");
            return sb.ToString();
        }
        else
            return "NA";
    }

    private string GetSubContractors(Project project, OMMDataContext context)
    {
        //var subContractors = from P in context.
        return "NA";
    }

    protected String GetQuotationPricingList(Quotation quotation)
    {
        StringBuilder sb = new StringBuilder(10);
        sb.Append("<b>Item Details:</b> <br/>");
        if (quotation.QuotationPricingLines != null && quotation.QuotationPricingLines.Count > 0)
        {            
            sb.Append("<table class='GridView' cellpadding='3' cellspacing='0' style='width:570px;'>");
            sb.Append(" <colgroup>");
            sb.Append("   <col style='width:10%;' />");
            sb.Append("   <col/>");
            sb.Append("   <col style='width:14%;' />");
            sb.Append("   <col style='width:12%;' />");
            sb.Append("   <col style='width:10%;' />");          
            sb.Append("   <col style='width:10%;' />");
            sb.Append(" </colgroup>");

            sb.Append("<tr>");
            sb.Append("   <th style='text-align:center;'>Item</th><th>Description</th><th>Pricing Type</th><th style='text-align:right;'>Unit Price</th><th style='text-align:center;'>Quantity</th><th style='text-align:right;'>Price</th>");
            sb.Append("</tr>");
            String currencySymbol = quotation.Currency == null ? String.Empty : quotation.Currency.ShortCode.Trim();

            decimal totalPrice = 0;
            for (int i = 0; i < quotation.QuotationPricingLines.Count; i++)
            {
                QuotationPricingLine pricingLine = quotation.QuotationPricingLines[i];
                decimal price = pricingLine.UnitPrice.GetValueOrDefault() * pricingLine.Quantity.GetValueOrDefault();
                sb.AppendFormat("<tr class='{0}'>", i % 2 == 0 ? "OddRowListing" : "EvenRowListing");
                //sb.Append("<tr>");
                sb.AppendFormat("   <td style='text-align:center;'>{0}</td>", pricingLine.Item.IsNullOrEmpty() ? "NA" : pricingLine.Item.HtmlEncode());
                sb.AppendFormat("   <td>{0}</td>", pricingLine.Description.IsNullOrEmpty() ? "NA" : WebUtil.FormatText(pricingLine.Description));
                sb.AppendFormat("   <td>{0}</td>", pricingLine.QuotationPricingType == null ? "NA" : pricingLine.QuotationPricingType.Name);
                sb.AppendFormat("   <td style='text-align:right;'>{0}{1}</td>", currencySymbol, String.Format(AppConstants.ValueOf.DECIMAL_FORMAT, pricingLine.UnitPrice.GetValueOrDefault()));
                sb.AppendFormat("   <td style='text-align:center;'>{0}</td>", pricingLine.Quantity.GetValueOrDefault());
                sb.AppendFormat("   <td style='text-align:right;'>{0}{1}</td>", currencySymbol, String.Format(AppConstants.ValueOf.DECIMAL_FORMAT, price));
                //sb.AppendFormat("   <td style='text-align:center;'><a href='javascript:void(0);' onclick='LoadPricingForEdit(' + i + ')'>Edit</a></td>';
                sb.Append("</tr>");
                totalPrice += price;                
            }
            sb.Append("<tr>");
            sb.AppendFormat("   <td colspan='6' style='text-align:right;'><b>Total Price:</b> &nbsp;{0}{1}</td>", currencySymbol, String.Format(AppConstants.ValueOf.DECIMAL_FORMAT, totalPrice));
            //sb.Append("   <td></td>");
            sb.Append("</tr>");
            sb.Append("</table>");
        }
        else
            sb.Append("NA <br/>");
        return sb.ToString();
    }
    protected String GetPersonnelList(OMMDataContext context)
    {
        var personnels = from P in context.EmploymentHistories
                         where P.ProjectID == _ProjectID
                         select P;

        StringBuilder sb = new StringBuilder(10);
        sb.Append("<b>Personnel:</b> <br/>");
        if (personnels != null && personnels.Count() > 0)
        {
            sb.Append("<table class='GridView' cellpadding='3' cellspacing='0' style='width:570px;'>");
            //sb.Append(" <colgroup>");
            //sb.Append("   <col style='width:10%;' />");
            //sb.Append("   <col/>");
            //sb.Append("   <col style='width:14%;' />");
            //sb.Append("   <col style='width:12%;' />");
            //sb.Append("   <col style='width:10%;' />");
            //sb.Append("   <col style='width:10%;' />");
            //sb.Append("   <col/>");
            //sb.Append(" </colgroup>");

            sb.Append("<tr>");
            sb.Append("   <th>Last Name</th><th>First Name</th><th>Start Date</th><th>End Date</th><th>Role Name</th><th style='text-align:right;'>Day Rate</th><th style='text-align:center;'>Contract Days#</th>");
            sb.Append("</tr>");
            //String currencySymbol = quotation.Currency == null ? String.Empty : quotation.Currency.ShortCode;

            //decimal totalPrice = 0;
            int i = 0;
            foreach (EmploymentHistory personnel in personnels)
            {                
                //sb.AppendFormat("<tr class='{0}'>", i % 2 == 0 ? "OddRowStyle" : "EventRowStyle");
                sb.AppendFormat("<tr class='{0}'>", i % 2 == 0 ? "OddRowListing" : "EvenRowListing");
                //sb.Append("<tr>");
                sb.AppendFormat("   <td>{0}</td>", GetPersonnelLNLink(personnel)); //personnel.Contact.LastName.HtmlEncode());
                sb.AppendFormat("   <td>{0}</td>", GetPersonnelLink(personnel));        
                sb.AppendFormat("   <td>{0}</td>", personnel.StartDate.GetValueOrDefault() == DateTime.MinValue ? "NA" : personnel.StartDate.GetValueOrDefault().ToString(AppConstants.ValueOf.DATE_FROMAT_DISPLAY));
                sb.AppendFormat("   <td>{0}</td>", personnel.EndDate.GetValueOrDefault() == DateTime.MinValue ? "NA" : personnel.EndDate.GetValueOrDefault().ToString(AppConstants.ValueOf.DATE_FROMAT_DISPLAY));
                sb.AppendFormat("   <td>{0}</td>", personnel.Role == null ? "NA" : personnel.Role.Name.HtmlEncode());
                sb.AppendFormat("   <td style='text-align:right;'>{0}</td>", personnel.DayRate.GetValueOrDefault() > 0 ? String.Format(AppConstants.ValueOf.DECIMAL_FORMAT, personnel.DayRate.GetValueOrDefault()) : "NA");
                sb.AppendFormat("   <td style='text-align:center;'>{0}</td>", personnel.Contract_days.GetValueOrDefault() > 0 ? personnel.Contract_days.GetValueOrDefault().ToString() : "NA");
                sb.Append("</tr>");
                i++;
            }            
            sb.Append("</table>");
        }
        else
            sb.Append("NA <br/>");
         Project project = context.Projects.SingleOrDefault(P => P.ID == _ProjectID);
      
        sb.Append(" <a href='EmploymentHistory.aspx?ProjectID=" + project.ID  + "&_controller=EmploymentHistory&_commandName=New&_commandArgument=createForm1' target='_blank'>Add Personnel</a><br/></br>");
        
        return sb.ToString();
    }

    protected String GetPersonnelLink(EmploymentHistory personnel)
    {
        return String.Format("<a href='{0}?{1}={2}' target='_blank'>{3}</a>", AppConstants.Pages.PERSONNEL_DETAILS,
            AppConstants.QueryString.ID, personnel.ContactID, personnel.Contact.FirstNames.HtmlEncode());
    }


    protected String GetPersonnelLNLink(EmploymentHistory personnel)
    {
        return String.Format("<a href='{0}?{1}={2}' target='_blank'>{3}</a>", AppConstants.Pages.PERSONNEL_DETAILS,
            AppConstants.QueryString.ID, personnel.ContactID, personnel.Contact.LastName.HtmlEncode());
    }


    protected void ShowErrorMessage()
    {
        //pnlDetails.Visible = false;
        WebUtil.ShowMessageBox(divDetails, "Sorry! the requested project was not found.", true);
    }
}
