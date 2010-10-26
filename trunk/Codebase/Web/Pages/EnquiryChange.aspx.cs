using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BUDI2_NS.Data;
using System.Web.Services;
using App.CustomModels;
using App.Core.Extensions;

public partial class Pages_EnquiryChange : BasePage
{
    private const String ENQUIRY_ID = "ID";
    private bool _IsEditMode = false;
    protected int _EnquiryID = 0;    
    protected int _StatusID = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if(!IsPostBack)
        {
            BindDropdownList();
            BindEnquiryDetails();
        }
    }
    /// <summary>
    /// Binds Global Page Information 
    /// </summary>
    protected void BindPageInfo()
    {
        _EnquiryID = WebUtil.GetQueryStringInInt(ENQUIRY_ID);        
        if (_EnquiryID > 0)
        {
            _IsEditMode = true;
            ltrHeading.Text = "Edit Enquiry";            
        }
        Page.Title = WebUtil.GetPageTitle(ltrHeading.Text);
    }
    /// <summary>
    /// Binds Dropdown Lists for this page
    /// </summary>
    protected void BindDropdownList()
    {
        OMMDataContext context = new OMMDataContext();
        ///Bind the Contacs DropdownList
        var contacts = from P in context.ClientContacts
                       orderby P.Client.Name, P.Name
                       select new
                       {
                           P.ID,
                           Name = String.Format("{0} ({1})", P.Client.Name, P.Name)
                       };
        ddlContact.DataSource = contacts;
        ddlContact.DataTextField = "Name";
        ddlContact.DataValueField = "ID";        
        ddlContact.DataBind();
        ddlContact.Items.Insert(0, new ListItem(String.Empty, String.Empty));

        ///Bind Enquiry Type Dropdown List
        var enquiryTypes = from P in context.EnquiryTypes
                           orderby P.Name
                           select new
                           {
                               ID = P.ID,
                               Name = P.Name
                           };
        ddlEnquiryType.DataSource = enquiryTypes;
        ddlEnquiryType.DataTextField = "Name";
        ddlEnquiryType.DataValueField = "ID";
        ddlEnquiryType.DataBind();

        // NEw EnqSouretype -Momin
        ///Bind EnquirySource Type Dropdown List
        var enquirySourceTypes = from P in context.EnquirySourceTypes
                           orderby P.Name
                           select new
                           {
                               ID = P.ID,
                               Name = P.Name
                           };

        ddlEnquirySourceTypes.DataSource = enquirySourceTypes;
        ddlEnquirySourceTypes.DataTextField = "Name";
        ddlEnquirySourceTypes.DataValueField = "ID";
        ddlEnquirySourceTypes.DataBind();


    }
    /// <summary>
    /// Binds Enquiry Details for Edit Mode
    /// </summary>
    protected void BindEnquiryDetails()
    {
        if (_IsEditMode)
        {
            OMMDataContext context = new OMMDataContext();
            Enquiry enquiry = context.Enquiries.SingleOrDefault(P => P.ID == _EnquiryID);
            if (enquiry == null)
                ShowNotFoundMessage();
            else
            {
                _StatusID = enquiry.StatusID;
                txtClientName.Text = enquiry.ClientContact.Client.Name;
                txtContactName.Text = enquiry.ClientContact.Name;
                txtJobTitle.Text = enquiry.ClientContact.JobTitle;
                if (enquiry.ClientContact.Country != null)
                    txtCountry.Text = enquiry.ClientContact.Country.Name;                

                ddlEnquiryType.SetSelectedItem(enquiry.TypeID.ToString());
                txtDetails.Text = enquiry.EnquiryLines.Count > 0 ? enquiry.EnquiryLines[0].Details : String.Empty;
            
            //Momin
                ddlEnquirySourceTypes.SetSelectedItem(enquiry.SourceTypeID.ToString());
            }
        }
    }
    /// <summary>
    /// Shows a not found message in the UI
    /// </summary>
    protected void ShowNotFoundMessage()
    {
        WebUtil.ShowMessageBox(divMessage, "Sorry! the requested Enquiry was not found.", true);
        pnlDetails.Visible = false;
    }
    /// <summary>
    /// Page Method to Save Enquiry (Ajax Method)
    /// </summary>
    /// <param name="customEnquiry"></param>
    /// <returns></returns>
    [WebMethod]
    public static String SaveEnquiry(App.CustomModels.CustomEnquiry customEnquiry)
    {        
        OMMDataContext context = new OMMDataContext();
        Enquiry enquiry = null;
        if (customEnquiry.ID == 0)
        {
            enquiry = new Enquiry();
            context.Enquiries.InsertOnSubmit(enquiry);
        }
        else
            enquiry = context.Enquiries.SingleOrDefault(P => P.ID == customEnquiry.ID);

        MapEnquiry(customEnquiry, enquiry, context);
        
        context.SubmitChanges();
        SaveEnquiryLineItems(customEnquiry, enquiry, context);
        return String.Format("{0}:{1}", enquiry.ID, enquiry.Number);
    }
    /// <summary>
    /// Saves Enquiry Line Items
    /// </summary>
    /// <param name="customEnquiry"></param>
    /// <param name="enquiry"></param>
    /// <param name="context"></param>
    private static void SaveEnquiryLineItems(CustomEnquiry customEnquiry, Enquiry enquiry, OMMDataContext context)
    {        
        EnquiryLine enquiryLine = new EnquiryLine();
        if (enquiry.EnquiryLines.Count > 0)
            enquiryLine = enquiry.EnquiryLines[0];
        else
        {
            enquiry.EnquiryLines.Add(enquiryLine);
            context.EnquiryLines.InsertOnSubmit(enquiryLine);
        }
        
        enquiryLine.Details = customEnquiry.Details;
        enquiryLine.EnquiryID = enquiry.ID;
        enquiryLine.ChangedByUserID = SessionCache.CurrentUser.ID;
        enquiryLine.ChangedOn = DateTime.Now;

        context.SubmitChanges();        
    }
    /// <summary>
    /// Prepares the object to save with LINQ
    /// </summary>
    /// <param name="customEnquiry"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    private static void MapEnquiry(App.CustomModels.CustomEnquiry customEnquiry, Enquiry enquiry, OMMDataContext context)
    {   
        enquiry.ContactID = customEnquiry.ContactID;
        enquiry.TypeID = customEnquiry.TypeID;
        enquiry.StatusID = customEnquiry.StatusID;
        
        //if(customEnquiry.StatusID == EnquiryStatus.New)
        if (customEnquiry.ID == 0) ///Insert Mode
        {
            enquiry.Number = context.GenerateNewEnquiryNumber(enquiry.TypeID);
            ///Source Type ID will not be editable once set.
            enquiry.SourceTypeID = customEnquiry.SourceTypeID;
            enquiry.CreatedByUserID = SessionCache.CurrentUser.ID;
            enquiry.CreatedByUsername = SessionCache.CurrentUser.UserName;
            enquiry.CreatedOn = DateTime.Now;            
        }        
        ///Following Data Loggin Should be Done for both Insert and Update
        enquiry.ChangedByUserID = SessionCache.CurrentUser.ID;
        enquiry.ChangedByUsername = SessionCache.CurrentUser.UserName;
        enquiry.ChangedOn = DateTime.Now;         
    }
    /// <summary>
    /// Page Method to Get Client Contact Details in Simplified/Customized Format
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [WebMethod]
    public static App.CustomModels.CustomContact GetClientContact(int id)
    {
        OMMDataContext context = new OMMDataContext();        
        ClientContact contact = context.ClientContacts.SingleOrDefault(P => P.ID == id);
        return MapObject(contact);
        //return contact;
    }
    /// <summary>
    /// Maps LINQ Object to a Custom Object. 
    /// Note: LINQ Objects are not working for JSON Serialization, So a Custom Object has been used to 
    /// Transfer data over the network.
    /// </summary>
    /// <param name="contact"></param>
    /// <returns></returns>
    private static App.CustomModels.CustomContact MapObject(ClientContact contact)
    {
        App.CustomModels.CustomContact customContact = new App.CustomModels.CustomContact();
        if (contact != null)
        {            
            customContact.ClientName = contact.Client.Name;
            customContact.ContactName = contact.Name;
            customContact.JobTitle = contact.JobTitle;
            if(contact.Country != null)
                customContact.Country = contact.Country.Name;            
        }
        return customContact;
    }
}
