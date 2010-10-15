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
    #region SMS Sending Operations

    readonly char[] COMMA_SEPARATOR = { ',' };
    readonly char[] COLON_SEPARATOR = { ':' };

    private string m_lastError = string.Empty;
    const string SERVICE_TEST_OK = "Service running";

    private OMMDataContext _DataContext = new OMMDataContext();


    

    /// <summary>
    /// Sends SMS to the corresponding receipients
    /// </summary>
    /// <param name="receiPients"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    [WebMethod]
    public bool SendSms(String receiPients, String message)
    {
        string[] arr = receiPients.Split(',');

        PrepareDataAndSendSMS(receiPients);

        return true;
    }


    private void PrepareDataAndSendSMS(String receiPientsList)
    {
        //OMMDataContext dataContext = new OMMDataContext();
        IList<Message_Recipient> filteredReceivers = new List<Message_Recipient>();
        IList<Message> messages = _DataContext.Messages.ToList();
        IList<Message_Recipient> recipients = new List<Message_Recipient>();
        int lastRecipientID = 0;
        foreach (Message msg in messages)
        {
            filteredReceivers.Clear();
            if (!msg.Delivered)
            {
                //string[] arr = receiPientsList.Split(new char[] { ',' });

                //int arrLength = arr.Length;

                int[] ids = WebUtil.GetIntArray(receiPientsList);
                
                //for (int i = 0; i < arr.Length; i++)
                //{
                //    ids[i] = Convert.ToInt16(arr[i].Trim());
                //}

                //int[] test = {7};

                recipients = (from P in _DataContext.Message_Recipients
                              where P.Message_ID == msg.ID
                              && (from I in ids select I).Contains(P.ID)
                              orderby P.Recipient_ID, P.Try_Order
                              select P).ToList();

                if (recipients != null && recipients.Count > 0)
                {
                    foreach (Message_Recipient receipient in recipients)
                    {
                        if (lastRecipientID != receipient.ID)
                        {
                            // RULES
                            // Number not sent -> Send and go to next contact
                            // Number sent and failed -> Go to next number
                            // Number sent but not delivered -> Go to next contact
                            // Number sent and delivered -> Go to next contact
                            //if (mrr.Status_ID == (int)MESSAGE_STATUSES.NOT_SENT)
                            if (receipient.Status_ID == (int)MessageStatuss.NOT_SENT)
                            {
                                filteredReceivers.Add(receipient);
                            }
                        }
                        lastRecipientID = receipient.ID;
                    }
                    ///TODO: Send Messages to the Filtered Receipients
                    ///

                    if (filteredReceivers.Count > 0)
                        SendMessage(msg, filteredReceivers);
                }
            }
        }
    }    

    private bool SendMessage(Message message, IList<Message_Recipient> receivers)
    {
        string BILLING_REF = ConfigReader.BILLING_REF;
        string ORIGINATOR = ConfigReader.ORIGINATOR;


        //Message smsr = null;
        SMS_Message smsr = null;
        String userName = ConfigReader.TextAnywhereClientID;
        String password = ConfigReader.TextAnywhereClientPassword;
        String[] sendSMSReplyArray = null;
        String sendSmsReply = string.Empty;
        String[] returnCodePair = null;

        System.Text.StringBuilder destinationNumbers = destinationNumbers = new System.Text.StringBuilder(10);

        // Check recipients list not empty
        if (receivers.Count <= 0)
        {
            return false;
        }

        //// Request a new SMS_ID from the database
        //if (null == (smsr = GetNewSMSRow()))
        //{
        //    return false;
        //}

        // Check SMS Service is running
        if (!IsSmsServiceRunning())
        {
            return false;
        }

        // Build destination number list
        foreach (Message_Recipient mrr in receivers)
        {
            if (destinationNumbers.Length > 0)
            {
                destinationNumbers.Append(COMMA_SEPARATOR);
            }

            destinationNumbers.Append(mrr.Destination);
        }
        if (ConfigReader.SendSmsToClient)
        {
            SMSService.TextAnywhere_SMS smsService = new SMSService.TextAnywhere_SMS();            
            sendSmsReply = smsService.SendSMSEx(
                        userName,
                        password,
                        smsr.Client_Ref,
                        BILLING_REF,
                        (int)CONNECTION_TYPES.TEST,
                        ORIGINATOR,
                        (int)ORIGINATOR_TYPES.NAME,
                        destinationNumbers.ToString(),
                        message.Text,
                        0,
                        (int)REPLY_TYPES.NONE, "");
        }
        // Extract return codes
        sendSMSReplyArray = sendSmsReply.Split(COMMA_SEPARATOR);

        if (sendSMSReplyArray.Length != receivers.Count)
        {
            m_lastError = "Unable to send SMS message.  SMS Service did not return the expected data.";
            return false;
        }

        // Iterate the return codes
        foreach (string returnCode in sendSMSReplyArray)
        {
            returnCodePair = returnCode.Split(COLON_SEPARATOR);

            // Look up the phone number in the recipients list
            foreach (Message_Recipient receiver in receivers)
            {
                // Phone number match
                if (receiver.Destination == returnCodePair[0])
                {
                    // Store return code, sms id, and update message status accordingly

                    // Update row
                    //mrr.BeginEdit();
                    receiver.SMS_ID = smsr.ID;
                    receiver.SMS_Status_ID = Convert.ToInt32(returnCodePair[1]);

                    if (SMSStatusIDNotSent(receiver.SMS_Status_ID.GetValueOrDefault()))
                    {
                        receiver.Status_ID = (int)MESSAGE_STATUSES.NOT_SENT;
                    }
                    else
                    {
                        receiver.Status_ID = (int)MESSAGE_STATUSES.SENT_UNDELIVERED;
                    }

                    receiver.Updated_On = DateTime.Now;
                    //mrr.EndEdit();
                    //TODO: Update Receiver Information
                    _DataContext.SubmitChanges();
                }
            }
        }
        return true;
    }

    // Check the SMS service is running
    private bool IsSmsServiceRunning()
    {
        String serviceTestReply = string.Empty;
        //string username = string.Empty;
        //string password = string.Empty;

        String userName = ConfigReader.TextAnywhereClientID;
        String password = ConfigReader.TextAnywhereClientPassword;

        //if (!GetServiceUsernameAndPassword(ref username, ref password))
        //{
        //    return false;
        //}

        if (String.IsNullOrEmpty(userName.Trim()) || String.IsNullOrEmpty(password.Trim()))
        {
            m_lastError = "Unable to connect to the TextAnyWhere SMS Web Service.\r\nCheck your service username and password.";
            return false;
        }

        SMSService.TextAnywhere_SMS ta_sms = new SMSService.TextAnywhere_SMS();
        //SmsService.TextAnywhere_SMS ta_sms = new SmsService.TextAnywhere_SMS();
        serviceTestReply = ta_sms.ServiceTest(userName, password);

        if (!serviceTestReply.StartsWith(SERVICE_TEST_OK))
        {
            m_lastError = "Unable to connect to the TextAnyWhere SMS Web Service.\r\nCheck your service username and password and ensure there is an available internet connection.";
            return false;
        }
        return true;
    }

    // Does this status id mean not sent ?
    private bool SMSStatusIDNotSent(int id)
    {
        // All status codes beginning with 2 or 3 mean not sent
        while (id >= 10)
        {
            id /= 10;
        }

        return (id == 2) || (id == 3);
    }

    #endregion

}



enum MESSAGE_STATUSES
{
    NOT_SENT = 0,
    SENT_UNDELIVERED = 1,
    SENT_DELIVERED = 2,
    SENT_DELIVERY_FAILED = 3
};
enum MessageStatuss
{
    NOT_SENT = 0,
    SENT_UNDELIVERED = 1,
    SENT_DELIVERED = 2,
    SENT_DELIVERY_FAILED = 3
};
enum CONNECTION_TYPES
{
    TEST = 1,
    LOW_VOLUME = 2,
    HIGH_VOLUME = 4
};

enum ORIGINATOR_TYPES
{
    NUMBER = 0,
    NAME = 1
};

enum REPLY_TYPES
{
    NONE = 0,
    WEB_SERVICE = 1,
    EMAIL = 2,
    URL = 3
};

