using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.XPath;

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
        
        private bool _conflictDetectionEnabled;
        
        private DynamicExpression[] _expressions;
        
        private IPlugIn _plugIn;
        
        public ControllerConfiguration(string path) : 
                this(new XPathDocument(path))
        {
        }
        
        public ControllerConfiguration(XPathDocument document) : 
                this(document.CreateNavigator())
        {
        }
        
        public ControllerConfiguration(XPathNavigator navigator)
        {
            _navigator = navigator;
            _namespaceManager = new XmlNamespaceManager(_navigator.NameTable);
            _namespaceManager.AddNamespace("c", ControllerConfiguration.Namespace);
            _resolver = ((IXmlNamespaceResolver)(_namespaceManager));
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
        
        public string ConnectionStringName
        {
            get
            {
                return _connectionStringName;
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
    }
}
