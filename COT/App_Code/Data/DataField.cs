using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Security;

namespace BUDI2_NS.Data
{
	public enum DataFieldMaskType
    {
        
        None,
        
        Date,
        
        Number,
        
        Time,
        
        DateTime,
    }
    
    public enum DataFieldAggregate
    {
        
        None,
        
        Sum,
        
        Count,
        
        Average,
        
        Max,
        
        Min,
    }
    
    public enum OnDemandDisplayStyle
    {
        
        Thumbnail,
        
        Link,
    }
    
    public enum TextInputMode
    {
        
        Text,
        
        Password,
        
        RichText,
        
        Note,
        
        Static,
    }
    
    public enum FieldSearchMode
    {
        
        Default,
        
        Required,
        
        Suggested,
        
        Allowed,
        
        Forbidden,
    }
    
    public class DataField
    {
        
        private string _name;
        
        private string _aliasName;
        
        private string _type;
        
        private string _label;
        
        private bool _isPrimaryKey;
        
        private bool _readOnly;
        
        private string _defaultValue;
        
        private string _headerText;
        
        private string _footerText;
        
        private bool _hidden;
        
        private bool _allowQBE;
        
        private bool _allowSorting;
        
        private bool _allowLEV;
        
        private string _dataFormatString;
        
        private string _copy;
        
        private string _hyperlinkFormatString;
        
        private bool _formatOnClient;
        
        private string _sourceFields;
        
        private int _categoryIndex;
        
        private bool _allowNulls;
        
        private int _columns;
        
        private int _rows;
        
        private bool _onDemand;
        
        private FieldSearchMode _search;
        
        private string _itemsDataController;
        
        private string _itemsDataView;
        
        private string _itemsDataValueField;
        
        private string _itemsDataTextField;
        
        private string _itemsStyle;
        
        private string _itemsNewDataView;
        
        private List<object[]> _items;
        
        private DataFieldAggregate _aggregate;
        
        private string _onDemandHandler;
        
        private OnDemandDisplayStyle _onDemandStyle;
        
        private TextInputMode _textMode;
        
        private DataFieldMaskType _maskType;
        
        private string _mask;
        
        private string _contextFields;
        
        private string _selectExpression;
        
        private string _formula;
        
        private bool _showInSummary;
        
        private bool _isMirror;
        
        private bool _htmlEncode;
        
        private int _autoCompletePrefixLength;
        
        private bool _calculated;
        
        private string _configuration;
        
        private string _editor;
        
        private bool _autoSelect;
        
        private bool _searchOnStart;
        
        private string _itemsDescription;
        
        private string _pivot;
        
        public DataField()
        {
            _items = new List<object[]>();
            _formatOnClient = true;
        }
        
        public DataField(XPathNavigator field, IXmlNamespaceResolver nm) : 
                this()
        {
            this._name = field.GetAttribute("name", String.Empty);
            this._type = field.GetAttribute("type", String.Empty);
            this._label = field.GetAttribute("label", String.Empty);
            this._isPrimaryKey = (field.GetAttribute("isPrimaryKey", String.Empty) == "true");
            this._readOnly = (field.GetAttribute("readOnly", String.Empty) == "true");
            this._onDemand = (field.GetAttribute("onDemand", String.Empty) == "true");
            this._defaultValue = field.GetAttribute("default", String.Empty);
            this._allowNulls = !((field.GetAttribute("allowNulls", String.Empty) == "false"));
            this._hidden = (field.GetAttribute("hidden", String.Empty) == "true");
            this._allowQBE = !((field.GetAttribute("allowQBE", String.Empty) == "false"));
            this._allowLEV = (field.GetAttribute("allowLEV", String.Empty) == "true");
            this._allowSorting = !((field.GetAttribute("allowSorting", String.Empty) == "false"));
            this._sourceFields = field.GetAttribute("sourceFields", String.Empty);
            if (field.GetAttribute("onDemandStyle", String.Empty) == "Link")
            	this._onDemandStyle = OnDemandDisplayStyle.Link;
            this._onDemandHandler = field.GetAttribute("onDemandHandler", String.Empty);
            this._contextFields = field.GetAttribute("contextFields", String.Empty);
            this._selectExpression = field.GetAttribute("select", String.Empty);
            bool computed = (field.GetAttribute("computed", String.Empty) == "true");
            if (computed)
            {
                _formula = ((string)(field.Evaluate("string(self::c:field/c:formula)", nm)));
                if (String.IsNullOrEmpty(_formula))
                	_formula = "null";
            }
            this._showInSummary = (field.GetAttribute("showInSummary", String.Empty) == "true");
            this._htmlEncode = !((field.GetAttribute("htmlEncode", String.Empty) == "false"));
            _calculated = (field.GetAttribute("calculated", String.Empty) == "true");
            this._configuration = ((string)(field.Evaluate("string(self::c:field/c:configuration)", nm)));
            this._dataFormatString = field.GetAttribute("dataFormatString", String.Empty);
            _formatOnClient = !((field.GetAttribute("formatOnClient", String.Empty) == "false"));
            string editor = field.GetAttribute("editor", String.Empty);
            if (!(String.IsNullOrEmpty(editor)))
            	_editor = editor;
        }
        
        public DataField(XPathNavigator field, IXmlNamespaceResolver nm, bool hidden) : 
                this(field, nm)
        {
            this._hidden = hidden;
        }
        
        public DataField(DataField field) : 
                this()
        {
            this._isMirror = true;
            this._name = (field.Name + "_Mirror");
            this._type = field.Type;
            this._label = field.Label;
            this._readOnly = field.ReadOnly;
            this._allowNulls = field.AllowNulls;
            this._allowQBE = field.AllowQBE;
            this._allowSorting = field.AllowSorting;
            this._allowLEV = field.AllowLEV;
            this._dataFormatString = field.DataFormatString;
            if (!(this._dataFormatString.Contains("{")))
            	this._dataFormatString = String.Format("{{0:{0}}}", this._dataFormatString);
            field._aliasName = this._name;
            this.FormatOnClient = false;
            field.FormatOnClient = true;
            field.DataFormatString = String.Empty;
            this._hidden = true;
        }
        
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        
        public string AliasName
        {
            get
            {
                return _aliasName;
            }
            set
            {
                _aliasName = value;
            }
        }
        
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        
        public string Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }
        
        public bool IsPrimaryKey
        {
            get
            {
                return _isPrimaryKey;
            }
            set
            {
                _isPrimaryKey = value;
            }
        }
        
        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
            }
        }
        
        public string DefaultValue
        {
            get
            {
                return _defaultValue;
            }
        }
        
        public bool HasDefaultValue
        {
            get
            {
                return !(String.IsNullOrEmpty(_defaultValue));
            }
        }
        
        public string HeaderText
        {
            get
            {
                return _headerText;
            }
            set
            {
                _headerText = value;
                if (!(String.IsNullOrEmpty(value)) && String.IsNullOrEmpty(_label))
                	_label = value;
            }
        }
        
        public string FooterText
        {
            get
            {
                return _footerText;
            }
            set
            {
                _footerText = value;
            }
        }
        
        public bool Hidden
        {
            get
            {
                return _hidden;
            }
            set
            {
                _hidden = value;
            }
        }
        
        public bool AllowQBE
        {
            get
            {
                return _allowQBE;
            }
            set
            {
                _allowQBE = value;
            }
        }
        
        public bool AllowSorting
        {
            get
            {
                return _allowSorting;
            }
            set
            {
                _allowSorting = value;
            }
        }
        
        public bool AllowLEV
        {
            get
            {
                return _allowLEV;
            }
            set
            {
                _allowLEV = value;
            }
        }
        
        public string DataFormatString
        {
            get
            {
                return _dataFormatString;
            }
            set
            {
                _dataFormatString = value;
            }
        }
        
        public string Copy
        {
            get
            {
                return _copy;
            }
            set
            {
                _copy = value;
            }
        }
        
        public string HyperlinkFormatString
        {
            get
            {
                return _hyperlinkFormatString;
            }
            set
            {
                _hyperlinkFormatString = value;
            }
        }
        
        public bool FormatOnClient
        {
            get
            {
                return _formatOnClient;
            }
            set
            {
                _formatOnClient = value;
            }
        }
        
        public string SourceFields
        {
            get
            {
                return _sourceFields;
            }
            set
            {
                _sourceFields = value;
            }
        }
        
        public int CategoryIndex
        {
            get
            {
                return _categoryIndex;
            }
            set
            {
                _categoryIndex = value;
            }
        }
        
        public bool AllowNulls
        {
            get
            {
                return _allowNulls;
            }
            set
            {
                _allowNulls = value;
            }
        }
        
        public int Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
            }
        }
        
        public int Rows
        {
            get
            {
                return _rows;
            }
            set
            {
                _rows = value;
            }
        }
        
        public bool OnDemand
        {
            get
            {
                return _onDemand;
            }
            set
            {
                _onDemand = value;
            }
        }
        
        public FieldSearchMode Search
        {
            get
            {
                return _search;
            }
            set
            {
                _search = value;
            }
        }
        
        public string ItemsDataController
        {
            get
            {
                return _itemsDataController;
            }
            set
            {
                _itemsDataController = value;
            }
        }
        
        public string ItemsDataView
        {
            get
            {
                return _itemsDataView;
            }
            set
            {
                _itemsDataView = value;
            }
        }
        
        public string ItemsDataValueField
        {
            get
            {
                return _itemsDataValueField;
            }
            set
            {
                _itemsDataValueField = value;
            }
        }
        
        public string ItemsDataTextField
        {
            get
            {
                return _itemsDataTextField;
            }
            set
            {
                _itemsDataTextField = value;
            }
        }
        
        public string ItemsStyle
        {
            get
            {
                return _itemsStyle;
            }
            set
            {
                _itemsStyle = value;
            }
        }
        
        public string ItemsNewDataView
        {
            get
            {
                return _itemsNewDataView;
            }
            set
            {
                _itemsNewDataView = value;
            }
        }
        
        public List<object[]> Items
        {
            get
            {
                return _items;
            }
        }
        
        public DataFieldAggregate Aggregate
        {
            get
            {
                return _aggregate;
            }
            set
            {
                _aggregate = value;
            }
        }
        
        public string OnDemandHandler
        {
            get
            {
                return _onDemandHandler;
            }
            set
            {
                _onDemandHandler = value;
            }
        }
        
        public OnDemandDisplayStyle OnDemandStyle
        {
            get
            {
                return _onDemandStyle;
            }
            set
            {
                _onDemandStyle = value;
            }
        }
        
        public TextInputMode TextMode
        {
            get
            {
                return _textMode;
            }
            set
            {
                _textMode = value;
            }
        }
        
        public DataFieldMaskType MaskType
        {
            get
            {
                return _maskType;
            }
            set
            {
                _maskType = value;
            }
        }
        
        public string Mask
        {
            get
            {
                return _mask;
            }
            set
            {
                _mask = value;
            }
        }
        
        public string ContextFields
        {
            get
            {
                return _contextFields;
            }
        }
        
        public string Formula
        {
            get
            {
                return _formula;
            }
            set
            {
                _formula = value;
            }
        }
        
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
        
        public bool IsMirror
        {
            get
            {
                return _isMirror;
            }
        }
        
        public bool HtmlEncode
        {
            get
            {
                return _htmlEncode;
            }
            set
            {
                _htmlEncode = value;
            }
        }
        
        public int AutoCompletePrefixLength
        {
            get
            {
                return _autoCompletePrefixLength;
            }
            set
            {
                _autoCompletePrefixLength = value;
            }
        }
        
        public bool Calculated
        {
            get
            {
                return _calculated;
            }
            set
            {
                _calculated = value;
            }
        }
        
        public string Configuration
        {
            get
            {
                return _configuration;
            }
            set
            {
                _configuration = value;
            }
        }
        
        public string Editor
        {
            get
            {
                return _editor;
            }
            set
            {
                _editor = value;
            }
        }
        
        public bool AutoSelect
        {
            get
            {
                return _autoSelect;
            }
            set
            {
                _autoSelect = value;
            }
        }
        
        public bool SearchOnStart
        {
            get
            {
                return _searchOnStart;
            }
            set
            {
                _searchOnStart = value;
            }
        }
        
        public string ItemsDescription
        {
            get
            {
                return _itemsDescription;
            }
            set
            {
                _itemsDescription = value;
            }
        }
        
        public string Pivot
        {
            get
            {
                return _pivot;
            }
            set
            {
                _pivot = value;
            }
        }
        
        public string SelectExpression()
        {
            return _selectExpression;
        }
        
        public void NormalizeDataFormatString()
        {
            if (!(String.IsNullOrEmpty(_dataFormatString)))
            {
                string fmt = _dataFormatString;
                if (!(fmt.Contains("{")))
                	_dataFormatString = String.Format("{{0:{0}}}", fmt);
            }
            else
            	if (_type == "DateTime")
                	_dataFormatString = "{0:d}";
        }
        
        public string ExpressionName()
        {
            if (IsMirror)
            	return Name.Substring(0, (Name.Length - "_Mirror".Length));
            return Name;
        }
        
        public bool SupportsStaticItems()
        {
            return (!(String.IsNullOrEmpty(ItemsDataController)) && !(((ItemsStyle == "AutoComplete") || (ItemsStyle == "Lookup"))));
        }
        
        public override string ToString()
        {
            if (!(String.IsNullOrEmpty(Formula)))
            	return String.Format("{0} as {1}; SQL: {2}", Name, Type, Formula);
            else
            	return String.Format("{0} as {1}", Name, Type);
        }
    }
}
