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
    private string _DeleteOn = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            CheckAndDeleteData();
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
                ////---------------------
                ////Personal Details
                ////---------------------
                //txtLastName.Text = contact.LastName;
                //lblLastName.Text = contact.LastName;

                //txtFirstNames.Text = contact.FirstNames;
                //lblFirstNames.Text = contact.FirstNames;

                //txtAddress.Text = contact.Address;
                //lblAddress.Text = contact.Address;

                //txtPostcode.Text = contact.Postcode;
                //lblPostcode.Text = contact.Postcode;

                //ddlCountryID.SetSelectedItem(contact.CountryID.ToString());
                //lblCountryID.Text = ddlCountryID.SelectedItem.Text.ToString();

                //ddlMaritalStatusID.SetSelectedItem(contact.MaritalStatusID.ToString());
                //lblMaritalStatusID.Text = ddlMaritalStatusID.SelectedItem.Text.ToString();

                //txtPlaceOfBirth.Text = contact.PlaceOfBirth;
                //lblPlaceOfBirth.Text = contact.PlaceOfBirth;

                //txtDateOfBirth.Text = contact.DateOfBirth.HasValue ?
                //    contact.DateOfBirth.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat) : String.Empty;
                //lblDateOfBirth.Text = contact.DateOfBirth.HasValue ?
                //    contact.DateOfBirth.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat) : String.Empty;

                //ddlCountryOfBirthID.SetSelectedItem(contact.CountryOfBirthID.ToString());
                //lblCountryOfBirthID.Text = ddlCountryOfBirthID.SelectedItem.Text.ToString();
                ////End Personal Details


                //---------------------
                //Other Details
                //---------------------
                txtDateOfLastMeeting.Text = contact.DateOfLastMeeting.HasValue ? contact.DateOfLastMeeting.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat) : String.Empty;
                lblDateOfLastMeeting.Text = contact.DateOfLastMeeting.HasValue ? contact.DateOfLastMeeting.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat) : String.Empty;

                txtPreferredDayRate.Text = contact.PreferredDayRate.HasValue ? contact.PreferredDayRate.GetValueOrDefault().ToString() : String.Empty;
                lblPreferredDayRate.Text = contact.PreferredDayRate.HasValue ? contact.PreferredDayRate.GetValueOrDefault().ToString() : String.Empty;

                ddlDayRateCurrencyID.SetSelectedItem(contact.DayRateCurrencyID.GetValueOrDefault().ToString());
                lblDayRateCurrencyID.Text = ddlDayRateCurrencyID.SelectedItem.Text.ToString();

                chkNoSMSorEmail.Checked = contact.NoSMSorEmail;
                chkInactive.Checked = contact.Inactive;
                // txtPPESizes.Text = contact.PPE_Sizes;
                ddlPPE_Size.SetSelectedItem(contact.PPE_Sizes);
                lblPPE_Size.Text = ddlPPE_Size.SelectedItem.Text.ToString();

                txtcoverall.Text = contact.Coverall;
                lblCoverall.Text = contact.Coverall;

                // txtbootsize.Text = contact.Boots.HasValue ? contact.Boots.GetValueOrDefault().ToString() : string.Empty;
                ddlbootsize.SetSelectedItem(contact.Boots.ToString());
                lblBootsize.Text = ddlbootsize.SelectedItem.Text.ToString();
                //End Other details


                //---------------------
                //Emploment Details
                //---------------------
                txtcompanyname.Text = contact.CopmpanyName;
                lblCompanyname.Text = contact.CopmpanyName;

                txtcompanyreg.Text = contact.CompanyReg;
                lblCompanyreg.Text = contact.CompanyReg;

                txtcompanyvat.Text = contact.CompanyVat;
                lblCompanyvat.Text = contact.CompanyVat;

                txtcompanyadr.Text = contact.CompanyAddress;
                lblCompanyadr.Text = contact.CompanyAddress;

                //   ddlemploymentstatus.SetSelectedItem(contact.employment_status.ToString());
                //  ddlinsurance.SetSelectedItem(contact.Insurance.ToString());
                ddlemploymentstatus.SetSelectedItem(contact.employment_status);
                lblEmploymentstatus.Text = ddlemploymentstatus.SelectedItem.Text.ToString();

                ddlinsurance.SetSelectedItem(contact.Insurance);
                lblInsurance.Text = ddlinsurance.SelectedItem.Text.ToString();
                //End Employment Details


                //var telephoneNumbers = from P in context.TelephoneNumbers where P.ContactID == _ID select new App.CustomModels.PersonnelTelephone { ID = P.ID, Number = P.Number, TypeID = P.TypeID };
                //if (telephoneNumbers != null && telephoneNumbers.Count() > 0)
                //    hdnTelephoneNumbers.Value = telephoneNumbers.ToList().ToJSON();

                //var emailAddress = from P in context.EmailAddresses where P.ContactID == _ID select new App.CustomModels.PersonnelEmail { ID = P.ID, Email = P.Address };
                //if (emailAddress != null && emailAddress.Count() > 0)
                //    hdnEmailAddresses.Value = emailAddress.ToList().ToJSON();

                //var contactRoles = from P in context.ContactRoles where P.ContactID == _ID select new App.CustomModels.PersonnelRole { ID = P.ID, RoleID = P.RoleID, Order = P.RoleOrder };
                //if (contactRoles != null && contactRoles.Count() > 0)
                //    hdnContactRoles.Value = contactRoles.ToList().ToJSON();

                //var Notes =
                //    from p in context.ContactsNotes
                //    join cp in context.ContactCommsTypes on p.ContactCommsTypeID equals cp.ID
                //    join u in context.Users on p.ChangedByUserID equals u.ID
                //    where p.ContactID == _ID
                //    select new App.CustomModels.ConNote
                //    {
                //        ID = p.ID,
                //        Notes = p.Notes,
                //        CommsTypeID = (p.ContactCommsTypeID == null) ?
                //            "" : p.ContactCommsTypeID.ToString(),
                //        CommsType = cp.Name,
                //        ChangedBy = u.DisplayName,
                //        ChangedOn = p.ChangedOn.ToString()

                //    };
                ////from P in context.ContactsNotes where P.ContactID == _ID select new App.CustomModels.ConNote { ID = P.ID, Notes = P.Notes, CommsTypeID = (P.ContactCommsTypeID == null) ? "" : P.ContactCommsTypeID.ToString() };
                //if (Notes != null && Notes.Count() > 0)
                //    hdnNotes.Value = Notes.ToList().ToJSON();


            }
            enableDisable(1);
        }
        else
        {
            enableDisable(0);
        }
    }

    public void enableDisable(int i)
    {
        if (i == 1)
        {

            txtLastName.Visible = false;
            lblLastName.Visible = true;

            txtFirstNames.Visible = false;
            lblFirstNames.Visible = true;

            txtAddress.Visible = false;
            lblAddress.Visible = true;

            txtPostcode.Visible = false;
            lblPostcode.Visible = true;

            txtPlaceOfBirth.Visible = false;
            lblPlaceOfBirth.Visible = true;

            txtDateOfBirth.Visible = false;
            lblDateOfBirth.Visible = true;

            ddlCountryID.Visible = false;
            lblCountryID.Visible = true;

            ddlMaritalStatusID.Visible = false;
            lblMaritalStatusID.Visible = true;

            ddlCountryOfBirthID.Visible = false;
            lblCountryOfBirthID.Visible = true;

            txtcompanyname.Visible = false;
            lblCompanyname.Visible = true;

            txtcompanyreg.Visible = false;
            lblCompanyreg.Visible = true;

            txtcompanyvat.Visible = false;
            lblCompanyvat.Visible = true;

            txtcompanyadr.Visible = false;
            lblCompanyadr.Visible = true;

            ddlemploymentstatus.Visible = false;
            lblEmploymentstatus.Visible = true;

            ddlinsurance.Visible = false;
            lblInsurance.Visible = true;


            txtDateOfLastMeeting.Visible = false;
            lblDateOfLastMeeting.Visible = true;

            txtPreferredDayRate.Visible = false;
            lblPreferredDayRate.Visible = true;

            ddlDayRateCurrencyID.Visible = false;
            lblDayRateCurrencyID.Visible = true;


            ddlPPE_Size.Visible = false;
            lblPPE_Size.Visible = true;

            txtcoverall.Visible = false;
            lblCoverall.Visible = true;

            ddlbootsize.Visible = false;
            lblBootsize.Visible = true;

            pnlEditButton.Visible = true;
            pnlSaveButton.Visible = false;
        }
        else
        {
            txtLastName.Visible = true;
            lblLastName.Visible = false;

            txtFirstNames.Visible = true;
            lblFirstNames.Visible = false;

            txtAddress.Visible = true;
            lblAddress.Visible = false;

            txtPostcode.Visible = true;
            lblPostcode.Visible = false;

            txtPlaceOfBirth.Visible = true;
            lblPlaceOfBirth.Visible = false;

            txtDateOfBirth.Visible = true;
            lblDateOfBirth.Visible = false;

            ddlCountryID.Visible = true;
            lblCountryID.Visible = false;


            ddlMaritalStatusID.Visible = true;
            lblMaritalStatusID.Visible = false;

            ddlCountryOfBirthID.Visible = true;
            lblCountryOfBirthID.Visible = false;



            txtcompanyname.Visible = true;
            lblCompanyname.Visible = false;

            txtcompanyreg.Visible = true;
            lblCompanyreg.Visible = false;

            txtcompanyvat.Visible = true;
            lblCompanyvat.Visible = false;

            txtcompanyadr.Visible = true;
            lblCompanyadr.Visible = false;

            ddlemploymentstatus.Visible = true;
            lblEmploymentstatus.Visible = false;

            ddlinsurance.Visible = true;
            lblInsurance.Visible = false;



            txtDateOfLastMeeting.Visible = true;
            lblDateOfLastMeeting.Visible = false;

            txtPreferredDayRate.Visible = true;
            lblPreferredDayRate.Visible = false;

            ddlDayRateCurrencyID.Visible = true;
            lblDayRateCurrencyID.Visible = false;


            ddlPPE_Size.Visible = true;
            lblPPE_Size.Visible = false;

            txtcoverall.Visible = true;
            lblCoverall.Visible = false;

            ddlbootsize.Visible = true;
            lblBootsize.Visible = false;

            pnlEditButton.Visible = false;
            pnlSaveButton.Visible = true;
        }
    }

    /// <summary>
    /// Binds Dropdownlists for the initial request.
    /// </summary>
    protected void BindDropDownLists()
    {
        BindDropdownList.Currencies(ddlDayRateCurrencyID);
        //BindDropdownList.Clients(ddlClientID);
        //BindDropdownList.Contactses(ddlContactID);
        BindDropdownList.Projects(ddlProjectID);
        BindDropdownList.Roles(ddlRoleID);

        BindDropdownList.RolesNoBlank(ddlRoles);
        BindDropdownList.Currencies_EmpHistory(ddlCurrencyCode);
        //BindDropdownList.Userses(ddlChangedByUserID);

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
                    pnlEmpHistory.Visible = false;
                    ShowNotFoundMessage();
                }
                else
                {
                    pnlEmpHistory.Visible = true;

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
            if (_DeleteOn == "R")
            {
                OMMDataContext context = new OMMDataContext();
                var conRoles = context.ContactRoles.FirstOrDefault(P => P.ID == _ID && P.ContactID == _ContactID);
                if (conRoles == null)
                    WebUtil.ShowMessageBox(divMessage, "Sorry! requested Contact Roles was found for delete. Delete Failed.", true);
                else
                {
                    context.ContactRoles.DeleteOnSubmit(conRoles);
                    try
                    {
                        context.SubmitChanges();
                        WebUtil.ShowMessageBox(divMessage, "Contact Roles deleted successfully.", false);
                    }
                    catch
                    {
                        WebUtil.ShowMessageBox(divMessage, "Sorry! this Contact Roles contains related information. Delete failed.", true);
                    }
                }
            }
            if (_DeleteOn == "EH")
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
    }

    protected void BindEmploymentHistoryList()
    {
        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", _ContactID) };

        DataSet ds = dao.GetDataSet(AppSQL.GET_EMPLOYMENT_HISTORY_BY_CONTACT, parameters, false); //dao.GetPagedData(AppSQL.GET_NOTES_BY_CONTACT, parameters, pageNumber, PAGE_SIZE, out totalRecord);
        //Bind the List Control
        ucEmploymentHistoryList.DataSource = ds.Tables[0];
        ucEmploymentHistoryList.EditLink = Request.Url.AbsolutePath + "?" + AppConstants.QueryString.CONTACT_ID + "={0}&" + AppConstants.QueryString.ID + "={1}";
        ucEmploymentHistoryList.DeleteLink = Request.Url.AbsolutePath + "?" + 
            AppConstants.QueryString.CONTACT_ID + "={0}&" + 
            AppConstants.QueryString.ID + "={1}&" +
            AppConstants.QueryString.DELETE + "=True&" +
            AppConstants.QueryString.DeleteOn + "=EH";
        ucEmploymentHistoryList.DataBind();

        ///Bind the Pager Control
        //ucNoteListPager.TotalRecord = totalRecord;
        //ucNoteListPager.PageNo = pageNumber;
        //ucNoteListPager.PageSize = PAGE_SIZE;
        //ucNoteListPager.DataBind();
    }

    protected void BindRoleList()
    {
        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", _ContactID) };

        DataSet ds = dao.GetDataSet(AppSQL.GET_CONTACT_ROLES, parameters, false); //dao.GetPagedData(AppSQL.GET_NOTES_BY_CONTACT, parameters, pageNumber, PAGE_SIZE, out totalRecord);
        //Bind the List Control
        ucContactRoles.DataSource = ds.Tables[0];
        //ucContactRoles.EditLink = Request.Url.AbsolutePath + "?" + AppConstants.QueryString.CONTACT_ID + "={0}&" + AppConstants.QueryString.ID + "={1}";
        ucContactRoles.DeleteLink = Request.Url.AbsolutePath + "?" + 
            AppConstants.QueryString.CONTACT_ID + "={0}&" + 
            AppConstants.QueryString.ID + "={1}&" + 
            AppConstants.QueryString.DELETE + "=True&"+
            AppConstants.QueryString.DeleteOn + "=R";
        ucContactRoles.DataBind();
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


    protected void btnSaveSkills_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            SaveSkills();
            return;
        }
    }

    protected void SaveSkills()
    {
        OMMDataContext context = new OMMDataContext();
        if (context.Contacts.FirstOrDefault(P => P.ID == _ContactID) != null)
        {
            ////pnlEmpHistory.Visible = true;
            //ShowNotFoundMessage();
            Contact entity = context.Contacts.FirstOrDefault(P => P.ID == _ContactID);



            if (txtDateOfLastMeeting.Text.IsNullOrEmpty())
                entity.DateOfLastMeeting = null;
            else
                entity.DateOfLastMeeting =
                    txtDateOfLastMeeting.Text.ToDateTime(ConfigReader.CSharpCalendarDateFormat); ;

            if (txtPreferredDayRate.Text.IsNullOrEmpty())
                entity.PreferredDayRate = null;
            else
                entity.PreferredDayRate =
                    Convert.ToInt32(Convert.ToDouble(txtPreferredDayRate.Text));

            entity.PPE_Sizes = ddlPPE_Size.SelectedItem.Text;

            entity.Coverall = (txtcoverall.Text.IsNullOrEmpty() ? null : txtcoverall.Text);
            entity.Boots = Convert.ToInt32(ddlbootsize.SelectedItem.Text);

            entity.NoSMSorEmail = chkNoSMSorEmail.Checked ? true : false;
            entity.Inactive = chkInactive.Checked ? true : false;

            entity.CopmpanyName = (txtcompanyname.Text.IsNullOrEmpty() ? null : txtcompanyname.Text);
            entity.CompanyReg = (txtcompanyreg.Text.IsNullOrEmpty() ? null : txtcompanyreg.Text);
            entity.CompanyVat = (txtcompanyvat.Text.IsNullOrEmpty() ? null : txtcompanyvat.Text);
            entity.CompanyAddress = (txtcompanyadr.Text.IsNullOrEmpty() ? null : txtcompanyadr.Text);


            entity.employment_status = ddlemploymentstatus.SelectedItem.Text;
            entity.Insurance = ddlinsurance.SelectedItem.Text; 

            //if (txtEndDate.Text.IsNullOrEmpty())
            //    entity.EndDate = null;
            //else
            //    entity.EndDate = txtEndDate.Text.ToDateTime(ConfigReader.CSharpCalendarDateFormat); ;

            //if (ddlProjectID.SelectedValue.ToInt() == 0)
            //    entity.ProjectID = null;
            //else
            //    entity.ProjectID = ddlProjectID.SelectedValue.ToInt();
            ////if (ddlClientID.SelectedValue.ToInt() == 0)
            ////    entity.ClientID = null;
            ////else
            ////    entity.ClientID = ddlClientID.SelectedValue.ToInt();


            //if (ddlRoleID.SelectedValue.ToInt() == 0)
            //    entity.RoleID = null;
            //else
            //    entity.RoleID = ddlRoleID.SelectedValue.ToInt();

            //if (ddlCurrencyCode.SelectedValue.ToInt() == 0)
            //    entity.CurrencyID = null;
            //else
            //    entity.CurrencyID = ddlCurrencyCode.SelectedValue.ToInt();

            //if (txtDayRate.Text.IsNullOrEmpty())
            //    entity.DayRate = null;
            //else
            //    entity.DayRate = txtDayRate.Text.ToDecimal();
            //entity.Notes = txtNotes.Text;
            //entity.ChangedByUserID = SessionCache.CurrentUser.ID;
            //entity.ChangedOn = DateTime.Now;
            ////entity.Version = txtVersion.Text;
            ////if (txtContractdays.Text.Trim() != "")
            ////    entity.Contract_days = Convert.ToInt32(txtContractdays.Text);

            //if (radFive.Checked)
            //    entity.Contract_days = 5;
            //else if (radSeven.Checked)
            //    entity.Contract_days = 7;
            //else
            //    entity.Contract_days = null;


            //if (txtTravelRate.Text.Trim() != "")
            //    entity.TravelRate = Convert.ToInt32(txtTravelRate.Text);

            //entity.Office_Onsh_Rate_type = ddlRateType.SelectedValue;
            //entity.Hour_Standby_Rate_type = ddlHourStandby.SelectedValue;


            //if (txtTravelCost.Text.Trim() != "")
            //    entity.TravelCost = Convert.ToInt32(txtTravelCost.Text);
            ////entity.CurrencyID = txtCurrencyID.Text;

            ////if (txtContractdays.Text.Trim() != "")
            ////    entity.OffshoreRate = Convert.ToDecimal(txtOffshoreRate.Text);

            ////entity.Office_Onsh_Rate_type = txtOfficeOnshRatetype.Text;

            //if (txtOfficeOnshoreRate.Text.Trim() != "")
            //    entity.OfficeOnshoreRate = Convert.ToDecimal(txtOfficeOnshoreRate.Text);

            ////entity.Hour_Standby_Rate_type = txtHourStandbyRatetype.Text;

            //if (txtOfficeOnshoreRate.Text.Trim() != "")
            //    entity.HourStandbyRate = Convert.ToDecimal(txtHourStandbyRate.Text);
            //entity.ProjectCode_other = txtProjectCodeother.Text;


            context.SubmitChanges();



            String url = String.Format("{0}?{1}={2}&{3}=True"
                , Request.Url.AbsolutePath
                , AppConstants.QueryString.CONTACT_ID, _ContactID
                , AppConstants.QueryString.SUCCESS_MSG);
            Response.Redirect(url);

        }
    }

    protected void btnAddRoles_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            OMMDataContext context = new OMMDataContext();
            if (context.ContactRoles.FirstOrDefault(P => P.ContactID == _ContactID 
                && P.RoleID == Convert.ToInt32(ddlRoles.SelectedValue)) == null)
            {

                ContactRole entity = null;

                entity = new ContactRole();
                entity.ContactID = _ContactID;
                entity.RoleID = Convert.ToInt32(ddlRoles.SelectedValue);
                entity.ChangedByUserID = SessionCache.CurrentUser.ID;
                entity.ChangedOn = DateTime.Now;

                context.ContactRoles.InsertOnSubmit(entity);
                context.SubmitChanges();

                String url = String.Format("{0}?{1}={2}&{3}=True"
                    , Request.Url.AbsolutePath
                    , AppConstants.QueryString.CONTACT_ID, _ContactID
                    , AppConstants.QueryString.SUCCESS_MSG);
                Response.Redirect(url);
            }
        }
    }

    protected void btnEnableEdit_Click(object sender, EventArgs e)
    {
        enableDisable(2);
    }
}
