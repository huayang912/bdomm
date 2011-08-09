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
	public class ViewPage
    {
        
        private int _skipCount;
        
        private int _readCount;
        
        private string[] _originalFilter;
        
        private bool _requiresMetaData;
        
        private bool _requiresRowCount;
        
        private object[] _aggregates;
        
        private object[] _newRow;
        
        private List<DataField> _fields;
        
        private string _sortExpression;
        
        private int _totalRowCount;
        
        private int _pageIndex;
        
        private int _pageSize;
        
        private int _pageOffset;
        
        private string[] _filter;
        
        private List<object[]> _rows;
        
        private string _distinctValueFieldName;
        
        private List<View> _views;
        
        private List<ActionGroup> _actionGroups;
        
        private List<Category> _categories;
        
        private DynamicExpression[] _expressions;
        
        private string _controller;
        
        private string _view;
        
        private bool _inTransaction;
        
        private bool _allowDistinctFieldInFilter;
        
        private string[] _icons;
        
        private FieldValue[] _levs;
        
        [ThreadStatic]
        public static bool PopulatingStaticItems;
        
        private SortedDictionary<string, object> _customFilter;
        
        private DataTable _pivotColumns;
        
        public ViewPage() : 
                this(new PageRequest(0, 0, null, null))
        {
        }
        
        public ViewPage(DistinctValueRequest request) : 
                this(new PageRequest(0, 0, null, request.Filter))
        {
            _distinctValueFieldName = request.FieldName;
            _pageSize = request.MaximumValueCount;
            _allowDistinctFieldInFilter = request.AllowFieldInFilter;
            _controller = request.Controller;
            _view = request.View;
        }
        
        public ViewPage(PageRequest request)
        {
            this.PageOffset = request.PageOffset;
            _requiresMetaData = ((request.PageIndex == -1) || request.RequiresMetaData);
            _requiresRowCount = ((request.PageIndex < 0) || request.RequiresRowCount);
            if (request.PageIndex == -2)
            	request.PageIndex = 0;
            _pageSize = request.PageSize;
            if (request.PageIndex > 0)
            	_pageIndex = request.PageIndex;
            _rows = new List<object[]>();
            _fields = new List<DataField>();
            _skipCount = (_pageIndex * request.PageSize);
            _readCount = request.PageSize;
            _sortExpression = request.SortExpression;
            _filter = request.Filter;
            _totalRowCount = -1;
            _views = new List<View>();
            _actionGroups = new List<ActionGroup>();
            _categories = new List<Category>();
            _controller = request.Controller;
            _view = request.View;
            _inTransaction = !(String.IsNullOrEmpty(request.Transaction));
        }
        
        public bool RequiresMetaData
        {
            get
            {
                return _requiresMetaData;
            }
        }
        
        public bool RequiresRowCount
        {
            get
            {
                return _requiresRowCount;
            }
        }
        
        public bool RequiresAggregates
        {
            get
            {
                foreach (DataField field in Fields)
                	if (field.Aggregate != DataFieldAggregate.None)
                    	return true;
                return false;
            }
        }
        
        public object[] Aggregates
        {
            get
            {
                return _aggregates;
            }
            set
            {
                _aggregates = value;
            }
        }
        
        public object[] NewRow
        {
            get
            {
                return _newRow;
            }
            set
            {
                _newRow = value;
            }
        }
        
        public List<DataField> Fields
        {
            get
            {
                return _fields;
            }
        }
        
        public string SortExpression
        {
            get
            {
                return _sortExpression;
            }
            set
            {
                _sortExpression = value;
            }
        }
        
        public int TotalRowCount
        {
            get
            {
                return _totalRowCount;
            }
            set
            {
                _totalRowCount = value;
                int pageCount = (value / this.PageSize);
                if ((value % this.PageSize) > 0)
                	pageCount++;
                if (pageCount <= PageIndex)
                	this._pageIndex = 0;
            }
        }
        
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
        }
        
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
        }
        
        public int PageOffset
        {
            get
            {
                return _pageOffset;
            }
            set
            {
                _pageOffset = value;
            }
        }
        
        public string[] Filter
        {
            get
            {
                return _filter;
            }
        }
        
        public List<object[]> Rows
        {
            get
            {
                return _rows;
            }
        }
        
        public string DistinctValueFieldName
        {
            get
            {
                return _distinctValueFieldName;
            }
        }
        
        public List<View> Views
        {
            get
            {
                return _views;
            }
        }
        
        public List<ActionGroup> ActionGroups
        {
            get
            {
                return _actionGroups;
            }
        }
        
        public List<Category> Categories
        {
            get
            {
                return _categories;
            }
        }
        
        public DynamicExpression[] Expressions
        {
            get
            {
                return _expressions;
            }
            set
            {
                _expressions = value;
            }
        }
        
        public string Controller
        {
            get
            {
                return _controller;
            }
        }
        
        public string View
        {
            get
            {
                return _view;
            }
        }
        
        public bool InTransaction
        {
            get
            {
                return _inTransaction;
            }
        }
        
        public bool AllowDistinctFieldInFilter
        {
            get
            {
                return _allowDistinctFieldInFilter;
            }
        }
        
        public string[] Icons
        {
            get
            {
                return _icons;
            }
            set
            {
                _icons = value;
            }
        }
        
        public bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }
        
        public FieldValue[] LEVs
        {
            get
            {
                return _levs;
            }
            set
            {
                _levs = value;
            }
        }
        
        public bool SkipNext()
        {
            if (_skipCount == 0)
            	return false;
            _skipCount--;
            return true;
        }
        
        public bool ReadNext()
        {
            if (_readCount == 0)
            	return false;
            _readCount--;
            return true;
        }
        
        public void AcceptAllRows()
        {
            _readCount = Int32.MaxValue;
            _skipCount = 0;
        }
        
        public DataField FindField(string name)
        {
            foreach (DataField field in Fields)
            	if (field.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                	return field;
            return null;
        }
        
        public bool PopulateStaticItems(DataField field, FieldValue[] contextValues)
        {
            if (field.SupportsStaticItems() && (String.IsNullOrEmpty(field.ContextFields) || (contextValues != null)))
            {
                if (PopulatingStaticItems)
                	return true;
                PopulatingStaticItems = true;
                try
                {
                    string[] filter = null;
                    if (!(String.IsNullOrEmpty(field.ContextFields)))
                    {
                        List<string> contextFilter = new List<string>();
                        Match m = Regex.Match(field.ContextFields, "(\\w+)=(.+?)($|,)");
                        while (m.Success)
                        {
                            Match vm = Regex.Match(m.Groups[2].Value, "^\'(.+?)\'$");
                            if (vm.Success)
                            	contextFilter.Add(String.Format("{0}:={1}", m.Groups[1].Value, vm.Groups[1].Value));
                            else
                            	foreach (FieldValue cv in contextValues)
                                	if (cv.Name == m.Groups[2].Value)
                                    {
                                        contextFilter.Add(String.Format("{0}:={1}", m.Groups[1].Value, cv.NewValue));
                                        break;
                                    }
                            m = m.NextMatch();
                        }
                        filter = contextFilter.ToArray();
                    }
                    PageRequest request = new PageRequest(-1, 1000, field.ItemsDataTextField, filter);
                    ViewPage page = ControllerFactory.CreateDataController().GetPage(field.ItemsDataController, field.ItemsDataView, request);
                    int dataValueFieldIndex = page.Fields.IndexOf(page.FindField(field.ItemsDataValueField));
                    if (dataValueFieldIndex == -1)
                    	foreach (DataField aField in page.Fields)
                        	if (aField.IsPrimaryKey)
                            {
                                dataValueFieldIndex = page.Fields.IndexOf(aField);
                                break;
                            }
                    int dataTextFieldIndex = page.Fields.IndexOf(page.FindField(field.ItemsDataTextField));
                    if (dataTextFieldIndex == -1)
                    {
                        int i = 0;
                        while ((dataTextFieldIndex == -1) && (i < page.Fields.Count))
                        {
                            DataField f = page.Fields[i];
                            if (!(f.Hidden) && (f.Type == "String"))
                            	dataTextFieldIndex = i;
                            i++;
                        }
                        if (dataTextFieldIndex == -1)
                        	dataTextFieldIndex = 0;
                    }
                    List<int> fieldIndexes = new List<int>();
                    fieldIndexes.Add(dataValueFieldIndex);
                    fieldIndexes.Add(dataTextFieldIndex);
                    if (!(String.IsNullOrEmpty(field.Copy)))
                    {
                        Match m = Regex.Match(field.Copy, "(\\w+)=(\\w+)");
                        while (m.Success)
                        {
                            int copyFieldIndex = page.Fields.IndexOf(page.FindField(m.Groups[2].Value));
                            if (copyFieldIndex >= 0)
                            	fieldIndexes.Add(copyFieldIndex);
                            m = m.NextMatch();
                        }
                    }
                    foreach (object[] row in page.Rows)
                    {
                        object[] values = new object[fieldIndexes.Count];
                        for (int i = 0; (i < fieldIndexes.Count); i++)
                        {
                            int copyFieldIndex = fieldIndexes[i];
                            if (copyFieldIndex >= 0)
                            	values[i] = row[copyFieldIndex];
                        }
                        field.Items.Add(values);
                    }
                    return true;
                }
                finally
                {
                    PopulatingStaticItems = false;
                }
            }
            return false;
        }
        
        public ViewPage ToResult(ControllerConfiguration configuration, XPathNavigator mainView)
        {
            if (!(_requiresMetaData))
            {
                Fields.Clear();
                Expressions = null;
            }
            else
            {
                XPathNodeIterator viewIterator = configuration.Navigator.Select("/c:dataController/c:views/c:view[not(@virtualViewId!=\'\')]", configuration.Resolver);
                while (viewIterator.MoveNext())
                	Views.Add(new View(viewIterator.Current, mainView, configuration.Resolver));
                XPathNodeIterator actionGroupIterator = configuration.Navigator.Select("/c:dataController/c:actions//c:actionGroup", configuration.Resolver);
                while (actionGroupIterator.MoveNext())
                	ActionGroups.Add(new ActionGroup(actionGroupIterator.Current, configuration.Resolver));
                foreach (DataField field in Fields)
                	PopulateStaticItems(field, null);
            }
            if (_originalFilter != null)
            	_filter = _originalFilter;
            if (RequiresMetaData && ((HttpContext.Current != null) && (HttpContext.Current.Session != null)))
            	LEVs = ((FieldValue[])(HttpContext.Current.Session[String.Format("{0}$LEVs", _controller)]));
            AddPivotHeadersToFields();
            return this;
        }
        
        public DataTable ToDataTable()
        {
            return ToDataTable("table");
        }
        
        public DataTable ToDataTable(string tableName)
        {
            DataTable table = new DataTable(tableName);
            foreach (DataField field in Fields)
            	table.Columns.Add(field.Name, DataControllerBase.TypeMap[field.Type]);
            foreach (object[] row in Rows)
            {
                DataRow newRow = table.NewRow();
                for (int i = 0; (i < Fields.Count); i++)
                {
                    object v = row[i];
                    if (v == null)
                    	v = DBNull.Value;
                    newRow[i] = v;
                }
                table.Rows.Add(newRow);
            }
            table.AcceptChanges();
            return table;
        }
        
        public List<T> ToList<T>()
        
        {
            Type objectType = typeof(T);
            List<T> list = new List<T>();
            object[] args = new object[1];
            foreach (object[] row in Rows)
            {
                T instance = ((T)(objectType.Assembly.CreateInstance(objectType.FullName)));
                int i = 0;
                foreach (DataField field in Fields)
                {
                    args[0] = row[i];
                    try
                    {
                        objectType.InvokeMember(field.Name, System.Reflection.BindingFlags.SetProperty, null, instance, args);
                    }
                    catch (Exception )
                    {
                    }
                    i++;
                }
                list.Add(instance);
            }
            return list;
        }
        
        public bool CustomFilteredBy(string fieldName)
        {
            return ((_customFilter != null) && _customFilter.ContainsKey(fieldName));
        }
        
        public void ApplyDataFilter(IDataFilter dataFilter, string controller, string view, string lookupContextController, string lookupContextView, string lookupContextFieldName)
        {
            if (dataFilter == null)
            	return;
            if (_filter == null)
            	_filter = new string[0];
            IDataFilter2 dataFilter2 = null;
            if (typeof(IDataFilter2).IsInstanceOfType(dataFilter))
            {
                dataFilter2 = ((IDataFilter2)(dataFilter));
                dataFilter2.AssignContext(controller, view, lookupContextController, lookupContextView, lookupContextFieldName);
            }
            List<string> newFilter = new List<string>(_filter);
            _customFilter = new SortedDictionary<string, object>();
            if (dataFilter2 != null)
            	dataFilter2.Filter(controller, view, _customFilter);
            else
            	dataFilter.Filter(_customFilter);
            foreach (string key in _customFilter.Keys)
            {
                object v = _customFilter[key];
                if ((v == null) || !(v.GetType().IsArray))
                	v = new object[] {
                            v};
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}:", key);
                foreach (object item in ((Array)(v)))
                {
                    if (dataFilter2 != null)
                    	sb.Append(item);
                    else
                    	sb.AppendFormat("={0}", item);
                    sb.Append(Convert.ToChar(0));
                }
                newFilter.Add(sb.ToString());
            }
            _originalFilter = _filter;
            _filter = newFilter.ToArray();
        }
        
        public void UpdateFieldValue(string fieldName, object[] row, object value)
        {
            for (int i = 0; (i < Fields.Count); i++)
            	if (Fields[i].Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))
                	row[i] = value;
        }
        
        public object SelectFieldValue(string fieldName, object[] row)
        {
            for (int i = 0; (i < Fields.Count); i++)
            	if (Fields[i].Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))
                	return row[i];
            return null;
        }
        
        public void AddPivotHeadersToFields()
        {
            if (_pivotColumns == null)
            	return;
            foreach (DataField field in Fields)
            	if (!(field.Hidden) && String.IsNullOrEmpty(field.Pivot))
                {
                    Match m = Regex.Match(field.Name, "PivotCol(\\d+)_(\\w+)");
                    if (m.Success)
                    {
                        int rowIndex = Convert.ToInt32(m.Groups[1].Value);
                        if (rowIndex < _pivotColumns.Rows.Count)
                        {
                            string prefix = null;
                            foreach (DataColumn c in _pivotColumns.Columns)
                            	if (c.DataType == typeof(string))
                                {
                                    prefix = Convert.ToString(_pivotColumns.Rows[rowIndex][c]);
                                    break;
                                }
                            if (String.IsNullOrEmpty(field.HeaderText))
                            	field.Label = String.Format("{0} {1}", prefix, field.Label);
                            else
                            	field.HeaderText = String.Format("{0} {1}", prefix, field.HeaderText);
                        }
                        else
                        	field.Hidden = true;
                    }
                }
        }
        
        public DataTable EnumeratePivotColumns()
        {
            if (_pivotColumns == null)
            	foreach (DataField field in Fields)
                	if ((field.Pivot == "ColumnKey") && !(field.Name.StartsWith("PivotCol")))
                    {
                        PageRequest request = new PageRequest();
                        request.Controller = field.ItemsDataController;
                        request.View = field.ItemsDataView;
                        request.RequiresMetaData = true;
                        request.PageIndex = 0;
                        request.PageSize = 10;
                        ViewPage page = ControllerFactory.CreateDataController().GetPage(field.ItemsDataController, field.ItemsDataView, request);
                        _pivotColumns = page.ToDataTable();
                        // rename columns
                        Match m = Regex.Match(field.Copy, "(\\w+)=(\\w+)");
                        while (m.Success)
                        {
                            DataColumn c = _pivotColumns.Columns[m.Groups[2].Value];
                            if (c != null)
                            	c.ColumnName = m.Groups[1].Value;
                            m = m.NextMatch();
                        }
                        break;
                    }
            return _pivotColumns;
        }
    }
}
