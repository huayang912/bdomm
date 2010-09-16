using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Web.Configuration;

namespace BUDI2_NS.Data
{
	public partial class SqlStatement : IDisposable
    {
        
        private bool _disposed;
        
        private object _scalar;
        
        private DbConnection _connection;
        
        private DbCommand _command;
        
        private DbDataReader _reader;
        
        private string _parameterMarker;
        
        public SqlStatement(CommandType commandType, string commandText, string connectionStringName)
        {
            ConnectionStringSettings settings = CreateConnectionStringSettings(connectionStringName);
            DbProviderFactory factory = DbProviderFactories.GetFactory(settings.ProviderName);
            _connection = factory.CreateConnection();
            _connection.ConnectionString = settings.ConnectionString;
            _connection.Open();
            _command = _connection.CreateCommand();
            _command.CommandType = commandType;
            _command.CommandText = commandText;
            _parameterMarker = GetParameterMarker(settings);
        }
        
        public DbDataReader Reader
        {
            get
            {
                return _reader;
            }
        }
        
        public DbCommand Command
        {
            get
            {
                return _command;
            }
        }
        
        public object Scalar
        {
            get
            {
                return _scalar;
            }
        }
        
        public DbParameterCollection Parameters
        {
            get
            {
                return _command.Parameters;
            }
        }
        
        public object this[string name]
        {
            get
            {
                return _reader[name];
            }
        }
        
        public string ParameterMarker
        {
            get
            {
                return _parameterMarker;
            }
        }
        
        public object this[int index]
        {
            get
            {
                return _reader[index];
            }
        }
        
        public static ConnectionStringSettings CreateConnectionStringSettings(string connectionStringName)
        {
            if (String.IsNullOrEmpty(connectionStringName))
            	connectionStringName = "BUDI2_NS";
            return WebConfigurationManager.ConnectionStrings[connectionStringName];
        }
        
        public static string GetParameterMarker(string connectionStringName)
        {
            ConnectionStringSettings settings = CreateConnectionStringSettings(connectionStringName);
            return GetParameterMarker(settings);
        }
        
        public static string GetParameterMarker(ConnectionStringSettings settings)
        {
            if (settings.ProviderName.Contains("Oracle"))
            	return ":";
            else
            	return "@";
        }
        
        public void Close()
        {
            if ((_reader != null) && !(_reader.IsClosed))
            	_reader.Close();
            if ((_command != null) && (_command.Connection.State == ConnectionState.Open))
            	_command.Connection.Close();
        }
        
        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        
        public void Dispose(bool disposing)
        {
            Close();
            if (!(_disposed))
            {
                if (_reader != null)
                	_reader.Dispose();
                if (_command != null)
                	_command.Dispose();
                if (_connection != null)
                	_connection.Dispose();
                _disposed = true;
            }
            if (disposing)
            	GC.SuppressFinalize(this);
        }
        
        public DbDataReader ExecuteReader()
        {
            _reader = _command.ExecuteReader();
            return _reader;
        }
        
        public object ExecuteScalar()
        {
            _scalar = _command.ExecuteScalar();
            return _scalar;
        }
        
        public int ExecuteNonQuery()
        {
            return _command.ExecuteNonQuery();
        }
        
        public bool Read()
        {
            if (_reader == null)
            	ExecuteReader();
            return _reader.Read();
        }
        
        private DbParameter AddParameterWithoutValue(string parameterName)
        {
            DbParameter p = _command.CreateParameter();
            p.ParameterName = parameterName;
            p.Value = DBNull.Value;
            _command.Parameters.Add(p);
            return p;
        }
        
        private DbParameter AddParameterWithValue(string parameterName, object value)
        {
            DbParameter p = _command.CreateParameter();
            p.ParameterName = parameterName;
            p.Value = value;
            _command.Parameters.Add(p);
            return p;
        }
        
        public DbParameter AddParameter(string parameterName, sbyte value)
        {
            return AddParameterWithValue(parameterName, value);
        }
        
        public DbParameter AddParameter(string parameterName, Nullable<sbyte> value)
        {
            if (value.HasValue)
            	return AddParameterWithValue(parameterName, value);
            else
            	return AddParameterWithoutValue(parameterName);
        }
        
        public DbParameter AddParameter(string parameterName, byte value)
        {
            return AddParameterWithValue(parameterName, value);
        }
        
        public DbParameter AddParameter(string parameterName, Nullable<byte> value)
        {
            if (value.HasValue)
            	return AddParameterWithValue(parameterName, value);
            else
            	return AddParameterWithoutValue(parameterName);
        }
        
        public DbParameter AddParameter(string parameterName, short value)
        {
            return AddParameterWithValue(parameterName, value);
        }
        
        public DbParameter AddParameter(string parameterName, Nullable<short> value)
        {
            if (value.HasValue)
            	return AddParameterWithValue(parameterName, value);
            else
            	return AddParameterWithoutValue(parameterName);
        }
        
        public DbParameter AddParameter(string parameterName, ushort value)
        {
            return AddParameterWithValue(parameterName, value);
        }
        
        public DbParameter AddParameter(string parameterName, Nullable<ushort> value)
        {
            if (value.HasValue)
            	return AddParameterWithValue(parameterName, value);
            else
            	return AddParameterWithoutValue(parameterName);
        }
        
        public DbParameter AddParameter(string parameterName, int value)
        {
            return AddParameterWithValue(parameterName, value);
        }
        
        public DbParameter AddParameter(string parameterName, Nullable<int> value)
        {
            if (value.HasValue)
            	return AddParameterWithValue(parameterName, value);
            else
            	return AddParameterWithoutValue(parameterName);
        }
        
        public DbParameter AddParameter(string parameterName, uint value)
        {
            return AddParameterWithValue(parameterName, value);
        }
        
        public DbParameter AddParameter(string parameterName, Nullable<uint> value)
        {
            if (value.HasValue)
            	return AddParameterWithValue(parameterName, value);
            else
            	return AddParameterWithoutValue(parameterName);
        }
        
        public DbParameter AddParameter(string parameterName, long value)
        {
            return AddParameterWithValue(parameterName, value);
        }
        
        public DbParameter AddParameter(string parameterName, Nullable<long> value)
        {
            if (value.HasValue)
            	return AddParameterWithValue(parameterName, value);
            else
            	return AddParameterWithoutValue(parameterName);
        }
        
        public DbParameter AddParameter(string parameterName, ulong value)
        {
            return AddParameterWithValue(parameterName, value);
        }
        
        public DbParameter AddParameter(string parameterName, Nullable<ulong> value)
        {
            if (value.HasValue)
            	return AddParameterWithValue(parameterName, value);
            else
            	return AddParameterWithoutValue(parameterName);
        }
        
        public DbParameter AddParameter(string parameterName, float value)
        {
            return AddParameterWithValue(parameterName, value);
        }
        
        public DbParameter AddParameter(string parameterName, Nullable<float> value)
        {
            if (value.HasValue)
            	return AddParameterWithValue(parameterName, value);
            else
            	return AddParameterWithoutValue(parameterName);
        }
        
        public DbParameter AddParameter(string parameterName, decimal value)
        {
            return AddParameterWithValue(parameterName, value);
        }
        
        public DbParameter AddParameter(string parameterName, Nullable<decimal> value)
        {
            if (value.HasValue)
            	return AddParameterWithValue(parameterName, value);
            else
            	return AddParameterWithoutValue(parameterName);
        }
        
        public DbParameter AddParameter(string parameterName, double value)
        {
            return AddParameterWithValue(parameterName, value);
        }
        
        public DbParameter AddParameter(string parameterName, Nullable<double> value)
        {
            if (value.HasValue)
            	return AddParameterWithValue(parameterName, value);
            else
            	return AddParameterWithoutValue(parameterName);
        }
        
        public DbParameter AddParameter(string parameterName, char value)
        {
            return AddParameterWithValue(parameterName, value);
        }
        
        public DbParameter AddParameter(string parameterName, Nullable<char> value)
        {
            if (value.HasValue)
            	return AddParameterWithValue(parameterName, value);
            else
            	return AddParameterWithoutValue(parameterName);
        }
        
        public DbParameter AddParameter(string parameterName, bool value)
        {
            return AddParameterWithValue(parameterName, value);
        }
        
        public DbParameter AddParameter(string parameterName, Nullable<bool> value)
        {
            if (value.HasValue)
            	return AddParameterWithValue(parameterName, value);
            else
            	return AddParameterWithoutValue(parameterName);
        }
        
        public DbParameter AddParameter(string parameterName, System.DateTime value)
        {
            return AddParameterWithValue(parameterName, value);
        }
        
        public DbParameter AddParameter(string parameterName, Nullable<System.DateTime> value)
        {
            if (value.HasValue)
            	return AddParameterWithValue(parameterName, value);
            else
            	return AddParameterWithoutValue(parameterName);
        }
        
        public DbParameter AddParameter(string parameterName, object value)
        {
            if ((value == null) || DBNull.Value.Equals(value))
            	return AddParameterWithoutValue(parameterName);
            else
            	return AddParameterWithValue(parameterName, value);
        }
    }
    
    public class SqlProcedure : SqlStatement
    {
        
        public SqlProcedure(string procedureName) : 
                this(procedureName, null)
        {
        }
        
        public SqlProcedure(string procedureName, string connectionStringName) : 
                base(CommandType.StoredProcedure, procedureName, connectionStringName)
        {
        }
    }
    
    public class SqlText : SqlStatement
    {
        
        public SqlText(string text) : 
                this(text, null)
        {
        }
        
        public SqlText(string text, string connectionStringName) : 
                base(CommandType.Text, text, connectionStringName)
        {
        }
    }
}
