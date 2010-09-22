using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BUDI2_NS.Data;
using System.Web.Services;
using App.CustomModels;
public partial class Pages_EnquiryChange : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindDropdownList();
        }
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
                           Name = String.Format("{0} ({1})", P.Name, P.Client.Name)
                       };
        ddlContact.DataSource = contacts;
        ddlContact.DataTextField = "Name";
        ddlContact.DataValueField = "ID";        
        ddlContact.DataBind();

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
    }
    /// <summary>
    /// Page Method to Save Enquiry
    /// </summary>
    /// <param name="customEnquiry"></param>
    /// <returns></returns>
    [WebMethod]
    public static String SaveEnquiry(App.CustomModels.CustomEnquiry customEnquiry)
    {        
        OMMDataContext context = new OMMDataContext();
        Enquiry enquiry = MapEnquiry(customEnquiry, context);
        context.Enquiries.InsertOnSubmit(enquiry);
        context.SubmitChanges();
        return String.Empty;
    }
    /// <summary>
    /// Prepares the object to save with LINQ
    /// </summary>
    /// <param name="customEnquiry"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    private static Enquiry MapEnquiry(App.CustomModels.CustomEnquiry customEnquiry, OMMDataContext context)
    {
        Enquiry enquiry = new Enquiry();
        ///Load from database if in edit mode
        if (customEnquiry.ID > 0)
            enquiry = context.Enquiries.SingleOrDefault(P => P.ID == customEnquiry.ID);

        enquiry.ContactID = customEnquiry.ContactID;
        enquiry.TypeID = customEnquiry.TypeID;
        //if(customEnquiry.StatusID == EnquiryStatus.New)
        if (customEnquiry.ID > 0)
        {
            enquiry.CreatedByUserID = 2;
            enquiry.CreatedByUsername = "Admin";
            enquiry.CreatedOn = DateTime.Now;
        }        
        enquiry.ChangedByUserID = 37; ///Currently Logged In User
        enquiry.ChangedByUsername = "Funny People";                    
        enquiry.ChangedOn = DateTime.Now;
        return enquiry;       
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
