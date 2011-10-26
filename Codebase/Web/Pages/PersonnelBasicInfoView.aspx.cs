using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using App.Core.Extensions;
using App.Data;

public partial class Pages_PersonnelBasicInfoView : BasePage
{
    private int _ContactID = 0;
    private int _ID = 0;
    private bool _IsEditMode = false;
    private bool _IsDeleteMode = false;
    private const int PAGE_SIZE = 15;
    private string _DeleteOn = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            //CheckAndDeleteData();
            BindDropDownLists();
            BindEmploymentHistoryInfo();
            BindEmploymentHistoryList();
            BindRoleList();

            BindPersonnelInfo();
            
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
        _DeleteOn = WebUtil.GetQueryStringInString(AppConstants.QueryString.DeleteOn);
        _IsDeleteMode = String.Compare(WebUtil.GetQueryStringInString(AppConstants.QueryString.DELETE), "True", true) == 0 ? true : false;
        if (_ID > 0 && !_IsDeleteMode)
            _IsEditMode = true;

        Page.Title = WebUtil.GetPageTitle("Manage Employment History");
    }



    protected void BindPersonnelInfo()
    {
        if (_ContactID > 0)
        {
            OMMDataContext context = new OMMDataContext();
            var contact = context.Contacts.SingleOrDefault(P => P.ID == _ContactID);
            if (contact == null)
                ShowNotFoundMessage();
            else
            {
                lblDateOfLastMeeting.Text = contact.DateOfLastMeeting.HasValue ? contact.DateOfLastMeeting.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat) : String.Empty;

                lblPreferredDayRate.Text = contact.PreferredDayRate.HasValue ? contact.PreferredDayRate.GetValueOrDefault().ToString() : String.Empty;

                lblDayRateCurrencyID.Text = contact.DayRateCurrencyID.GetValueOrDefault().ToString().IsNullOrEmpty() ?
                    "" : contact.DayRateCurrencyID.GetValueOrDefault().ToString();

                chkNoSMSorEmail.Checked = contact.NoSMSorEmail;
                chkInactive.Checked = contact.Inactive;
                lblPPE_Size.Text = contact.PPE_Sizes.IsNullOrEmpty() ? "" : contact.PPE_Sizes;// ddlPPE_Size.SelectedItem.Text.ToString();

                lblCoverall.Text = contact.Coverall;

                lblBootsize.Text = contact.Boots.ToString();// ddlbootsize.SelectedItem.Text.ToString();
                

                //---------------------
                //Emploment Details
                //---------------------
                lblCompanyname.Text = contact.CopmpanyName;

                lblCompanyreg.Text = contact.CompanyReg;

                lblCompanyvat.Text = contact.CompanyVat;

                lblCompanyadr.Text = contact.CompanyAddress;

                lblEmploymentstatus.Text = contact.employment_status.IsNullOrEmpty() ?
                    "" : contact.employment_status;
                    
                    

                lblInsurance.Text = contact.Insurance.IsNullOrEmpty() ?
                    "" : contact.Insurance;

              
            }
        }
        else
        {
        }
    }



    /// <summary>
    /// Binds Dropdownlists for the initial request.
    /// </summary>
    protected void BindDropDownLists()
    {
        
    }

    protected void ShowSuccessMessage()
    {
        if (String.Compare(WebUtil.GetQueryStringInString(AppConstants.QueryString.SUCCESS_MSG), "True", false) == 0)
            WebUtil.ShowMessageBox(divMessage, "Saved Successfully.", false);
    }

    /// <summary>
    /// Binds EmploymentHistory Info Requested through Query Strings
    /// </summary>
    protected void BindEmploymentHistoryInfo()
    {
        OMMDataContext context = new OMMDataContext();
        if (context.Contacts.FirstOrDefault(P => P.ID == _ContactID) == null)
        {
            //pnlEmpHistory.Visible = true;
            ShowNotFoundMessage();
        }


        else
        {
            //pnlEmpHistory.Visible = false;

            if (_IsEditMode)
            {
                EmploymentHistory entity = context.EmploymentHistories.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);//dao.GetByID(_ID);
                if (entity == null)
                {
                    //pnlEmpHistory.Visible = false;
                    ShowNotFoundMessage();
                }
                else
                {
                    
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
        WebUtil.ShowMessageBox(divMessage, "Information was not found.", true);
    }

    

    protected void BindEmploymentHistoryList()
    {
        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", _ContactID) };

        DataSet ds = dao.GetDataSet(AppSQL.GET_EMPLOYMENT_HISTORY_BY_CONTACT, parameters, false); //dao.GetPagedData(AppSQL.GET_NOTES_BY_CONTACT, parameters, pageNumber, PAGE_SIZE, out totalRecord);
        //Bind the List Control
        ucEmploymentHistoryList.DataSource = ds.Tables[0];
        //ucEmploymentHistoryList.EditLink = Request.Url.AbsolutePath + "?" + AppConstants.QueryString.CONTACT_ID + "={0}&" + AppConstants.QueryString.ID + "={1}";
        //ucEmploymentHistoryList.DeleteLink = Request.Url.AbsolutePath + "?" + 
        //    AppConstants.QueryString.CONTACT_ID + "={0}&" + 
        //    AppConstants.QueryString.ID + "={1}&" +
        //    AppConstants.QueryString.DELETE + "=True&" +
        //    AppConstants.QueryString.DeleteOn + "=EH";
        ucEmploymentHistoryList.DataBind();

    }

    protected void BindRoleList()
    {
        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", _ContactID) };

        DataSet ds = dao.GetDataSet(AppSQL.GET_CONTACT_ROLES, parameters, false); //dao.GetPagedData(AppSQL.GET_NOTES_BY_CONTACT, parameters, pageNumber, PAGE_SIZE, out totalRecord);
        //Bind the List Control
        ucContactRoles.DataSource = ds.Tables[0];
        //ucContactRoles.EditLink = Request.Url.AbsolutePath + "?" + AppConstants.QueryString.CONTACT_ID + "={0}&" + AppConstants.QueryString.ID + "={1}";
        //ucContactRoles.DeleteLink = Request.Url.AbsolutePath + "?" + 
        //    AppConstants.QueryString.CONTACT_ID + "={0}&" + 
        //    AppConstants.QueryString.ID + "={1}&" + 
        //    AppConstants.QueryString.DELETE + "=True&"+
        //    AppConstants.QueryString.DeleteOn + "=R";
        ucContactRoles.DataBind();
    }

    

    protected void btnList_Click(object sender, EventArgs e)
    {
        //Response.Redirect(AppConstants.Pages.EMPLOYMENTHISTORY_LIST);
        //return;
    }


    

}
