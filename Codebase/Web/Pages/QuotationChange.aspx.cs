using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using App.Core.Extensions;

public partial class Pages_QuotationChange : BasePage
{    
    private bool _IsEditMode = false;
    protected int _ID = 0;
    protected int _EnquiryID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            BindDropDownLists();
            BindQuotationsInfo();
        }
    }
    /// <summary>
    /// Bindis the Page Initialization Variables
    /// </summary>
    protected void BindPageInfo()
    {
        _ID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
        _EnquiryID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ENQUIRY_ID);

        if (_EnquiryID > 0 && _ID > 0)
        {
            _IsEditMode = true;
            ltrHeading.Text = "Edit Quotations Wizard";
        }
        Page.Title = WebUtil.GetPageTitle(ltrHeading.Text);
    }

    /// <summary>
    /// Binds Dropdownlists for the initial request.
    /// </summary>
    protected void BindDropDownLists()
    {
        OMMDataContext dataContext = new OMMDataContext();

        ///TODO: Where In example in LINQ
        //int[] ids = {1,2,3};        
        //IList<Message_Recipient> recipients = (from R in dataContext.Message_Recipients
        //                                       where (from I in ids select I).Contains(R.ID)
        //                                       select R).ToList();

        IList<Currency> currencies = (from C in dataContext.Currencies select C).ToList();
        ddlCurrency.DataSource = currencies;
        ddlCurrency.DataValueField = "ShortCode";
        ddlCurrency.DataTextField = "Description";
        ddlCurrency.DataBind();

        IList<QuotationPricingType> pricingTypes = (from C in dataContext.QuotationPricingTypes select C).ToList();
        ddlPricingTypeID.DataSource = pricingTypes;
        ddlPricingTypeID.DataValueField = "ID";
        ddlPricingTypeID.DataTextField = "Name";
        ddlPricingTypeID.DataBind();
    }
    /// <summary>
    /// Binds Quotations Info Requested through Query Strings
    /// </summary>
    protected void BindQuotationsInfo()
    {        
        OMMDataContext dataContext = new OMMDataContext();
        Enquiry enquiry = dataContext.Enquiries.SingleOrDefault(E => E.ID == _EnquiryID);
        if (enquiry != null)
            ltrHeading.Text = String.Format("Create New Quotation Wizard - Enquiry {0}", enquiry.Number);

        Page.Title = ltrHeading.Text;

        if (_IsEditMode)
        {            
            Quotation quotation = dataContext.Quotations.SingleOrDefault(Q => Q.EnquiryID == _EnquiryID && Q.ID == _ID);
            if (enquiry == null || quotation == null)
                ShowErroMessag();            
            else
            {                
                //txtNumber.Text = entity.Number;
                //ddlEnquiryID.SetSelectedItem(entity.EnquiryID);
                //ddlStatusID.SetSelectedItem(entity.StatusID);
                txtSubcontractor.Text = quotation.Subcontractor;
                txtScopeOfWork.Text = quotation.ScopeOfWork;
                txtMainEquipment.Text = quotation.MainEquipment;
                txtValidityDays.Text = quotation.ValidityDays.ToString();
                txtSchedule.Text = quotation.Schedule;
                txtSubmissionDate.Text = quotation.SubmissionDate.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat);
                //chkDecisionSuccessful.Checked = entity.DecisionSuccessful;
                txtDecisionDate.Text = quotation.DecisionDate.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat);
                //txtCreatedOn.Text = entity.CreatedOn.ToString(ConfigReader.CSharpCalendarDateFormat));
                //ddlCreatedByUserID.SetSelectedItem(entity.CreatedByUserID);
                //txtChangedOn.Text = entity.ChangedOn.ToString(ConfigReader.CSharpCalendarDateFormat));
                //ddlChangedByUserID.SetSelectedItem(entity.ChangedByUserID);
                //txtVersion.Text = entity.Version;
                //ddlSubmittedToClientContactID.SetSelectedItem(entity.SubmittedToClientContactID);
                //ddlCurrencyID.SetSelectedItem(entity.CurrencyID);
                //txtCreatedByUsername.Text = entity.CreatedByUsername;
                //txtChangedByUsername.Text = entity.ChangedByUsername;
                //txtContractawardedto.Text = entity.Contractawardedto;
                //txtContractawardedValue.Text = entity.ContractawardedValue;
                //txtNewStatusID.Text = entity.NewStatusID; 
                BindQuotationPricingList(dataContext);     
            }            
        }
    }
    /// <summary>
    /// This Prepares a JSON String from the Pricing Collection
    /// Which will be used in JavaScript Generate the List from JavaScript
    /// </summary>
    /// <param name="dataContext"></param>
    protected void BindQuotationPricingList(OMMDataContext dataContext)
    {
        IList<QuotationPricingLine> pricings = (from Q in dataContext.QuotationPricingLines
                                                where Q.QuotationID == _ID
                                                select Q).ToList();
        
        if(pricings != null && pricings.Count > 0)
            hdnQuotationPricings.Value = pricings.ToJSON();

    }
    /// <summary>
    /// Shows a Message in the UI and Hides the Data Editing Controls
    /// </summary>
    protected void ShowErroMessag()
    {
        pnlDetails.Visible = false;
        WebUtil.ShowMessageBox(divMessage, "Sorry! requested Quotation was not found.", true);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            //SaveQuotations();
            //Response.Redirect(AppConstants.Pages.QUOTATIONS_LIST);
            return;
        }
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        //Response.Redirect(AppConstants.Pages.QUOTATIONS_LIST);
        return;
    }

    #region Page Methods

    private static void MapObject(Quotation quotation, App.CustomModels.CustomQuotation customQuotation, OMMDataContext dataContext)
    {
        if (customQuotation.ID == 0)
        {
            quotation.CreatedByUserID = SessionCache.CurrentUser.ID;
            quotation.CreatedByUsername = SessionCache.CurrentUser.UserName;
            quotation.CreatedOn = DateTime.Now;
            quotation.Number = dataContext.GenerateNewQuotationNumber(quotation.EnquiryID, false);
            quotation.StatusID = App.CustomModels.QuotationStatus.NotSubmitted;
        }
        quotation.ChangedByUserID = SessionCache.CurrentUser.ID;
        quotation.ChangedByUsername = SessionCache.CurrentUser.UserName;
        quotation.ChangedOn = DateTime.Now;

        quotation.EnquiryID = customQuotation.EnquiryID;
        quotation.Subcontractor = customQuotation.Subcontractor;
        quotation.ScopeOfWork = customQuotation.ScopeOfWork;
        quotation.MainEquipment = customQuotation.MainEquipment;
        quotation.Schedule = customQuotation.Scheduel;
        quotation.ValidityDays = customQuotation.ValidityDays;
        quotation.SubmissionDate = WebUtil.GetDate(customQuotation.SubmissionDate);
        quotation.DecisionDate = WebUtil.GetDate(customQuotation.DecisionDate);

    }
    private static void ProcessAndSaveQuotation(Quotation quotation, IList < App.CustomModels.CustomQuotationPricingLine > pricingLineItems, OMMDataContext dataContext)
    {
        Enquiry enquiry = dataContext.Enquiries.SingleOrDefault(E => E.ID == quotation.EnquiryID);
        ///Enquiry Section
        if (quotation.ID == 0)
        {
            enquiry.StatusID = App.CustomModels.EnquiryStatus.Quoted;
            dataContext.Quotations.InsertOnSubmit(quotation);            
        }
        else if (quotation.StatusID == 3)
            enquiry.StatusID = App.CustomModels.EnquiryStatus.Closed;
        else if (quotation.StatusID == App.CustomModels.QuotationStatus.Successful)
            enquiry.StatusID = App.CustomModels.EnquiryStatus.Closed;
        else if (quotation.StatusID == App.CustomModels.QuotationStatus.ReQquoteRequested)
        {
            //TODO: Add Copy Function. Will be found in the Win App at
            ///QuotationDataset.cs. Line 591
        }
        ///QuotationPricingLine Section
        foreach (App.CustomModels.CustomQuotationPricingLine pricing in pricingLineItems)
        {
            quotation.QuotationPricingLines.Add(PreparePricingLineItem(pricing));
        }        

        ///Project Section
        ///TODO: Could not found the code that how it is bind to the UI while creating quotation
        //Project project = new Project();
        //if (project.ID == 0)
        //{
        //    project.CreatedByUserID = SessionCache.CurrentUser.ID;
        //    project.CreatedByUsername = SessionCache.CurrentUser.UserName;
        //    project.CreatedOn = DateTime.Now;
        //    project.StatusID = App.CustomModels.ProjectStatus.InProgress;
        //    project.Number = dataContext.GenerateNewProjectNumber();
        //}
        //project.ChangedByUserID = SessionCache.CurrentUser.ID;
        //project.ChangedByUsername = SessionCache.CurrentUser.UserName;
        //project.ChangedOn = DateTime.Now;
        //project.StartDate = DateTime.Now;
        
        //project.Description = quotation.ScopeOfWork;
        //project.Name = enquiry.EnguirySubject;
        //quotation.Projects.Add(project);

        //enquiry.Quotations.Add(quotation);

        dataContext.SubmitChanges();        
    }

    private static QuotationPricingLine PreparePricingLineItem(App.CustomModels.CustomQuotationPricingLine pricing)
    {
        QuotationPricingLine lineItem = new QuotationPricingLine();
        lineItem.Description = pricing.Description;
        lineItem.Item = pricing.Item;
        lineItem.PricingTypeID = pricing.PricingTypeID;
        lineItem.Quantity = pricing.Quantity;
        //lineItem.QuotationID = pricing.QuotationID;
        lineItem.UnitPrice = pricing.UnitPrice;
        return lineItem;
    }

    [WebMethod]
    public static String SaveQuotation(App.CustomModels.CustomQuotation customQuotation, IList<App.CustomModels.CustomQuotationPricingLine> pricingLineItems)
    {
        OMMDataContext dataContext = new OMMDataContext();
        Quotation quotation = new Quotation();
        if (customQuotation.ID == 0)
        {
            dataContext.Quotations.InsertOnSubmit(quotation);
        }
        else
        {
            quotation = dataContext.Quotations.SingleOrDefault(P => P.ID == customQuotation.ID);
        }
        MapObject(quotation, customQuotation, dataContext);
        ProcessAndSaveQuotation(quotation, pricingLineItems, dataContext);
        //dataContext.SubmitChanges();
        return String.Format("{0}:{1}", quotation.ID, quotation.Number);
    }
    #endregion
}
