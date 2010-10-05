using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using App.Core.Extensions;

public partial class Pages_QuotationDecision : BasePage
{
    protected int _QuotationID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            BindDropdownList();
            BindQuotationsInfo();
        }
    }
    /// <summary>
    /// Bindis the Page Initialization Variables
    /// </summary>
    protected void BindPageInfo()
    {
        _QuotationID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);        

        if (_QuotationID == 0)
        {            
            //ltrHeading.Text = "Edit Quotations Wizard";
            //ShowErroMessag("Requested Quotation ");
        }
        Page.Title = WebUtil.GetPageTitle(ltrHeading.Text);
    }
    
    /// <summary>
    /// Binds Dropdown Lists for this page
    /// </summary>
    protected void BindDropdownList()
    {
        OMMDataContext context = new OMMDataContext();
        ///Bind the Contacs DropdownList
        var contacts = from P in context.ClientContacts
                       orderby P.Client.Name, P.Name
                       select new
                       {
                           P.ID,
                           Name = String.Format("{0} ({1})", P.Name, P.Client.Name)
                       };
        ddlContact.DataSource = contacts;
        ddlContact.DataTextField = "Name";
        ddlContact.DataValueField = "ID";        
        ddlContact.DataBind();
        ddlContact.Items.Insert(0, new ListItem(String.Empty, String.Empty));
    }

    /// <summary>
    /// Binds Quotations Info Requested through Query Strings
    /// </summary>
    protected void BindQuotationsInfo()
    {
        OMMDataContext dataContext = new OMMDataContext();
        ///Bind Quotation First
        Quotation quotation = dataContext.Quotations.SingleOrDefault(E => E.ID == _QuotationID);
        if (quotation == null)
        {
            ShowErroMessag("Q", String.Empty);
            return;
        }
        else
        {
            ltrHeading.Text = String.Format("Quotation Submit Wizard - Quotation {0} Enquiry {1}", quotation.Number, quotation.Enquiry.Number);
            if (quotation.StatusID != App.CustomModels.QuotationStatus.NotSubmitted)
            {
                ShowErroMessag("V", quotation.Number);
                return;
            }
            else
                BindClientContactInfo(quotation.Enquiry);
        }
        Page.Title = ltrHeading.Text;       
    }
    protected void BindClientContactInfo(Enquiry enquiry)
    {
        if (enquiry != null)
        {
            ddlContact.SetSelectedItem(enquiry.ContactID.ToString());
            txtClientName.Text = enquiry.ClientContact.Client.Name;
            txtContactName.Text = enquiry.ClientContact.Name;
            txtJobTitle.Text = enquiry.ClientContact.JobTitle;
            if(enquiry.ClientContact.CountryID.GetValueOrDefault() > 0 ) 
                txtCountry.Text = enquiry.ClientContact.Country.Name;
        }
    }
    /// <summary>
    /// Shows a Message in the UI and Hides the Data Editing Controls
    /// </summary>
    protected void ShowErroMessag(String msgType, String quotationNumber)
    {
        pnlDetails.Visible = false;        
        if (msgType == "Q")
            WebUtil.ShowMessageBox(divMessage, "Sorry! requested Quotation was not found.", true);
        else if (msgType == "V")
            WebUtil.ShowMessageBox(divMessage, String.Format("Sorry! Only <b>Not Submit</b> Quotations can be Submitted."), true);
        else if (msgType == "VC")
            WebUtil.ShowMessageBox(divMessage, String.Format("Sorry! Enquiry {0} has been closed .", quotationNumber), true);
    }

    /// <summary>
    /// Changes the Status of a Quotation
    /// </summary>
    /// <param name="quotationID"></param>
    /// <param name="decision"></param>
    /// <returns></returns>
    //[WebMethod]
    //public static bool SaveDecision(int quotationID, int decision)
    //{
    //    OMMDataContext dataContext = new OMMDataContext();
    //    var quotation = dataContext.Quotations.SingleOrDefault(Q => Q.ID == quotationID);
    //    if (quotation != null)
    //    {
    //        quotation.StatusID = decision;
    //        quotation.ChangedByUserID = SessionCache.CurrentUser.ID;
    //        quotation.ChangedByUsername = SessionCache.CurrentUser.UserNameWeb;
    //        quotation.ChangedOn = DateTime.Now;
    //        //if (decision == App.CustomModels.QuotationStatus.Successful || decision == App.CustomModels.QuotationStatus.Unsuccessful)
    //        //    quotation.Enquiry.StatusID = App.CustomModels.EnquiryStatus.Closed;
    //        //else if (decision == App.CustomModels.QuotationStatus.ReQquoteRequested)
    //        //{
    //        //    //Create a New Quotation for this Enquiry with this objects data

    //        //}
    //        dataContext.SubmitChanges();
    //        return true;
    //    }
    //    return false;
    //}
}
