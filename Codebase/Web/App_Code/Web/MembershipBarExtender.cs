using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text;
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
    public class MembershipBarExtender : AquariumExtenderBase
    {
        
        public MembershipBarExtender() : 
                base("Web.Membership")
        {
        }
        
        protected override void ConfigureDescriptor(ScriptBehaviorDescriptor descriptor)
        {
            descriptor.AddProperty("displayRememberMe", Properties["DisplayRememberMe"]);
            descriptor.AddProperty("rememberMeSet", Properties["RememberMeSet"]);
            descriptor.AddProperty("displaySignUp", Properties["DisplaySignUp"]);
            descriptor.AddProperty("displayPasswordRecovery", Properties["DisplayPasswordRecovery"]);
            descriptor.AddProperty("displayMyAccount", Properties["DisplayMyAccount"]);
            descriptor.AddProperty("welcome", Properties["Welcome"]);
            descriptor.AddProperty("displayHelp", Properties["DisplayHelp"]);
            descriptor.AddProperty("enableHistory", Properties["EnableHistory"]);
            descriptor.AddProperty("enablePermalinks", Properties["EnablePermalinks"]);
            descriptor.AddProperty("displayLogin", Properties["DisplayLogin"]);
            if (Properties.ContainsKey("IdleUserTimeout"))
            	descriptor.AddProperty("idleTimeout", Properties["IdleUserTimeout"]);
            string link = Page.Request["_link"];
            if (!(String.IsNullOrEmpty(link)))
            {
                string[] permalink = Encoding.Default.GetString(Convert.FromBase64String(link.Split(',')[0])).Split('?');
                descriptor.AddProperty("commandLine", permalink[1]);
            }
        }
        
        protected override void ConfigureScripts(List<ScriptReference> scripts)
        {
            scripts.Add(CreateScriptReference("~/Scripts/Web.MembershipResources.js"));
            scripts.Add(CreateScriptReference("~/Scripts/Web.Membership.js"));
        }
    }
}
