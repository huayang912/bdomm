using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace BUDI2_NS.Data
{
	public class ControllerConfiguration
    {
        
        private XPathNavigator _navigator;
        
        private XmlNamespaceManager _namespaceManager;
        
        private IXmlNamespaceResolver _resolver;
        
        private string _actionHandlerType;
        
        private string _dataFilterType;
        
        private string _handlerType;
        
        public const string Namespace = "urn:schemas-codeontime-com:data-aquarium";
        
        private string _connectionStringName;
        
        private string _controllerName;
        
        private bool _conflictDetectionEnabled;
        
        private DynamicExpression[] _expressions;
        
        private IPlugIn _plugIn;
        
        private string _rawConfiguration;
        
        private bool _usesVariables;
        
        private bool _requiresLocalization;
        
        public ControllerConfiguration(string path) : 
                this(File.OpenRead(path))
        {
        }
        
        public ControllerConfiguration(Stream stream)
        {
            StreamReader sr = new StreamReader(stream);
            this._rawConfiguration = sr.ReadToEnd();
            sr.Close();
            this._usesVariables = Regex.IsMatch(this._rawConfiguration, "\\$\\w+\\$");
            this._requiresLocalization = Regex.IsMatch(this._rawConfiguration, "\\^\\w+\\^");
            Initialize(new XPathDocument(new StringReader(this._rawConfiguration)).CreateNavigator());
        }
        
        public ControllerConfiguration(XPathDocument document) : 
                this(document.CreateNavigator())
        {
        }
        
        public ControllerConfiguration(XPathNavigator navigator)
        {
            Initialize(navigator);
        }
        
        public string ConnectionStringName
        {
            get
            {
                return _connectionStringName;
            }
        }
        
        public string ControllerName
        {
            get
            {
                return _controllerName;
            }
        }
        
        public bool ConflictDetectionEnabled
        {
            get
            {
                return _conflictDetectionEnabled;
            }
        }
        
        public IXmlNamespaceResolver Resolver
        {
            get
            {
                return _resolver;
            }
        }
        
        public XPathNavigator Navigator
        {
            get
            {
                return _navigator;
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
        
        public IPlugIn PlugIn
        {
            get
            {
                return _plugIn;
            }
        }
        
        public string RawConfiguration
        {
            get
            {
                return _rawConfiguration;
            }
        }
        
        public bool UsesVariables
        {
            get
            {
                return _usesVariables;
            }
        }
        
        public bool RequiresLocalization
        {
            get
            {
                return _requiresLocalization;
            }
        }
        
        protected virtual void Initialize(XPathNavigator navigator)
        {
            _navigator = navigator;
            _namespaceManager = new XmlNamespaceManager(_navigator.NameTable);
            _namespaceManager.AddNamespace("c", ControllerConfiguration.Namespace);
            _resolver = ((IXmlNamespaceResolver)(_namespaceManager));
            ResolveBaseViews();
            _controllerName = ((string)(_navigator.Evaluate("string(/c:dataController/@name)", _resolver)));
            _connectionStringName = ((string)(_navigator.Evaluate("string(/c:dataController/@connectionStringName)", _resolver)));
            if (String.IsNullOrEmpty(_connectionStringName))
            	_connectionStringName = "BUDI2_NS";
            _conflictDetectionEnabled = ((bool)(_navigator.Evaluate("/c:dataController/@conflictDetection=\'compareAllValues\'", _resolver)));
            _handlerType = ((string)(_navigator.Evaluate("string(/c:dataController/@handler)", _resolver)));
            if (String.IsNullOrEmpty(_handlerType))
            {
                Type t = Type.GetType("BUDI2_NS.Rules.SharedBusinessRules");
                if (t != null)
                	_handlerType = t.FullName;
            }
            _actionHandlerType = _handlerType;
            _dataFilterType = _handlerType;
            string s = ((string)(_navigator.Evaluate("string(/c:dataController/@actionHandlerType)", _resolver)));
            if (!(String.IsNullOrEmpty(s)))
            	_actionHandlerType = s;
            s = ((string)(_navigator.Evaluate("string(/c:dataController/@dataFilterType)", _resolver)));
            if (!(String.IsNullOrEmpty(s)))
            	_dataFilterType = s;
            List<DynamicExpression> expressions = new List<DynamicExpression>();
            XPathNodeIterator expressionIterator = _navigator.Select("//c:expression", _resolver);
            while (expressionIterator.MoveNext())
            	expressions.Add(new DynamicExpression(expressionIterator.Current, _namespaceManager));
            _expressions = expressions.ToArray();
            string plugInType = ((string)(_navigator.Evaluate("string(/c:dataController/@plugIn)", _resolver)));
            if (!(String.IsNullOrEmpty(plugInType)))
            {
                Type t = Type.GetType(plugInType);
                _plugIn = ((IPlugIn)(t.Assembly.CreateInstance(t.FullName)));
                _plugIn.Config = this;
            }
        }
        
        protected virtual void ResolveBaseViews()
        {
            XPathNavigator firstUnresolvedView = _navigator.SelectSingleNode("/c:dataController/c:views/c:view[@baseViewId!=\'\' and not (.//c:dataField)]", _resolver);
            if (firstUnresolvedView != null)
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(_navigator.OuterXml);
                _navigator = document.CreateNavigator();
                XPathNodeIterator unresolvedViewIterator = _navigator.Select("/c:dataController/c:views/c:view[@baseViewId!=\'\']", _resolver);
                while (unresolvedViewIterator.MoveNext())
                {
                    string baseViewId = unresolvedViewIterator.Current.GetAttribute("baseViewId", String.Empty);
                    XPathNavigator baseView = _navigator.SelectSingleNode(String.Format("/c:dataController/c:views/c:view[@id=\'{0}\']", baseViewId), _resolver);
                    if (baseView != null)
                    {
                        List<XPathNavigator> nodesToDelete = new List<XPathNavigator>();
                        XPathNodeIterator emptyNodeIterator = unresolvedViewIterator.Current.Select("c:*[not(child::*) and .=\'\']", _resolver);
                        while (emptyNodeIterator.MoveNext())
                        	nodesToDelete.Add(emptyNodeIterator.Current.Clone());
                        foreach (XPathNavigator n in nodesToDelete)
                        	n.DeleteSelf();
                        XPathNodeIterator copyNodeIterator = baseView.Select("c:*", _resolver);
                        while (copyNodeIterator.MoveNext())
                        	if (unresolvedViewIterator.Current.SelectSingleNode(("c:" + copyNodeIterator.Current.LocalName), _resolver) == null)
                            	unresolvedViewIterator.Current.AppendChild(copyNodeIterator.Current.OuterXml);
                    }
                }
            }
        }
        
        private void InitializeHandler(object handler)
        {
            if ((handler != null) && (handler is BusinessRules))
            	((BusinessRules)(handler)).ControllerName = ControllerName;
        }
        
        public BusinessRules CreateBusinessRules()
        {
            IActionHandler handler = CreateActionHandler();
            if (handler == null)
            	return null;
            else
            	return ((BusinessRules)(handler));
        }
        
        public IActionHandler CreateActionHandler()
        {
            if (String.IsNullOrEmpty(_actionHandlerType))
            	return null;
            else
            {
                Type t = Type.GetType(_actionHandlerType);
                object handler = t.Assembly.CreateInstance(t.FullName);
                InitializeHandler(handler);
                return ((IActionHandler)(handler));
            }
        }
        
        public IDataFilter CreateDataFilter()
        {
            if (String.IsNullOrEmpty(_dataFilterType))
            	return null;
            else
            {
                Type t = Type.GetType(_dataFilterType);
                object dataFilter = t.Assembly.CreateInstance(t.FullName);
                InitializeHandler(dataFilter);
                if (typeof(IDataFilter).IsInstanceOfType(dataFilter))
                	return ((IDataFilter)(dataFilter));
                else
                	return null;
            }
        }
        
        public IRowHandler CreateRowHandler()
        {
            if (String.IsNullOrEmpty(_actionHandlerType))
            	return null;
            else
            {
                Type t = Type.GetType(_actionHandlerType);
                object handler = t.Assembly.CreateInstance(t.FullName);
                InitializeHandler(handler);
                if (typeof(IRowHandler).IsInstanceOfType(handler))
                	return ((IRowHandler)(handler));
                else
                	return null;
            }
        }
        
        public void AssignDynamicExpressions(ViewPage page)
        {
            List<DynamicExpression> list = new List<DynamicExpression>();
            foreach (DynamicExpression de in _expressions)
            	if (de.AllowedInView(page.View))
                	list.Add(de);
            page.Expressions = list.ToArray();
        }
        
        public ControllerConfiguration Clone()
        {
            string variablesPath = Path.Combine(HttpRuntime.AppDomainAppPath, "Controllers\\_variables.xml");
            SortedDictionary<string, string> variables = ((SortedDictionary<string, string>)(HttpRuntime.Cache[variablesPath]));
            if (variables == null)
            {
                variables = new SortedDictionary<string, string>();
                if (File.Exists(variablesPath))
                {
                    XPathDocument varDoc = new XPathDocument(variablesPath);
                    XPathNavigator varNav = varDoc.CreateNavigator();
                    XPathNodeIterator varIterator = varNav.Select("/variables/variable");
                    while (varIterator.MoveNext())
                    {
                        string varName = varIterator.Current.GetAttribute("name", String.Empty);
                        string varValue = varIterator.Current.Value;
                        if (!(variables.ContainsKey(varName)))
                        	variables.Add(varName, varValue);
                        else
                        	variables[varName] = varValue;
                    }
                }
                HttpRuntime.Cache.Insert(variablesPath, variables, new CacheDependency(variablesPath));
            }
            return new ControllerConfiguration(new XPathDocument(new StringReader(new ControllerConfigurationUtility(_rawConfiguration, variables).ReplaceVariables())));
        }
        
        public ControllerConfiguration Localize(string controller)
        {
            string localizedContent = Localizer.Replace("Controllers", (controller + ".xml"), _navigator.OuterXml);
            if (PlugIn != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(localizedContent);
                return new ControllerConfiguration(doc.CreateNavigator());
            }
            else
            	return new ControllerConfiguration(new XPathDocument(new StringReader(localizedContent)));
        }
    }
    
    public class ControllerConfigurationUtility
    {
        
        private string _rawConfiguration;
        
        private SortedDictionary<string, string> _variables;
        
        public ControllerConfigurationUtility(string rawConfiguration, SortedDictionary<string, string> variables)
        {
            _rawConfiguration = rawConfiguration;
            _variables = variables;
        }
        
        public string ReplaceVariables()
        {
            return Regex.Replace(_rawConfiguration, "\\$(\\w+)\\$([\\s\\S]*?)\\$(\\w+)\\$", DoReplace);
        }
        
        private string DoReplace(Match m)
        {
            if (m.Groups[1].Value == m.Groups[3].Value)
            {
                string s = null;
                if (_variables.TryGetValue(m.Groups[1].Value, out s))
                	return s;
                else
                	return m.Groups[2].Value;
            }
            return m.Value;
        }
    }
}
