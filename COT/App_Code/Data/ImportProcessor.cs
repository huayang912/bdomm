using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Security;

namespace BUDI2_NS.Data
{
	public class ImportMapDictionary : SortedDictionary<int, DataField>
    {
    }
    
    public class ImportLookupDictionary : SortedDictionary<string, DataField>
    {
    }
    
    public partial class ImportProcessor : ImportProcessorBase
    {
    }
    
    public class ImportProcessorBase
    {
        
        public ImportProcessorBase()
        {
        }
        
        public static string SharedTempPath
        {
            get
            {
                string p = WebConfigurationManager.AppSettings["SharedTempPath"];
                if (String.IsNullOrEmpty(p))
                	p = Path.GetTempPath();
                if (!(Path.IsPathRooted(p)))
                	p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p);
                return p;
            }
        }
        
        public static void Execute(ActionArgs args)
        {
            Process(args);
        }
        
        private static void Process(object args)
        {
            ImportProcessor ip = new ImportProcessor();
            List<string> arguments = new List<string>(((ActionArgs)(args)).CommandArgument.Split(';'));
            string fileName = Path.Combine(ImportProcessor.SharedTempPath, arguments[0]);
            arguments.RemoveAt(0);
            string controller = arguments[0];
            arguments.RemoveAt(0);
            string view = arguments[0];
            arguments.RemoveAt(0);
            string notify = arguments[0];
            arguments.RemoveAt(0);
            try
            {
                ip.Process(fileName, controller, view, notify, arguments);
            }
            finally
            {
                if (File.Exists(fileName))
                	try
                    {
                        File.Delete(fileName);
                    }
                    catch (Exception )
                    {
                    }
            }
        }
        
        public OleDbDataReader OpenRead(string fileName, string selectClause)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            string tableName = null;
            OleDbConnectionStringBuilder connectionString = new OleDbConnectionStringBuilder();
            connectionString.Provider = "Microsoft.ACE.OLEDB.12.0";
            if (extension == ".csv")
            {
                connectionString["Extended Properties"] = "text;HDR=Yes;FMT=Delimited";
                connectionString.DataSource = Path.GetDirectoryName(fileName);
                tableName = Path.GetFileName(fileName);
            }
            else
            	if (extension == ".xls")
                {
                    connectionString["Extended Properties"] = "Excel 8.0;HDR=Yes;IMEX=1";
                    connectionString.DataSource = fileName;
                }
                else
                	if (extension == ".xlsx")
                    {
                        connectionString["Extended Properties"] = "Excel 12.0 Xml;HDR=YES";
                        connectionString.DataSource = fileName;
                    }
            OleDbConnection connection = new OleDbConnection(connectionString.ToString());
            connection.Open();
            if (String.IsNullOrEmpty(tableName))
            {
                DataTable tables = connection.GetSchema("Tables");
                tableName = Convert.ToString(tables.Rows[0]["TABLE_NAME"]);
            }
            try
            {
                OleDbCommand command = connection.CreateCommand();
                command.CommandText = String.Format("select {0} from [{1}]", selectClause, tableName);
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception )
            {
                connection.Close();
                throw;
            }
        }
        
        private void EnumerateFields(OleDbDataReader reader, ViewPage page, ImportMapDictionary map, ImportLookupDictionary lookups, List<string> userMapping)
        {
            List<String> mappedFields = new List<string>();
            for (int i = 0; (i < reader.FieldCount); i++)
            {
                string fieldName = reader.GetName(i);
                DataField field = null;
                bool autoDetect = true;
                if (userMapping != null)
                {
                    string mappedFieldName = userMapping[i];
                    autoDetect = String.IsNullOrEmpty(mappedFieldName);
                    if (!(autoDetect))
                    	fieldName = mappedFieldName;
                }
                if (autoDetect)
                	foreach (DataField f in page.Fields)
                    	if (fieldName.Equals(f.HeaderText, StringComparison.CurrentCultureIgnoreCase) || fieldName.Equals(f.Label, StringComparison.CurrentCultureIgnoreCase))
                        {
                            field = f;
                            break;
                        }
                if (field == null)
                	field = page.FindField(fieldName);
                if (field != null)
                {
                    if (!(String.IsNullOrEmpty(field.AliasName)))
                    	field = page.FindField(field.AliasName);
                    if (!(field.ReadOnly))
                    {
                        if (!(mappedFields.Contains(field.Name)))
                        {
                            map.Add(i, field);
                            mappedFields.Add(field.Name);
                        }
                    }
                    else
                    	foreach (DataField f in page.Fields)
                        	if (f.AliasName == field.Name)
                            {
                                map.Add(i, field);
                                lookups.Add(field.Name, f);
                                break;
                            }
                }
            }
        }
        
        private void ResolveLookups(ImportLookupDictionary lookups)
        {
            foreach (string fieldName in lookups.Keys)
            {
                DataField lookupField = lookups[fieldName];
                if ((lookupField.Items.Count == 0) && (String.IsNullOrEmpty(lookupField.ItemsDataValueField) || String.IsNullOrEmpty(lookupField.ItemsDataTextField)))
                {
                    PageRequest lookupRequest = new PageRequest();
                    lookupRequest.Controller = lookupField.ItemsDataController;
                    lookupRequest.View = lookupField.ItemsDataView;
                    lookupRequest.RequiresMetaData = true;
                    ViewPage lp = ControllerFactory.CreateDataController().GetPage(lookupRequest.Controller, lookupRequest.View, lookupRequest);
                    if (String.IsNullOrEmpty(lookupField.ItemsDataValueField))
                    	foreach (DataField f in lp.Fields)
                        	if (f.IsPrimaryKey)
                            {
                                lookupField.ItemsDataValueField = f.Name;
                                break;
                            }
                    if (String.IsNullOrEmpty(lookupField.ItemsDataTextField))
                    	foreach (DataField f in lp.Fields)
                        	if ((!(f.IsPrimaryKey) && !(f.Hidden)) && (!(f.AllowNulls) || (f.Type == "String")))
                            {
                                lookupField.ItemsDataTextField = f.Name;
                                break;
                            }
                }
            }
        }
        
        protected virtual void BeforeProcess(string fileName, string controller, string view, string notify, List<string> userMapping)
        {
        }
        
        protected virtual void AfterProcess(string fileName, string controller, string view, string notify, List<string> userMapping)
        {
        }
        
        public virtual void Process(string fileName, string controller, string view, string notify, List<string> userMapping)
        {
            BeforeProcess(fileName, controller, view, notify, userMapping);
            string logFileName = Path.GetTempFileName();
            StreamWriter log = File.CreateText(logFileName);
            log.WriteLine("{0:s} Import process started.", DateTime.Now);
            // retrieve metadata
            PageRequest request = new PageRequest();
            request.Controller = controller;
            request.View = view;
            request.RequiresMetaData = true;
            ViewPage page = ControllerFactory.CreateDataController().GetPage(controller, view, request);
            // open data reader and enumerate fields
            OleDbDataReader reader = OpenRead(fileName, "*");
            ImportMapDictionary map = new ImportMapDictionary();
            ImportLookupDictionary lookups = new ImportLookupDictionary();
            EnumerateFields(reader, page, map, lookups, userMapping);
            // resolve lookup data value field and data text fields
            ResolveLookups(lookups);
            // insert records from the file
            int recordCount = 0;
            int errorCount = 0;
            NumberFormatInfo nfi = CultureInfo.CurrentCulture.NumberFormat;
            Regex numberCleanupRegex = new Regex(String.Format("[^\\d\\{0}\\{1}\\{2}]", nfi.CurrencyDecimalSeparator, nfi.NegativeSign, nfi.NumberDecimalSeparator));
            while (reader.Read())
            {
                ActionArgs args = new ActionArgs();
                args.Controller = controller;
                args.View = view;
                args.LastCommandName = "New";
                args.CommandName = "Insert";
                List<FieldValue> values = new List<FieldValue>();
                foreach (int index in map.Keys)
                {
                    DataField field = map[index];
                    object v = reader[index];
                    if (DBNull.Value.Equals(v))
                    	v = null;
                    else
                    	if (field.Type != "String" && (v is string))
                        {
                            string s = ((string)(v));
                            if (field.Type == "Boolean")
                            	v = s.ToLower();
                            else
                            	if (!(field.Type.StartsWith("Date")) && field.Type != "Time")
                                	v = numberCleanupRegex.Replace(s, String.Empty);
                        }
                    if (v != null)
                    {
                        DataField lookupField = null;
                        if (lookups.TryGetValue(field.Name, out lookupField))
                        	if (lookupField.Items.Count > 0)
                            {
                                // copy static values
                                foreach (object[] item in lookupField.Items)
                                	if (Convert.ToString(item[1]).Equals(Convert.ToString(v), StringComparison.CurrentCultureIgnoreCase))
                                    	values.Add(new FieldValue(lookupField.Name, item[0]));
                            }
                            else
                            {
                                PageRequest lookupRequest = new PageRequest();
                                lookupRequest.Controller = lookupField.ItemsDataController;
                                lookupRequest.View = lookupField.ItemsDataView;
                                lookupRequest.RequiresMetaData = true;
                                lookupRequest.PageSize = 1;
                                lookupRequest.Filter = new string[] {
                                        String.Format("{0}:={1}{2}", lookupField.ItemsDataTextField, v, Convert.ToChar(0))};
                                ViewPage vp = ControllerFactory.CreateDataController().GetPage(lookupRequest.Controller, lookupRequest.View, lookupRequest);
                                if (vp.Rows.Count > 0)
                                	values.Add(new FieldValue(lookupField.ItemsDataValueField, vp.Rows[0][vp.Fields.IndexOf(vp.FindField(lookupField.ItemsDataValueField))]));
                            }
                        else
                        	values.Add(new FieldValue(field.Name, v));
                    }
                }
                recordCount++;
                if (values.Count > 0)
                {
                    args.Values = values.ToArray();
                    ActionResult r = ControllerFactory.CreateDataController().Execute(controller, view, args);
                    if (r.Errors.Count > 0)
                    {
                        log.WriteLine("{0:s} Error importing record #{1}.", DateTime.Now, recordCount);
                        log.WriteLine();
                        foreach (string s in r.Errors)
                        	log.WriteLine(s);
                        foreach (FieldValue v in values)
                        	if (v.Modified)
                            	log.WriteLine("{0}={1};", v.Name, v.Value);
                        log.WriteLine();
                        errorCount++;
                    }
                }
                else
                {
                    log.WriteLine("{0:s} Record #1 has been ignored.", DateTime.Now, recordCount);
                    errorCount++;
                }
            }
            reader.Close();
            log.WriteLine("{0:s} Processed {1} records. Detected {2} errors.", DateTime.Now, recordCount, errorCount);
            log.Close();
            if (!(String.IsNullOrEmpty(notify)))
            {
                string[] recipients = notify.Split(',');
                SmtpClient client = new SmtpClient();
                foreach (string s in recipients)
                {
                    string address = s.Trim();
                    if (!(String.IsNullOrEmpty(address)))
                    {
                        MailMessage message = new MailMessage();
                        try
                        {
                            message.To.Add(new MailAddress(address));
                            message.Subject = String.Format("Import of {0} has been completed", controller);
                            message.Body = File.ReadAllText(logFileName);
                            client.Send(message);
                        }
                        catch (Exception )
                        {
                        }
                    }
                }
            }
            File.Delete(logFileName);
            AfterProcess(fileName, controller, view, notify, userMapping);
        }
        
        public int CountRecords(string fileName)
        {
            OleDbDataReader reader = OpenRead(fileName, "count(*)");
            try
            {
                reader.Read();
                return Convert.ToInt32(reader[0]);
            }
            finally
            {
                reader.Close();
            }
        }
        
        public string MapFieldName(DataField field)
        {
            string s = field.HeaderText;
            if (String.IsNullOrEmpty(s))
            	s = field.Label;
            if (String.IsNullOrEmpty(s))
            	s = field.Name;
            return s;
        }
        
        public string CreateListOfAvailableFields(string controller, string view)
        {
            PageRequest request = new PageRequest();
            request.Controller = controller;
            request.View = view;
            request.RequiresMetaData = true;
            ViewPage page = ControllerFactory.CreateDataController().GetPage(controller, view, request);
            StringBuilder sb = new StringBuilder();
            foreach (DataField f in page.Fields)
            	if (!(f.Hidden) && !(f.ReadOnly))
                {
                    sb.AppendFormat("{0}=", f.Name);
                    DataField field = f;
                    if (!(String.IsNullOrEmpty(f.AliasName)))
                    	field = page.FindField(f.AliasName);
                    sb.AppendLine(MapFieldName(field));
                }
            return sb.ToString();
        }
        
        public string CreateInitialFieldMap(string fileName, string controller, string view)
        {
            // retreive metadata
            PageRequest request = new PageRequest();
            request.Controller = controller;
            request.View = view;
            request.RequiresMetaData = true;
            ViewPage page = ControllerFactory.CreateDataController().GetPage(controller, view, request);
            // create initial map
            StringBuilder sb = new StringBuilder();
            OleDbDataReader reader = OpenRead(fileName, "*");
            try
            {
                ImportMapDictionary map = new ImportMapDictionary();
                ImportLookupDictionary lookups = new ImportLookupDictionary();
                EnumerateFields(reader, page, map, lookups, null);
                for (int i = 0; (i < reader.FieldCount); i++)
                {
                    sb.AppendFormat("{0}=", reader.GetName(i));
                    DataField field = null;
                    if (map.TryGetValue(i, out field))
                    {
                        string fieldName = field.Name;
                        foreach (DataField lookupField in lookups.Values)
                        	if (lookupField.AliasName == field.Name)
                            {
                                fieldName = lookupField.Name;
                                break;
                            }
                        sb.Append(fieldName);
                    }
                    sb.AppendLine();
                }
            }
            finally
            {
                reader.Close();
            }
            return sb.ToString();
        }
    }
}
