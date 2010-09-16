using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
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
	public abstract class AquariumExtenderBase : ExtenderControl
    {
        
        private string _clientComponentName;
        
        private string _servicePath;
        
        private SortedDictionary<string, object> _properties;
        
        public AquariumExtenderBase(string clientComponentName)
        {
            this._clientComponentName = clientComponentName;
        }
        
        [System.ComponentModel.Description("A path to a data controller web service.")]
        [System.ComponentModel.DefaultValue("~/Services/DataControllerService.asmx")]
        public string ServicePath
        {
            get
            {
                if (String.IsNullOrEmpty(_servicePath))
                	return "~/Services/DataControllerService.asmx";
                return _servicePath;
            }
            set
            {
                _servicePath = value;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        public SortedDictionary<string, object> Properties
        {
            get
            {
                if (_properties == null)
                	_properties = new SortedDictionary<string, object>();
                return _properties;
            }
        }
        
        protected override System.Collections.Generic.IEnumerable<ScriptDescriptor> GetScriptDescriptors(Control targetControl)
        {
            if (ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
            {
                bool requireRegistration = false;
                Control c = this;
                while (!(requireRegistration) && ((c != null) && !((c is HtmlForm))))
                {
                    if (c is UpdatePanel)
                    	requireRegistration = true;
                    c = c.Parent;
                }
                if (!(requireRegistration))
                	return null;
            }
            ScriptBehaviorDescriptor descriptor = new ScriptBehaviorDescriptor(_clientComponentName, targetControl.ClientID);
            descriptor.AddProperty("id", this.ClientID);
            descriptor.AddProperty("baseUrl", ResolveClientUrl("~"));
            descriptor.AddProperty("servicePath", ResolveClientUrl(ServicePath));
            ConfigureDescriptor(descriptor);
            return new ScriptBehaviorDescriptor[] {
                    descriptor};
        }
        
        protected abstract void ConfigureDescriptor(ScriptBehaviorDescriptor descriptor);
        
        public static ScriptReference CreateScriptReference(string p)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentUICulture;
            List<string> scripts = ((List<string>)(HttpRuntime.Cache["AllApplicationScripts"]));
            if (scripts == null)
            {
                scripts = new List<string>();
                string[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Scripts"), "*.js");
                foreach (string script in files)
                {
                    Match m = Regex.Match(Path.GetFileName(script), "^(.+?)\\.(\\w\\w(\\-\\w+)*)\\.js$", RegexOptions.Compiled);
                    if (m.Success)
                    	scripts.Add(m.Value);
                }
                HttpRuntime.Cache["AllApplicationScripts"] = scripts;
            }
            if (scripts.Count > 0)
            {
                Match name = Regex.Match(p, "^(?\'Path\'.+\\/)(?\'Name\'.+?)\\.js$", RegexOptions.Compiled);
                if (name.Success)
                {
                    string test = String.Format("{0}.{1}.js", name.Groups["Name"].Value, culture.Name);
                    bool success = scripts.Contains(test);
                    if (!(success))
                    {
                        test = String.Format("{0}.{1}.js", name.Groups["Name"].Value, culture.Name.Substring(0, 2));
                        success = scripts.Contains(test);
                    }
                    if (success)
                    	p = (name.Groups["Path"].Value + test);
                }
            }
            return new ScriptReference(p);
        }
        
        protected override System.Collections.Generic.IEnumerable<ScriptReference> GetScriptReferences()
        {
            if (ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
            	return null;
            List<ScriptReference> scripts = new List<ScriptReference>();
            scripts.Add(CreateScriptReference("~/Scripts/Web.DataViewResources.js"));
            scripts.Add(CreateScriptReference("~/Scripts/Web.Menu.js"));
            scripts.Add(CreateScriptReference("~/Scripts/Web.DataView.js"));
            scripts.AddRange(ScriptObjectBuilder.GetScriptReferences(typeof(ModalPopupExtender)));
            scripts.AddRange(ScriptObjectBuilder.GetScriptReferences(typeof(AlwaysVisibleControlExtender)));
            scripts.AddRange(ScriptObjectBuilder.GetScriptReferences(typeof(PopupControlExtender)));
            scripts.AddRange(ScriptObjectBuilder.GetScriptReferences(typeof(CalendarExtender)));
            scripts.AddRange(ScriptObjectBuilder.GetScriptReferences(typeof(MaskedEditExtender)));
            scripts.AddRange(ScriptObjectBuilder.GetScriptReferences(typeof(AutoCompleteExtender)));
            ConfigureScripts(scripts);
            return scripts;
        }
        
        protected abstract void ConfigureScripts(List<ScriptReference> scripts);
        
        protected override void OnLoad(EventArgs e)
        {
            if (ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
            	return;
            base.OnLoad(e);
            CalendarExtender ce = new CalendarExtender();
            Controls.Add(ce);
            ScriptObjectBuilder.RegisterCssReferences(ce);
            Controls.Clear();
        }
    }
}
