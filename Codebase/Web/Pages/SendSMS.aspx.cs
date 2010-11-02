using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Pages_SendSMS : System.Web.UI.Page
{
    //net.textanywhere.ws.TextAnywhere_SMS ta_sms = new net.textanywhere.ws.TextAnywhere_SMS();
    private SMSService.TextAnywhere_SMS _SmsService = new SMSService.TextAnywhere_SMS();

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            tbxMessage.Text = "This is a test message. ";
            //Thorugh the query string mobile number list will be
            //sended with comma separator.
            String commSeperatedID = WebUtil.GetQueryStringInString(AppConstants.QueryString.ID);

            int[] ids = WebUtil.GetIntArray(commSeperatedID);
            if (ids == null || ids.Length == 0)
                ShowErrorMessage();
            else
            {
                OMMDataContext dataContext = new OMMDataContext();
                //IList<TelephoneNumber> recipients = (from R in dataContext.TelephoneNumbers
                //                                       where (from I in ids select I).Contains(R.ContactID)
                //                                       select new{R.ContactID,R.ID,R.Number}).SingleOrDefault();
                                                     
                //select R)..ToList();

                var recipients = (from R in dataContext.TelephoneNumbers
                                  where 
                                  (from I in ids select I).Contains(R.ContactID)
                                  && (R.TypeID==2)
                                  select new { R.ContactID, R.ID, R.Number });

                GridView1.DataSource = recipients;
                GridView1.DataBind();
            }
        }

        //OMMDataContext context = new OMMDataContext();
        //context.Message_Recipients
    }
    protected void ShowErrorMessage()
    {
        tblContainer.Visible = false;
        WebUtil.ShowMessageBox(divMessage, "Sorry! the requested receipient was not found.", true);
    }
    protected void BindPageInfo()
    {
        Page.Title = WebUtil.GetPageTitle("Send SMS");
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            //btnPrevious.Enabled = true;
            //btnNext.Enabled = false;

            //divRecipientList.Visible = true;
            //divNewMessage.Visible = false;

            //GridView1.DataSourceID = "LinqDataSource1";
            //GridView1.DataBound();
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            //btnPrevious.Enabled = false;
            //btnNext.Enabled = true;

            //divRecipientList.Visible = false;
            //divNewMessage.Visible = true;

            //GridView1.DataSourceID = "LinqDataSource1";
            //GridView1.DataBound();
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnFinish_Click(object sender, EventArgs e)
    {
        

        try
        {
            //??????????????
            //// Make sure message is updated to the dataset
            //if (m_stepNumber == 1)
            //{
            //    messagesBindingSource.EndEdit();
            //}


            // Make sure user is aware of contacts who are inactive or opted out
            if (HasInactiveOrOptedOutRecipients)
            {

                //?????

                //if (MessageBox.Show("Some of the selected recipients have either opted out of SMS contact, or are currently marked inactive.  Proceed anyway?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                //{
                //    //this.Cursor = oldCursor;
                //    return;
                //}
            }

            if (!VerifyRecipients_V())
            {
                return;
            }

            if (!SaveMessagesAndRecipients())
            {
                return;
            }


            //???????

            //if (!messagesDataSet.SendSMS())
            //{
            //    MessageBox.Show(messagesDataSet.LastError, TITLE_BAR, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //    this.Cursor = oldCursor;
            //    return;
            //}

            
            //?????
            //m_cancelled = false;
            //Close();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }


    public bool SaveMessagesAndRecipients()
    {
        //??????

        //MessagesTableAdapter mta = null;
        //Message_RecipientsTableAdapter mrta = null;
        //PersonnelTransaction tran = null;
        //MessagesDataSet mds = null;

        //mds = (MessagesDataSet)this.GetChanges();

        //if (mds == null)
        //{
        //    return true;
        //}


        //// Make sure message is provided
        //foreach (MessagesRow mr in mds.Messages)
        //{
        //    if (mr.Text.Trim().Length == 0)
        //    {
        //        m_lastError = "You must enter a message.";
        //        return false;
        //    }

        //    // Make sure recipients are provided for each message
        //    if (mr.GetMessage_RecipientsRows().Length == 0)
        //    {
        //        m_lastError = "You must define at least one recipient.";
        //        return false;
        //    }
        //}

        //// Make sure all phone numbers are provided and validated
        //foreach (Message_RecipientsRow mrr in mds.Message_Recipients)
        //{
        //    if (mrr.Destination.Trim().Length == 0)
        //    {
        //        m_lastError = "One or more phone numbers are invalid.";
        //        return false;
        //    }

        //    if (mrr.Is_Phone_Number && mrr.IsSMS_CreditsNull())
        //    {
        //        m_lastError = "One or more phone numbers are invalid.";
        //        return false;
        //    }
        //}

        //// Save message and recipients to database
        //tran = new PersonnelTransaction();

        //try
        //{
        //    mta = new MessagesTableAdapter();
        //    mrta = new Message_RecipientsTableAdapter();

        //    mta.DisableAcceptChangesOnUpdate();
        //    mrta.DisableAcceptChangesOnUpdate();

        //    tran.Begin();

        //    mta.SetTransaction(tran);
        //    mrta.SetTransaction(tran);

        //    if (mta.Update(mds.Messages) != mds.Messages.Rows.Count)
        //    {
        //        m_lastError = "Unable to save messages to database.";
        //        tran.Rollback();
        //        return false;
        //    }

        //    if (mrta.Update(mds.Message_Recipients) != mds.Message_Recipients.Rows.Count)
        //    {
        //        m_lastError = "Unable to save message recipients to database.";
        //        tran.Rollback();
        //        return false;
        //    }

        //    tran.Commit();

        //    this.Messages.Merge(mds.Messages);
        //    this.Message_Recipients.Merge(mds.Message_Recipients);

        //    this.AcceptChanges();
        //    return true;
        //}
        //catch (Exception ex)
        //{
        //    m_lastError = ex.Message;
        //    tran.Rollback();
        //    return false;
        //}
        //finally
        //{
        //    if (mta != null)
        //    {
        //        mta.Dispose();
        //    }

        //    if (mrta != null)
        //    {
        //        mrta.Dispose();
        //    }
        //}

        return true;
    }


    public bool SendSMS()
    {

        //??????

        //int? lastRecipientId = null;
        //bool skipContact = false;
        //List<Message_RecipientsRow> receivers = new List<Message_RecipientsRow>();
        //Message_RecipientsRow[] recipients = null;

        //// Loop across all messages
        //foreach (MessagesRow mr in this.Messages)
        //{
        //    receivers.Clear();

        //    // Check message not delivered
        //    if (!mr.Delivered)
        //    {
        //        // Get an ordered list of message recipients
        //        recipients = (Message_RecipientsRow[])this.Message_Recipients.Select("Message_ID=" + mr.ID.ToString(), "Recipient_ID ASC, Try_Order ASC");
        //        lastRecipientId = null;

        //        foreach (Message_RecipientsRow mrr in recipients)
        //        {
        //            if (lastRecipientId == null || mrr.Recipient_ID != lastRecipientId)
        //            {
        //                // Contact change - reset skip flag
        //                skipContact = false;
        //            }

        //            if (!skipContact)
        //            {
        //                // RULES
        //                // Number not sent -> Send and go to next contact
        //                // Number sent and failed -> Go to next number
        //                // Number sent but not delivered -> Go to next contact
        //                // Number sent and delivered -> Go to next contact

        //                if (mrr.Status_ID == (int)MESSAGE_STATUSES.NOT_SENT)
        //                {
        //                    // Send and go to next contact
        //                    receivers.Add(mrr);
        //                    skipContact = true;
        //                }
        //                else if (mrr.Status_ID == (int)MESSAGE_STATUSES.SENT_UNDELIVERED
        //                        || mrr.Status_ID == (int)MESSAGE_STATUSES.SENT_DELIVERED)
        //                {
        //                    // Go to next contact
        //                    skipContact = true;
        //                }
        //                else
        //                {
        //                    // Go to next number
        //                }
        //            }

        //            lastRecipientId = mrr.Recipient_ID;
        //        }

        //        // Send this message to all of the receivers
        //        if (receivers.Count > 0)
        //        {
        //            if (!SendSMSPrivate(mr, receivers))
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //}

        //// Save message deliveries to database
        //if (!SaveMessageDeliveries())
        //{
        //    return false;
        //}

        return true;
    }


    public bool HasInactiveOrOptedOutRecipients
    {
        get
        {
            //foreach (Message_RecipientsRow mrr in this.Message_Recipients)
            //{
            //    if (mrr.ContactIsInactive || mrr.ContactIsOptedOut)
            //    {
            //        return true;
            //    }
            //}

            return false;
        }
    }


    //private void PrepareDataAndSendSMS()
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
    //            recipients = (from P in _DataContext.Message_Recipients
    //                          where P.Message_ID == msg.ID
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
    //                        if (receipient.Status_ID == (int)MessageStatus.NOT_SENT)
    //                        {
    //                            filteredReceivers.Add(receipient);
    //                        }
    //                    }
    //                    lastRecipientID = receipient.ID;
    //                }
    //                ///TODO: Send Messages to the Filtered Receipients
    //                if (filteredReceivers.Count > 0)
    //                    SendMessage(msg, filteredReceivers);
    //            }
    //        }
    //    }
    //}

    //private bool SendMessage(Message message, IList<Message_Recipient> receivers)
    //{
    //    Message smsr = null;
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

    //    //SmsService.TextAnywhere_SMS ta_sms = new SmsService.TextAnywhere_SMS();
    //    //sendSmsReply = ta_sms.SendSMSEx(
    //    //            username,
    //    //            password,
    //    //            smsr.Client_Ref,
    //    //            BILLING_REF,
    //    //            (int)CONNECTION_TYPES.TEST,
    //    //            ORIGINATOR,
    //    //            (int)ORIGINATOR_TYPES.NAME,
    //    //            destinationNumbers.ToString(),
    //    //            message.Text,
    //    //            0,
    //    //            (int)REPLY_TYPES.NONE, "");

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

    //// Check the SMS service is running
    //private bool IsSmsServiceRunning()
    //{
    //    String serviceTestReply = string.Empty;
    //    //string username = string.Empty;
    //    //string password = string.Empty;

    //    String userName = ConfigReader.TextAnywhereClientID;
    //    String password = ConfigReader.TextAnywhereClientPassword;

    //    //if (!GetServiceUsernameAndPassword(ref username, ref password))
    //    //{
    //    //    return false;
    //    //}

    //    if (String.IsNullOrEmpty(userName.Trim()) || String.IsNullOrEmpty(password.Trim()))
    //    {
    //        m_lastError = "Unable to connect to the TextAnyWhere SMS Web Service.\r\nCheck your service username and password.";
    //        return false;
    //    }

    //    SmsService.TextAnywhere_SMS ta_sms = new SmsService.TextAnywhere_SMS();
    //    serviceTestReply = ta_sms.ServiceTest(userName, password);

    //    if (!serviceTestReply.StartsWith(SERVICE_TEST_OK))
    //    {
    //        m_lastError = "Unable to connect to the TextAnyWhere SMS Web Service.\r\nCheck your service username and password and ensure there is an available internet connection.";
    //        return false;
    //    }
    //    return true;
    //}

    //// Does this status id mean not sent ?
    //private bool SMSStatusIDNotSent(int id)
    //{
    //    // All status codes beginning with 2 or 3 mean not sent
    //    while (id >= 10)
    //    {
    //        id /= 10;
    //    }

    //    return (id == 2) || (id == 3);
    //}


    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            //?????????
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnVerifyPhoneNumbers_Click(object sender, EventArgs e)
    {
        try
        {
            VerifyRecipients();

        }
        catch (Exception ex)
        {
        }
    }

    private void VerifyRecipients()
    {
        //if (!messagesDataSet.VerifyRecipients())
        //{
        //    MessageBox.Show(messagesDataSet.LastError, TITLE_BAR, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //}

        if (VerifyRecipients_V())
        {
            
            //btnFinish.Enabled = true;
            lblMessage.Text = "";
            lblMessage.ForeColor = System.Drawing.Color.Green;
        }
        else
        {
            //btnFinish.Enabled = false;
            //lblMessage.Text = "Not all recipients verified";
        }
        
    }


    public bool VerifyRecipients_V()
    {
        string formatNumberReply = string.Empty;
        string checkNumberReply = string.Empty;
        string[] checkNumberReplyArray = null;

        bool formatSuccess = true;


        try
        {
            if (!CheckSMSServiceRunning())
            {
                return false;
            }

            //?????????

            //this.Message_Recipients.ValidationEnabled = false;

            //foreach (Message_RecipientsRow mrr in this.Message_Recipients)
            //{
            //    if (mrr.IsSMS_CreditsNull() && mrr.Is_Phone_Number)
            //    {
            //        formatNumberReply = ta_sms.FormatNumber(mrr.Destination);

            //        if (!formatNumberReply.Contains(FORMAT_NUMBER_FAILED))
            //        {
            //            checkNumberReply = ta_sms.CheckNumber(formatNumberReply);

            //            if (checkNumberReply != CHECK_NUMBER_FAILED)
            //            {
            //                checkNumberReplyArray = checkNumberReply.Split(COMMA_SEPARATOR);

            //                if (!checkNumberReplyArray[1].StartsWith("+"))
            //                {
            //                    checkNumberReplyArray[1] = "+" + checkNumberReplyArray[1];
            //                }

            //                mrr.BeginEdit();
            //                mrr.Destination = checkNumberReplyArray[1];
            //                mrr.SMS_Credits = Convert.ToDecimal(checkNumberReplyArray[2]);
            //                mrr.EndEdit();
            //            }
            //            else
            //            {
            //                formatSuccess = false;
            //            }
            //        }
            //        else
            //        {
            //            formatSuccess = false;
            //        }
            //    }
            //}

            //?????????

            if (!formatSuccess)
            {
                lblMessage.Text = "Not all phone numbers could be verified.";
            }

            return formatSuccess;
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }
        finally
        {
            _SmsService.Dispose();


            //?????????
            //this.Message_Recipients.ValidationEnabled = true;
            //?????????
        }
    }

    // Check the SMS service is running
    private bool CheckSMSServiceRunning()
    {
        string serviceTestReply = string.Empty;
        string username = string.Empty;
        string password = string.Empty;

        if (!GetServiceUsernameAndPassword(ref username, ref password))
        {
            return false;
        }

        if (username.Trim().Length == 0 || password.Trim().Length == 0)
        {
            lblMessage.Text = "Unable to connect to the TextAnyWhere SMS Web Service.\r\nCheck your service username and password.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }

        


        serviceTestReply = _SmsService.ServiceTest(username, password);


        //??????
        if (!serviceTestReply.StartsWith("Service running"))//("SERVICE_TEST_OK"))
        {
            lblMessage.Text = "Unable to connect to the TextAnyWhere SMS Web Service.\r\nCheck your service username and password and ensure there is an available internet connection.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }

        return true;
    }

    private bool GetServiceUsernameAndPassword(ref string Username, ref string Password)
    {
        try
        {
            Username = "BU0727999";// OMM.Personnel.DAL.Properties.Settings.Default.TextAnywhereClientID;
            Password = "madrid"; //OMM.Personnel.DAL.Properties.Settings.Default.TextAnywhereClientPassword;

            return true;
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Unable to get username and password for the TextAnywhere Web service.\r\n" + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }
    }

    protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //try
        //{
        //    tbxMobileNumber.Text = GridView1.Rows[e.NewSelectedIndex].Cells[1].Text;
        //    //divNewMessage.Visible = false;
        //    //divRecipientList.Visible = false;
        //    //divButton1.Visible = false;
        //    //divSingleSMS.Visible = true;

        //    lblMessage.Text = "";
        //}
        //catch (Exception ex)
        //{
        //    lblMessage.Text = ex.Message;
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //}
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //try
        //{
        //    ClientScript.GetPostBackClientHyperlink((GridView)sender, "Select$" + e.Row.RowIndex);

        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        e.Row.Attributes["onmouseover"] =
        //        "javascript:setMouseOverColor(this);";
        //        e.Row.Attributes["onmouseout"] =
        //        "javascript:setMouseOutColor(this);";
        //        e.Row.Attributes["onclick"] =
        //        ClientScript.GetPostBackClientHyperlink
        //        (this.GridView1, "Select$" + e.Row.RowIndex);

        //        //e.Row.Height=10;
        //    }

        //    //Clean the error message
        //    lblMessage.Text = "";
        //}
        //catch (Exception ex)
        //{
        //    lblMessage.Text = ex.Message;
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //}
    }
    protected void btnBackToAll_Click(object sender, EventArgs e)
    {
        try
        {
            //divNewMessage.Visible = false;
            //divRecipientList.Visible = true;
            //divButton1.Visible = true;
            //divSingleSMS.Visible = false;

            lblMessage.Text = "";
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    //protected void  btnCancel_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect(@"~\Pages\Home.aspx");
    //}
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
