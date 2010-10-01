using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for AjaxService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class AjaxService : System.Web.Services.WebService {

    public AjaxService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    /// <summary>
    /// Page Method to Get Client Contact Details in Simplified/Customized Format
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [WebMethod]
    public App.CustomModels.CustomContact GetClientContact(int id)
    {
        OMMDataContext context = new OMMDataContext();
        ClientContact contact = context.ClientContacts.SingleOrDefault(P => P.ID == id);
        return MapClientContactObject(contact);        
    }
    /// <summary>
    /// Maps LINQ Object to a Custom Object. 
    /// Note: LINQ Objects are not working for JSON Serialization, So a Custom Object has been used to 
    /// Transfer data over the network.
    /// </summary>
    /// <param name="contact"></param>
    /// <returns></returns>
    private App.CustomModels.CustomContact MapClientContactObject(ClientContact contact)
    {
        App.CustomModels.CustomContact customContact = new App.CustomModels.CustomContact();
        if (contact != null)
        {
            customContact.ClientName = contact.Client.Name;
            customContact.ContactName = contact.Name;
            customContact.JobTitle = contact.JobTitle;
            if (contact.Country != null)
                customContact.Country = contact.Country.Name;
        }
        return customContact;
    }
    /// <summary>
    /// Submits a Quotation
    /// </summary>
    /// <param name="id">Quotation ID</param>
    /// <param name="clientContactId">Client Contact ID</param>
    /// <returns></returns>
    [WebMethod]
    public String SubmitQuotation(int id, int clientContactId)
    {
        OMMDataContext dataContext = new OMMDataContext();
        var quotation = dataContext.Quotations.SingleOrDefault(Q => Q.ID == id);
        if (quotation != null)
        {
            quotation.StatusID = App.CustomModels.QuotationStatus.Submitted;
            quotation.Number = dataContext.GenerateNewQuotationNumber(quotation.EnquiryID, true);
            dataContext.SubmitChanges();
            return quotation.Number;
        }
        return String.Empty;
    }
}

