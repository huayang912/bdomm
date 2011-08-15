using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using App.Core;
using App.Core.Extensions;
using System.Xml;

namespace App.Data
{
    [Serializable]
    public class DbParameter
    {
        public DbParameter(String name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
        public string Name { get; private set; }
        public object Value { get; set; }
    }
}