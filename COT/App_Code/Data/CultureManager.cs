using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace BUDI2_NS.Data
{
	public class CultureManager
    {
        
        public const string AutoDetectCulture = "Detect,Detect";
        
        public static string[] SupportedCultures = new string[] {
                "en-GB,en-GB"};
        
        public static void Initialize()
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx == null)
            	return;
            HttpCookie cultureCookie = ctx.Request.Cookies[".COTCULTURE"];
            string culture = null;
            if (cultureCookie != null)
            	culture = cultureCookie.Value;
            if (String.IsNullOrEmpty(culture) || (culture == CultureManager.AutoDetectCulture))
            	if (ctx.Request.UserLanguages != null)
                	foreach (string l in ctx.Request.UserLanguages)
                    {
                        string[] languageInfo = l.Split(';');
                        foreach (string c in SupportedCultures)
                        	if (c.StartsWith(languageInfo[0]))
                            {
                                culture = c;
                                break;
                            }
                        if (culture != null)
                        	break;
                    }
                else
                	culture = SupportedCultures[0];
            if (!(String.IsNullOrEmpty(culture)))
            {
                int cultureIndex = Array.IndexOf(SupportedCultures, culture);
                if (!((cultureIndex == -1)))
                {
                    string[] ci = culture.Split(',');
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci[0]);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(ci[1]);
                    if (ctx.Handler is Page)
                    {
                        Page p = ((Page)(ctx.Handler));
                        p.Culture = ci[0];
                        p.UICulture = ci[1];
                        if (cultureCookie != null)
                        {
                            if (cultureCookie.Value == CultureManager.AutoDetectCulture)
                            	cultureCookie.Expires = DateTime.Now.AddDays(-14);
                            else
                            	cultureCookie.Expires = DateTime.Now.AddDays(14);
                            ctx.Response.AppendCookie(cultureCookie);
                        }
                    }
                }
            }
        }
        
        public static string ResolveEmbeddedResourceName(string resourceName, string culture)
        {
            return ResolveEmbeddedResourceName(typeof(CultureManager).Assembly, resourceName, culture);
        }
        
        public static string ResolveEmbeddedResourceName(string resourceName)
        {
            return ResolveEmbeddedResourceName(typeof(CultureManager).Assembly, resourceName, Thread.CurrentThread.CurrentUICulture.Name);
        }
        
        public static string ResolveEmbeddedResourceName(Assembly a, string resourceName, string culture)
        {
            string extension = Path.GetExtension(resourceName);
            string fileName = Path.GetFileNameWithoutExtension(resourceName);
            string localizedResourceName = String.Format("{0}.{1}{2}", fileName, culture.Replace("-", "_"), extension);
            ManifestResourceInfo mri = a.GetManifestResourceInfo(localizedResourceName);
            if (mri == null)
            {
                if (culture.Contains("-"))
                	localizedResourceName = String.Format("{0}.{1}_{2}", fileName, culture.Substring(0, culture.LastIndexOf("-")).Replace("-", "_"), extension);
                else
                	localizedResourceName = String.Format("{0}.{1}_{2}", fileName, culture, extension);
                mri = a.GetManifestResourceInfo(localizedResourceName);
            }
            if (mri == null)
            	localizedResourceName = resourceName;
            return localizedResourceName;
        }
    }
    
    public class GenericHandlerBase
    {
        
        public GenericHandlerBase()
        {
            CultureManager.Initialize();
        }
    }
}
