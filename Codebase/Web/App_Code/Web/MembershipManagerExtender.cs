using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BUDI2_NS.Data;
using AjaxControlToolkit;

namespace BUDI2_NS.Web
{
	[TargetControlType(typeof(HtmlGenericControl))]
    [ToolboxItem(false)]
    public class MembershipManagerExtender : AquariumExtenderBase
    {
        
        public MembershipManagerExtender() : 
                base("Web.MembershipManager")
        {
        }
        
        protected override void ConfigureDescriptor(ScriptBehaviorDescriptor descriptor)
        {
        }
        
        protected override void ConfigureScripts(List<ScriptReference> scripts)
        {
            scripts.Add(CreateScriptReference("~/Scripts/Web.MembershipResources.js"));
            scripts.Add(CreateScriptReference("~/Scripts/Web.MembershipManager.js"));
            scripts.AddRange(ScriptObjectBuilder.GetScriptReferences(typeof(TabContainer)));
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            TabContainer tc = new TabContainer();
            Controls.Add(tc);
            ScriptObjectBuilder.RegisterCssReferences(tc);
            Controls.Clear();
        }
    }
}
