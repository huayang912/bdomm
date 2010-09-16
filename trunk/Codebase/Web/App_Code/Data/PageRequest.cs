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
	public class PageRequest
    {
        
        private int _pageIndex;
        
        private int _pageSize;
        
        private string _sortExpression;
        
        private string[] _filter;
        
        private string _contextKey;
        
        private bool _filterIsExternal;
        
        private string _cookie;
        
        private bool _requiresMetaData;
        
        private bool _requiresRowCount;
        
        private string _controller;
        
        private string _view;
        
        private string _lookupContextController;
        
        private string _lookupContextView;
        
        private string _lookupContextFieldName;
        
        private bool _inserting;
        
        private string _lastCommandName;
        
        private string _lastCommandArgument;
        
        private FieldValue[] _externalFilter;
        
        public PageRequest()
        {
        }
        
        public PageRequest(int pageIndex, int pageSize, string sortExpression, string[] filter)
        {
            this._pageIndex = pageIndex;
            this._pageSize = pageSize;
            this._sortExpression = sortExpression;
            this._filter = filter;
        }
        
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                _pageIndex = value;
            }
        }
        
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
        
        public bool FilterIsExternal
        {
            get
            {
                return _filterIsExternal;
            }
            set
            {
                _filterIsExternal = value;
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
        
        public bool RequiresMetaData
        {
            get
            {
                return _requiresMetaData;
            }
            set
            {
                _requiresMetaData = value;
            }
        }
        
        public bool RequiresRowCount
        {
            get
            {
                return _requiresRowCount;
            }
            set
            {
                _requiresRowCount = value;
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
        
        public string LookupContextController
        {
            get
            {
                return _lookupContextController;
            }
            set
            {
                _lookupContextController = value;
            }
        }
        
        public string LookupContextView
        {
            get
            {
                return _lookupContextView;
            }
            set
            {
                _lookupContextView = value;
            }
        }
        
        public string LookupContextFieldName
        {
            get
            {
                return _lookupContextFieldName;
            }
            set
            {
                _lookupContextFieldName = value;
            }
        }
        
        public bool Inserting
        {
            get
            {
                return _inserting;
            }
            set
            {
                _inserting = value;
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
        
        public string LastCommandArgument
        {
            get
            {
                return _lastCommandArgument;
            }
            set
            {
                _lastCommandArgument = value;
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
        
        public bool IsModal
        {
            get
            {
                return (!(String.IsNullOrEmpty(_contextKey)) && _contextKey.Contains("_ModalDataView"));
            }
        }
        
        public void AssignContext(string controller, string view)
        {
            _controller = controller;
            _view = view;
            string referrer = String.Empty;
            if ((HttpContext.Current != null) && (HttpContext.Current.Request.UrlReferrer != null))
            	referrer = HttpContext.Current.Request.UrlReferrer.AbsolutePath;
            this._contextKey = string.Format("{0}/{1}.{2}.{3}", referrer, controller, view, ContextKey);
        }
    }
}
