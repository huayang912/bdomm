
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using App.Core.Extensions;
using App.Data;


public partial class Pages_PersonnelCertification : BasePage
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
            //WebUtil.ShowMessageBox(divMessage, "", false);

            BindDropdownList.CertificateTypes(ddlCertificateType);

            CheckAndDeleteData();
            BindDropDownLists();
            BindCertificationInfo();
            BindCertificationList(1);
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
        
        Page.Title = WebUtil.GetPageTitle("Manage Certification");
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
        if (String.Compare(WebUtil.GetQueryStringInString(AppConstants.QueryString.SUCCESS_MSG), "True", false) == 0)
            WebUtil.ShowMessageBox(divMessage, "Certification Details Saved Successfully.", false);
    }
    protected void CheckAndDeleteData()
    {
        if (_IsDeleteMode)
        {
            OMMDataContext context = new OMMDataContext();
            var CertificationDetails = context.Certificates.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);
            if (CertificationDetails == null)
                WebUtil.ShowMessageBox(divMessage, "Sorry! requested Certification Details not found for delete. Delete Failed.", true);
            else
            {
                context.Certificates.DeleteOnSubmit(CertificationDetails);
                try
                {
                    context.SubmitChanges();
                    WebUtil.ShowMessageBox(divMessage, "Certification Details deleted successfully.", false);
                }
                catch
                {
                    WebUtil.ShowMessageBox(divMessage, "Sorry! this Certification contains related information. Delete failed.", true);
                }
            }
        }
    }
    protected void BindCertificationList(int pageNumber)
    {
        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", _ContactID) };
        int totalRecord = 0;
        DataSet ds = dao.GetDataSet(AppSQL.GET_CRETIFICATION_DETAILS_BY_CONTACT, parameters, false);
        
        //Bind the List Control
        ucCertificationList.DataSource = ds.Tables[0];
        ucCertificationList.EditLink = Request.Url.AbsolutePath + "?" + 
            AppConstants.QueryString.CONTACT_ID + "={0}&" + 
            AppConstants.QueryString.ID + "={1}";
        ucCertificationList.DeleteLink = Request.Url.AbsolutePath + "?" + 
            AppConstants.QueryString.CONTACT_ID + "={0}&" + 
            AppConstants.QueryString.ID + "={1}&" + 
            AppConstants.QueryString.DELETE + "=True";
        ucCertificationList.DataBind();

        ///Bind the Pager Control
        //ucNoteListPager.TotalRecord = totalRecord;
        //ucNoteListPager.PageNo = pageNumber;
        //ucNoteListPager.PageSize = PAGE_SIZE;
        //ucNoteListPager.DataBind();
    }
    /// <summary>
    /// Binds ContactsNotes Info Requested through Query Strings
    /// </summary>
    protected void BindCertificationInfo()
    {
        OMMDataContext context = new OMMDataContext();
        if (context.Certificates.FirstOrDefault(P => P.ContactID == _ContactID) == null)
            ShowNotFoundMessage();
        else
        {
            if (_IsEditMode)
            {                
                Certificate entity = 
                    context.Certificates.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);
                
                if (entity == null)
                    ShowNotFoundMessage();
                else
                {
                    ddlCertificateType.SetSelectedItem((entity.TypeID.ToString().Trim().IsNullOrEmpty()) ? 
                        String.Empty : entity.TypeID.ToString().Trim());

                    tbxDetails.Text = entity.Details;
                    tbxExpiryDate.Text = entity.ExpiryDate.ToString();
                    tbxPlaceIssued.Text = entity.PlaceIssued;
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
        WebUtil.ShowMessageBox(divMessage, "Requested Certification Details was not found.", true);
    }
    protected void SaveContactsCertification()
    {
        OMMDataContext context = new OMMDataContext();
        Certificate entity = null;

        if (_IsEditMode)
            entity = context.Certificates.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID); 
        else
        {
            entity = new Certificate();
            entity.ContactID = _ContactID;
            context.Certificates.InsertOnSubmit(entity);
        }



        if (!ddlCertificateType.SelectedValue.IsNullOrEmpty())
        {
            entity.TypeID = Convert.ToInt32(ddlCertificateType.SelectedValue);
        }
        entity.Details = tbxDetails.Text;
        if (tbxExpiryDate.Text.IsNullOrEmpty())
            entity.ExpiryDate = null;
        else
            entity.ExpiryDate = tbxExpiryDate.Text.ToDateTime(ConfigReader.CSharpCalendarDateFormat); //Convert.ToDateTime(tbxExpiryDate.Text);
        entity.PlaceIssued = tbxPlaceIssued.Text;

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
            SaveContactsCertification();
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
        BindCertificationList(e.PageIndex);
    }
}














