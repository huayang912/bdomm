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
    public const String SLIDER_DIRECTORY = "/Images/SliderImages";
    public const String UPLOADED_RATE_DIRECTORY = "/UploadedRate";
    public const String CONFIRMATION_MSG_CUSTOM_ATTR = "ConfirmationMessage";    

    #region Pages
    public class Pages
    {
        #region Private Pages
        //public const String PERFORMENCE_TABLE = "/Pages/Private/PerformenceTable.aspx";
        //public const String STAFF_LIST = "/Pages/Private/StaffList.aspx";
        //public const String STAFF_CHANGE = "/Pages/Private/StaffChange.aspx";
        //public const String USER_CHANGE = "/Pages/Private/UserChange.aspx";
        //public const String USER_LIST = "/Pages/Private/UserList.aspx";
        //public const String FDP_CHANGE = "/Pages/Private/FDPChange.aspx";
        //public const String FDP_LIST = "/Pages/Private/FDPList.aspx";
        //public const String CHANGE_PASSWORD = "/Pages/Private/ChangePassword.aspx";
        //public const String CATCHMENT = "/Pages/Private/Catchment.aspx";
        //public const String CATCHMENT_FRAME_DISTRICT = "/Pages/Private/IframePages/Catchment_Frame_District.aspx";
        //public const String CATCHMENT_FRAME_FDP = "/Pages/Private/IframePages/Catchment_Frame_FDP.aspx";
        //public const String CATCHMENT_FRAME_PROGRAM = "/Pages/Private/IframePages/Catchment_Frame_Program.aspx";
        //public const String CATCHMENT_FRAME_SERVICESITE = "/Pages/Private/IframePages/Catchment_Frame_ServiceSite.aspx";
        //public const String CATCHMENT_FRAME_STAFF = "/Pages/Private/IframePages/Catchment_Frame_Staff.aspx";
        //public const String CATCHMENT_FRAME_UNION = "/Pages/Private/IframePages/Catchment_Frame_Union.aspx";
        //public const String CATCHMENT_FRAME_UPAZILLA = "/Pages/Private/IframePages/Catchment_Frame_Upazilla.aspx";
        //public const String CATCHMENT_FRAME_VILLAGE = "/Pages/Private/IframePages/Catchment_Frame_Village.aspx";

        //public const String DAM_CHANGE_STEP1 = "/Pages/Private/DAMChange_Step1.aspx";
        //public const String DAM_CHANGE_STEP2 = "/Pages/Private/DAMChange_Step2.aspx";
        //public const String DAM_CHANGE_STEP3 = "/Pages/Private/DAMChange_Step3.aspx";
        //public const String DAM_LIST = "/Pages/Private/DAMList.aspx";
        //public const String VOLUNTEER_LIST = "/Pages/Private/VolunteerList.aspx";
        //public const String VOLUNTEER_CHANGE = "/Pages/Private/VolunteerChange.aspx";
        //public const String WAYBILL_CHANGE = "/Pages/Private/WayBillChange.aspx";
        //public const String WAYBILL_LIST = "/Pages/Private/WayBillList.aspx";

        //public const String WAYBILL_CHANGE_STEP1 = "/Pages/Private/WayBillChangeStep1.aspx";
        //public const String WAYBILL_CHANGE_STEP2 = "/Pages/Private/WayBillChangeStep2.aspx";
        //public const String WAYBILL_CHANGE_STEP3 = "/Pages/Private/WayBillChangeStep3.aspx";

        //public const String STACKING_CHANGE_STEP1 = "/Pages/Private/StackingChangeStep1.aspx";
        //public const String STACKING_CHANGE_STEP2 = "/Pages/Private/StackingChangeStep2.aspx";
        //public const String STACKING_CHANGE_STEP3 = "/Pages/Private/StackingChangeStep3.aspx";

        //public const String RECONSTITUTION_CHANGE_STEP1 = "/Pages/Private/ReconstitutionStep1.aspx";
        //public const String RECONSTITUTION_CHANGE_STEP2 = "/Pages/Private/ReconstitutionStep2.aspx";
        //public const String RECONSTITUTION_CHANGE_STEP3 = "/Pages/Private/ReconstitutionStep3.aspx";
                
        #endregion        

        #region Public Pages
        public const String HOME_PAGE = "/Default.aspx";
        public const String LOG_IN = "/Pages/Public/Login.aspx";
        public const String ERROR = "/Error.aspx";
        public const String ORGANIZATION_MANAGEMENT = "/Pages/Private/OrganizationManagement.aspx";
        public const String SHOW_CONTENT = "/Pages/Public/ShowContent.aspx";
        public const String SHOW_MESSAGE = "/Pages/Public/ShowMessage.aspx";
        #endregion
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
        //public const String DECIMAL_FORMAT_CALCULATION = "{0:00.###}";
        public const String DATE_FROMAT_DISPLAY = "MMMM dd, yyyy";
        //public const String DATE_FROMAT_DISPLAY_WITH_TIME = "dddd, MMMM dd, yyyy hh:mm:ss tt";
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
        //public const String ORG_CODE = "OrgCode";
        //public const String STAFF_ID = "StaffID";
        //public const String USER_ID = "UserID";
        public const String ID = "ID";
        public const String ENQUIRY_ID = "EnqID";
        public const String LOG_OUT = "Logout";
        //public const String STARTS_WITH = "Start";
        //public const String KEYWORD = "K";
        //public const String DISTRICT_CODE = "District";
        //public const String UPAZILLA_CODE = "Upazilla";
        //public const String UNION_CODE = "Union";
        //public const String VILLAGE_CODE = "Village";
        //public const String SERVICE_SITE_CODE = "ServiceSite";
        //public const String FDP_CODE = "FDP";
        //public const String WAREHOUSE_CODE = "WareHouse";
        //public const String PROGRAM_CODE = "Prog";
        //public const String TRACKING_ID = "Tracking";
        //public const String WAY_BILL_NO = "WB";
        //public const String BL_NO = "BL";
        //public const String WAY_BILL_STACKID = "StackID";
        //public const String CARD_NUMBER = "CardID";
        //public const String RLDC = "Rldc";
        //public const String DELETE = "Delete";
        //public const String DAM = "DAM";
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
