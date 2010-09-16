<%@ WebHandler Language="C#" Class="BUDI2_NS.Handlers.Export" %>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using BUDI2_NS.Data;

namespace BUDI2_NS.Handlers
{
	public class Export : IHttpHandler
    {
        
        bool IHttpHandler.IsReusable
        {
            get
            {
                return true;
            }
        }
        
        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            string fileName = null;
            string q = context.Request.Params["q"];
            if (!(String.IsNullOrEmpty(q)))
            {
                if (q.Contains("{"))
                {
                    q = Convert.ToBase64String(Encoding.Default.GetBytes(q));
                    context.Response.Redirect(("~/Export.ashx?q=" + HttpUtility.UrlEncode(q)));
                }
                q = Encoding.Default.GetString(Convert.FromBase64String(q));
                // 
#pragma warning disable 0618
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                // 
#pragma warning restore 0618
                ActionArgs args = serializer.Deserialize<ActionArgs>(q);
                // execute data export
                IDataController controller = ControllerFactory.CreateDataController();
                // create an Excel Web Query
                if ((args.CommandName == "ExportRowset") && !(context.Request.Url.AbsoluteUri.Contains("&d")))
                {
                    string webQueryUrl = (context.Request.Url.AbsoluteUri + "&d");
                    context.Response.Write(("Web\r\n1\r\n" + webQueryUrl));
                    context.Response.ContentType = "text/x-ms-iqy";
                    context.Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}{1}.iqy", args.Controller, args.View));
                    return;
                }
                // export data in the requested format
                ActionResult result = controller.Execute(args.Controller, args.View, args);
                fileName = ((string)(result.Values[0].Value));
                // send file to output
                if (File.Exists(fileName))
                {
                    if (args.CommandName == "ExportCsv")
                    {
                        context.Response.ContentType = "text/csv";
                        context.Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}{1}.csv", args.Controller, args.View));
                    }
                    else
                    	if (args.CommandName == "ExportRowset")
                        	context.Response.ContentType = "text/xml";
                        else
                        	context.Response.ContentType = "application/rss+xml";
                    StreamReader reader = File.OpenText(fileName);
                    while (!(reader.EndOfStream))
                    {
                        string s = reader.ReadLine();
                        context.Response.Output.WriteLine(s);
                    }
                    reader.Close();
                    File.Delete(fileName);
                }
            }
            if (String.IsNullOrEmpty(fileName))
            	throw new HttpException(404, String.Empty);
        }
    }
}
