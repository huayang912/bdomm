using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using App.Core.Extensions;


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

                sb.Append(GetQuotationPricingList(project.Quotation));
            }

            divDetails.InnerHtml = sb.ToString();
        }
    }

    protected String GetQuotationPricingList(Quotation quotation)
    {
        StringBuilder sb = new StringBuilder(10);
        sb.Append("<br/><b>Item Details:</b> <br/>");
        if (quotation.QuotationPricingLines != null && quotation.QuotationPricingLines.Count > 0)
        {            
            sb.Append("<table class='GridView' cellpadding='3' cellspacing='0' style='width:100%;'>");
            sb.Append(" <colgroup>");
            sb.Append("   <col style='width:12%;' />");
            sb.Append("   <col style='width:35%;' />");
            sb.Append("   <col style='width:15%;' />");
            sb.Append("   <col/>");
            sb.Append("   <col style='width:10%;' />");
            //sb.Append("   <col style='width:10%;' />");
            //sb.Append("   <col style='width:8%;' />");
            sb.Append("   <col />");
            sb.Append(" </colgroup>");

            sb.Append("<tr>");
            sb.Append("   <th>Item</th><th>Description</th><th>Pricing Type</th><th style='text-align:right;'>Unit Price</th><th style='text-align:center;'>Quantity</th><th style='text-align:right;'>Price</th>");
            sb.Append("</tr>");
            String currencySymbol = quotation.Currency == null ? String.Empty : quotation.Currency.ShortCode;

            decimal totalPrice = 0;
            for (int i = 0; i < quotation.QuotationPricingLines.Count; i++)
            {
                QuotationPricingLine pricingLine = quotation.QuotationPricingLines[i];
                decimal price = pricingLine.UnitPrice.GetValueOrDefault() * pricingLine.Quantity.GetValueOrDefault();
                sb.AppendFormat("<tr class='{0}'>", i % 2 == 0 ? "OddRowListing" : "EvenRowListing");
                sb.AppendFormat("   <td>{0}</td>", pricingLine.Item.IsNullOrEmpty() ? "NA" : pricingLine.Item.HtmlEncode());
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
            sb.Append("NA");
        return sb.ToString();
    }

    protected void ShowErrorMessage()
    {
        //pnlDetails.Visible = false;
        WebUtil.ShowMessageBox(divDetails, "Sorry! the requested project was not found.", true);
    }
}
