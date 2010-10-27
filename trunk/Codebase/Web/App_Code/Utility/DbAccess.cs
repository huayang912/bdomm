using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;


//namespace App.Utility
//{
    public class DbAccess
    {
        private const String CONNECTION_STRING_NAME = "BUDI2_NS";
        private String _ConnectionString = String.Empty;
        ///// <summary>
        ///// Saves Connection String to the Application Configuration File
        ///// </summary>
        //private void SaveConnectionString()
        //{           
        //    //System.Configuration.ConfigurationSettings config = ConfigurationManager.

        //    System.Configuration.Configuration config =
        //            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        //    ConnectionStringsSection connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
        //    connectionStringsSection.ConnectionStrings[CONNECTION_STRING_NAME].ConnectionString = ConnectionInfo.ConnectionString;
        //    ///Save the Changes to the Connection String
        //    config.Save();
        //    ///Refresh the Configuration Section for Databae Connection Strings
        //    ConfigurationManager.RefreshSection("connectionStrings");           
        //} 
        internal Database Database
        {
            get;
            private set;
        }
        public DbAccess()
        {
            _ConnectionString = ConfigurationManager.ConnectionStrings[CONNECTION_STRING_NAME].ConnectionString;
            //SaveConnectionString();
            Database = DatabaseFactory.CreateDatabase(CONNECTION_STRING_NAME);
        }
        /// <summary>
        /// Gets DataSet Executing a SQL Statement
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public DataSet GetData(String SQL)
        {
            using (DbCommand command = Database.GetSqlStringCommand(SQL))
            {
                return Database.ExecuteDataSet(command);
            }
        }
        /// <summary>
        /// Creates A Database Object like Table, Stored Procedure, Function or Trigger
        /// </summary>
        /// <param name="commandText"></param>
        public void ExecuteCommand(String commandText)
        {            
            using (DbCommand command = Database.GetSqlStringCommand(commandText))
            {
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 90;
                Database.ExecuteScalar(command);                
            }
        }
        ///// <summary>
        ///// Executes Scripts With GO Statement within it
        ///// </summary>
        ///// <param name="commandText"></param>
        //public void ExecuteScript(String commandText)
        //{            
        //    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(_ConnectionString);
        //    Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(new Microsoft.SqlServer.Management.Common.ServerConnection(conn));
        //    server.ConnectionContext.ExecuteNonQuery(commandText);
        //}
        ///// <summary>
        ///// Checks Whether an Object Exists in the Databae or Not
        ///// </summary>
        ///// <param name="objectName"></param>
        ///// <param name="objectType"></param>
        ///// <returns></returns>
        //public bool IsExists(String objectName, DBObjectType objectType)
        //{
        //    String SQL = PrepareFindSQL(objectName, objectType);
        //    using (DbCommand command = Database.GetSqlStringCommand(SQL))
        //    {
        //        command.CommandType = CommandType.Text;
        //        command.CommandTimeout = 90;
        //        IDataReader reader = Database.ExecuteReader(command);
        //        if (reader.Read())
        //        {
        //            reader.Close();
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        ///// <summary>
        ///// Deletes an Existing Database Object
        ///// </summary>
        ///// <param name="dbObject"></param>
        //internal void DropObject(DBObject dbObject)
        //{
        //    String SQL = PrepareDropSQL(dbObject);
        //    using (DbCommand command = Database.GetSqlStringCommand(SQL))
        //    {
        //        command.CommandType = CommandType.Text;
        //        command.CommandTimeout = 90;
        //        Database.ExecuteScalar(command);
        //    }
        //}
        ///// <summary>
        ///// Prepares Drop SQL for Table, Stored Procedure and Function
        ///// </summary>
        ///// <param name="dbObject"></param>
        ///// <returns></returns>
        //private String PrepareDropSQL(DBObject dbObject)
        //{
        //    if (dbObject.Type == DBObjectType.Table)
        //        return String.Format("DROP TABLE {0}", dbObject.Name);
        //    else if (dbObject.Type == DBObjectType.StoredProcedure)
        //        return String.Format("DROP PROCEDURE {0}", dbObject.Name);
        //    else if (dbObject.Type == DBObjectType.Function)
        //        return String.Format("DROP FUNCTION {0}", dbObject.Name);

        //    return String.Empty;
        //}
//        /// <summary>
//        /// Prepares a StoredProcedure or Function Finding SQL 
//        /// </summary>
//        /// <param name="objectName"></param>
//        /// <param name="objectType"></param>
//        /// <returns></returns>
//        private String PrepareFindSQL(String objectName, DBObjectType objectType)
//        {
//            if (objectType == DBObjectType.Table)
//                return String.Format(@"SELECT * FROM sys.objects 
//                    WHERE object_id = OBJECT_ID(N'{0}') 
//                        AND type in (N'U')", objectName);
//            else if (objectType == DBObjectType.StoredProcedure)
//                return String.Format(@"SELECT * FROM sys.objects 
//                    WHERE object_id = OBJECT_ID(N'{0}') 
//                        AND type in (N'P', N'PC')", objectName);
//            else
//                return String.Format(@"SELECT * FROM sys.objects 
//                    WHERE object_id = OBJECT_ID(N'{0}') 
//                        AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')", objectName);            
            
//        }        
    }
//}
