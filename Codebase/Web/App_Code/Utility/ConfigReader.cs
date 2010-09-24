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
         
}
