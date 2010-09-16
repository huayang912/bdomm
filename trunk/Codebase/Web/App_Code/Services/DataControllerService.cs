using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using BUDI2_NS.Data;

namespace BUDI2_NS.Services
{
	[WebService(Namespace="http://www.codeontime.com/productsdaf.aspx")]
    [WebServiceBinding(ConformsTo=WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class DataControllerService : System.Web.Services.WebService
    {
        
        public DataControllerService()
        {
        }
        
        protected List<string[]> Permalinks
        {
            get
            {
                List<string[]> links = ((List<string[]>)(Session["Permalinks"]));
                if (links == null)
                {
                    links = new List<string[]>();
                    Session["Permalinks"] = links;
                }
                return links;
            }
        }
        
        [WebMethod(EnableSession=true)]
        [ScriptMethod]
        public ViewPage GetPage(string controller, string view, PageRequest request)
        {
            return ControllerFactory.CreateDataController().GetPage(controller, view, request);
        }
        
        [WebMethod(EnableSession=true)]
        [ScriptMethod]
        public object[] GetListOfValues(string controller, string view, DistinctValueRequest request)
        {
            return ControllerFactory.CreateDataController().GetListOfValues(controller, view, request);
        }
        
        [WebMethod(EnableSession=true)]
        [ScriptMethod]
        public ActionResult Execute(string controller, string view, ActionArgs args)
        {
            return ControllerFactory.CreateDataController().Execute(controller, view, args);
        }
        
        [WebMethod(EnableSession=true)]
        [ScriptMethod]
        public string[] GetCompletionList(string prefixText, int count, string contextKey)
        {
            return ControllerFactory.CreateAutoCompleteManager().GetCompletionList(prefixText, count, contextKey);
        }
        
        protected string[] FindPermalink(string link)
        {
            foreach (string[] entry in Permalinks)
            	if (entry[0] == link)
                	return entry;
            return null;
        }
        
        [WebMethod(EnableSession=true)]
        [ScriptMethod]
        public void SavePermalink(string link, string html)
        {
            string[] permalink = FindPermalink(link);
            if (Permalinks.Contains(permalink))
            	Permalinks.Remove(permalink);
            if (!(String.IsNullOrEmpty(html)))
            	Permalinks.Insert(0, new string[] {
                            link,
                            html});
            else
            	if (Permalinks.Count > 0)
                	Permalinks.RemoveAt(0);
            while (Permalinks.Count > 20)
            	Permalinks.RemoveAt((Permalinks.Count - 1));
        }
        
        [WebMethod]
        [ScriptMethod]
        public string EncodePermalink(string link)
        {
            return String.Format("{0}://{1}{2}/default.aspx?_link={3}", Context.Request.Url.Scheme, Context.Request.Url.Authority, Context.Request.ApplicationPath, HttpUtility.UrlEncode(Convert.ToBase64String(Encoding.Default.GetBytes(link))));
        }
        
        [WebMethod(EnableSession=true)]
        [ScriptMethod]
        public string[][] ListAllPermalinks()
        {
            return Permalinks.ToArray();
        }
    }
}
