using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

public partial class _Default : System.Web.UI.Page 
{

    readonly char[] COMMA_SEPARATOR = { ',' };
    readonly char[] COLON_SEPARATOR = { ':' };

    private string m_lastError = string.Empty;
    const string SERVICE_TEST_OK = "Service running";

    private OMMDataContext _DataContext = new OMMDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSendSMS_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            PrepareDataAndSendSMS();
        }
    }

    private void PrepareDataAndSendSMS()
    {
        //OMMDataContext dataContext = new OMMDataContext();
        IList<Message_Recipient> filteredReceivers = new List<Message_Recipient>();
        IList<Message> messages = _DataContext.Messages.ToList() ;
        IList<Message_Recipient> recipients = new List<Message_Recipient>();
        int lastRecipientID = 0;
        foreach (Message msg in messages)
        {
            filteredReceivers.Clear();
            if (!msg.Delivered)
            {
                recipients = (from P in _DataContext.Message_Recipients
                              where P.Message_ID == msg.ID
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
                            if (receipient.Status_ID == (int)MessageStatus.NOT_SENT)
                            {
                                filteredReceivers.Add(receipient);
                            }
                        }
                        lastRecipientID = receipient.ID;
                    }
                    ///TODO: Send Messages to the Filtered Receipients
                    if (filteredReceivers.Count > 0)
                        SendMessage(msg, filteredReceivers);
                }
            }
        }        
    }

    private bool SendMessage(Message message, IList<Message_Recipient> receivers)
    {
        Message smsr = null;
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

        //SmsService.TextAnywhere_SMS ta_sms = new SmsService.TextAnywhere_SMS();
        //sendSmsReply = ta_sms.SendSMSEx(
        //            username,
        //            password,
        //            smsr.Client_Ref,
        //            BILLING_REF,
        //            (int)CONNECTION_TYPES.TEST,
        //            ORIGINATOR,
        //            (int)ORIGINATOR_TYPES.NAME,
        //            destinationNumbers.ToString(),
        //            message.Text,
        //            0,
        //            (int)REPLY_TYPES.NONE, "");

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

        SmsService.TextAnywhere_SMS ta_sms = new SmsService.TextAnywhere_SMS();
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
}



enum MESSAGE_STATUSES
{
    NOT_SENT = 0,
    SENT_UNDELIVERED = 1,
    SENT_DELIVERED = 2,
    SENT_DELIVERY_FAILED = 3
};
enum MessageStatus
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
