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

    [WebMethod (EnableSession=true)]
    public String SaveProject(App.CustomModels.CustomProject customProject)
    {
        OMMDataContext context = new OMMDataContext();

        Project project = new Project();
        if (customProject.ID > 0)
        {
            project = context.Projects.SingleOrDefault(P => P.ID == customProject.ID);
            project.ChangedByUserID = SessionCache.CurrentUser.ID;
            project.ChangedByUsername = SessionCache.CurrentUser.UserName; //SessionCache.CurrentUser.UserNameWeb;
            project.ChangedOn = DateTime.Now;
        }
        else
        {
            context.Projects.InsertOnSubmit(project);
            project.CreatedByUserID = project.ChangedByUserID = SessionCache.CurrentUser.ID;
            project.CreatedByUsername = project.ChangedByUsername = SessionCache.CurrentUser.UserName;//SessionCache.CurrentUser.UserNameWeb;
            project.CreatedOn = project.ChangedOn = DateTime.Now;
            project.Number = context.GenerateNewProjectNumber();
            project.StatusID = App.CustomModels.ProjectStatus.InProgress;
        }
        project.QuotationID = customProject.QuotationID;
        project.Name = customProject.Name;
        project.Description = customProject.Description;
        project.StartDate = WebUtil.GetDate(customProject.StartDate);
        project.EndDate = WebUtil.GetDate(customProject.EndDate);
        
        context.SubmitChanges();
        return String.Format("{0}:{1}", project.ID, project.Number);
    }

    [WebMethod (EnableSession=true)]
    public long SaveProjectNote(App.CustomModels.CustomProjectNote customNote)
    {
        OMMDataContext context = new OMMDataContext();
        ProjectNote note = null;
        if (customNote.ID > 0)
            note = context.ProjectNotes.SingleOrDefault(P => P.ID == customNote.ID);
        else
        {
            note = new ProjectNote();
            context.ProjectNotes.InsertOnSubmit(note);
        }
        note.ProjectID = customNote.ProjectID;
        note.Details = customNote.Details;
        note.CreatedBy = SessionCache.CurrentUser.ID;
        note.CreatedDate = DateTime.Now;
        context.SubmitChanges();
        return note.ID;
    }

    #region SMS Sending Operations

    //readonly char[] COMMA_SEPARATOR = { ',' };
    //readonly char[] COLON_SEPARATOR = { ':' };

    //private string m_lastError = string.Empty;
    //const string SERVICE_TEST_OK = "Service running";

    //private OMMDataContext _DataContext = new OMMDataContext();


    

    ///// <summary>
    ///// Sends SMS to the corresponding receipients
    ///// </summary>
    ///// <param name="receiPients"></param>
    ///// <param name="message"></param>
    ///// <returns></returns>
    //[WebMethod]
    //public bool SendSms(String receiPients, String message)
    //{
    //    //string[] arr = receiPients.Split(',');
    //    //PrepareDataAndSendSMS(receiPients);

    //    //at first save the message to Messages Table
    //    string currIdentity = saveToMessagesTable(message);

    //    //Now save details message for individual reciepents in : Message_Recipients TABLE
    //    saveToMessageRecipientsTable(Convert.ToInt32(currIdentity), receiPients);
        
    //    //Finally send message to the reciepents mobile
    //    sendMessage(receiPients, message);

    //    return true;
    //}
    #endregion
}