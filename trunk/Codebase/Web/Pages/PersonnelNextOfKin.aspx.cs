
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using App.Core.Extensions;
using App.Data;


public partial class Pages_PersonnelNextOfKin : BasePage
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

            loadImmediateFamilyDetails();

            CheckAndDeleteData();
            BindDropDownLists();
            BindContactsPassportInfo();

            BindNextOfKinList(1);
            
            ShowSuccessMessage();
        }
    }

    public void loadImmediateFamilyDetails()
    {
        OMMDataContext context = new OMMDataContext();

        var _ImmediateFamily = context.ContactsNextOfKins.FirstOrDefault(P => P.ContactID == _ContactID);

        if (_ImmediateFamily == null || _ContactID == 0)
        {
            tbxSaveUpdate.Text = "Save";
        }
        else
        {
            tbxSaveUpdate.Text = "Update";

            tbxMotherName.Text =
                (_ImmediateFamily.MothersName.IsNullOrEmpty()) ? "" : 
                _ImmediateFamily.MothersName.ToString();
            tbxFatherName.Text =
                (_ImmediateFamily.FathersName.IsNullOrEmpty()) ? "" : 
                _ImmediateFamily.FathersName.ToString();
            tbxChildName.Text =
                (_ImmediateFamily.ChildrensNames.IsNullOrEmpty()) ? "" : 
                _ImmediateFamily.ChildrensNames.ToString();

        }
    }

    protected void btnSaveBasicInfo_Click(object sender, EventArgs e)
    {
        SaveContactsNextOfKin();
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

        Page.Title = WebUtil.GetPageTitle("Manage Next Of Kin Details");
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
            OMMDataContext context = new OMMDataContext();
            var _nextOfKin = context.NextOfKins.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);

            if (_nextOfKin == null)
                WebUtil.ShowMessageBox(divMessage,
                    "Sorry! requested Next Of Kin Details not found for delete. Delete Failed.", true);
            else
            {
                context.NextOfKins.DeleteOnSubmit(_nextOfKin);
                try
                {
                    context.SubmitChanges();
                    WebUtil.ShowMessageBox(divMessage, "Next Of Kin Details deleted successfully.", false);
                }
                catch
                {
                    WebUtil.ShowMessageBox(divMessage,
                        "Sorry! this Next Of Kin details contains related information. Delete failed.", true);
                }
            }
        }
    }

    protected void BindNextOfKinList(int pageNumber)
    {
        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", _ContactID) };
        int totalRecord = 0;
        //DataSet ds = dao.GetPagedData(AppSQL.GET_BANK_DETAILS_BY_CONTACT, parameters, pageNumber, PAGE_SIZE, out totalRecord);
        DataSet ds = dao.GetDataSet(AppSQL.GET_NEXT_OF_KIN_BY_CONTACT, parameters, false);
        
        //Bind the List Control
        ucPassportList.DataSource = ds.Tables[0];
        ucPassportList.EditLink = Request.Url.AbsolutePath + "?" + 
            AppConstants.QueryString.CONTACT_ID + "={0}&" +
            AppConstants.QueryString.ID + "={1}&" +
            AppConstants.QueryString.ON_TABLE + "=Next Of Kin";
        ucPassportList.DeleteLink = Request.Url.AbsolutePath + "?" + 
            AppConstants.QueryString.CONTACT_ID + "={0}&" + 
            AppConstants.QueryString.ID + "={1}&" +
            AppConstants.QueryString.DELETE + "=True&" +
            AppConstants.QueryString.ON_TABLE + "=Next Of Kin";
        ucPassportList.DataBind();

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
        OMMDataContext context = new OMMDataContext();
        if (context.NextOfKins.FirstOrDefault(P => P.ContactID == _ContactID) == null)
            ShowNotFoundMessage();
        else
        {
            if (_IsEditMode)
            {
                NextOfKin entity =
                    context.NextOfKins.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);
                if (entity == null)
                    ShowNotFoundMessage();
                else
                {
                    tbxName.Text = entity.Name;
                    tbxRelationShip.Text = entity.Relationship;
                    tbxAddress.Text = entity.Address;
                    tbxPostCode.Text = entity.Postcode;
                    ddlCountry.SetSelectedItem((entity.CountryID.ToString().Trim().IsNullOrEmpty()) ?
                            String.Empty : entity.CountryID.ToString().Trim());
                    tbxHomeNumber.Text = entity.HomeNumber;
                    tbxWorkNumber.Text = entity.WorkNumber;
                    tbxMobile.Text = entity.MobileNumber;                    
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

    protected void SaveContactsNextOfKin()
    {
        SaveBasicInfo();


        OMMDataContext context = new OMMDataContext();
        NextOfKin entity = null;

        if (_IsEditMode)
            entity = context.NextOfKins.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);
        else
        {
            entity = new NextOfKin();
            entity.ContactID = _ContactID;
            context.NextOfKins.InsertOnSubmit(entity);
        }

        entity.Name = tbxName.Text;
        entity.Relationship = tbxRelationShip.Text;
        entity.Address = tbxAddress.Text;
        entity.Postcode = tbxPostCode.Text;
        
        if (!ddlCountry.SelectedValue.IsNullOrEmpty())
        {
            entity.CountryID = Convert.ToInt32(ddlCountry.SelectedValue);
        }

        entity.HomeNumber = tbxHomeNumber.Text;
        entity.WorkNumber = tbxWorkNumber.Text;
        entity.MobileNumber = tbxMobile.Text;
        
        
        entity.ChangedByUserID = SessionCache.CurrentUser.ID;
        entity.ChangedOn = DateTime.Now;
        
        context.SubmitChanges();
        String url = String.Format("{0}?{1}={2}&{3}=True"
            , Request.Url.AbsolutePath
            , AppConstants.QueryString.CONTACT_ID
            , _ContactID
            , AppConstants.QueryString.SUCCESS_MSG);
        Response.Redirect(url);
    }


    protected void SaveBasicInfo()
    {
        //Save information in the database
        OMMDataContext context = new OMMDataContext();
        //int contactID = WebUtil.GetQueryStringInInt("ID");

        if (tbxSaveUpdate.Text == "Save")
        {
            ContactsNextOfKin conNextOfKin = new ContactsNextOfKin();

            conNextOfKin.MothersName =
                (tbxMotherName.Text.ToString().Trim().IsNullOrEmpty()) ? "" :
                tbxMotherName.Text.ToString().Trim();
            conNextOfKin.FathersName =
                (tbxFatherName.Text.ToString().Trim().IsNullOrEmpty()) ? "" :
                tbxFatherName.Text.ToString().Trim();
            conNextOfKin.ChildrensNames =
                (tbxChildName.Text.ToString().Trim().IsNullOrEmpty()) ? "" :
                tbxChildName.Text.ToString().Trim();

            conNextOfKin.ChangedByUserID = SessionCache.CurrentUser.ID;
            conNextOfKin.ChangedOn = System.DateTime.Today;

            context.ContactsNextOfKins.InsertOnSubmit(conNextOfKin);
            context.SubmitChanges();
        }
        if (tbxSaveUpdate.Text == "Update")
        {
            var ContactsNextOfKin = context.ContactsNextOfKins.FirstOrDefault(P => P.ContactID == _ContactID);


            if (ContactsNextOfKin == null || _ContactID == 0)
            {

            }
            else
            {
                ContactsNextOfKin.MothersName =
                    (tbxMotherName.Text.ToString().Trim().IsNullOrEmpty()) ? "" :
                    tbxMotherName.Text.ToString().Trim();
                ContactsNextOfKin.FathersName =
                    (tbxFatherName.Text.ToString().Trim().IsNullOrEmpty()) ? "" :
                    tbxFatherName.Text.ToString().Trim();
                ContactsNextOfKin.ChildrensNames =
                    (tbxChildName.Text.ToString().Trim().IsNullOrEmpty()) ? "" :
                    tbxChildName.Text.ToString().Trim();

                ContactsNextOfKin.ChangedByUserID = SessionCache.CurrentUser.ID;
                ContactsNextOfKin.ChangedOn = System.DateTime.Today;

                context.SubmitChanges();
            }
        }


        //ShowSuccessMessage();
    }


    protected void btnList_Click(object sender, EventArgs e)
    {
        //Response.Redirect(AppConstants.Pages.CONTACTSNOTES_LIST);
        //return;
    }
    protected void ucPassportListPager_PageIndexChanged(object sender, PagerEventArgs e)
    {
        //BindPassportList(e.PageIndex);
    }
}














