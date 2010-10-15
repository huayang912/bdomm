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
/// Summary description for ConfigReader
/// </summary>
public class ConfigReader
{
    private static String GetAppSettings(String key)
    {
        return ConfigurationManager.AppSettings[key];
    }

    public static String TextAnywhereClientID
    {
        get
        {
            return GetAppSettings("TextAnywhereClientID");
        }
    }
    public static String TextAnywhereClientPassword
    {
        get
        {
            return GetAppSettings("TextAnywhereClientPassword");
        }
    }
    public static String CSharpCalendarDateFormat
    {
        get
        {
            return GetAppSettings("CSharpCalendarDateFormat");
        }
    }
    public static String JSCalendarDateFormat
    {
        get
        {
            return GetAppSettings("JSCalendarDateFormat");
        }
    }
    public static bool SendSmsToClient
    {
        get
        {
            String value = GetAppSettings("SendSmsToClient");
            if (String.Compare(value, "True", true) == 0)
                return true;
            return false;
        }
    }

    public static string BILLING_REF
    {
        get
        {
            String value = GetAppSettings("BILLING_REF");
            return value;
        }
    }

    public static string ORIGINATOR
    {
        get
        {
            String value = GetAppSettings("ORIGINATOR");
            return value;
        }
    }
}
