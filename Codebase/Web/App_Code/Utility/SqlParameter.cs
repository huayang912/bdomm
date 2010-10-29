using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using App.Core;
using App.Core.Extensions;
using System.Xml;


[Serializable]
public class SqlParameter
{
    private object _value;

    public String Name { get; private set; }
    public DbType DataType { get; private set; }
    public object Value
    {
        get { return GetNullSafeValue(_value); }
        private set { _value = value; }
    }

    public bool IsOutParameter { get; private set; }

    public int OutParameterSize { get; private set; }

    protected SqlParameter(String name, object value)
    {
        Name = name;
        _value = value;
    }
    public SqlParameter(String name, DbType type, object value)
        : this(name, value)
    {
        DataType = type;
    }
    public SqlParameter(String name, object value, bool isOutParameter, int outParameterSize)
    {
        Name = name;
        Value = value;
        IsOutParameter = isOutParameter;
        OutParameterSize = outParameterSize;
    }

    public SqlParameter(String name, DbType type, object value, bool isOutParameter, int outParameterSize)
        : this(name, value, isOutParameter, outParameterSize)
    {
        DataType = type;
    }

    #region Parameter Value Helper Methods
    private static object GetNullSafeValue(object value)
    {
        if (value == null)
            return DBNull.Value;
        else
            return value;
    }
    #endregion
}



