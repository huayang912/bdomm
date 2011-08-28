using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for AppConstants
/// </summary>
public class AppConstants
{
    public const String ERROR_MESSAGE = @"Sorry!<br/>System encountered a problem while saving your preference. An administrator has been notified about the details of this problem. Please try after some time.";
    //public const String SLIDER_DIRECTORY = "/Images/SliderImages";
    //public const String UPLOADED_RATE_DIRECTORY = "/UploadedRate";
    public const String CONFIRMATION_MSG_CUSTOM_ATTR = "ConfirmationMessage";
    public const String TEMP_DIRECTORY = "/Temp";
    public const String ENQUIRY_ATTACHMENTS = "/EnquiryAttachments";
    public const String PERSONNEL_CV_DIRECTORY = "/UploadedCV";
    

    #region Pages
    public class Pages
    {        
        public const String HOME_PAGE = "~/Pages/Home.aspx";
        public const String ACCESS_DENIED = "~/AccessDenied.aspx";
        public const String LOG_IN = "~/Login.aspx";
        public const String ERROR = "/Error.aspx";
        public const String ORGANIZATION_MANAGEMENT = "/Pages/Private/OrganizationManagement.aspx";
        public const String SHOW_CONTENT = "/Pages/Public/ShowContent.aspx";
        public const String SHOW_MESSAGE = "/Pages/Public/ShowMessage.aspx";

        public const String QUOTATION_SUBMIT = "QuotationSubmit.aspx";
        public const String QUOTATION_DECISION = "QuotationDecision.aspx";
        public const String QUOTATION_CHANGE = "QuotationChange.aspx";
        public const String QUOTATION_DETAILS = "QuoteSrch.aspx";
        public const String QUOTATION_DETAILS_Archive = "quotations_unsuccess.aspx";          
        public const String ENQUIRY_DETAILS = "EnquirySch.aspx";
        public const String ENQUIRY_DETAILS_Archive = "EnquirySch_archive.aspx";
        public const String PERSONNEL_DETAILS = "PersonnelDetails.aspx";
        public const String PROJECT_NOTE_DETAILS = "ProjectNoteDetails.aspx";
    }
    #endregion

    #region Value Formats
    public class ValueOf
    {
        /// <summary>
        /// Is used only for Client Side jQuery Calendar
        /// </summary>
        //public const String CALENDAR_DATE_FORMAT = "dd/mm/yy";

        public const String DECIMAL_FORMAT = "{0:0,0.00}";
        //public const string DECIMAL_FORMAT_CALCULATION = "{0:00.#############}";
        public const String DECIMAL_FORMAT_FOR_TEXTBOX = "{0:0}";
        public const String DATE_FROMAT_DISPLAY = "MMMM dd, yyyy";
        public const String DATE_FROMAT_DISPLAY_WITH_TIME = "dddd, MMMM dd, yyyy hh:mm:ss tt";
    }
    #endregion

    #region User Roles
    public class UserRoles
    {
        public const String ADMINISTRATOR = "Administrator";
        public const String MEMBER = "Member";
    }
    #endregion

    #region UI CSS Classes
    public class UI
    {
        public const String ERROR_MESSAGE_CLASS = "ErrorMessage";
        public const String BOLD_MESSAGE_CLASS = "MessageCommon";
        public const String MESSAGE_BOX_CLASS = "MessageBox";
        public const String ERROR_MESSAGE_BOX_CLASS = "ErrorMessageBox";
        public const String SELECTION_CHECK_BOX_NAME = "chkSelection";
    }
    #endregion

    #region Query String Params
    public class QueryString
    {        
        public const String ID = "ID";
        public const String ENQUIRY_ID = "EnqID";
        public const String LOG_OUT = "Logout";
        public const String QUOTATION_ID = "QuotationID";
        public const String CONTACT_ID = "ContactID";
        public const String SUCCESS_MSG = "SCMSG";
        public const String DELETE = "Delete";
        public const String ON_TABLE = "OnTable";    
    }

    #endregion

    #region Cookie Variables
    public class Cookie
    {
        public const String BASE = "McAID";
        public const String REMEMBER_ME = "RememberMe";
    }

    #endregion

    #region Email Templates
    public class EmailTemplate
    {
        public const String GENERAL_EMAIL_TEMPLATE = "GeneralTemplate.html";
    }
    /// <summary>
    /// Custom Tag Constants in the Email Templates
    /// </summary>
    public class ETConstants
    {
        public const String MESSAGE = "[*MESSAGE*]";
    }
    #endregion

    #region Messages
    public static class Message
    {
        public const String EDIT_PERMISSION_DENIED = "You do not have permission to edit this data.";
        public const String DELETE_PERMISSION_DENIED = "You do not have permission to delete this data.";
    }
    #endregion
}
