using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Data;
using App.Core.Extensions;
using System.Web.Services;

public partial class Pages_PersonnelChange : BasePage
{
    protected int _ID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            CheckPersonnelInfo();
        }
    }
    protected void BindPageInfo()
    {
        _ID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
        if (_ID > 0)
            ltrHeading.Text = "Edit Personnel";

        Page.Title = WebUtil.GetPageTitle(ltrHeading.Text);
        this.Master.SelectedTab = SelectedTab.Personnel;
    }
    protected void CheckPersonnelInfo()
    {
        if (_ID > 0)
        {
            OMMDataContext context = new OMMDataContext();
            var personnel = context.Contacts.FirstOrDefault(P => P.ID == _ID);
            if (personnel == null)
            {
                WebUtil.ShowMessageBox(divMessage, "Sorrey! requested personnel was not found.", true);
                pnlFormContainer.Visible = false ;
            }
        }
    }
    
    #region SMS Sending Functionality

    private const char COMMA_SEPARATOR = ',';
    private const char COLON_SEPARATOR = ':';
    private static String _ErrorMessage = String.Empty;
    private const String SERVICE_TEST_OK = "Service running";
    private static SMSService.TextAnywhere_SMS _SmsService = null;

    [WebMethod]
    public static App.CustomModels.SendSmsStatus SendSms(String telephoneNumber, String messageText)
    {
        String userName = ConfigReader.TextAnywhereClientID;
        String password = ConfigReader.TextAnywhereClientPassword;
        String[] serviceReplyArray = null;
        String serviceReply = String.Empty;
        ///Create the Web Service Object for Sending SMS
        _SmsService = new SMSService.TextAnywhere_SMS();
        if (IsSmsServiceRunning())
        {
            if (ConfigReader.SendSmsToClient)
            {
                OMMDataContext context = new OMMDataContext();
                SMS_Message smsMessageRef = context.dalSMSMessagesInsert(String.Empty, DateTime.Now).SingleOrDefault();
                //SMSService.TextAnywhere_SMS smsService = new SMSService.TextAnywhere_SMS();
                serviceReply = _SmsService.SendSMSEx(userName, password,
                            smsMessageRef.Client_Ref, ConfigReader.BILLING_REF,
                            (int)CONNECTION_TYPES.LOW_VOLUME, ConfigReader.ORIGINATOR,
                            (int)ORIGINATOR_TYPES.NAME, telephoneNumber,
                            messageText, 0, (int)REPLY_TYPES.NONE, String.Empty);
            }
            // Extract return codes
            serviceReplyArray = serviceReply.Split(COMMA_SEPARATOR);

            if (serviceReplyArray.Length != 1) //receivers.Count)
            {
                _ErrorMessage = "Unable to send SMS message. SMS Service did not return the expected response.";
            }
        }
        App.CustomModels.SendSmsStatus reply = new App.CustomModels.SendSmsStatus();
        if (String.IsNullOrEmpty(_ErrorMessage))
        {
            reply.StatusID = 1;
            reply.Message = telephoneNumber;
        }
        else
        {
            reply.StatusID = -1;
            reply.Message = _ErrorMessage;
        }
        return reply;
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
            _ErrorMessage = "Unable to connect to the TextAnyWhere SMS Web Service.<br/>Check application configuration for service username and password.";
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

    #endregion SMS Sending Functionality
}
