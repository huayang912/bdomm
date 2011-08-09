using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using BUDI2_NS.Data;

namespace BUDI2_NS.Web
{
	public partial class PageBase : System.Web.UI.Page
    {
        
        protected override void InitializeCulture()
        {
            CultureManager.Initialize();
            base.InitializeCulture();
        }
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft)
            	foreach (Control c in Controls)
                	ChangeCurrentCultureTextFlowDirection(c);
        }
        
        private bool ChangeCurrentCultureTextFlowDirection(Control c)
        {
            if (c is HtmlGenericControl)
            {
                HtmlGenericControl gc = ((HtmlGenericControl)(c));
                if (gc.TagName == "body")
                {
                    gc.Attributes["dir"] = "rtl";
                    gc.Attributes["class"] = "RTL";
                    return true;
                }
            }
            else
            	foreach (Control child in c.Controls)
                {
                    bool result = ChangeCurrentCultureTextFlowDirection(child);
                    if (result)
                    	return true;
                }
            return false;
        }
        
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder sb = new StringBuilder();
            HtmlTextWriter tempWriter = new HtmlTextWriter(new StringWriter(sb));
            base.Render(tempWriter);
            tempWriter.Flush();
            tempWriter.Close();
            writer.Write(BUDI2_NS.Data.Localizer.Replace("Pages", Path.GetFileName(Request.PhysicalPath), sb.ToString()));
        }
    }
}
