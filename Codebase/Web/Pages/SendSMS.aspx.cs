using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Services;

public partial class Pages_SendSMS : BasePage
{    
    private int[] _ContactIDs;   

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            BindContactsList();
        }
    }
    protected void BindPageInfo()
    {        
        Page.Title = WebUtil.GetPageTitle("Send SMS");
        String commSeperatedID = WebUtil.GetQueryStringInString(AppConstants.QueryString.ID);
        _ContactIDs = WebUtil.GetIntArray(commSeperatedID);
    }
    /// <summary>
    /// Binds Contacts List From Query String Parameters
    /// </summary>
    protected void BindContactsList()
    {
        //txtMessage.Text = "This is a test message. ";        
        if (_ContactIDs == null || _ContactIDs.Length == 0)
            ShowErrorMessage();
        else
        {
            OMMDataContext dataContext = new OMMDataContext();
            var recipients = (from T in dataContext.TelephoneNumbers
                              where(from C in _ContactIDs select C).Contains(T.ContactID) && (T.TypeID == 2)
                              select new 
                              { 
                                  T.ContactID, 
                                  T.ID, 
                                  T.Number 
                              });
            gvRecipients.DataSource = recipients;
            gvRecipients.DataBind();
        }
    }
    protected void ShowErrorMessage()
    {
        pnlMainContainer.Visible = false;
        WebUtil.ShowMessageBox(divMessage, "Sorry! the requested contact was not found.", true);
    }

    #region AJAX Methods For Sending SMS
    
    private static OMMDataContext _DataContext = null;
    private const char COMMA_SEPARATOR = ',';
    private const char COLON_SEPARATOR = ':';
    private static String _ErrorMessage = String.Empty;
    private const String SERVICE_TEST_OK = "Service running";
    private static SMSService.TextAnywhere_SMS _SmsService = null;

    /// <summary>
    /// Sends SMS to the corresponding receipients
    /// </summary>
    /// <param name="receiPients"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    [WebMethod]
    public static App.CustomModels.SendSmsStatus SendSms(String commaSeparaterTelephoneIDs, String messageText)
    {
        //return new App.CustomModels.SendSmsStatus();
        _DataContext = new OMMDataContext();
        _ErrorMessage = String.Empty;
        //Get Int array from the receipients list
        int[] telephoneIDs = WebUtil.GetIntArray(commaSeparaterTelephoneIDs);
        ///Create the Web Service Object for Sending SMS
        _SmsService = new SMSService.TextAnywhere_SMS();
        //at first save the message to Messages Table
        Message message = SaveMessage(messageText.Trim());
        //Now save details message for individual reciepents in : Message_Recipients TABLE
        SaveMessageRecipients(message.ID, telephoneIDs);
        //Finally send message to the reciepents mobile
        String sentCells = SendMessage(telephoneIDs, message);
        App.CustomModels.SendSmsStatus reply = new App.CustomModels.SendSmsStatus();
        if (String.IsNullOrEmpty(_ErrorMessage))
        {
            reply.StatusID = 1;
            reply.Message = sentCells;
        }
        else
        {
            reply.StatusID = -1;
            reply.Message = _ErrorMessage;
        }
        return reply;
    }
    protected static Message SaveMessage(String messageText)
    {        
        Message message = new Message();
        message.Text = messageText;
        ///Set the Message Delivery Status as Not Delivered. 
        ///It will be Reset after Message Delivery
        message.Delivered = false;
        _DataContext.Messages.InsertOnSubmit(message);
        _DataContext.SubmitChanges();
        return message;
    }

    protected static void SaveMessageRecipients(int messageID, int[] telephoneIDs)
    {
        ///Collect all receipents number from the telephone number table        
        IList<TelephoneNumber> recipients = new List<TelephoneNumber>();

        recipients = (from P in _DataContext.TelephoneNumbers
                      where (from I in telephoneIDs select I).Contains(P.ID)
                      select P).ToList();

        if (recipients != null && recipients.Count > 0)
        {
            foreach (TelephoneNumber receipient in recipients)
            {
                Message_Recipient msgRecipient = new Message_Recipient();
                msgRecipient.Message_ID = messageID;
                msgRecipient.Recipient_ID = receipient.ID;
                msgRecipient.Recipient_Name = String.Format("{0} {1}", receipient.Contact.FirstNames, receipient.Contact.LastName);
                msgRecipient.Destination = receipient.Number;
                msgRecipient.Try_Order = 1;
                ///TODO: I think we have to review the Following line of code.
                msgRecipient.Is_Phone_Number = true;
                msgRecipient.Status_ID = 1;
                msgRecipient.Updated_On = DateTime.Now;
                _DataContext.Message_Recipients.InsertOnSubmit(msgRecipient);
                _DataContext.SubmitChanges();
            }
        }
    }

    protected static String SendMessage(int[] telephoneIDs, Message message)
    {        
        //Get Int array from the receipients list
        //int[] ids = WebUtil.GetIntArray(receiPients);
        int lastRecipientID = 0;
        //collect all receipents number from the telephone number table
        //OMMDataContext dataContext = new OMMDataContext();
        IList<TelephoneNumber> recipients = new List<TelephoneNumber>();
        IList<TelephoneNumber> filteredRecipients = new List<TelephoneNumber>();
        Message_Recipient msgRecipient = new Message_Recipient();

        recipients = (from P in _DataContext.TelephoneNumbers
                      where
                      (from I in telephoneIDs select I).Contains(P.ID)
                      select P).ToList();

        if (recipients != null && recipients.Count > 0)
        {
            foreach (TelephoneNumber receipient in recipients)
            {
                if (lastRecipientID != receipient.ID)
                {
                    //// RULES
                    //// Number not sent -> Send and go to next contact
                    //// Number sent and failed -> Go to next number
                    //// Number sent but not delivered -> Go to next contact
                    //// Number sent and delivered -> Go to next contact                    
                    filteredRecipients.Add(receipient);                    
                }
                lastRecipientID = receipient.ID;
            }
            ///TODO: Send Messages to the Filtered Receipients
            if (filteredRecipients.Count > 0)
            {
                SendFinalMessage(message, filteredRecipients);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < filteredRecipients.Count; i++)
                {
                    TelephoneNumber cell = filteredRecipients[i];
                    if (i > 0)
                        sb.Append(", ");
                    sb.Append(cell.Number);
                }
                return sb.ToString();
            }            
        }
        return String.Empty;
    }


    private static bool SendFinalMessage(Message message, IList<TelephoneNumber> receivers)
    {
        //String BILLING_REF = ConfigReader.BILLING_REF;
        //String ORIGINATOR = ConfigReader.ORIGINATOR;        

        ///Generate a New Message Reference by Executing a Stored Procedure (dalSMSMessagesInsert) as in Win App. 
        ///[Note: Don't Know Why We are Doing This]
        SMS_Message smsMessageRef = _DataContext.dalSMSMessagesInsert(String.Empty, DateTime.Now).SingleOrDefault();
        String userName = ConfigReader.TextAnywhereClientID;
        String password = ConfigReader.TextAnywhereClientPassword;
        String[] WSSendSMSReplyArray = null;
        String WSSendSmsReply = String.Empty;
        //String[] returnCodePair = null;

        System.Text.StringBuilder sb = new System.Text.StringBuilder(10);

        // Check recipients list not empty
        if (receivers.Count <= 0)
        {
            return false;
        }
        // Check SMS Service is running
        if (!IsSmsServiceRunning())
        {
            return false;
        }

        // Build Comma Separated destination number list
        foreach (TelephoneNumber mrr in receivers)
        {
            if (sb.Length > 0)            
                sb.Append(COMMA_SEPARATOR);
            
            sb.Append(mrr.Number);
        }
        if (ConfigReader.SendSmsToClient)
        {
            //SMSService.TextAnywhere_SMS smsService = new SMSService.TextAnywhere_SMS();
            WSSendSmsReply = _SmsService.SendSMSEx(userName, password,
                        smsMessageRef.Client_Ref, ConfigReader.BILLING_REF,                        
                        (int)CONNECTION_TYPES.LOW_VOLUME, ConfigReader.ORIGINATOR,
                        (int)ORIGINATOR_TYPES.NAME, sb.ToString(),
                        message.Text, 0, (int)REPLY_TYPES.NONE, String.Empty);
        }
        // Extract return codes
        WSSendSMSReplyArray = WSSendSmsReply.Split(COMMA_SEPARATOR);

        if (WSSendSMSReplyArray.Length != receivers.Count)
        {
            _ErrorMessage = "Unable to send SMS message. SMS Service did not return the expected response.";
            return false;
        }
        else
        {
            ///After Successfully Sending Message Set Message status to delivered            
            message.Delivered = true;            
            _DataContext.SubmitChanges();
        }
        return true;
    }
    /// <summary>
    /// Checks Whether the SMS Service is Running or Not
    /// </summary>
    /// <returns></returns>
    protected static bool IsSmsServiceRunning()
    {
        String serviceTestReply = string.Empty;        
        String userName = ConfigReader.TextAnywhereClientID;
        String password = ConfigReader.TextAnywhereClientPassword;

        if (String.IsNullOrEmpty(userName.Trim()) || String.IsNullOrEmpty(password.Trim()))
        {
            _ErrorMessage = "Unable to connect to the TextAnyWhere SMS Web Service.<br/>Check your service username and password.";
            return false;
        }        
        serviceTestReply = _SmsService.ServiceTest(userName, password);

        if (!serviceTestReply.StartsWith(SERVICE_TEST_OK))
        {
            _ErrorMessage = "Unable to connect to the TextAnyWhere SMS Web Service.\r\nCheck your service username and password and ensure there is an available internet connection.";
            return false;
        }
        return true;
    }
    #region Obsolete Code for SMS Sending Operation
    //private static void PrepareDataAndSendSMS(String receiPientsList)
    //{
    //    //OMMDataContext dataContext = new OMMDataContext();
    //    IList<Message_Recipient> filteredReceivers = new List<Message_Recipient>();
    //    IList<Message> messages = _DataContext.Messages.ToList();
    //    IList<Message_Recipient> recipients = new List<Message_Recipient>();
    //    int lastRecipientID = 0;
    //    foreach (Message msg in messages)
    //    {
    //        filteredReceivers.Clear();
    //        if (!msg.Delivered)
    //        {
    //            int[] ids = WebUtil.GetIntArray(receiPientsList);

    //            recipients = (from P in _DataContext.Message_Recipients
    //                          where P.Message_ID == msg.ID
    //                          && (from I in ids select I).Contains(P.ID)
    //                          orderby P.Recipient_ID, P.Try_Order
    //                          select P).ToList();

    //            if (recipients != null && recipients.Count > 0)
    //            {
    //                foreach (Message_Recipient receipient in recipients)
    //                {
    //                    if (lastRecipientID != receipient.ID)
    //                    {
    //                        // RULES
    //                        // Number not sent -> Send and go to next contact
    //                        // Number sent and failed -> Go to next number
    //                        // Number sent but not delivered -> Go to next contact
    //                        // Number sent and delivered -> Go to next contact
    //                        //if (mrr.Status_ID == (int)MESSAGE_STATUSES.NOT_SENT)
    //                        if (receipient.Status_ID == (int)MessageStatuss.NOT_SENT)
    //                        {
    //                            filteredReceivers.Add(receipient);
    //                        }
    //                    }
    //                    lastRecipientID = receipient.ID;
    //                }
    //                ///TODO: Send Messages to the Filtered Receipients
    //                ///

    //                if (filteredReceivers.Count > 0)
    //                    SendMessage(msg, filteredReceivers);
    //            }
    //        }
    //    }
    //}

    //private static bool SendMessage(Message message, IList<Message_Recipient> receivers)
    //{
    //    string BILLING_REF = ConfigReader.BILLING_REF;
    //    string ORIGINATOR = ConfigReader.ORIGINATOR;


    //    //Message smsr = null;
    //    SMS_Message smsr = null;
    //    String userName = ConfigReader.TextAnywhereClientID;
    //    String password = ConfigReader.TextAnywhereClientPassword;
    //    String[] sendSMSReplyArray = null;
    //    String sendSmsReply = string.Empty;
    //    String[] returnCodePair = null;

    //    System.Text.StringBuilder destinationNumbers = destinationNumbers = new System.Text.StringBuilder(10);

    //    // Check recipients list not empty
    //    if (receivers.Count <= 0)
    //    {
    //        return false;
    //    }

    //    //// Request a new SMS_ID from the database
    //    //if (null == (smsr = GetNewSMSRow()))
    //    //{
    //    //    return false;
    //    //}

    //    // Check SMS Service is running
    //    if (!IsSmsServiceRunning())
    //    {
    //        return false;
    //    }

    //    // Build destination number list
    //    foreach (Message_Recipient mrr in receivers)
    //    {
    //        if (destinationNumbers.Length > 0)
    //        {
    //            destinationNumbers.Append(COMMA_SEPARATOR);
    //        }

    //        destinationNumbers.Append(mrr.Destination);
    //    }
    //    if (ConfigReader.SendSmsToClient)
    //    {
    //        SMSService.TextAnywhere_SMS smsService = new SMSService.TextAnywhere_SMS();
    //        sendSmsReply = smsService.SendSMSEx(
    //                    userName,
    //                    password,
    //                    smsr.Client_Ref,
    //                    BILLING_REF,
    //                    (int)CONNECTION_TYPES.TEST,
    //                    ORIGINATOR,
    //                    (int)ORIGINATOR_TYPES.NAME,
    //                    destinationNumbers.ToString(),
    //                    message.Text,
    //                    0,
    //                    (int)REPLY_TYPES.NONE, "");




    //    }



    //    // Extract return codes
    //    sendSMSReplyArray = sendSmsReply.Split(COMMA_SEPARATOR);

    //    if (sendSMSReplyArray.Length != receivers.Count)
    //    {
    //        m_lastError = "Unable to send SMS message.  SMS Service did not return the expected data.";
    //        return false;
    //    }



    //    // Iterate the return codes
    //    foreach (string returnCode in sendSMSReplyArray)
    //    {
    //        returnCodePair = returnCode.Split(COLON_SEPARATOR);

    //        // Look up the phone number in the recipients list
    //        foreach (Message_Recipient receiver in receivers)
    //        {
    //            // Phone number match
    //            if (receiver.Destination == returnCodePair[0])
    //            {
    //                // Store return code, sms id, and update message status accordingly

    //                // Update row
    //                //mrr.BeginEdit();
    //                receiver.SMS_ID = smsr.ID;
    //                receiver.SMS_Status_ID = Convert.ToInt32(returnCodePair[1]);

    //                if (SMSStatusIDNotSent(receiver.SMS_Status_ID.GetValueOrDefault()))
    //                {
    //                    receiver.Status_ID = (int)MESSAGE_STATUSES.NOT_SENT;
    //                }
    //                else
    //                {
    //                    receiver.Status_ID = (int)MESSAGE_STATUSES.SENT_UNDELIVERED;
    //                }

    //                receiver.Updated_On = DateTime.Now;
    //                //mrr.EndEdit();
    //                //TODO: Update Receiver Information
    //                _DataContext.SubmitChanges();
    //            }
    //        }
    //    }
    //    return true;
    //}    

    //// Does this status id mean not sent ?
    //private static bool SMSStatusIDNotSent(int id)
    //{
    //    // All status codes beginning with 2 or 3 mean not sent
    //    while (id >= 10)
    //    {
    //        id /= 10;
    //    }

    //    return (id == 2) || (id == 3);
    //}
    #endregion Obsolete Code for SMS Sending Operation
    #endregion
}