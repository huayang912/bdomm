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
	public class ActionResult
    {
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private List<string> _errors;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private List<FieldValue> _values;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _canceled;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _navigateUrl;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _clientScript;
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _rowsAffected;
        
        private bool _rowNotFound;
        
        public ActionResult()
        {
            this._errors = new List<string>();
            this._values = new List<FieldValue>();
        }
        
        public List<string> Errors
        {
            get
            {
                return _errors;
            }
        }
        
        public List<FieldValue> Values
        {
            get
            {
                return _values;
            }
        }
        
        public bool Canceled
        {
            get
            {
                return _canceled;
            }
            set
            {
                _canceled = value;
            }
        }
        
        public string NavigateUrl
        {
            get
            {
                return _navigateUrl;
            }
            set
            {
                _navigateUrl = value;
            }
        }
        
        public string ClientScript
        {
            get
            {
                return _clientScript;
            }
            set
            {
                _clientScript = value;
            }
        }
        
        public int RowsAffected
        {
            get
            {
                return _rowsAffected;
            }
            set
            {
                _rowsAffected = value;
            }
        }
        
        public bool RowNotFound
        {
            get
            {
                return _rowNotFound;
            }
            set
            {
                _rowNotFound = value;
            }
        }
        
        public void RaiseExceptionIfErrors()
        {
            if (Errors.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string error in Errors)
                {
                    sb.AppendLine(error);
                    throw new Exception(sb.ToString());
                }
            }
        }
        
        public T ToObject<T>()
        
        {
            Type objectType = typeof(T);
            T theObject = ((T)(objectType.Assembly.CreateInstance(objectType.FullName)));
            AssignTo(theObject);
            return theObject;
        }
        
        public void AssignTo(object instance)
        {
            foreach (FieldValue v in Values)
            	v.AssignTo(instance);
        }
        
        public void ShowMessage(string message)
        {
            ExecuteOnClient(String.Format("Web.DataView.showMessage(\'{0}\');", Localizer.JavaScriptStringEncode(message)));
        }
        
        public void Focus(string fieldName, string message)
        {
            ExecuteOnClient(String.Format("this._focus(\'{0}\',\'{1}\');", fieldName, Localizer.JavaScriptStringEncode(message)));
        }
        
        public void ExecuteOnClient(string javaScript)
        {
            if (!(String.IsNullOrEmpty(ClientScript)) && !(ClientScript.EndsWith(";")))
            	ClientScript = (ClientScript + ";");
            if (!(String.IsNullOrEmpty(javaScript)))
            	ClientScript = (ClientScript + javaScript);
        }
        
        public void ShowLastView()
        {
            ExecuteOnClient("this.goToView(this._lastViewId);");
        }
        
        public void ShowView(string viewId)
        {
            ExecuteOnClient(String.Format("this.goToView(\'{0}\');", viewId));
        }
        
        public void ShowAlert(string message)
        {
            ExecuteOnClient(String.Format("alert(\'{0}\');", Localizer.JavaScriptStringEncode(message)));
        }
        
        public void ShowAlert(string message, string fieldName)
        {
            ExecuteOnClient(String.Format("this._focus(\'{0}\', \'{1}\');", fieldName, Localizer.JavaScriptStringEncode(message)));
        }
        
        public void HideModal()
        {
            ExecuteOnClient("this.endModalState(\'Cancel\');");
        }
        
        public void ShowModal(string controller, string view, string startCommandName, string startCommandArgument)
        {
            ExecuteOnClient(String.Format("Web.DataView.showModal(null, \'{0}\', \'{1}\', \'{2}\', \'{3}\');", controller, view, startCommandName, startCommandArgument));
        }
        
        public void Refresh()
        {
            ExecuteOnClient("this.refresh();");
        }
        
        public void Continue()
        {
            ExecuteOnClient("this._continueAfterScript=true;");
        }
        
        public void Merge(ActionResult result)
        {
            foreach (string error in result.Errors)
            	Errors.Add(error);
            ClientScript = (ClientScript + result.ClientScript);
            foreach (FieldValue v in result.Values)
            	Values.Add(v);
        }
    }
}
