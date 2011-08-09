using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BUDI2_NS.Handlers
{
	public partial class Details : BUDI2_NS.Web.PageBase
    {
        
        public string CssClass
        {
            get
            {
                return "Tall Wide";
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(IsPostBack))
            {
                string link = Request.QueryString["l"];
                if (!(String.IsNullOrEmpty(link)))
                {
                    if (!(link.Contains("&")))
                    	link = Encoding.Default.GetString(Convert.FromBase64String(link));
                    Match m = Regex.Match(link, "(.+?)(&|$)");
                    if (m.Success)
                    {
                        Div1.Visible = true;
                        Extender1.Controller = m.Groups[1].Value;
                        m = m.NextMatch();
                        while (m.Success)
                        {
                            Match pair = Regex.Match(m.Groups[1].Value, "^(\\w+)=(.+)$");
                            if (pair.Success)
                            {
                                if (!(String.IsNullOrEmpty(Extender1.FilterFields)))
                                {
                                    Extender1.FilterFields = (Extender1.FilterFields + ",");
                                    ExtenderFilter.Value = (ExtenderFilter.Value + ",");
                                }
                                Extender1.FilterFields = (Extender1.FilterFields + pair.Groups[1].Value);
                                ExtenderFilter.Value = (ExtenderFilter.Value + pair.Groups[2].Value);
                            }
                            m = m.NextMatch();
                        }
                    }
                }
            }
        }
    }
}
