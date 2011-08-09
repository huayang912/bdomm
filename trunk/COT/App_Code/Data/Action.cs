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
	public class Action
    {
        
        private string _commandName;
        
        private string _commandArgument;
        
        private string _headerText;
        
        private string _description;
        
        private string _cssClass;
        
        private string _confirmation;
        
        private string _whenLastCommandName;
        
        private string _whenLastCommandArgument;
        
        private bool _whenKeySelected;
        
        private string _whenClientScript;
        
        private bool _causesValidation;
        
        private string _whenTag;
        
        private string _whenHRef;
        
        private string _whenView;
        
        public Action()
        {
        }
        
        public Action(XPathNavigator action, IXmlNamespaceResolver resolver)
        {
            this._commandName = ((string)(action.Evaluate("string(@commandName)")));
            this._commandArgument = ((string)(action.Evaluate("string(@commandArgument)")));
            this._headerText = ((string)(action.Evaluate("string(@headerText)")));
            this._description = ((string)(action.Evaluate("string(@description)")));
            this._cssClass = ((string)(action.Evaluate("string(@cssClass)")));
            this._confirmation = ((string)(action.Evaluate("string(@confirmation)")));
            this._whenLastCommandName = ((string)(action.Evaluate("string(@whenLastCommandName)")));
            this._whenLastCommandArgument = action.GetAttribute("whenLastCommandArgument", String.Empty);
            this._causesValidation = !((action.GetAttribute("causesValidation", String.Empty) == "false"));
            this._whenKeySelected = (action.GetAttribute("whenKeySelected", String.Empty) == "true");
            this._whenTag = action.GetAttribute("whenTag", String.Empty);
            this._whenHRef = action.GetAttribute("whenHRef", String.Empty);
            this._whenView = action.GetAttribute("whenView", String.Empty);
            this._whenClientScript = action.GetAttribute("whenClientScript", String.Empty);
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
        
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        
        public string CssClass
        {
            get
            {
                return _cssClass;
            }
            set
            {
                _cssClass = value;
            }
        }
        
        public string Confirmation
        {
            get
            {
                return _confirmation;
            }
        }
        
        public string WhenLastCommandName
        {
            get
            {
                return _whenLastCommandName;
            }
            set
            {
                _whenLastCommandName = value;
            }
        }
        
        public string WhenLastCommandArgument
        {
            get
            {
                return _whenLastCommandArgument;
            }
            set
            {
                _whenLastCommandArgument = value;
            }
        }
        
        public bool WhenKeySelected
        {
            get
            {
                return _whenKeySelected;
            }
            set
            {
                _whenKeySelected = value;
            }
        }
        
        public string WhenClientScript
        {
            get
            {
                return _whenClientScript;
            }
            set
            {
                _whenClientScript = value;
            }
        }
        
        public bool CausesValidation
        {
            get
            {
                return _causesValidation;
            }
            set
            {
                _causesValidation = value;
            }
        }
        
        public string WhenTag
        {
            get
            {
                return _whenTag;
            }
            set
            {
                _whenTag = value;
            }
        }
        
        public string WhenHRef
        {
            get
            {
                return _whenHRef;
            }
            set
            {
                _whenHRef = value;
            }
        }
        
        public string WhenView
        {
            get
            {
                return _whenView;
            }
            set
            {
                _whenView = value;
            }
        }
    }
}
