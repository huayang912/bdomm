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

        IList<Currency> currencies = (from C in dataContext.Currencies select C).ToList();
        ddlCurrency.DataSource = currencies;
        foreach (Currency c in currencies)
        {
            ddlCurrency.Items.Add(new ListItem(c.Description, String.Format("{0}:{1}", c.ID, c.ShortCode)));
        }

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
        ///Bind Quotation First
        Enquiry enquiry = dataContext.Enquiries.SingleOrDefault(E => E.ID == _EnquiryID);
        if (enquiry == null)
        {
            ShowErroMessag("E", String.Empty);
            return;
        }
        else
        {
            ltrHeading.Text = String.Format("Create New Quotation Wizard - Enquiry {0}", enquiry.Number);
            //if (enquiry.StatusID == App.CustomModels.EnquiryStatus.Quoted)
            //{
            //    ShowErroMessag("V", enquiry.Number);
            //    return;
            //}
            //else 
            if (enquiry.StatusID == App.CustomModels.EnquiryStatus.Closed)
            {
                ShowErroMessag("VC", enquiry.Number);
                return;
            }
        }

        Page.Title = ltrHeading.Text;

        if (_IsEditMode)
        {            
            Quotation quotation = dataContext.Quotations.SingleOrDefault(Q => Q.EnquiryID == _EnquiryID && Q.ID == _ID);
            if (enquiry == null || quotation == null)
                ShowErroMessag("Q", String.Empty);            
            else
            {
                if (quotation.StatusID == App.CustomModels.QuotationStatus.NotSubmitted)
                {
                    txtSubcontractor.Text = quotation.Subcontractor;
                    txtScopeOfWork.Text = quotation.ScopeOfWork;
                    txtMainEquipment.Text = quotation.MainEquipment;
                    txtValidityDays.Text = quotation.ValidityDays.ToString();
                    txtSchedule.Text = quotation.Schedule;
                    txtSubmissionDate.Text = quotation.SubmissionDate.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat);
                    txtDecisionDate.Text = quotation.DecisionDate.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat);
                    BindQuotationPricingList(dataContext);
                }
                else                
                    ShowErroMessag("QCE", String.Empty);                
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
        IList<App.CustomModels.CustomQuotationPricingLine> pricings = (from Q in dataContext.QuotationPricingLines
                                                where Q.QuotationID == _ID
                                                select new App.CustomModels.CustomQuotationPricingLine
                                                {
                                                    ID = Q.ID,
                                                    Description = Q.Description,
                                                    Item = Q.Item,
                                                    PricingTypeID = Q.PricingTypeID.GetValueOrDefault(),
                                                    Price = Q.UnitPrice.GetValueOrDefault() * Q.Quantity.GetValueOrDefault(),
                                                    PricingType = Q.PricingTypeID == null ? String.Empty : Q.QuotationPricingType.Name,
                                                    Quantity = Q.Quantity.GetValueOrDefault(),
                                                    QuotationID = Q.QuotationID,
                                                    UnitPrice = Q.UnitPrice.GetValueOrDefault()
                                                }
                                                ).ToList();
        
        if(pricings != null && pricings.Count > 0)
            hdnQuotationPricings.Value = pricings.ToJSON();

    }
    /// <summary>
    /// Shows a Message in the UI and Hides the Data Editing Controls
    /// </summary>
    protected void ShowErroMessag(String msgType,  String enquiryNumber)
    {
        pnlDetails.Visible = false;
        if (msgType == "E")
            WebUtil.ShowMessageBox(divMessage, "Sorry! requested Enquiry was not found.", true);
        else if (msgType == "Q")
            WebUtil.ShowMessageBox(divMessage, "Sorry! requested Quotation was not found.", true);
        else if (msgType == "V")
            WebUtil.ShowMessageBox(divMessage, String.Format("Sorry! Enquiry {0} has already been Quotated .", enquiryNumber), true);
        else if (msgType == "VC")
            WebUtil.ShowMessageBox(divMessage, String.Format("Sorry! Enquiry {0} has been closed .", enquiryNumber), true);
        else if (msgType == "QCE")
            WebUtil.ShowMessageBox(divMessage, "Sorry! This Quotation cannot be edited", true);
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
        quotation.CurrencyID = customQuotation.CurrencyID;
        //var currency = dataContext.Currencies.SingleOrDefault(C => C.Description == ddlCurrency.SelectedValue);
        //if(currency != null)
        //    quotation.CurrencyID = currency.ID;
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
        else if (quotation.StatusID == App.CustomModels.QuotationStatus.Unsuccessful)// 3)
            enquiry.StatusID = App.CustomModels.EnquiryStatus.Closed;
        else if (quotation.StatusID == App.CustomModels.QuotationStatus.Successful)
            enquiry.StatusID = App.CustomModels.EnquiryStatus.Closed;
        else if (quotation.StatusID == App.CustomModels.QuotationStatus.ReQquoteRequested)
        {
            //TODO: Add Copy Function. Will be found in the Win App at
            ///QuotationDataset.cs. Line 591
        }
        ///QuotationPricingLine Section
        if (pricingLineItems != null && pricingLineItems.Count > 0)
        {
            foreach (App.CustomModels.CustomQuotationPricingLine pricing in pricingLineItems)
            {
                quotation.QuotationPricingLines.Add(PreparePricingLineItem(pricing));
            }
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
