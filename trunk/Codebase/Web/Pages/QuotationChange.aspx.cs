using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

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

        if (_ID > 0)
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
        if (_IsEditMode)
        {            
            //QuotationsDAO dao = new QuotationsDAO();
            //Quotations entity = dao.GetByID(_ID);
            //if (entity == null)
            //    ShowErroMessag();
            //else
            //{ 
            //    txtNumber.Text = entity.Number;
            //    ddlEnquiryID.SetSelectedItem(entity.EnquiryID);
            //    ddlStatusID.SetSelectedItem(entity.StatusID);
            //    txtSubcontractor.Text = entity.Subcontractor;
            //    txtScopeOfWork.Text = entity.ScopeOfWork;
            //    txtMainEquipment.Text = entity.MainEquipment;
            //    txtValidityDays.Text = entity.ValidityDays;
            //    txtSchedule.Text = entity.Schedule;
            //    txtSubmissionDate.Text = entity.SubmissionDate.ToString(ConfigReader.CSharpCalendarDateFormat));
            //    chkDecisionSuccessful.Checked = entity.DecisionSuccessful;
            //    txtDecisionDate.Text = entity.DecisionDate.ToString(ConfigReader.CSharpCalendarDateFormat));
            //    txtCreatedOn.Text = entity.CreatedOn.ToString(ConfigReader.CSharpCalendarDateFormat));
            //    ddlCreatedByUserID.SetSelectedItem(entity.CreatedByUserID);
            //    txtChangedOn.Text = entity.ChangedOn.ToString(ConfigReader.CSharpCalendarDateFormat));
            //    ddlChangedByUserID.SetSelectedItem(entity.ChangedByUserID);
            //    txtVersion.Text = entity.Version;
            //    ddlSubmittedToClientContactID.SetSelectedItem(entity.SubmittedToClientContactID);
            //    ddlCurrencyID.SetSelectedItem(entity.CurrencyID);
            //    txtCreatedByUsername.Text = entity.CreatedByUsername;
            //    txtChangedByUsername.Text = entity.ChangedByUsername;
            //    txtContractawardedto.Text = entity.Contractawardedto;
            //    txtContractawardedValue.Text = entity.ContractawardedValue;
            //    txtNewStatusID.Text = entity.NewStatusID;                      
            //}            
        }
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
            SaveQuotations();
            //Response.Redirect(AppConstants.Pages.QUOTATIONS_LIST);
            return;
        }
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        //Response.Redirect(AppConstants.Pages.QUOTATIONS_LIST);
        return;
    }
    protected void SaveQuotations()
    {
        //QuotationsDAO dao = new QuotationsDAO();
        //Quotations entity = new Quotations();

        //if (_IsEditMode)
        //{
        //    entity = dao.GetByID(_ID);
        //}
        //entity.ID = txtID.Text;
        //entity.Number = txtNumber.Text;
        //entity.EnquiryID = ddlEnquiryID.SelectedValue;
        //entity.StatusID = ddlStatusID.SelectedValue;
        //entity.Subcontractor = txtSubcontractor.Text;
        //entity.ScopeOfWork = txtScopeOfWork.Text;
        //entity.MainEquipment = txtMainEquipment.Text;
        //entity.ValidityDays = txtValidityDays.Text;
        //entity.Schedule = txtSchedule.Text;
        //entity.SubmissionDate = WebUtil.GetDate(txtSubmissionDate.Text);
        //entity.DecisionSuccessful = chkDecisionSuccessful.Checked;
        //entity.DecisionDate = WebUtil.GetDate(txtDecisionDate.Text);
        //entity.CreatedOn = WebUtil.GetDate(txtCreatedOn.Text);
        //entity.CreatedByUserID = ddlCreatedByUserID.SelectedValue;
        //entity.ChangedOn = WebUtil.GetDate(txtChangedOn.Text);
        //entity.ChangedByUserID = ddlChangedByUserID.SelectedValue;
        //entity.Version = txtVersion.Text;
        //entity.SubmittedToClientContactID = ddlSubmittedToClientContactID.SelectedValue;
        //entity.CurrencyID = ddlCurrencyID.SelectedValue;
        //entity.CreatedByUsername = txtCreatedByUsername.Text;
        //entity.ChangedByUsername = txtChangedByUsername.Text;
        //entity.Contractawardedto = txtContractawardedto.Text;
        //entity.ContractawardedValue = txtContractawardedValue.Text;
        //entity.NewStatusID = txtNewStatusID.Text;
        //dao.Save(entity);
    }

    [WebMethod]
    public static int SaveQuotation(App.CustomModels.CustomQuotation customQuotation, IList<App.CustomModels.CustomQuotationPricingLine> pricingLineItems)
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

        //dataContext.SubmitChanges();
        return 0;
    }

    private static void MapObject(Quotation quotation, App.CustomModels.CustomQuotation customQuotation, OMMDataContext dataContext)
    {        
        if (customQuotation.ID == 0)
        {
            quotation.CreatedByUserID = SessionCache.CurrentUser.ID;
            quotation.CreatedByUsername = SessionCache.CurrentUser.UserName;
            quotation.CreatedOn = DateTime.Now;
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
}
