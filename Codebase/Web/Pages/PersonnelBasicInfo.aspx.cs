using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Core.Extensions;
using System.Web.Services;

public partial class Pages_PersonnelBasicInfo : BasePage
{
    protected int _ID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            BindDropdownLists();
            BindPersonnelInfo();
        }
    }
    protected void BindPageInfo()
    {
        _ID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
        //if (_ID > 0)
        //    ltrHeading.Text = "Edit Personnel";
        //Page.Title = WebUtil.GetPageTitle(ltrHeading.Text);
        //lblStep1Title.Text = ltrHeading.Text;
        //lblStep1Title.Text = "Edit Personnel";
    }
    protected void BindDropdownLists()
    {
        BindDropdownList.Countries(ddlCountryID);
        BindDropdownList.Countries(ddlCountryOfBirthID);
        BindDropdownList.Currencies(ddlDayRateCurrencyID);
        BindDropdownList.MaritalStatuses(ddlMaritalStatusID);

        OMMDataContext context = new OMMDataContext();
        var roles = from P in context.Roles select new { P.ID, P.Name };
        if (roles != null && roles.Count() > 0)
            hdnRoles.Value = roles.ToList().ToJSON();
        
        var telephoneTypes = from P in context.TelephoneNumberTypes select new { P.ID, P.Name };
        if (telephoneTypes != null && telephoneTypes.Count() > 0)
            hdnTelephoneTypes.Value = telephoneTypes.ToList().ToJSON();

        var commTypes = from P in context.ContactCommsTypes select new { P.ID, P.Name };
        if (commTypes != null && commTypes.Count() > 0)
            hdnCommTypes.Value = commTypes.ToList().ToJSON();
    }
    protected void BindPersonnelInfo()
    {
        if (_ID > 0)
        {
            OMMDataContext context = new OMMDataContext();
            var contact = context.Contacts.SingleOrDefault(P => P.ID == _ID);
            if (contact == null)
                ShowNotFoundMessage();
            else
            {
                //---------------------
                //Personal Details
                //---------------------
                txtLastName.Text = contact.LastName;
                lblLastName.Text = contact.LastName;

                txtFirstNames.Text = contact.FirstNames;
                lblFirstNames.Text = contact.FirstNames;

                txtAddress.Text = contact.Address;
                lblAddress.Text = contact.Address;

                txtPostcode.Text = contact.Postcode;
                lblPostcode.Text = contact.Postcode;

                ddlCountryID.SetSelectedItem(contact.CountryID.ToString());
                lblCountryID.Text = ddlCountryID.SelectedItem.Text.ToString();
                                
                ddlMaritalStatusID.SetSelectedItem(contact.MaritalStatusID.ToString());
                lblMaritalStatusID.Text = ddlMaritalStatusID.SelectedItem.Text.ToString();

                txtPlaceOfBirth.Text = contact.PlaceOfBirth;
                lblPlaceOfBirth.Text = contact.PlaceOfBirth;

                txtDateOfBirth.Text = contact.DateOfBirth.HasValue ?
                    contact.DateOfBirth.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat) : String.Empty;
                lblDateOfBirth.Text = contact.DateOfBirth.HasValue ?
                    contact.DateOfBirth.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat) : String.Empty;

                ddlCountryOfBirthID.SetSelectedItem(contact.CountryOfBirthID.ToString());
                lblCountryOfBirthID.Text = ddlCountryOfBirthID.SelectedItem.Text.ToString();
                //End Personal Details


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

                txtcompanyreg.Text = contact.CompanyVat;
                lblCompanyreg.Text = contact.CompanyVat;

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


                var telephoneNumbers = from P in context.TelephoneNumbers where P.ContactID == _ID select new App.CustomModels.PersonnelTelephone { ID = P.ID, Number = P.Number, TypeID = P.TypeID };
                if (telephoneNumbers != null && telephoneNumbers.Count() > 0)
                    hdnTelephoneNumbers.Value = telephoneNumbers.ToList().ToJSON();

                var emailAddress = from P in context.EmailAddresses where P.ContactID == _ID select new App.CustomModels.PersonnelEmail { ID = P.ID, Email = P.Address };
                if (emailAddress != null && emailAddress.Count() > 0)
                    hdnEmailAddresses.Value = emailAddress.ToList().ToJSON();

                var contactRoles = from P in context.ContactRoles where P.ContactID == _ID select new App.CustomModels.PersonnelRole { ID = P.ID, RoleID = P.RoleID, Order = P.RoleOrder };
                if (contactRoles != null && contactRoles.Count() > 0)
                    hdnContactRoles.Value = contactRoles.ToList().ToJSON();

                var Notes =
                    from p in context.ContactsNotes
                    join cp in context.ContactCommsTypes on p.ContactCommsTypeID equals cp.ID
                    join u in context.Users on p.ChangedByUserID equals u.ID
                    where p.ContactID == _ID
                    select new App.CustomModels.ConNote
                    {
                        ID = p.ID,
                        Notes = p.Notes,
                        CommsTypeID = (p.ContactCommsTypeID == null) ?
                            "" : p.ContactCommsTypeID.ToString(),
                        CommsType = cp.Name,
                        ChangedBy = u.DisplayName,
                        ChangedOn = p.ChangedOn.ToString()

                    };
                //from P in context.ContactsNotes where P.ContactID == _ID select new App.CustomModels.ConNote { ID = P.ID, Notes = P.Notes, CommsTypeID = (P.ContactCommsTypeID == null) ? "" : P.ContactCommsTypeID.ToString() };
                if (Notes != null && Notes.Count() > 0)
                    hdnNotes.Value = Notes.ToList().ToJSON();


            }
            enableDisable(1);
        }
        else
        {
           enableDisable(0);
        }
    }
    protected void ShowNotFoundMessage()
    {
        pnlDetails.Visible = false;
        WebUtil.ShowMessageBox(divMessage, "Sorry! Requested Personnel Was not found.", true);
    }

    [WebMethod]
    public static int SavePersonnel(App.CustomModels.Personnel personnel, 
                                    List<App.CustomModels.PersonnelTelephone> telephones,
                                    List<App.CustomModels.ConNote> ConNotes,
                                    List<App.CustomModels.PersonnelEmail> emails, 
                                    List<App.CustomModels.PersonnelRole> roles)
    {
        OMMDataContext context = new OMMDataContext();
        Contact contact = populateContact(personnel, context);
        ///Bind Telephones
        if (telephones != null && telephones.Count > 0)
        {
            foreach (App.CustomModels.PersonnelTelephone telephone in telephones)
            {
                TelephoneNumber phone = null;
                if (telephone.ID > 0)
                    phone = context.TelephoneNumbers.SingleOrDefault(P => P.ID == telephone.ID);
                else
                {
                    phone = new TelephoneNumber();
                    contact.TelephoneNumbers.Add(phone);
                }
                phone.Number = telephone.Number;
                phone.TypeID = telephone.TypeID;
                phone.ChangedByUserID = SessionCache.CurrentUser.ID;
                phone.ChangedOn = DateTime.Now; 
            }
        }

        ///Bind Notes
        if (ConNotes != null && ConNotes.Count > 0)
        {
            foreach (App.CustomModels.ConNote conNote in ConNotes)
            {
                ContactsNote note = null;
                if (conNote.ID > 0)
                    note = context.ContactsNotes.SingleOrDefault(P => P.ID == conNote.ID);
                else
                {
                    note = new ContactsNote();
                    contact.ContactsNotes.Add(note);
                }
                note.Notes = conNote.Notes;
                if (conNote.CommsTypeID != null)
                    note.ContactCommsTypeID = Convert.ToInt32(conNote.CommsTypeID);

                note.ChangedByUserID = SessionCache.CurrentUser.ID;
                note.ChangedOn = DateTime.Now;
            }
        }

        ///Bind Eamils
        if (emails != null && emails.Count > 0)
        {
            foreach (App.CustomModels.PersonnelEmail email in emails)
            {
                EmailAddress contactEmail = null;
                if (email.ID > 0)
                    contactEmail = context.EmailAddresses.SingleOrDefault(P => P.ID == email.ID);
                else
                {
                    contactEmail = new EmailAddress();
                    contact.EmailAddresses.Add(contactEmail);
                }                
                contactEmail.Address = email.Email;
                contactEmail.ChangedByUserID = SessionCache.CurrentUser.ID;
                contactEmail.ChangedOn = DateTime.Now;
            }
        }
        ///Roles
      
        context.SubmitChanges();
        return contact.ID;
    }

    private static Contact populateContact(App.CustomModels.Personnel personnel, OMMDataContext context)
    {
        WebUtil.LoginUser();
        Contact contact = null;
        if (personnel.ID > 0)
            contact = context.Contacts.SingleOrDefault(P => P.ID == personnel.ID);
        else
        {
            contact = new Contact();
            contact.CreatedByUserID = SessionCache.CurrentUser.ID;
            contact.CreatedOn = DateTime.Now;            
            context.Contacts.InsertOnSubmit(contact);             
        }
        contact.FirstNames = personnel.FirstName;
        contact.LastName = personnel.LastName;
        contact.Address = personnel.Address;
        contact.MaritalStatusID = personnel.MaritalStatus;
        contact.Postcode = personnel.PostCode;
      //  contact.CountryID = personnel.CountryID;

        if (personnel.CountryID.ToInt() > 0)
            contact.CountryID = personnel.CountryID.ToInt();
        else
            contact.CountryID = null;
      
        
        contact.PlaceOfBirth = personnel.PlaceOfBirth;
       

        if (personnel.CountryOfBirthID.ToInt() > 0)
            contact.CountryOfBirthID = personnel.CountryOfBirthID.ToInt();
        else
            contact.CountryOfBirthID = null;
       
        
        ///Date of Birth (Nullable)
        if (personnel.DateOfBirth.IsNullOrEmpty())
            contact.DateOfBirth = null;
        else
            contact.DateOfBirth = personnel.DateOfBirth.ToDateTime(ConfigReader.CSharpCalendarDateFormat);
        
    
        
     
        contact.ChangedOn = DateTime.Now;
        contact.ChangedByUserID = SessionCache.CurrentUser.ID;

        return contact;
    }

    #region Delete PageMethods
    [WebMethod]
    public static bool DeleteTelephone(int id)
    {
        OMMDataContext context = new OMMDataContext();
        var phone = context.TelephoneNumbers.SingleOrDefault(P => P.ID == id);
        if (phone != null)
        {
            context.TelephoneNumbers.DeleteOnSubmit(phone);
            context.SubmitChanges();
        }
        return true;
    }

    [WebMethod]
    public static bool DeleteNotes(int id)
    {
        OMMDataContext context = new OMMDataContext();
        var Notes = context.ContactsNotes.SingleOrDefault(P => P.ID == id);
        if (Notes != null)
        {
            context.ContactsNotes.DeleteOnSubmit(Notes);
            context.SubmitChanges();
        }
        return true;
    }

    [WebMethod]
    public static bool DeleteRole(int id)
    {
        OMMDataContext context = new OMMDataContext();
        var role = context.ContactRoles.SingleOrDefault(P => P.ID == id);
        if (role != null)
        {
            context.ContactRoles.DeleteOnSubmit(role);
            context.SubmitChanges();
        }
        return true;
    }
    [WebMethod]
    public static bool DeleteEmail(int id)
    {
        OMMDataContext context = new OMMDataContext();
        var email = context.EmailAddresses.SingleOrDefault(P => P.ID == id);
        if (email != null)
        {
            context.EmailAddresses.DeleteOnSubmit(email);
            context.SubmitChanges();
        }
        return true;
    }
    #endregion Delete PageMethods
    protected void ddlemploymentstatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }




    protected void Button1_Click(object sender, EventArgs e)
    {
        enableDisable(2);
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

            Button1.Visible = false;
        }
    }
}
