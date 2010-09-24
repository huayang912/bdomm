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
//using App.Models.Admin;
//using App.Models.Commodity;

/// <summary>
/// Summary description for SessionCache
/// </summary>
public class SessionCache
{
    /// <summary>
    /// Keeps Currently Logged In User
    /// </summary>
    public static User CurrentUser
    {
        get
        {
            if (HttpContext.Current.Session == null)
            {
                return null;
            }
            if (HttpContext.Current.Session["CURRENT_USER"] == null) return null;
            return HttpContext.Current.Session["CURRENT_USER"] as User;
        }
        set
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session["CURRENT_USER"] = value;
            }
        }
    }
    /// <summary>
    /// Clears the session.
    /// </summary>
    public static void ClearSession()
    {
        HttpContext.Current.Session.Clear();
    }
    public static String NotificationMessage
    {
        get
        {
            if (HttpContext.Current.Session == null)
            {
                return null;
            }
            if (HttpContext.Current.Session["NOTIFICATION_MESSAGE"] == null) return String.Empty;
            return HttpContext.Current.Session["NOTIFICATION_MESSAGE"] as String;
        }
        set
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session["NOTIFICATION_MESSAGE"] = value;
            }
        }
    }
    /// <summary>
    /// Tracks Failed Login Attempt
    /// </summary>
    public static int FailedLoginAttemptCount
    {
        get
        {
            if (HttpContext.Current.Session == null || HttpContext.Current.Session["FAILED_LOGIN_ATTEMP_COUNT"] == null)
                return 0;
            return Convert.ToInt32(HttpContext.Current.Session["FAILED_LOGIN_ATTEMP_COUNT"]);
        }
        set
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session["FAILED_LOGIN_ATTEMP_COUNT"] = value;
            }
        }
    }    
    /// <summary>
    /// Checks for the Edit Mode of Any Multi Step Form Data
    /// </summary>
    public static bool IsEditMode
    {
        get
        {
            if (HttpContext.Current.Session == null || HttpContext.Current.Session["IS_EDIT_MODE"] == null)
                return false;
            return Convert.ToBoolean(HttpContext.Current.Session["IS_EDIT_MODE"]);
        }
        set
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session["IS_EDIT_MODE"] = value;
            }
        }
    }
    
}
