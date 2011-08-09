<%@ WebHandler Language="C#" Class="BUDI2_NS.Handlers.Report" %>

using System;
using System.Web.Caching;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Web;
using Microsoft.Reporting.WebForms;
using BUDI2_NS.Data;

namespace BUDI2_NS.Handlers
{
	public class Report : GenericHandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        
        bool IHttpHandler.IsReusable
        {
            get
            {
                return false;
            }
        }
        
        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            string c = context.Request["c"];
            string q = context.Request["q"];
            if (String.IsNullOrEmpty(c) || String.IsNullOrEmpty(q))
            	throw new Exception("Invalid report request.");
            // 
#pragma warning disable 0618
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            // 
#pragma warning restore 0618
            // create a data table for report
            PageRequest request = serializer.Deserialize<PageRequest>(q);
            request.PageIndex = 0;
            request.PageSize = Int32.MaxValue;
            // load report definition
            string reportTemplate = Controller.CreateReportInstance(null, context.Request.Form["a"], request.Controller, request.View);
            // create a data reader
            DbDataReader reader = ControllerFactory.CreateDataEngine().ExecuteReader(request);
            DataTable table = new DataTable(request.Controller);
            for (int i = 0; (i < reader.FieldCount); i++)
            	table.Columns.Add(new DataColumn(reader.GetName(i), reader.GetFieldType(i)));
            table.Load(reader);
            reader.Close();
            // figure report output format
            Match m = Regex.Match(c, "^(ReportAs|Report)(Pdf|Excel|Image|Word|)$");
            string reportFormat = m.Groups[2].Value;
            if (String.IsNullOrEmpty(reportFormat))
            	reportFormat = "Pdf";
            // render a report
            string mimeType = null;
            string encoding = null;
            string fileNameExtension = null;
            string[] streams = null;
            Warning[] warnings = null;
            using (LocalReport report = new LocalReport())
            {
                report.EnableHyperlinks = true;
                report.ExecuteReportInCurrentAppDomain(System.Reflection.Assembly.GetExecutingAssembly().Evidence);
                report.LoadReportDefinition(new StringReader(reportTemplate));
                report.DataSources.Add(new ReportDataSource(request.Controller, table));
                byte[] reportData = report.Render(reportFormat, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                // send report data to the client
                context.Response.Clear();
                context.Response.ContentType = mimeType;
                context.Response.AddHeader("Content-Length", reportData.Length.ToString());
                context.Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}_{1}.{2}", request.Controller, request.View, fileNameExtension));
                context.Response.OutputStream.Write(reportData, 0, reportData.Length);
            }
        }
    }
}
