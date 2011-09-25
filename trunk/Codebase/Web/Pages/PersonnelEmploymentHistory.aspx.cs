using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using App.Core.Extensions;
using App.Data;

public partial class Pages_PersonnelEmploymentHistory : BasePage
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
            BindEmploymentHistoryInfo();
            BindEmploymentHistoryList();
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

        Page.Title = WebUtil.GetPageTitle("Manage Employment History");
    }

    /// <summary>
    /// Binds Dropdownlists for the initial request.
    /// </summary>
    protected void BindDropDownLists()
    {
        //BindDropdownList.Clients(ddlClientID);
        //BindDropdownList.Contactses(ddlContactID);
        BindDropdownList.Projects(ddlProjectID);
        BindDropdownList.Roles(ddlRoleID);
        BindDropdownList.Currencies_EmpHistory(ddlCurrencyCode);
        //BindDropdownList.Userses(ddlChangedByUserID);

    }
    protected void ShowSuccessMessage()
    {
        if (String.Compare(WebUtil.GetQueryStringInString(AppConstants.QueryString.SUCCESS_MSG), "True", false) == 0)
            WebUtil.ShowMessageBox(divMessage, "Employment History Saved Successfully.", false);
    }
    /// <summary>
    /// Binds EmploymentHistory Info Requested through Query Strings
    /// </summary>
    protected void BindEmploymentHistoryInfo()
    {
        OMMDataContext context = new OMMDataContext();
        if (context.Contacts.FirstOrDefault(P => P.ID == _ContactID) == null)
            ShowNotFoundMessage();
        else
        {
            if (_IsEditMode)
            {
                EmploymentHistory entity = context.EmploymentHistories.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);//dao.GetByID(_ID);
                if (entity == null)
                    ShowNotFoundMessage();
                else
                {
                    //ddlContactID.SetSelectedItem(entity.ContactID.ToString());
                    txtStartDate.Text = entity.StartDate.HasValue ? entity.StartDate.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat) : String.Empty;
                    txtEndDate.Text = entity.EndDate.HasValue ? entity.EndDate.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat) : String.Empty;
                    
                    //ddlClientID.SetSelectedItem(entity.ClientID.GetValueOrDefault().ToString());
                    
                    txtDayRate.Text = entity.DayRate.HasValue ? String.Format(AppConstants.ValueOf.DECIMAL_FORMAT_FOR_TEXTBOX, entity.DayRate.GetValueOrDefault()) : String.Empty;
                    txtNotes.Text = entity.Notes;
                    //ddlChangedByUserID.SetSelectedItem(entity.ChangedByUserID.ToString());
                    //txtChangedOn.Text = entity.ChangedOn.ToString(ConfigReader.CSharpCalendarDateFormat);
                    //txtVersion.Text = entity.Version;

                    ddlRoleID.SetSelectedItem(entity.RoleID.GetValueOrDefault().ToString());
                    ddlCurrencyCode.SetSelectedItem(entity.CurrencyID.GetValueOrDefault().ToString());
                    ddlProjectID.SetSelectedItem(entity.ProjectID.GetValueOrDefault().ToString());
                    //ddlRateType.SetSelectedItem(entity.Office_Onsh_Rate_type.ToString());
                    //ddlHourStandby.SetSelectedItem(entity.Hour_Standby_Rate_type.ToString());

                    ddlRateType.SetSelectedItem((entity.Office_Onsh_Rate_type.IsNullOrEmpty()) ?
                        String.Empty : entity.Office_Onsh_Rate_type.ToString().Trim());                   

                    ddlHourStandby.SetSelectedItem((entity.Hour_Standby_Rate_type.IsNullOrEmpty()) ?
                       String.Empty : entity.Hour_Standby_Rate_type.ToString().Trim());
                   

                    if (entity.Contract_days.ToString().Trim() == "5")
                        radFive.Checked = true;
                    else if (entity.Contract_days.ToString().Trim() == "7")
                        radSeven.Checked = true;
                    else
                        radNull.Checked = true;

                    txtTravelRate.Text = entity.TravelRate.ToString();
                    txtTravelCost.Text = entity.TravelCost.ToString();
                    //txtCurrencyID.Text = entity.CurrencyID;
                    txtOffshoreRate.Text = entity.OffshoreRate.ToString();
                    //txtOfficeOnshRatetype.Text = entity.Office_Onsh_Rate_type;
                    txtOfficeOnshoreRate.Text = entity.OfficeOnshoreRate.ToString();
                    //txtHourStandbyRatetype.Text = entity.Hour_Standby_Rate_type;
                    txtHourStandbyRate.Text = entity.HourStandbyRate.ToString();
                    txtProjectCodeother.Text = entity.ProjectCode_other;
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
        WebUtil.ShowMessageBox(divMessage, "Requested Employment History was not found.", true);
    }
    protected void CheckAndDeleteData()
    {
        if (_IsDeleteMode)
        {
            OMMDataContext context = new OMMDataContext();
            var history = context.EmploymentHistories.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);
            if (history == null)
                WebUtil.ShowMessageBox(divMessage, "Sorry! requested Employment History was found for delete. Delete Failed.", true);
            else
            {
                context.EmploymentHistories.DeleteOnSubmit(history);
                try
                {
                    context.SubmitChanges();
                    WebUtil.ShowMessageBox(divMessage, "Employment History deleted successfully.", false);
                }
                catch
                {
                    WebUtil.ShowMessageBox(divMessage, "Sorry! this Employment History contains related information. Delete failed.", true);
                }
            }
        }
    }
    protected void BindEmploymentHistoryList()
    {
        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", _ContactID) };

        DataSet ds = dao.GetDataSet(AppSQL.GET_EMPLOYMENT_HISTORY_BY_CONTACT, parameters, false); //dao.GetPagedData(AppSQL.GET_NOTES_BY_CONTACT, parameters, pageNumber, PAGE_SIZE, out totalRecord);
        //Bind the List Control
        ucEmploymentHistoryList.DataSource = ds.Tables[0];
        ucEmploymentHistoryList.EditLink = Request.Url.AbsolutePath + "?" + AppConstants.QueryString.CONTACT_ID + "={0}&" + AppConstants.QueryString.ID + "={1}";
        ucEmploymentHistoryList.DeleteLink = Request.Url.AbsolutePath + "?" + AppConstants.QueryString.CONTACT_ID + "={0}&" + AppConstants.QueryString.ID + "={1}&" + AppConstants.QueryString.DELETE + "=True";
        ucEmploymentHistoryList.DataBind();

        ///Bind the Pager Control
        //ucNoteListPager.TotalRecord = totalRecord;
        //ucNoteListPager.PageNo = pageNumber;
        //ucNoteListPager.PageSize = PAGE_SIZE;
        //ucNoteListPager.DataBind();
    }
    protected void SaveEmploymentHistory()
    {
        OMMDataContext context = new OMMDataContext();
        EmploymentHistory entity = null;

        if (_IsEditMode)
            entity = context.EmploymentHistories.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID); //dao.GetByID(_ID);        
        else
        {
            entity = new EmploymentHistory();
            entity.ContactID = _ContactID;
            context.EmploymentHistories.InsertOnSubmit(entity);
        }

        //entity.ContactID = ddlContactID.SelectedValue.ToInt();
        if (txtStartDate.Text.IsNullOrEmpty())
            entity.StartDate = null;
        else
            entity.StartDate = txtStartDate.Text.ToDateTime(ConfigReader.CSharpCalendarDateFormat); ;
        if (txtEndDate.Text.IsNullOrEmpty())
            entity.EndDate = null;
        else
            entity.EndDate = txtEndDate.Text.ToDateTime(ConfigReader.CSharpCalendarDateFormat); ;

        if (ddlProjectID.SelectedValue.ToInt() == 0)
            entity.ProjectID = null;
        else
            entity.ProjectID = ddlProjectID.SelectedValue.ToInt();
        //if (ddlClientID.SelectedValue.ToInt() == 0)
        //    entity.ClientID = null;
        //else
        //    entity.ClientID = ddlClientID.SelectedValue.ToInt();


        if (ddlRoleID.SelectedValue.ToInt() == 0)
            entity.RoleID = null;
        else
            entity.RoleID = ddlRoleID.SelectedValue.ToInt();

        if (ddlCurrencyCode.SelectedValue.ToInt() == 0)
            entity.CurrencyID = null;
        else
            entity.CurrencyID = ddlCurrencyCode.SelectedValue.ToInt();

        if (txtDayRate.Text.IsNullOrEmpty())
            entity.DayRate = null;
        else
            entity.DayRate = txtDayRate.Text.ToDecimal();
        entity.Notes = txtNotes.Text;
        entity.ChangedByUserID = SessionCache.CurrentUser.ID;
        entity.ChangedOn = DateTime.Now;
        //entity.Version = txtVersion.Text;
        //if (txtContractdays.Text.Trim() != "")
        //    entity.Contract_days = Convert.ToInt32(txtContractdays.Text);

        if(radFive.Checked)
            entity.Contract_days = 5;
        else if(radSeven.Checked)
            entity.Contract_days = 7;
        else
            entity.Contract_days = null;


        if (txtTravelRate.Text.Trim() != "")
            entity.TravelRate = Convert.ToInt32(txtTravelRate.Text);

        entity.Office_Onsh_Rate_type = ddlRateType.SelectedValue;
        entity.Hour_Standby_Rate_type = ddlHourStandby.SelectedValue;


        if (txtTravelCost.Text.Trim() != "")
            entity.TravelCost = Convert.ToInt32(txtTravelCost.Text);
        //entity.CurrencyID = txtCurrencyID.Text;

        //if (txtContractdays.Text.Trim() != "")
        //    entity.OffshoreRate = Convert.ToDecimal(txtOffshoreRate.Text);
        
        //entity.Office_Onsh_Rate_type = txtOfficeOnshRatetype.Text;

        if (txtOfficeOnshoreRate.Text.Trim() != "")
            entity.OfficeOnshoreRate = Convert.ToDecimal(txtOfficeOnshoreRate.Text);

        //entity.Hour_Standby_Rate_type = txtHourStandbyRatetype.Text;

        if (txtOfficeOnshoreRate.Text.Trim() != "")
            entity.HourStandbyRate = Convert.ToDecimal(txtHourStandbyRate.Text);
        entity.ProjectCode_other = txtProjectCodeother.Text;
        context.SubmitChanges();
        String url = String.Format("{0}?{1}={2}&{3}=True"
            , Request.Url.AbsolutePath
            , AppConstants.QueryString.CONTACT_ID, _ContactID
            , AppConstants.QueryString.SUCCESS_MSG);
        Response.Redirect(url);        
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            SaveEmploymentHistory();
            //Response.Redirect(AppConstants.Pages.EMPLOYMENTHISTORY_LIST);
            return;
        }
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        //Response.Redirect(AppConstants.Pages.EMPLOYMENTHISTORY_LIST);
        //return;
    } 
}
