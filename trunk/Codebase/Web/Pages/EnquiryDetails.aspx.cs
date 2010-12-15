using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Services;
using App.CustomModels;

public partial class Pages_EnquiryDetails : BasePage
{
    protected int _EnquiryID = 0;
    protected bool _HasClosed = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            BindEnquiryDetails();
        }
    }
    protected void BindPageInfo()
    {
        Page.Title = WebUtil.GetPageTitle("Close Enquiry");
        _EnquiryID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
    }
    protected void BindEnquiryDetails()
    {
        OMMDataContext context = new OMMDataContext();
        Enquiry enquiry = context.Enquiries.SingleOrDefault(E => E.ID == _EnquiryID);
        if (enquiry == null)
        {
            pnlDetails.Visible = false;
            WebUtil.ShowMessageBox(divMessage, "Sorry! the requested Enquiry was not found.", true);
        }
        else
        {
            ltrHeading.Text = String.Format("Close Enquiry: Enquiry {0}", enquiry.Number);
            ltrEnquiryDetails.Text = GetEnqueryDetailsHtml(enquiry);
            if (enquiry.StatusID == EnquiryStatus.Closed)
                _HasClosed = true;
        }
    }

    protected String GetEnqueryDetailsHtml(Enquiry enquiry)
    {
        StringBuilder sb = new StringBuilder(10);
        sb.AppendFormat("<b>Client: </b> {0}<br/>", enquiry.ClientContact.Client.Name);
        sb.AppendFormat("<b>Contact: </b> {0}<br/>", enquiry.ClientContact.Name);
        sb.AppendFormat("<b>Quotations: </b> {0}<br/>", enquiry.Quotations.Count);
        sb.AppendFormat("<b>Attachments: </b>{0}</br>", GetAttachmentsList(enquiry));
        return sb.ToString();
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
    [WebMethod]
    public static bool CloseEnquery(int enqueryID)
    {
        if (enqueryID > 0)
        {
            OMMDataContext dataContext = new OMMDataContext();
            Enquiry enquiry = dataContext.Enquiries.SingleOrDefault(P => P.ID == enqueryID);
            if (enquiry != null)
            {
                enquiry.StatusID = EnquiryStatus.Closed;
                enquiry.ChangedByUsername = SessionCache.CurrentUser.UserNameWeb;
                enquiry.ChangedByUserID = SessionCache.CurrentUser.ID;
                enquiry.ChangedOn = DateTime.Now;
                dataContext.SubmitChanges();
                return true;
            }
        }
        return false;        
    }
}
