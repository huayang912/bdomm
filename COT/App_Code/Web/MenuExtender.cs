using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using BUDI2_NS.Data;

namespace BUDI2_NS.Web
{
	public enum MenuHoverStyle
    {
        
        Auto = 1,
        
        Click = 1,
        
        ClickAndStay = 1,
    }
    
    public enum MenuOrientation
    {
        
        Horizontal,
    }
    
    public enum MenuPopupPosition
    {
        
        Left,
        
        Right,
    }
    
    public enum MenuItemDescriptionStyle
    {
        
        None,
        
        Inline,
        
        ToolTip,
    }
    
    [TargetControlType(typeof(Panel))]
    [TargetControlType(typeof(HtmlContainerControl))]
    [DefaultProperty("TargetControlID")]
    public class MenuExtender : System.Web.UI.WebControls.HierarchicalDataBoundControl, IExtenderControl
    {
        
        private string _items;
        
        private ScriptManager _sm;
        
        private string _targetControlID;
        
        private bool _visible;
        
        private MenuHoverStyle _hoverStyle;
        
        private MenuPopupPosition _popupPosition;
        
        private MenuItemDescriptionStyle _itemDescriptionStyle;
        
        private bool _showSiteActions;
        
        public MenuExtender() : 
                base()
        {
            this.Visible = true;
            ItemDescriptionStyle = MenuItemDescriptionStyle.ToolTip;
            HoverStyle = MenuHoverStyle.Auto;
        }
        
        [IDReferenceProperty]
        [Category("Behavior")]
        [DefaultValue("")]
        public string TargetControlID
        {
            get
            {
                return _targetControlID;
            }
            set
            {
                _targetControlID = value;
            }
        }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public override bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
            }
        }
        
        public MenuHoverStyle HoverStyle
        {
            get
            {
                return _hoverStyle;
            }
            set
            {
                _hoverStyle = value;
            }
        }
        
        public MenuPopupPosition PopupPosition
        {
            get
            {
                return _popupPosition;
            }
            set
            {
                _popupPosition = value;
            }
        }
        
        public MenuItemDescriptionStyle ItemDescriptionStyle
        {
            get
            {
                return _itemDescriptionStyle;
            }
            set
            {
                _itemDescriptionStyle = value;
            }
        }
        
        [System.ComponentModel.Description("The \"Site Actions\" menu is automatically displayed.")]
        [System.ComponentModel.DefaultValue(false)]
        public bool ShowSiteActions
        {
            get
            {
                return _showSiteActions;
            }
            set
            {
                _showSiteActions = value;
            }
        }
        
        protected override void PerformDataBinding()
        {
            base.PerformDataBinding();
            if (!(IsBoundUsingDataSourceID) && (DataSource != null))
            	return;
            HierarchicalDataSourceView view = GetData(String.Empty);
            IHierarchicalEnumerable enumerable = view.Select();
            if (enumerable != null)
            {
                StringBuilder sb = new StringBuilder();
                RecursiveDataBindInternal(enumerable, sb);
                _items = sb.ToString();
            }
        }
        
        private void RecursiveDataBindInternal(IHierarchicalEnumerable enumerable, StringBuilder sb)
        {
            bool first = true;
            if (this.Site != null)
            	return;
            foreach (object item in enumerable)
            {
                IHierarchyData data = enumerable.GetHierarchyData(item);
                if (null != data)
                {
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(data);
                    if (props.Count > 0)
                    {
                        string title = ((string)(props["Title"].GetValue(data)));
                        string description = ((string)(props["Description"].GetValue(data)));
                        string url = ((string)(props["Url"].GetValue(data)));
                        if (first)
                        	first = false;
                        else
                        	sb.Append(",");
                        sb.AppendFormat("{{\'title\':\'{0}\',\'url\':\'{1}\'", Localizer.JavaScriptStringEncode(title), Localizer.JavaScriptStringEncode(url));
                        if (!(String.IsNullOrEmpty(description)))
                        	sb.AppendFormat(",\'description\':\'{0}\'", Localizer.JavaScriptStringEncode(description));
                        if (url == Page.Request.RawUrl)
                        	sb.Append(",\'selected\':true");
                        if (data.HasChildren)
                        {
                            IHierarchicalEnumerable childEnumerable = data.GetChildren();
                            if (null != childEnumerable)
                            {
                                sb.Append(",\'children\':[");
                                RecursiveDataBindInternal(childEnumerable, sb);
                                sb.Append("]");
                            }
                        }
                        sb.Append("}");
                    }
                }
            }
        }
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _sm = ScriptManager.GetCurrent(Page);
        }
        
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (null == _sm)
            	return;
            string script = String.Format("Web.Menu.Nodes.{0}=[{1}];", this.ClientID, _items);
            Control target = Page.Form.FindControl(TargetControlID);
            if ((null != target) && target.Visible)
            	ScriptManager.RegisterStartupScript(this, typeof(MenuExtender), "Nodes", script, true);
            _sm.RegisterExtenderControl<MenuExtender>(this, target);
        }
        
        protected override void Render(HtmlTextWriter writer)
        {
            if ((null == _sm) || _sm.IsInAsyncPostBack)
            	return;
            _sm.RegisterScriptDescriptors(this);
        }
        
        IEnumerable<ScriptDescriptor> IExtenderControl.GetScriptDescriptors(Control targetControl)
        {
            ScriptBehaviorDescriptor descriptor = new ScriptBehaviorDescriptor("Web.Menu", targetControl.ClientID);
            descriptor.AddProperty("id", this.ClientID);
            if (HoverStyle != MenuHoverStyle.Auto)
            	descriptor.AddProperty("hoverStyle", Convert.ToInt32(HoverStyle));
            if (PopupPosition != MenuPopupPosition.Left)
            	descriptor.AddProperty("popupPosition", Convert.ToInt32(PopupPosition));
            if (ItemDescriptionStyle != MenuItemDescriptionStyle.ToolTip)
            	descriptor.AddProperty("itemDescriptionStyle", Convert.ToInt32(ItemDescriptionStyle));
            if (ShowSiteActions)
            	descriptor.AddProperty("showSiteActions", "true");
            return new ScriptBehaviorDescriptor[] {
                    descriptor};
        }
        
        IEnumerable<ScriptReference> IExtenderControl.GetScriptReferences()
        {
            List<ScriptReference> scripts = new List<ScriptReference>();
            scripts.Add(AquariumExtenderBase.CreateScriptReference("~/Scripts/Web.DataViewResources.js"));
            scripts.Add(AquariumExtenderBase.CreateScriptReference("~/Scripts/Web.DataView.js"));
            scripts.Add(AquariumExtenderBase.CreateScriptReference("~/Scripts/Web.Menu.js"));
            scripts.AddRange(ScriptObjectBuilder.GetScriptReferences(typeof(PopupControlExtender)));
            return scripts;
        }
    }
}
