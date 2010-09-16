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
	public class ActionGroup
    {
        
        private List<Action> _actions;
        
        private string _scope;
        
        private string _headerText;
        
        private bool _flat;
        
        public ActionGroup()
        {
            this._actions = new List<Action>();
        }
        
        public ActionGroup(XPathNavigator actionGroup, IXmlNamespaceResolver resolver) : 
                this()
        {
            this._scope = ((string)(actionGroup.Evaluate("string(@scope)")));
            this._headerText = ((string)(actionGroup.Evaluate("string(@headerText)")));
            _flat = (actionGroup.GetAttribute("flat", String.Empty) == "true");
            XPathNodeIterator actionIterator = actionGroup.Select("c:action", resolver);
            while (actionIterator.MoveNext())
            	if (Controller.UserIsInRole(((string)(actionIterator.Current.Evaluate("string(@roles)")))))
                	this.Actions.Add(new Action(actionIterator.Current, resolver));
        }
        
        public List<Action> Actions
        {
            get
            {
                return _actions;
            }
        }
        
        public string Scope
        {
            get
            {
                return _scope;
            }
            set
            {
                _scope = value;
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
            }
        }
        
        public bool Flat
        {
            get
            {
                return _flat;
            }
            set
            {
                _flat = value;
            }
        }
    }
}
