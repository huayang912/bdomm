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
                txtLastName.Text = contact.LastName;
                txtFirstNames.Text = contact.FirstNames;
                txtAddress.Text = contact.Address;
                txtPostcode.Text = contact.Postcode;
                ddlCountryID.SetSelectedItem(contact.CountryID.ToString());
                ddlMaritalStatusID.SetSelectedItem(contact.MaritalStatusID.ToString());
                txtPlaceOfBirth.Text = contact.PlaceOfBirth;
                txtDateOfBirth.Text = contact.DateOfBirth.HasValue ? contact.DateOfBirth.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat) : String.Empty;
                ddlCountryOfBirthID.SetSelectedItem(contact.CountryOfBirthID.ToString());
                txtDateOfLastMeeting.Text = contact.DateOfLastMeeting.HasValue ? contact.DateOfLastMeeting.GetValueOrDefault().ToString(ConfigReader.CSharpCalendarDateFormat) : String.Empty;
                txtPreferredDayRate.Text = contact.PreferredDayRate.HasValue ? contact.PreferredDayRate.GetValueOrDefault().ToString() : String.Empty;
                ddlDayRateCurrencyID.SetSelectedItem(contact.DayRateCurrencyID.GetValueOrDefault().ToString());
                chkNoSMSorEmail.Checked = contact.NoSMSorEmail;
                chkInactive.Checked = contact.Inactive;

                var telephoneNumbers = from P in context.TelephoneNumbers where P.ContactID == _ID select new App.CustomModels.PersonnelTelephone { ID = P.ID, Number = P.Number, TypeID = P.TypeID };
                if(telephoneNumbers != null && telephoneNumbers.Count() > 0)
                    hdnTelephoneNumbers.Value = telephoneNumbers.ToList().ToJSON();

                var emailAddress = from P in context.EmailAddresses where P.ContactID == _ID select new App.CustomModels.PersonnelEmail { ID = P.ID, Email= P.Address};
                if (emailAddress != null && emailAddress.Count() > 0)
                    hdnEmailAddresses.Value = emailAddress.ToList().ToJSON();

                var contactRoles = from P in context.ContactRoles where P.ContactID == _ID select new App.CustomModels.PersonnelRole { ID = P.ID, RoleID = P.RoleID, Order = P.RoleOrder };
                if (contactRoles != null && contactRoles.Count() > 0)
                    hdnContactRoles.Value = contactRoles.ToList().ToJSON();
            }
        }
    }
    protected void ShowNotFoundMessage()
    {
        pnlDetails.Visible = false;
        WebUtil.ShowMessageBox(divMessage, "Sorry! Requested Personnel Was not found.", true);
    }

    [WebMethod]
    public static int SavePersonnel(App.CustomModels.Personnel personnel, List<App.CustomModels.PersonnelTelephone> telephones, List<App.CustomModels.PersonnelEmail> emails, List<App.CustomModels.PersonnelRole> roles)
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
        if (roles != null && roles.Count > 0)
        {
            foreach (App.CustomModels.PersonnelRole role in roles)
            {
                ContactRole contactRole = null;
                if (role.ID > 0)
                    contactRole = context.ContactRoles.SingleOrDefault(P => P.ID == role.ID);
                else
                {
                    contactRole = new ContactRole();
                    contact.ContactRoles.Add(contactRole);                    
                }
                contactRole.RoleID = role.RoleID;
                contactRole.RoleOrder = role.Order;
                contactRole.ChangedByUserID = SessionCache.CurrentUser.ID;
                contactRole.ChangedOn = DateTime.Now;
            }
        }
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
        contact.CountryID = personnel.CountryID;
        contact.PlaceOfBirth = personnel.PlaceOfBirth;
        if (personnel.CountryOfBirthID.ToInt() > 0)
            contact.CountryOfBirthID = personnel.CountryOfBirthID.ToInt();
        else
            contact.CountryOfBirthID = null;
        contact.NoSMSorEmail = personnel.NoSMSOrEmail;
        contact.Inactive = personnel.InActive;
        
        ///Date of Birth (Nullable)
        if (personnel.DateOfBirth.IsNullOrEmpty())
            contact.DateOfBirth = null;
        else
            contact.DateOfBirth = personnel.DateOfBirth.ToDateTime(ConfigReader.CSharpCalendarDateFormat);
        
        ///Date of Last Meeting (Nullable)
        if (personnel.DateOfLastMeeting.IsNullOrEmpty())
            contact.DateOfLastMeeting = null;
        else
            contact.DateOfLastMeeting = personnel.DateOfLastMeeting.ToDateTime(ConfigReader.CSharpCalendarDateFormat);

        
        contact.PreferredDayRate = personnel.PreferredDayRate;
        if(contact.PreferredDayRate.HasValue)
            contact.DayRateCurrencyID = personnel.DayRateCurrencyID;

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
}
