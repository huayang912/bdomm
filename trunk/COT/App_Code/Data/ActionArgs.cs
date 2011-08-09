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
	public class ActionArgs
    {
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _commandName;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _commandArgument;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _lastCommandName;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _contextKey;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _cookie;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _controller;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _view;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private FieldValue[] _values;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string[] _filter;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _sortExpression;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string[] _selectedValues;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private FieldValue[] _externalFilter;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _saveLEVs;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _transaction;
        
        public ActionArgs()
        {
            if ((HttpContext.Current != null) && (Current == null))
            	HttpContext.Current.Items["ActionArgs_Current"] = this;
        }
        
        public string CommandName
        {
            get
            {
                return _commandName;
            }
            set
            {
                _commandName = value;
            }
        }
        
        public string CommandArgument
        {
            get
            {
                return _commandArgument;
            }
            set
            {
                _commandArgument = value;
            }
        }
        
        public string LastCommandName
        {
            get
            {
                return _lastCommandName;
            }
            set
            {
                _lastCommandName = value;
            }
        }
        
        public string ContextKey
        {
            get
            {
                return _contextKey;
            }
            set
            {
                _contextKey = value;
            }
        }
        
        public string Cookie
        {
            get
            {
                return _cookie;
            }
            set
            {
                _cookie = value;
            }
        }
        
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
        
        public FieldValue[] Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
            }
        }
        
        public string[] Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
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
        
        public CommandConfigurationType SqlCommandType
        {
            get
            {
                CommandConfigurationType commandType = CommandConfigurationType.None;
                if (CommandName.Equals("update", StringComparison.OrdinalIgnoreCase))
                	commandType = CommandConfigurationType.Update;
                else
                	if (CommandName.Equals("insert", StringComparison.OrdinalIgnoreCase))
                    	commandType = CommandConfigurationType.Insert;
                    else
                    	if (CommandName.Equals("delete", StringComparison.OrdinalIgnoreCase))
                        	commandType = CommandConfigurationType.Delete;
                return commandType;
            }
        }
        
        public string[] SelectedValues
        {
            get
            {
                return _selectedValues;
            }
            set
            {
                _selectedValues = value;
            }
        }
        
        public FieldValue[] ExternalFilter
        {
            get
            {
                return _externalFilter;
            }
            set
            {
                _externalFilter = value;
            }
        }
        
        public bool SaveLEVs
        {
            get
            {
                return _saveLEVs;
            }
            set
            {
                _saveLEVs = value;
            }
        }
        
        public string Transaction
        {
            get
            {
                return _transaction;
            }
            set
            {
                _transaction = value;
            }
        }
        
        public static ActionArgs Current
        {
            get
            {
                return ((ActionArgs)(HttpContext.Current.Items["ActionArgs_Current"]));
            }
        }
        
        public FieldValue this[string name]
        {
            get
            {
                foreach (FieldValue v in Values)
                	if (v.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    	return v;
                return null;
            }
        }
        
        public bool RequiresPivot
        {
            get
            {
                if (Values != null)
                	foreach (FieldValue v in Values)
                    	if (v.Name.StartsWith("PivotCol"))
                        	return true;
                return false;
            }
        }
        
        public FieldValue[][] PivotValues
        {
            get
            {
                if (Values == null)
                	return null;
                List<FieldValue[]> valueSets = new List<FieldValue[]>();
                int valueSetIndex = 0;
                while (true)
                {
                    List<string> valueNames = new List<string>();
                    List<FieldValue> currentSet = new List<FieldValue>();
                    foreach (FieldValue v in Values)
                    {
                        Match m = Regex.Match(v.Name, "^PivotCol(\\d+)_(.+)$");
                        if (m.Success && (m.Groups[1].Value == valueSetIndex.ToString()))
                        {
                            v.Name = m.Groups[2].Value;
                            valueNames.Add(v.Name);
                            currentSet.Add(v);
                        }
                    }
                    if (currentSet.Count == 0)
                    	break;
                    foreach (FieldValue v in Values)
                    	if (!(v.Name.StartsWith("PivotCol")) && !(valueNames.Contains(v.Name)))
                        	currentSet.Add(v);
                    valueSetIndex++;
                    valueSets.Add(currentSet.ToArray());
                }
                return valueSets.ToArray();
            }
        }
        
        public FieldValue SelectFieldValueObject(String name)
        {
            if (Values != null)
            	foreach (FieldValue v in Values)
                	if (v.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    	return v;
            return null;
        }
        
        public T ToObject<T>()
        
        {
            Type objectType = typeof(T);
            T theObject = ((T)(objectType.Assembly.CreateInstance(objectType.FullName)));
            foreach (FieldValue v in Values)
            	v.AssignTo(theObject);
            return theObject;
        }
    }
}
