using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Core.Extensions;
using System.Data;
using App.Data;

public partial class Pages_PersonnelDetails2 : BasePage
{
    protected int _ID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            BindPersonnelDetails();
        }
    }
    protected void BindPageInfo()
    {
        Page.Title = WebUtil.GetPageTitle("Personnel Details");
        _ID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
        this.Master.SelectedTab = SelectedTab.Personnel;
    }
    protected void BindPersonnelDetails()
    {
        OMMDataContext context = new OMMDataContext();
        Contact personnel = context.Contacts.SingleOrDefault(P => P.ID == _ID);
        if (personnel == null)
            ShowNotFoundMessage();
        else
        {
            lblLastName.Text = personnel.LastName.HtmlEncode();
            lblFirstNames.Text = personnel.FirstNames.HtmlEncode();
            lblAddress.Text = WebUtil.FormatText(personnel.Address);
            lblPostcode.Text = personnel.Postcode.IsNullOrEmpty() ? "NA" : personnel.Postcode.HtmlEncode();
            lblCountryID.Text = personnel.Country.Name;
            lblMaritalStatusID.Text = personnel.MaritalStatuse.Name;
            lblPlaceOfBirth.Text = personnel.PlaceOfBirth.IsNullOrEmpty() ? "NA" : personnel.PlaceOfBirth.HtmlEncode();
            lblDateOfBirth.Text = personnel.DateOfBirth.HasValue ? personnel.DateOfBirth.GetValueOrDefault().ToString(AppConstants.ValueOf.DATE_FROMAT_DISPLAY)
                : "NA";
            lblCountryOfBirthID.Text = personnel.Country1 == null ? "NA" :
                personnel.Country1.Name;

            ///Bind Telephone Numbers
            UtilityDAO db = new UtilityDAO();
            DbParameter[] parameters = new[] { new DbParameter("@ContactID", _ID) };
            DataSet ds = db.GetDataSet(AppSQL.GET_TELEPHONE_NUMBERS_BY_CONTACT, parameters, false);
            ucTelephoneNumberList.DataSource = ds.Tables[0];
            
            ///Bind Emails            
            DataSet emails = db.GetDataSet(AppSQL.GET_EMAILS_BY_CONTACT, new[]{ new DbParameter("@ContactID", _ID) }, false);
            ucEmailList.DataSource = emails.Tables[0];

            ///Bind Employment History                        

            DataSet histories = db.GetDataSet(AppSQL.GET_EMPLOYMENT_HISTORY_BY_CONTACT, new[] { new DbParameter("@ContactID", _ID) }, false); //dao.GetPagedData(AppSQL.GET_NOTES_BY_CONTACT, parameters, pageNumber, PAGE_SIZE, out totalRecord);
            //Bind the List Control
            ucEmploymentHistory.DataSource = histories.Tables[0];
        }
    }
    protected void ShowNotFoundMessage()
    {
        pnlDetails.Visible = false;
        WebUtil.ShowMessageBox(divMessage, "Sorry! Requested Personnel Was not found.", true);
    }
}
