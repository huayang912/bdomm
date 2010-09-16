using System;
using System.Data;
using System.Collections.Generic;
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
	public enum DataViewSelectionMode
    {
        
        Single,
        
        Multiple,
    }
    
    public enum AutoHideMode
    {
        
        Nothing,
        
        Self,
        
        Container,
    }
    
    public class FieldFilter
    {
        
        private string _fieldName;
        
        private RowFilterOperation _operation;
        
        private object _value;
        
        public FieldFilter(string fieldName, RowFilterOperation operation) : 
                this(fieldName, operation, null)
        {
        }
        
        public FieldFilter(string fieldName, RowFilterOperation operation, object value)
        {
            this.FieldName = fieldName;
            this.Operation = operation;
            this.Value = value;
        }
        
        public string FieldName
        {
            get
            {
                return _fieldName;
            }
            set
            {
                _fieldName = value;
            }
        }
        
        public RowFilterOperation Operation
        {
            get
            {
                return _operation;
            }
            set
            {
                _operation = value;
            }
        }
        
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if ((((value != null) && (value is String)) && (Operation == RowFilterOperation.Like)) && !(((string)(value)).Contains("%")))
                	_value = String.Format("%{0}%", value);
                else
                	_value = value;
            }
        }
    }
    
    [TargetControlType(typeof(Panel))]
    [TargetControlType(typeof(HtmlContainerControl))]
    public class DataViewExtender : AquariumExtenderBase
    {
        
        private AutoHideMode _autoHide;
        
        private string _controller;
        
        private string _view;
        
        private int _pageSize;
        
        private bool _showActionBar;
        
        private string _filterSource;
        
        private string _filterFields;
        
        private string _startCommandName;
        
        private string _startCommandArgument;
        
        private string _selectedValue;
        
        private DataViewSelectionMode _selectionMode;
        
        private bool _showInSummary;
        
        private bool _showDescription;
        
        private bool _showViewSelector;
        
        private int _summaryFieldCount;
        
        private string _tag;
        
        private bool _enabled = true;
        
        private int _tabIndex;
        
        private bool _lookupMode;
        
        private string _lookupValue;
        
        private string _lookupText;
        
        private string _lookupPostBackExpression;
        
        private bool _allowCreateLookupItems;
        
        private bool _showQuickFind;
        
        public DataViewExtender() : 
                base("Web.DataView")
        {
            this._pageSize = 10;
            this._showActionBar = true;
            _showDescription = true;
            _showViewSelector = true;
            _summaryFieldCount = 5;
            _showQuickFind = true;
        }
        
        [System.ComponentModel.Description("Specifies user interface element that will be hidden if data view can be automati" +
            "cally hidden.")]
        [System.ComponentModel.DefaultValue(AutoHideMode.Nothing)]
        public AutoHideMode AutoHide
        {
            get
            {
                return _autoHide;
            }
            set
            {
                _autoHide = value;
            }
        }
        
        [System.ComponentModel.Description("The name of the data controller. Controllers are stored in the \"~/Controllers\" fo" +
            "lder of your project. Do not include the file extension.")]
        public string Controller
        {
            get
            {
                return _controller;
            }
            set
            {
                _controller = value;
            }
        }
        
        [System.ComponentModel.Description("The name of the startup view in the data controller. The first view is displayed " +
            "if the property is left blank.")]
        public string View
        {
            get
            {
                return _view;
            }
            set
            {
                _view = value;
            }
        }
        
        [System.ComponentModel.Description("The number of rows displayed by grid views of the data controller.")]
        [System.ComponentModel.DefaultValue(10)]
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }
        
        [System.ComponentModel.Description("Specifies if the action bar is displayed above the views of the data controller.")]
        [System.ComponentModel.DefaultValue(true)]
        public bool ShowActionBar
        {
            get
            {
                return _showActionBar;
            }
            set
            {
                _showActionBar = value;
            }
        }
        
        [System.ComponentModel.Description(@"Defines the external source of filtering values. This may be the name of URL parameter or DHTML element in the page. Data view extender will automatically recognize if the DHTML element is also extended and will interface with the client-side extender object.")]
        public string FilterSource
        {
            get
            {
                return _filterSource;
            }
            set
            {
                _filterSource = value;
            }
        }
        
        [System.ComponentModel.Description("Specify the field(s) of the data controller that shall be filtered with the value" +
            "s from the source defined by the FilterSource property.")]
        public string FilterFields
        {
            get
            {
                return _filterFields;
            }
            set
            {
                _filterFields = value;
            }
        }
        
        [System.ComponentModel.Description("Specify a command that must be executed when the data view is instantiated.")]
        public string StartCommandName
        {
            get
            {
                return _startCommandName;
            }
            set
            {
                _startCommandName = value;
            }
        }
        
        [System.ComponentModel.Description("Specify an argument of a command that must be executed when the data view is inst" +
            "antiated.")]
        public string StartCommandArgument
        {
            get
            {
                return _startCommandArgument;
            }
            set
            {
                _startCommandArgument = value;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        public string SelectedValue
        {
            get
            {
                if (_selectedValue == null)
                	_selectedValue = Page.Request.Params[String.Format("{0}_{1}_SelectedValue", ClientID, Controller)];
                return _selectedValue;
            }
            set
            {
                _selectedValue = value;
            }
        }
        
        [System.ComponentModel.Description("The selection mode for the data view.")]
        [System.ComponentModel.DefaultValue(DataViewSelectionMode.Single)]
        public DataViewSelectionMode SelectionMode
        {
            get
            {
                return _selectionMode;
            }
            set
            {
                _selectionMode = value;
            }
        }
        
        [System.ComponentModel.Description("The data view is presented in the page summary.")]
        [System.ComponentModel.DefaultValue(false)]
        public bool ShowInSummary
        {
            get
            {
                return _showInSummary;
            }
            set
            {
                _showInSummary = value;
            }
        }
        
        [System.ComponentModel.Description("The view descripition is displayed at the top the data extender target.")]
        [System.ComponentModel.DefaultValue(true)]
        public bool ShowDescription
        {
            get
            {
                return _showDescription;
            }
            set
            {
                _showDescription = value;
            }
        }
        
        [System.ComponentModel.Description("The view selector is displayed in the action bar.")]
        [System.ComponentModel.DefaultValue(true)]
        public bool ShowViewSelector
        {
            get
            {
                return _showViewSelector;
            }
            set
            {
                _showViewSelector = value;
            }
        }
        
        [System.ComponentModel.Description("The maximum number of fields that can be contributed to the summary.")]
        [System.ComponentModel.DefaultValue(5)]
        public int SummaryFieldCount
        {
            get
            {
                return _summaryFieldCount;
            }
            set
            {
                _summaryFieldCount = value;
            }
        }
        
        [System.ComponentModel.Description("The identifying string passed from the client to server. Use it to filter actions" +
            " and to program business rules.")]
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        public int TabIndex
        {
            get
            {
                return _tabIndex;
            }
            set
            {
                _tabIndex = value;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        public string LookupValue
        {
            get
            {
                return _lookupValue;
            }
            set
            {
                _lookupValue = value;
                _lookupMode = true;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        public string LookupText
        {
            get
            {
                return _lookupText;
            }
            set
            {
                _lookupText = value;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        public string LookupPostBackExpression
        {
            get
            {
                return _lookupPostBackExpression;
            }
            set
            {
                _lookupPostBackExpression = value;
            }
        }
        
        [System.ComponentModel.DefaultValue(true)]
        [System.ComponentModel.Browsable(false)]
        public bool AllowCreateLookupItems
        {
            get
            {
                return _allowCreateLookupItems;
            }
            set
            {
                _allowCreateLookupItems = value;
            }
        }
        
        [System.ComponentModel.DefaultValue(true)]
        public bool ShowQuickFind
        {
            get
            {
                return _showQuickFind;
            }
            set
            {
                _showQuickFind = value;
            }
        }
        
        protected override void ConfigureDescriptor(ScriptBehaviorDescriptor descriptor)
        {
            Page.ClientScript.RegisterHiddenField(String.Format("{0}_{1}_SelectedValue", ClientID, Controller), SelectedValue);
            descriptor.AddProperty("controller", this.Controller);
            descriptor.AddProperty("viewId", this.View);
            descriptor.AddProperty("pageSize", this.PageSize);
            descriptor.AddProperty("showActionBar", this.ShowActionBar);
            if (_lookupMode)
            {
                descriptor.AddProperty("mode", "Lookup");
                descriptor.AddProperty("lookupValue", LookupValue);
                descriptor.AddProperty("lookupText", LookupText);
                if (!(String.IsNullOrEmpty(LookupPostBackExpression)))
                	descriptor.AddProperty("lookupPostBackExpression", LookupPostBackExpression);
                if (AllowCreateLookupItems)
                	descriptor.AddProperty("newViewId", BUDI2_NS.Data.Controller.LookupActionArgument(Controller, "New"));
            }
            if (!(String.IsNullOrEmpty(FilterSource)))
            {
                Control source = NamingContainer.FindControl(FilterSource);
                if (source != null)
                	descriptor.AddProperty("filterSource", source.ClientID);
                else
                	descriptor.AddProperty("filterSource", this.FilterSource);
            }
            if (!(String.IsNullOrEmpty(FilterFields)))
            	descriptor.AddProperty("filterFields", this.FilterFields);
            descriptor.AddProperty("cookie", Guid.NewGuid().ToString());
            if (!(String.IsNullOrEmpty(StartCommandName)))
            	descriptor.AddProperty("startCommandName", StartCommandName);
            if (!(String.IsNullOrEmpty(StartCommandArgument)))
            	descriptor.AddProperty("startCommandArgument", StartCommandArgument);
            if (SelectionMode == DataViewSelectionMode.Multiple)
            	descriptor.AddProperty("selectionMode", "Multiple");
            if (!(Enabled))
            	descriptor.AddProperty("enabled", false);
            if (TabIndex > 0)
            	descriptor.AddProperty("tabIndex", TabIndex);
            if (ShowInSummary)
            	descriptor.AddProperty("showInSummary", "true");
            if (!(ShowDescription))
            	descriptor.AddProperty("showDescription", false);
            if (!(ShowViewSelector))
            	descriptor.AddProperty("showViewSelector", false);
            if (!(String.IsNullOrEmpty(Tag)))
            	descriptor.AddProperty("tag", Tag);
            if (SummaryFieldCount != 5)
            	descriptor.AddProperty("summaryFieldCount", SummaryFieldCount);
            if (!(ShowQuickFind))
            	descriptor.AddProperty("showQuickFind", false);
            if (AutoHide != AutoHideMode.Nothing)
            	descriptor.AddProperty("autoHide", Convert.ToInt16(AutoHide));
        }
        
        protected override void ConfigureScripts(List<ScriptReference> scripts)
        {
        }
        
        public void AssignFilter(List<FieldFilter> filter)
        {
            this.AssignFilter(filter.ToArray());
        }
        
        public void AssignFilter(FieldFilter[] filter)
        {
            // prepare a list of filter expressions
            SortedDictionary<string, string> list = new SortedDictionary<string, string>();
            foreach (FieldFilter ff in filter)
            {
                string filterExpression = null;
                if (!(list.TryGetValue(ff.FieldName, out filterExpression)))
                	filterExpression = String.Empty;
                else
                	filterExpression = (filterExpression + "\\0");
                filterExpression = (filterExpression 
                            + (RowFilterAttribute.ComparisonOperations[Convert.ToInt32(ff.Operation)] + Convert.ToString(ff.Value).Replace("\'", "\\\'")));
                list[ff.FieldName] = filterExpression;
            }
            // create a filter
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("var dv = Web.DataView.find(\'{0}\');dv.beginFilter();var f;", this.ID);
            foreach (string fieldName in list.Keys)
            	sb.AppendFormat("f=dv.findField(\'{0}\');if(f)dv.applyFilter(f,\':\', \'{1}\');", fieldName, list[fieldName]);
            sb.Append("dv.endFilter();");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(DataViewExtender), ("AsyncPostBackScript" + this.ID), sb.ToString(), true);
        }
    }
}
