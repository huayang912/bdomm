
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using App.Core.Extensions;
using App.Data;


public partial class Pages_PersonnelTravelDetails : BasePage
{
    private int _ContactID = 0;
    private int _ID = 0;
    private bool _IsEditMode = false;
    private string _OnTable = "";    
    private bool _IsDeleteMode = false;
    private const int PAGE_SIZE = 15;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            BindDropdownList.Countries(ddlCountry);

            loadBasicDetails();

            CheckAndDeleteData();
            BindDropDownLists();
            BindContactsPassportInfo();
            
            BindPassportList(1);
            BindVisaList(1);
            
            ShowSuccessMessage();
        }
    }

    public void loadBasicDetails()
    {
        OMMDataContext context = new OMMDataContext();
        
        var ContactsTravelDetails = context.ContactsTravels.FirstOrDefault(P => P.ContactID == _ContactID);

        if (ContactsTravelDetails == null || _ContactID == 0)
        {
            //btnSaveBasicInfo.Text = "Save";
            tbxUpdateSave.Text = "Save";
        }
        else
        {
            //btnSaveBasicInfo.Text = "Update";
            tbxUpdateSave.Text = "Update";

            tbxFrequentFlNumber.Text = 
                (ContactsTravelDetails.FrequentFlyerNumber.IsNullOrEmpty()) ? "" : ContactsTravelDetails.FrequentFlyerNumber.ToString();
            tbxPreferredAirport.Text = 
                (ContactsTravelDetails.PreferredAirport.IsNullOrEmpty()) ? "" : ContactsTravelDetails.PreferredAirport.ToString();
            tbxClosestAirport.Text = 
                (ContactsTravelDetails.ClosestAirport.IsNullOrEmpty()) ? "" : ContactsTravelDetails.ClosestAirport.ToString();


            //grdsearch.DataSource = ContactsNextOfKin;
            //grdsearch.DataBind();
        }
    }

    protected void btnSaveBasicInfo_Click(object sender, EventArgs e)
    {
        //Save information in the database
        OMMDataContext context = new OMMDataContext();
        //int contactID = WebUtil.GetQueryStringInInt("ID");

        if (tbxUpdateSave.Text == "Save")
        {
            ContactsTravel conTravel = new ContactsTravel();

            conTravel.FrequentFlyerNumber = 
                (tbxFrequentFlNumber.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxFrequentFlNumber.Text.ToString().Trim();
            conTravel.PreferredAirport = 
                (tbxPreferredAirport.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxPreferredAirport.Text.ToString().Trim();
            conTravel.ClosestAirport = 
                (tbxClosestAirport.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxClosestAirport.Text.ToString().Trim();
            conTravel.ChangedByUserID = SessionCache.CurrentUser.ID;
            conTravel.ChangedOn = System.DateTime.Today;

            context.ContactsTravels.InsertOnSubmit(conTravel);
            context.SubmitChanges();
        }
        if (tbxUpdateSave.Text == "Update")
        {
            var ContactsTravel = context.ContactsTravels.FirstOrDefault(P => P.ContactID == _ContactID);


            if (ContactsTravel == null || _ContactID == 0)
            {

            }
            else
            {
                ContactsTravel.FrequentFlyerNumber = 
                    (tbxFrequentFlNumber.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxFrequentFlNumber.Text.ToString().Trim();
                ContactsTravel.PreferredAirport = 
                    (tbxPreferredAirport.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxPreferredAirport.Text.ToString().Trim();
                ContactsTravel.ClosestAirport = 
                    (tbxClosestAirport.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxClosestAirport.Text.ToString().Trim();
                ContactsTravel.ChangedByUserID = SessionCache.CurrentUser.ID;
                ContactsTravel.ChangedOn = System.DateTime.Today;

                context.SubmitChanges();
            }
        }


        ShowSuccessMessage();
    }
    
    /// <summary>
    /// Bindis the Page Initialization Variables
    /// </summary>
    protected void BindPageInfo()
    {
        _ID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
        _ContactID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.CONTACT_ID);
        _IsDeleteMode = String.Compare(WebUtil.GetQueryStringInString(
            AppConstants.QueryString.DELETE), "True", true) == 0 ? true : false;
        if (_ID > 0 && !_IsDeleteMode)        
            _IsEditMode = true;

        _OnTable = WebUtil.GetQueryStringInString(AppConstants.QueryString.ON_TABLE);

        Page.Title = WebUtil.GetPageTitle("Manage Travel Details");
    }

    /// <summary>
    /// Binds Dropdownlists for the initial request.
    /// </summary>
    protected void BindDropDownLists()
    {
        //BindDropdownList.Contactses(ddlContactID);
        //BindDropdownList.Userses(ddlChangedByUserID);
    }
    protected void ShowSuccessMessage()
    {
        if (String.Compare(WebUtil.GetQueryStringInString
            (AppConstants.QueryString.SUCCESS_MSG), "True", false) == 0)
            WebUtil.ShowMessageBox(divMessage, "Information Saved Successfully.", false);
    }
    protected void CheckAndDeleteData()
    {
        if (_IsDeleteMode)
        {
            if (_OnTable == "PASSPORT")
            {
                OMMDataContext context = new OMMDataContext();
                var PassportDetails = context.Passports.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);
                if (PassportDetails == null)
                    WebUtil.ShowMessageBox(divMessage,
                        "Sorry! requested Passport Details not found for delete. Delete Failed.", true);
                else
                {
                    context.Passports.DeleteOnSubmit(PassportDetails);
                    try
                    {
                        context.SubmitChanges();
                        WebUtil.ShowMessageBox(divMessage, "Passport Details deleted successfully.", false);
                    }
                    catch
                    {
                        WebUtil.ShowMessageBox(divMessage,
                            "Sorry! this Passport details contains related information. Delete failed.", true);
                    }
                }
            }
            if (_OnTable == "VISA")
            {
                OMMDataContext context = new OMMDataContext();
                var VisaDetails = context.Visas.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);
                if (VisaDetails == null)
                    WebUtil.ShowMessageBox(divMessage,
                        "Sorry! requested VISA Details not found for delete. Delete Failed.", true);
                else
                {
                    context.Visas.DeleteOnSubmit(VisaDetails);
                    try
                    {
                        context.SubmitChanges();
                        WebUtil.ShowMessageBox(divMessage, "VISA Details deleted successfully.", false);
                    }
                    catch
                    {
                        WebUtil.ShowMessageBox(divMessage,
                            "Sorry! this VISA details contains related information. Delete failed.", true);
                    }
                }
            }
        }
    }
    protected void BindPassportList(int pageNumber)
    {
        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", _ContactID) };
        int totalRecord = 0;
        //DataSet ds = dao.GetPagedData(AppSQL.GET_BANK_DETAILS_BY_CONTACT, parameters, pageNumber, PAGE_SIZE, out totalRecord);
        DataSet ds = dao.GetDataSet(AppSQL.GET_PASSPORT_DETAILS_BY_CONTACT, parameters, false);
        
        //Bind the List Control
        ucPassportList.DataSource = ds.Tables[0];
        ucPassportList.EditLink = Request.Url.AbsolutePath + "?" + 
            AppConstants.QueryString.CONTACT_ID + "={0}&" +
            AppConstants.QueryString.ID + "={1}&" +
            AppConstants.QueryString.ON_TABLE + "=PASSPORT";
        ucPassportList.DeleteLink = Request.Url.AbsolutePath + "?" + 
            AppConstants.QueryString.CONTACT_ID + "={0}&" + 
            AppConstants.QueryString.ID + "={1}&" +
            AppConstants.QueryString.DELETE + "=True&" +
            AppConstants.QueryString.ON_TABLE + "=PASSPORT";
        ucPassportList.DataBind();

        ///Bind the Pager Control
        //ucNoteListPager.TotalRecord = totalRecord;
        //ucNoteListPager.PageNo = pageNumber;
        //ucNoteListPager.PageSize = PAGE_SIZE;
        //ucNoteListPager.DataBind();
    }
    protected void BindVisaList(int pageNumber)
    {
        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", _ContactID) };
        int totalRecord = 0;
        //DataSet ds = dao.GetPagedData(AppSQL.GET_BANK_DETAILS_BY_CONTACT, parameters, pageNumber, PAGE_SIZE, out totalRecord);
        DataSet ds = dao.GetDataSet(AppSQL.GET_VISA_DETAILS_BY_CONTACT, parameters, false);

        //Bind the List Control
        ucVisaList.DataSource = ds.Tables[0];
        ucVisaList.EditLink = Request.Url.AbsolutePath + "?" +
            AppConstants.QueryString.CONTACT_ID + "={0}&" +
            AppConstants.QueryString.ID + "={1}&" +
            AppConstants.QueryString.ON_TABLE + "=VISA";
        ucVisaList.DeleteLink = Request.Url.AbsolutePath + "?" +
            AppConstants.QueryString.CONTACT_ID + "={0}&" +
            AppConstants.QueryString.ID + "={1}&" +
            AppConstants.QueryString.DELETE + "=True&" +
            AppConstants.QueryString.ON_TABLE + "=VISA";
        ucVisaList.DataBind();

        ///Bind the Pager Control
        //ucNoteListPager.TotalRecord = totalRecord;
        //ucNoteListPager.PageNo = pageNumber;
        //ucNoteListPager.PageSize = PAGE_SIZE;
        //ucNoteListPager.DataBind();
    }


    /// <summary>
    /// Binds ContactsNotes Info Requested through Query Strings
    /// </summary>
    protected void BindContactsPassportInfo()
    {
        if (_OnTable == "PASSPORT")
        {
            OMMDataContext context = new OMMDataContext();
            if (context.Passports.FirstOrDefault(P => P.ContactID == _ContactID) == null)
                ShowNotFoundMessage();
            else
            {
                if (_IsEditMode)
                {
                    Passport entity =
                        context.Passports.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);
                    if (entity == null)
                        ShowNotFoundMessage();
                    else
                    {
                        tbxNumber.Text = entity.Number;
                        tbxWhereIssued.Text = entity.WhereIssued;
                        tbxExpiryDate.Text = (entity.ExpiryDate.IsNotNull()) ? entity.ExpiryDate.ToString() : "";
                        tbxNationality.Text = entity.Nationality;
                    }
                }
            }
        }
        if (_OnTable == "VISA")
        {
            OMMDataContext context = new OMMDataContext();
            if (context.Visas.FirstOrDefault(P => P.ContactID == _ContactID) == null)
                ShowNotFoundMessage();
            else
            {
                if (_IsEditMode)
                {
                    Visa entity =
                        context.Visas.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);
                    if (entity == null)
                        ShowNotFoundMessage();
                    else
                    {
                        //tbxCountry.Text = entity.CountryID.ToString();
                        ddlCountry.SetSelectedItem((entity.CountryID.ToString().Trim().IsNullOrEmpty()) ?
                            String.Empty : entity.CountryID.ToString().Trim());

                        tbxVisaType.Text = entity.VisaType;
                        tbxVisaExpDate.Text = (entity.ExpiryDate.IsNotNull()) ? entity.ExpiryDate.ToString() : "";
                        
                    }
                }
            }
        }
    }
    /// <summary>
    /// Shows a Message in the UI and Hides the Data Editing Controls
    /// </summary>
    protected void ShowNotFoundMessage()
    {
        //pnlFormContainer.Visible = false;
        WebUtil.ShowMessageBox(divMessage, "Requested Details was not found.", true);
    }

    protected void SaveContactsPassport()
    {
        OMMDataContext context = new OMMDataContext();
        Passport entity = null;

        if (_IsEditMode)
            entity = context.Passports.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);
        else
        {
            entity = new Passport();
            entity.ContactID = _ContactID;
            context.Passports.InsertOnSubmit(entity);
        }

        //ddlContactID.SelectedValue.ToInt();
        entity.Number = tbxNumber.Text;
        entity.WhereIssued = tbxWhereIssued.Text;
        if (tbxExpiryDate.Text.IsNullOrEmpty())
            entity.ExpiryDate = null;
        else
            entity.ExpiryDate = tbxExpiryDate.Text.ToDateTime(ConfigReader.CSharpCalendarDateFormat); //Convert.ToDateTime(tbxExpiryDate.Text.Trim());
        entity.Nationality = tbxNationality.Text;
        

        entity.ChangedByUserID = SessionCache.CurrentUser.ID;
        entity.ChangedOn = DateTime.Now;        
        //entity = entity.ChangedByUsername = SessionCache.CurrentUser.UserName;

        context.SubmitChanges();
        String url = String.Format("{0}?{1}={2}&{3}=True"
            , Request.Url.AbsolutePath
            , AppConstants.QueryString.CONTACT_ID
            , _ContactID
            , AppConstants.QueryString.SUCCESS_MSG);
        Response.Redirect(url);
    }


    protected void SaveContactsVisa()
    {
        OMMDataContext context = new OMMDataContext();
        Visa entity = null;

        if (_IsEditMode)
            entity = context.Visas.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);
        else
        {
            entity = new Visa();
            entity.ContactID = _ContactID;
            context.Visas.InsertOnSubmit(entity);
        }

        //ddlContactID.SelectedValue.ToInt();
        if (!ddlCountry.SelectedValue.IsNullOrEmpty())
        {
            entity.CountryID = Convert.ToInt32(ddlCountry.SelectedValue);
        }
        //entity.CountryID = Convert.ToInt32(tbxCountry.Text);

        entity.VisaType = tbxVisaType.Text;
        if (tbxExpiryDate.Text.Trim() != "")
            entity.ExpiryDate = Convert.ToDateTime(tbxExpiryDate.Text.Trim());
        
        entity.ChangedByUserID = SessionCache.CurrentUser.ID;
        entity.ChangedOn = DateTime.Now;
        //entity = entity.ChangedByUsername = SessionCache.CurrentUser.UserName;

        context.SubmitChanges();
        String url = String.Format("{0}?{1}={2}&{3}=True"
            , Request.Url.AbsolutePath
            , AppConstants.QueryString.CONTACT_ID
            , _ContactID
            , AppConstants.QueryString.SUCCESS_MSG);
        Response.Redirect(url);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            SaveContactsPassport();
            //Response.Redirect(AppConstants.Pages.CONTACTSNOTES_LIST);
            return;
        }
    }

    protected void btnSaveVisa_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            SaveContactsVisa();
            return;
        }
    }

    //protected void btnSaveBasicInfo_Click(object sender, EventArgs e)
    //{
    //    if (Page.IsValid)
    //    {
    //        //SaveContactsBank();
            
    //        return;
    //    }
    //}

    protected void btnList_Click(object sender, EventArgs e)
    {
        //Response.Redirect(AppConstants.Pages.CONTACTSNOTES_LIST);
        //return;
    }
    protected void ucPassportListPager_PageIndexChanged(object sender, PagerEventArgs e)
    {
        BindPassportList(e.PageIndex);
    }
}














