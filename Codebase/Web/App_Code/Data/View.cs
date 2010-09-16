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
	public class View
    {
        
        private string _id;
        
        private string _label;
        
        private string _headerText;
        
        private string _type;
        
        public View()
        {
        }
        
        public View(XPathNavigator view, IXmlNamespaceResolver resolver)
        {
            this._id = ((string)(view.Evaluate("string(@id)")));
            this._type = ((string)(view.Evaluate("string(@type)")));
            this._label = ((string)(view.Evaluate("string(@label)")));
            this._headerText = ((string)(view.Evaluate("string(c:headerText)", resolver)));
        }
        
        public string Id
        {
            get
            {
                return _id;
            }
        }
        
        public string Label
        {
            get
            {
                return _label;
            }
        }
        
        public string HeaderText
        {
            get
            {
                return _headerText;
            }
        }
        
        public string Type
        {
            get
            {
                return _type;
            }
        }
    }
}
