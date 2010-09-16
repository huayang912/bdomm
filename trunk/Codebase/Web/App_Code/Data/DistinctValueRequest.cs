
using System;

namespace BUDI2_NS.Data
{
	public class DistinctValueRequest
    {
        
        private string _fieldName;
        
        private string[] _filter;
        
        private string _lookupContextController;
        
        private string _lookupContextView;
        
        private string _lookupContextFieldName;
        
        private int _maximumValueCount;
        
        private bool _allowFieldInFilter;
        
        public DistinctValueRequest()
        {
        }
        
        public string FieldName
        {
            get
            {
                return _fieldName;
            }
            set
            {
                _fieldName = value;
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
        
        public int MaximumValueCount
        {
            get
            {
                if (_maximumValueCount <= 0)
                	return Controller.MaximumDistinctValues;
                return _maximumValueCount;
            }
            set
            {
                _maximumValueCount = value;
            }
        }
        
        public bool AllowFieldInFilter
        {
            get
            {
                return _allowFieldInFilter;
            }
            set
            {
                _allowFieldInFilter = value;
            }
        }
    }
}
