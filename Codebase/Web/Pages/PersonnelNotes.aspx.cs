
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using App.Core.Extensions;
using App.Data;


public partial class Pages_PersonnelNotes : BasePage
{
    private int _ContactID = 0;
    private int _ID = 0;
    private bool _IsEditMode = false;
    private bool _IsDeleteMode = false;
    private const int PAGE_SIZE = 15;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            CheckAndDeleteData();
            BindDropDownLists();
            BindContactsNotesInfo();
            BindNotesList(1);
            ShowSuccessMessage();
        }
    }
    
    /// <summary>
    /// Bindis the Page Initialization Variables
    /// </summary>
    protected void BindPageInfo()
    {
        _ID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
        _ContactID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.CONTACT_ID);
        _IsDeleteMode = String.Compare(WebUtil.GetQueryStringInString(AppConstants.QueryString.DELETE), "True", true) == 0 ? true : false;
        if (_ID > 0 && !_IsDeleteMode)        
            _IsEditMode = true;
        
        Page.Title = WebUtil.GetPageTitle("Manage Notes");
    }

    /// <summary>
    /// Binds Dropdownlists for the initial request.
    /// </summary>
    protected void BindDropDownLists()
    {
        BindDropdownList.CommunicationTypes(ddlCommType);
        //BindDropdownList.Userses(ddlChangedByUserID);

    }
    protected void ShowSuccessMessage()
    {
        if (String.Compare(WebUtil.GetQueryStringInString(AppConstants.QueryString.SUCCESS_MSG), "True", false) == 0)
            WebUtil.ShowMessageBox(divMessage, "Note Saved Successfully.", false);
    }
    protected void CheckAndDeleteData()
    {
        if (_IsDeleteMode)
        {
            OMMDataContext context = new OMMDataContext();
            var note = context.ContactsNotes.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);
            if (note == null)
                WebUtil.ShowMessageBox(divMessage, "Sorry! requested Note was found for delete. Delete Failed.", true);
            else
            {
                context.ContactsNotes.DeleteOnSubmit(note);
                try
                {
                    context.SubmitChanges();
                    WebUtil.ShowMessageBox(divMessage, "Note deleted successfully.", false);
                }
                catch
                {
                    WebUtil.ShowMessageBox(divMessage, "Sorry! this note contains related information. Delete failed.", true);
                }
            }
        }
    }
    protected void BindNotesList(int pageNumber)
    {
        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", _ContactID) };
        int totalRecord = 0;
        DataSet ds = dao.GetPagedData(AppSQL.GET_NOTES_BY_CONTACT, parameters, pageNumber, PAGE_SIZE, out totalRecord);
        //Bind the List Control
        ucNoteList.DataSource = ds.Tables[0];
        ucNoteList.EditLink = Request.Url.AbsolutePath + "?" + AppConstants.QueryString.CONTACT_ID + "={0}&" + AppConstants.QueryString.ID + "={1}";
        ucNoteList.DeleteLink = Request.Url.AbsolutePath + "?" + AppConstants.QueryString.CONTACT_ID + "={0}&" + AppConstants.QueryString.ID + "={1}&" + AppConstants.QueryString.DELETE + "=True";
        ucNoteList.DataBind();

        ///Bind the Pager Control
        ucNoteListPager.TotalRecord = totalRecord;
        ucNoteListPager.PageNo = pageNumber;
        ucNoteListPager.PageSize = PAGE_SIZE;
        ucNoteListPager.DataBind();
    }
    /// <summary>
    /// Binds ContactsNotes Info Requested through Query Strings
    /// </summary>
    protected void BindContactsNotesInfo()
    {
        OMMDataContext context = new OMMDataContext();
        if (context.Contacts.FirstOrDefault(P => P.ID == _ContactID) == null)
            ShowNotFoundMessage();
        else
        {
            if (_IsEditMode)
            {                
                ContactsNote entity = context.ContactsNotes.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);//dao.GetByID(_ID);
                if (entity == null)
                    ShowNotFoundMessage();
                else
                {
                    //UtilityDAO dao = new UtilityDAO();
                    //DbParameter[] parameters = new[] { new DbParameter("@CommTypeID", entity.ContactCommsTypeID) };
                    
                    ////DataSet ds = dao.GetPagedData(AppSQL.GET_BANK_DETAILS_BY_CONTACT, parameters, pageNumber, PAGE_SIZE, out totalRecord);
                    //DataSet ds = dao.GetDataSet(AppSQL.GET_BANK_DETAILS_BY_CONTACT, parameters, false);

                    //ddlContactID.SetSelectedItem(entity.ContactID.ToString());
                    txtNotes.Text = entity.Notes;
                    ddlCommType.SetSelectedItem(entity.ContactCommsTypeID.ToString());
                    //txtChangedOn.Text = entity.ChangedOn.ToString(ConfigReader.CSharpCalendarDateFormat);
                    //txtVersion.Text = entity.Version;
                    //txtCreatedByUsername.Text = entity.CreatedByUsername;
                    //txtChangedByUsername.Text = entity.ChangedByUsername;
                }
            }
        }
    }
    /// <summary>
    /// Shows a Message in the UI and Hides the Data Editing Controls
    /// </summary>
    protected void ShowNotFoundMessage()
    {
        pnlFormContainer.Visible = false;
        WebUtil.ShowMessageBox(divMessage, "Requested Note was not found.", true);
    }
    protected void SaveContactsNotes()
    {
        OMMDataContext context = new OMMDataContext();
        ContactsNote entity = null;

        if (_IsEditMode)
            entity = context.ContactsNotes.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID); //dao.GetByID(_ID);        
        else
        {
            entity = new ContactsNote();
            entity.ContactID = _ContactID;
            context.ContactsNotes.InsertOnSubmit(entity);
        }

        //ddlContactID.SelectedValue.ToInt();
        entity.Notes = txtNotes.Text;
        entity.ContactCommsTypeID = Convert.ToInt32(ddlCommType.SelectedValue);
        entity.ChangedByUserID = SessionCache.CurrentUser.ID;
        entity.ChangedOn = DateTime.Now;        
        entity.CreatedByUsername = entity.ChangedByUsername = SessionCache.CurrentUser.UserName;

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
            SaveContactsNotes();
            //Response.Redirect(AppConstants.Pages.CONTACTSNOTES_LIST);
            return;
        }
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        //Response.Redirect(AppConstants.Pages.CONTACTSNOTES_LIST);
        //return;
    }
    protected void ucNoteListPager_PageIndexChanged(object sender, PagerEventArgs e)
    {
        BindNotesList(e.PageIndex);
    }
}














