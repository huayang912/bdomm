using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Security;

namespace BUDI2_NS.Data
{
	public partial class Controller : DataControllerBase
    {
    }
    
    public class DataControllerBase : IDataController, IAutoCompleteManager, IDataEngine, IBusinessObject
    {
        
        public const int MaximumDistinctValues = 200;
        
        public const int MaximumRssItems = 200;
        
        private ControllerConfiguration _config;
        
        private XPathNavigator _view;
        
        private string _viewId;
        
        private string _parameterMarker;
        
        private string _viewType;
        
        private ViewPage _viewPage;
        
        private bool _viewOverridingDisabled;
        
        private bool _hasWhere;
        
        private string _viewFilter;
        
        private DbCommand _currentCommand;
        
        private SelectClauseDictionary _currentExpressions;
        
        private static SortedDictionary<string, Type> _typeMap;
        
        private static SortedDictionary<string, string> _rowsetTypeMap;
        
        private BusinessObjectParameters _parameters;
        
        static DataControllerBase()
        {
            // initialize type map
            _typeMap = new SortedDictionary<string, Type>();
            _typeMap.Add("AnsiString", typeof(string));
            _typeMap.Add("Binary", typeof(byte[]));
            _typeMap.Add("Byte", typeof(byte));
            _typeMap.Add("Boolean", typeof(bool));
            _typeMap.Add("Currency", typeof(decimal));
            _typeMap.Add("Date", typeof(DateTime));
            _typeMap.Add("DateTime", typeof(DateTime));
            _typeMap.Add("Decimal", typeof(decimal));
            _typeMap.Add("Double", typeof(double));
            _typeMap.Add("Guid", typeof(Guid));
            _typeMap.Add("Int16", typeof(short));
            _typeMap.Add("Int32", typeof(int));
            _typeMap.Add("Int64", typeof(long));
            _typeMap.Add("Object", typeof(object));
            _typeMap.Add("SByte", typeof(sbyte));
            _typeMap.Add("Single", typeof(float));
            _typeMap.Add("String", typeof(string));
            _typeMap.Add("Time", typeof(TimeSpan));
            _typeMap.Add("TimeSpan", typeof(DateTime));
            _typeMap.Add("UInt16", typeof(ushort));
            _typeMap.Add("UInt32", typeof(uint));
            _typeMap.Add("UInt64", typeof(ulong));
            _typeMap.Add("VarNumeric", typeof(object));
            _typeMap.Add("AnsiStringFixedLength", typeof(string));
            _typeMap.Add("StringFixedLength", typeof(string));
            _typeMap.Add("Xml", typeof(string));
            _typeMap.Add("DateTime2", typeof(DateTime));
            _typeMap.Add("DateTimeOffset", typeof(DateTimeOffset));
            // initialize rowset type map
            _rowsetTypeMap = new SortedDictionary<string, string>();
            _rowsetTypeMap.Add("AnsiString", "string");
            _rowsetTypeMap.Add("Binary", "bin.base64");
            _rowsetTypeMap.Add("Byte", "u1");
            _rowsetTypeMap.Add("Boolean", "boolean");
            _rowsetTypeMap.Add("Currency", "float");
            _rowsetTypeMap.Add("Date", "date");
            _rowsetTypeMap.Add("DateTime", "dateTime");
            _rowsetTypeMap.Add("Decimal", "float");
            _rowsetTypeMap.Add("Double", "float");
            _rowsetTypeMap.Add("Guid", "uuid");
            _rowsetTypeMap.Add("Int16", "i2");
            _rowsetTypeMap.Add("Int32", "i4");
            _rowsetTypeMap.Add("Int64", "i8");
            _rowsetTypeMap.Add("Object", "string");
            _rowsetTypeMap.Add("SByte", "i1");
            _rowsetTypeMap.Add("Single", "float");
            _rowsetTypeMap.Add("String", "string");
            _rowsetTypeMap.Add("Time", "time");
            _rowsetTypeMap.Add("UInt16", "u2");
            _rowsetTypeMap.Add("UInt32", "u4");
            _rowsetTypeMap.Add("UIn64", "u8");
            _rowsetTypeMap.Add("VarNumeric", "float");
            _rowsetTypeMap.Add("AnsiStringFixedLength", "string");
            _rowsetTypeMap.Add("StringFixedLength", "string");
            _rowsetTypeMap.Add("Xml", "string");
            _rowsetTypeMap.Add("DateTime2", "dateTime");
            _rowsetTypeMap.Add("DateTimeOffset", "dateTime.tz");
            _rowsetTypeMap.Add("TimeSpan", "time");
        }
        
        public DataControllerBase()
        {
            Initialize();
        }
        
        private IXmlNamespaceResolver Resolver
        {
            get
            {
                return _config.Resolver;
            }
        }
        
        public bool ViewOverridingDisabled
        {
            get
            {
                return _viewOverridingDisabled;
            }
            set
            {
                _viewOverridingDisabled = value;
            }
        }
        
        public static SortedDictionary<string, Type> TypeMap
        {
            get
            {
                return _typeMap;
            }
        }
        
        public static SortedDictionary<string, string> RowsetTypeMap
        {
            get
            {
                return _rowsetTypeMap;
            }
        }
        
        protected virtual bool YieldsSingleRow(DbCommand command)
        {
            return (command.CommandText.IndexOf("count(*)") == -1);
        }
        
        ViewPage IDataController.GetPage(string controller, string view, PageRequest request)
        {
            SelectView(controller, view);
            request.AssignContext(controller, this._viewId);
            ViewPage page = new ViewPage(request);
            if (_config.PlugIn != null)
            	_config.PlugIn.PreProcessPageRequest(request, page);
            _config.AssignDynamicExpressions(page);
            page.ApplyDataFilter(_config.CreateDataFilter(), request.Controller, request.View, request.LookupContextController, request.LookupContextView, request.LookupContextFieldName);
            using (DbConnection connection = CreateConnection())
            {
                if (page.RequiresRowCount && !((request.Inserting || request.DoesNotRequireData)))
                {
                    DbCommand countCommand = CreateCommand(connection);
                    ConfigureCommand(countCommand, page, CommandConfigurationType.SelectCount, null);
                    if (YieldsSingleRow(countCommand))
                    	page.TotalRowCount = 1;
                    else
                    	page.TotalRowCount = Convert.ToInt32(countCommand.ExecuteScalar());
                    if (page.RequiresAggregates)
                    {
                        DbCommand aggregateCommand = CreateCommand(connection);
                        ConfigureCommand(aggregateCommand, page, CommandConfigurationType.SelectAggregates, null);
                        DbDataReader reader = aggregateCommand.ExecuteReader();
                        if (reader.Read())
                        {
                            object[] aggregates = new object[page.Fields.Count];
                            for (int i = 0; (i < aggregates.Length); i++)
                            {
                                DataField field = page.Fields[i];
                                if (field.Aggregate != DataFieldAggregate.None)
                                {
                                    object v = reader[field.Name];
                                    if (!(DBNull.Value.Equals(v)))
                                    	aggregates[i] = v;
                                }
                            }
                            page.Aggregates = aggregates;
                        }
                        reader.Close();
                    }
                }
                if (page.RequiresMetaData)
                	PopulatePageCategories(page);
                DbCommand selectCommand = CreateCommand(connection);
                ConfigureCommand(selectCommand, page, CommandConfigurationType.Select, null);
                if ((page.PageSize > 0) && !((request.Inserting || request.DoesNotRequireData)))
                {
                    DbDataReader reader = TransactionManager.ExecuteReader(request, page, selectCommand);
                    while (page.SkipNext())
                    	reader.Read();
                    while (page.ReadNext() && reader.Read())
                    {
                        object[] values = new object[page.Fields.Count];
                        for (int i = 0; (i < values.Length); i++)
                        {
                            DataField field = page.Fields[i];
                            object v = reader[field.Name];
                            if (!(DBNull.Value.Equals(v)))
                            {
                                Type vt = v.GetType();
                                if ((vt.Equals(typeof(System.DateTimeOffset)) || vt.Equals(typeof(System.TimeSpan))) || String.IsNullOrEmpty(field.Type))
                                	v = v.ToString();
                                if (field.IsMirror)
                                	v = String.Format(field.DataFormatString, v);
                                values[i] = v;
                            }
                            if (!(String.IsNullOrEmpty(field.SourceFields)))
                            	values[i] = CreateValueFromSourceFields(field, reader);
                        }
                        page.Rows.Add(values);
                    }
                    reader.Close();
                }
            }
            if (_config.PlugIn != null)
            	_config.PlugIn.ProcessPageRequest(request, page);
            IRowHandler rowHandler = _config.CreateRowHandler();
            if (rowHandler != null)
            {
                if (request.Inserting)
                {
                    if (rowHandler.SupportsNewRow(request))
                    {
                        page.NewRow = new object[page.Fields.Count];
                        rowHandler.NewRow(request, page, page.NewRow);
                    }
                }
                else
                	if (rowHandler.SupportsPrepareRow(request))
                    	foreach (object[] row in page.Rows)
                        	rowHandler.PrepareRow(request, page, row);
                BusinessRules rules = ((BusinessRules)(rowHandler));
                if (rules != null)
                	rules.ProcessPageRequest(request, page);
            }
            page = page.ToResult(_config, _view);
            return page;
        }
        
        protected string CreateValueFromSourceFields(DataField field, DbDataReader reader)
        {
            string v = String.Empty;
            if (DBNull.Value.Equals(reader[field.Name]))
            	v = "null";
            Match m = Regex.Match(field.SourceFields, "(\\w+)\\s*(,|$)");
            while (m.Success)
            {
                if (v.Length > 0)
                	v = (v + "|");
                v = (v + Convert.ToString(reader[m.Groups[1].Value]));
                m = m.NextMatch();
            }
            return v;
        }
        
        private void PopulatePageCategories(ViewPage page)
        {
            XPathNodeIterator categoryIterator = _view.Select("c:categories/c:category", Resolver);
            while (categoryIterator.MoveNext())
            	page.Categories.Add(new Category(categoryIterator.Current, Resolver));
            if (page.Categories.Count == 0)
            	page.Categories.Add(new Category());
        }
        
        object[] IDataController.GetListOfValues(string controller, string view, DistinctValueRequest request)
        {
            SelectView(controller, view);
            ViewPage page = new ViewPage(request);
            page.ApplyDataFilter(_config.CreateDataFilter(), controller, view, request.LookupContextController, request.LookupContextView, request.LookupContextFieldName);
            List<object> distinctValues = new List<object>();
            using (DbConnection connection = CreateConnection())
            {
                DbCommand command = CreateCommand(connection);
                ConfigureCommand(command, page, CommandConfigurationType.SelectDistinct, null);
                DbDataReader reader = command.ExecuteReader();
                while (reader.Read() && (distinctValues.Count < page.PageSize))
                {
                    object v = reader.GetValue(0);
                    if (!(DBNull.Value.Equals(v)))
                    {
                        if (v is TimeSpan)
                        	v = v.ToString();
                    }
                    distinctValues.Add(v);
                }
                reader.Close();
            }
            return distinctValues.ToArray();
        }
        
        protected ViewPage CreateViewPage()
        {
            if (_viewPage == null)
            {
                _viewPage = new ViewPage();
                PopulatePageFields(_viewPage);
                EnsurePageFields(_viewPage, null);
            }
            return _viewPage;
        }
        
        private bool ExecuteWithPivot(string controller, string view, ActionArgs args, ActionResult result)
        {
            if (!(args.RequiresPivot))
            	return false;
            _config = CreateConfiguration(controller);
            using (DbConnection connection = CreateConnection())
            	using (SinglePhaseTransactionScope scope = new SinglePhaseTransactionScope())
                {
                    scope.Enlist(connection);
                    ViewOverridingDisabled = true;
                    int valueSetIndex = 0;
                    foreach (FieldValue[] valueSet in args.PivotValues)
                    {
                        args.Values = valueSet;
                        IDataController c = ((IDataController)(this));
                        ActionResult result2 = c.Execute(controller, view, args);
                        if (result2.RowNotFound)
                        {
                            if (args.CommandName == "Update")
                            {
                                result2.Errors.Clear();
                                string saveCommandName = args.CommandName;
                                string saveLastCommandName = args.LastCommandName;
                                args.CommandName = "Insert";
                                args.LastCommandName = "New";
                                result2 = c.Execute(controller, view, args);
                                args.CommandName = saveCommandName;
                                args.LastCommandName = saveCommandName;
                                args.LastCommandName = saveLastCommandName;
                            }
                        }
                        if (args.CommandName == "Delete")
                        {
                            result2.Errors.Clear();
                            result2.RowNotFound = false;
                        }
                        if (result2.Values != null)
                        	foreach (FieldValue v in result2.Values)
                            	v.Name = String.Format("PivotCol{1}_{0}", v.Name, valueSetIndex);
                        result.Merge(result2);
                        valueSetIndex++;
                    }
                    if (result.Errors.Count == 0)
                    	scope.Complete();
                    else
                    	scope.Rollback();
                }
            return true;
        }
        
        ActionResult IDataController.Execute(string controller, string view, ActionArgs args)
        {
            ActionResult result = new ActionResult();
            if (ExecuteWithPivot(controller, view, args, result))
            	return result;
            SelectView(controller, view);
            try
            {
                ValidateArguments(args);
                IActionHandler handler = _config.CreateActionHandler();
                if (_config.PlugIn != null)
                	_config.PlugIn.PreProcessArguments(args, result, CreateViewPage());
                if (args.SqlCommandType != CommandConfigurationType.None)
                	using (DbConnection connection = CreateConnection())
                    {
                        ExecutePreActionCommands(args, result, connection);
                        if (handler != null)
                        	handler.BeforeSqlAction(args, result);
                        if ((result.Errors.Count == 0) && !(result.Canceled))
                        {
                            DbCommand command = CreateCommand(connection, args);
                            if ((args.SelectedValues != null) && (((args.LastCommandName == "BatchEdit") && (args.CommandName == "Update")) || ((args.CommandName == "Delete") && (args.SelectedValues.Length > 1))))
                            {
                                ViewPage page = CreateViewPage();
                                PopulatePageFields(page);
                                string originalCommandText = command.CommandText;
                                foreach (string sv in args.SelectedValues)
                                {
                                    string[] key = sv.Split(',');
                                    int keyIndex = 0;
                                    foreach (FieldValue v in args.Values)
                                    {
                                        DataField field = page.FindField(v.Name);
                                        if (field != null)
                                        	if (!(field.IsPrimaryKey))
                                            	v.Modified = true;
                                            else
                                            	if (v.Name == field.Name)
                                                {
                                                    v.OldValue = key[keyIndex];
                                                    v.Modified = false;
                                                    keyIndex++;
                                                }
                                    }
                                    ConfigureCommand(command, null, args.SqlCommandType, args.Values);
                                    result.RowsAffected = (result.RowsAffected + TransactionManager.ExecuteNonQuery(command));
                                    if (handler != null)
                                    	handler.AfterSqlAction(args, result);
                                    command.CommandText = originalCommandText;
                                    command.Parameters.Clear();
                                    if (_config.PlugIn != null)
                                    	_config.PlugIn.ProcessArguments(args, result, page);
                                }
                            }
                            else
                            {
                                if (ConfigureCommand(command, null, args.SqlCommandType, args.Values))
                                {
                                    result.RowsAffected = TransactionManager.ExecuteNonQuery(args, result, CreateViewPage(), command);
                                    if (result.RowsAffected == 0)
                                    {
                                        result.RowNotFound = true;
                                        result.Errors.Add(Localizer.Replace("RecordChangedByAnotherUser", "The record has been changed by another user."));
                                    }
                                    else
                                    	ExecutePostActionCommands(args, result, connection);
                                }
                                if (handler != null)
                                	handler.AfterSqlAction(args, result);
                                if (_config.PlugIn != null)
                                	_config.PlugIn.ProcessArguments(args, result, CreateViewPage());
                            }
                        }
                    }
                else
                	if (args.CommandName.StartsWith("Export"))
                    	ExecuteDataExport(args, result);
                    else
                    	if (args.CommandName.Equals("PopulateDynamicLookups"))
                        	PopulateDynamicLookups(args, result);
                        else
                        	if (args.CommandName.Equals("ProcessImportFile"))
                            	ImportProcessor.Execute(args);
                            else
                            	if (args.CommandName.Equals("Execute"))
                                	using (DbConnection connection = CreateConnection())
                                    {
                                        DbCommand command = CreateCommand(connection, args);
                                        TransactionManager.ExecuteNonQuery(command);
                                    }
                                else
                                	if (handler != null)
                                    	handler.ExecuteAction(args, result);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(System.Reflection.TargetInvocationException))
                	ex = ex.InnerException;
                while (ex != null)
                {
                    result.Errors.Add(ex.Message);
                    ex = ex.InnerException;
                }
            }
            return result;
        }
        
        void PopulateDynamicLookups(ActionArgs args, ActionResult result)
        {
            ViewPage page = CreateViewPage();
            foreach (DataField field in page.Fields)
            	if (page.PopulateStaticItems(field, args.Values))
                	result.Values.Add(new FieldValue(field.Name, field.Items.ToArray()));
        }
        
        DbDataReader IDataEngine.ExecuteReader(PageRequest request)
        {
            ViewPage page = new ViewPage(request);
            if (_config == null)
            {
                _config = CreateConfiguration(request.Controller);
                SelectView(request.Controller, request.View);
            }
            page.ApplyDataFilter(_config.CreateDataFilter(), request.Controller, request.View, null, null, null);
            DbConnection connection = CreateConnection();
            DbCommand selectCommand = CreateCommand(connection);
            ConfigureCommand(selectCommand, page, CommandConfigurationType.Select, null);
            return selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }
        
        private void ExecuteDataExport(ActionArgs args, ActionResult result)
        {
            if (!(String.IsNullOrEmpty(args.CommandArgument)))
            {
                string[] arguments = args.CommandArgument.Split(',');
                if (arguments.Length > 0)
                {
                    bool sameController = (args.Controller == arguments[0]);
                    args.Controller = arguments[0];
                    if (arguments.Length == 1)
                    	args.View = "grid1";
                    else
                    	args.View = arguments[1];
                    if (sameController)
                    	args.SortExpression = null;
                    SelectView(args.Controller, args.View);
                }
            }
            PageRequest request = new PageRequest(-1, -1, null, null);
            request.SortExpression = args.SortExpression;
            request.Filter = args.Filter;
            request.ContextKey = null;
            request.PageIndex = 0;
            request.PageSize = Int32.MaxValue;
            if (args.CommandName.EndsWith("Template"))
            {
                request.PageSize = 0;
                args.CommandName = "ExportCsv";
            }
            // store export data to a temporary file
            string fileName = Path.GetTempFileName();
            StreamWriter writer = File.CreateText(fileName);
            try
            {
                ViewPage page = new ViewPage(request);
                page.ApplyDataFilter(_config.CreateDataFilter(), args.Controller, args.View, null, null, null);
                using (DbConnection connection = CreateConnection())
                {
                    DbCommand selectCommand = CreateCommand(connection);
                    ConfigureCommand(selectCommand, page, CommandConfigurationType.Select, null);
                    DbDataReader reader = selectCommand.ExecuteReader();
                    if (args.CommandName.EndsWith("Csv"))
                    	ExportDataAsCsv(page, reader, writer);
                    if (args.CommandName.EndsWith("Rss"))
                    	ExportDataAsRss(page, reader, writer);
                    if (args.CommandName.EndsWith("Rowset"))
                    	ExportDataAsRowset(page, reader, writer);
                    reader.Close();
                }
            }
            finally
            {
                writer.Close();
            }
            result.Values.Add(new FieldValue("FileName", null, fileName));
        }
        
        private void ExportDataAsRowset(ViewPage page, DbDataReader reader, StreamWriter writer)
        {
            string s = "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882";
            string dt = "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882";
            string rs = "urn:schemas-microsoft-com:rowset";
            string z = "#RowsetSchema";
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CloseOutput = false;
            XmlWriter output = XmlWriter.Create(writer, settings);
            output.WriteStartDocument();
            output.WriteStartElement("xml");
            output.WriteAttributeString("xmlns", "s", null, s);
            output.WriteAttributeString("xmlns", "dt", null, dt);
            output.WriteAttributeString("xmlns", "rs", null, rs);
            output.WriteAttributeString("xmlns", "z", null, z);
            // declare rowset schema
            output.WriteStartElement("Schema", s);
            output.WriteAttributeString("id", "RowsetSchema");
            output.WriteStartElement("ElementType", s);
            output.WriteAttributeString("name", "row");
            output.WriteAttributeString("content", "eltOnly");
            output.WriteAttributeString("CommandTimeout", rs, "60");
            List<DataField> fields = new List<DataField>();
            foreach (DataField field in page.Fields)
            	if (!((field.Hidden || field.OnDemand)) && !(fields.Contains(field)))
                {
                    DataField aliasField = field;
                    if (!(String.IsNullOrEmpty(field.AliasName)))
                    	aliasField = page.FindField(field.AliasName);
                    fields.Add(aliasField);
                }
            int number = 1;
            foreach (DataField field in fields)
            {
                field.NormalizeDataFormatString();
                output.WriteStartElement("AttributeType", s);
                output.WriteAttributeString("name", field.Name);
                output.WriteAttributeString("number", rs, number.ToString());
                output.WriteAttributeString("nullable", rs, "true");
                output.WriteAttributeString("name", rs, field.Label);
                output.WriteStartElement("datatype", s);
                output.WriteAttributeString("type", dt, RowsetTypeMap[field.Type]);
                if (field.DataFormatString == "{0:c}")
                	output.WriteAttributeString("dbtype", rs, "currency");
                output.WriteEndElement();
                output.WriteEndElement();
                number++;
            }
            output.WriteStartElement("extends", s);
            output.WriteAttributeString("type", "rs:rowbase");
            output.WriteEndElement();
            output.WriteEndElement();
            output.WriteEndElement();
            // output rowset data
            output.WriteStartElement("data", rs);
            while (reader.Read())
            {
                output.WriteStartElement("row", z);
                foreach (DataField field in fields)
                {
                    object v = reader[field.Name];
                    if (!(DBNull.Value.Equals(v)))
                    	if (!(String.IsNullOrEmpty(field.DataFormatString)) && !(((field.DataFormatString == "{0:d}") || (field.DataFormatString == "{0:c}"))))
                        	output.WriteAttributeString(field.Name, String.Format(field.DataFormatString, v));
                        else
                        	if (field.Type == "DateTime")
                            	output.WriteAttributeString(field.Name, ((DateTime)(v)).ToString("s"));
                            else
                            	output.WriteAttributeString(field.Name, v.ToString());
                }
                output.WriteEndElement();
            }
            output.WriteEndElement();
            output.WriteEndElement();
            output.WriteEndDocument();
            output.Close();
        }
        
        private void ExportDataAsRss(ViewPage page, DbDataReader reader, StreamWriter writer)
        {
            string appPath = Regex.Replace(HttpContext.Current.Request.Url.AbsoluteUri, "^(.+)Export.ashx.+$", "$1", RegexOptions.IgnoreCase);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CloseOutput = false;
            XmlWriter output = XmlWriter.Create(writer, settings);
            output.WriteStartDocument();
            output.WriteStartElement("rss");
            output.WriteAttributeString("version", "2.0");
            output.WriteStartElement("channel");
            output.WriteElementString("title", ((string)(_view.Evaluate("string(concat(/c:dataController/@label, \' | \',  @label))", Resolver))));
            output.WriteElementString("lastBuildDate", DateTime.Now.ToString("r"));
            output.WriteElementString("language", System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower());
            int rowCount = 0;
            while ((rowCount < MaximumRssItems) && reader.Read())
            {
                output.WriteStartElement("item");
                bool hasTitle = false;
                bool hasPubDate = false;
                StringBuilder desc = new StringBuilder();
                for (int i = 0; (i < page.Fields.Count); i++)
                {
                    DataField field = page.Fields[i];
                    if (!(field.Hidden))
                    {
                        if (rowCount == 0)
                        	field.NormalizeDataFormatString();
                        if (!(String.IsNullOrEmpty(field.AliasName)))
                        	field = page.FindField(field.AliasName);
                        string text = String.Empty;
                        object v = reader[field.Name];
                        if (!(DBNull.Value.Equals(v)))
                        	if (!(String.IsNullOrEmpty(field.DataFormatString)))
                            	text = String.Format(field.DataFormatString, v);
                            else
                            	text = Convert.ToString(v);
                        if (!(hasPubDate) && (field.Type == "DateTime"))
                        {
                            hasPubDate = true;
                            if (!(String.IsNullOrEmpty(text)))
                            	output.WriteElementString("pubDate", ((DateTime)(reader[field.Name])).ToString("r"));
                        }
                        if (!(hasTitle))
                        {
                            hasTitle = true;
                            output.WriteElementString("title", text);
                            StringBuilder link = new StringBuilder();
                            link.Append(_config.Navigator.Evaluate("string(/c:dataController/@name)", Resolver));
                            foreach (DataField pkf in page.Fields)
                            	if (pkf.IsPrimaryKey)
                                	link.Append(String.Format("&{0}={1}", pkf.Name, reader[pkf.Name]));
                            string itemGuid = String.Format("{0}Details.aspx?l={1}", appPath, HttpUtility.UrlEncode(Convert.ToBase64String(Encoding.Default.GetBytes(link.ToString()))));
                            output.WriteElementString("link", itemGuid);
                            output.WriteElementString("guid", itemGuid);
                        }
                        else
                        	if (!(String.IsNullOrEmpty(field.OnDemandHandler)) && (field.OnDemandStyle == OnDemandDisplayStyle.Thumbnail))
                            {
                                if (text.Equals("1"))
                                {
                                    desc.AppendFormat("{0}:<br /><img src=\"{1}Blob.ashx?{2}=t", HttpUtility.HtmlEncode(field.Label), appPath, field.OnDemandHandler);
                                    foreach (DataField f in page.Fields)
                                    	if (f.IsPrimaryKey)
                                        {
                                            desc.Append("|");
                                            desc.Append(reader[f.Name]);
                                        }
                                    desc.Append("\" style=\"width:92px;height:71px;\"/><br />");
                                }
                            }
                            else
                            	desc.AppendFormat("{0}: {1}<br />", HttpUtility.HtmlEncode(field.Label), HttpUtility.HtmlEncode(text));
                    }
                }
                output.WriteStartElement("description");
                output.WriteCData(String.Format("<span style=\\\"font-size:small;\\\">{0}</span>", desc.ToString()));
                output.WriteEndElement();
                output.WriteEndElement();
                rowCount++;
            }
            output.WriteEndElement();
            output.WriteEndElement();
            output.WriteEndDocument();
            output.Close();
        }
        
        private void ExportDataAsCsv(ViewPage page, DbDataReader reader, StreamWriter writer)
        {
            bool firstField = true;
            for (int i = 0; (i < page.Fields.Count); i++)
            {
                DataField field = page.Fields[i];
                if (!(field.Hidden))
                {
                    if (firstField)
                    	firstField = false;
                    else
                    	writer.Write(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                    if (!(String.IsNullOrEmpty(field.AliasName)))
                    	field = page.FindField(field.AliasName);
                    writer.Write("\"{0}\"", field.Label.Replace("\"", "\"\""));
                }
                field.NormalizeDataFormatString();
            }
            writer.WriteLine();
            while (reader.Read())
            {
                firstField = true;
                for (int j = 0; (j < page.Fields.Count); j++)
                {
                    DataField field = page.Fields[j];
                    if (!(field.Hidden))
                    {
                        if (firstField)
                        	firstField = false;
                        else
                        	writer.Write(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                        if (!(String.IsNullOrEmpty(field.AliasName)))
                        	field = page.FindField(field.AliasName);
                        string text = String.Empty;
                        object v = reader[field.Name];
                        if (!(DBNull.Value.Equals(v)))
                        {
                            if (!(String.IsNullOrEmpty(field.DataFormatString)))
                            	text = String.Format(field.DataFormatString, v);
                            else
                            	text = Convert.ToString(v);
                            writer.Write("\"{0}\"", text.Replace("\"", "\"\""));
                        }
                        else
                        	writer.Write("\"\"");
                    }
                }
                writer.WriteLine();
            }
        }
        
        private void ValidateArguments(ActionArgs args)
        {
            if ((args.CommandName == "PopulateDynamicLookups") || (args.CommandName == "Calculate"))
            	return;
            if ((args.CommandName == "ProcessImportFile") || (args.CommandName == "ExportTemplate"))
            	return;
            if ((args.CommandName == "UploadFile") || (args.CommandName == "DownloadFile"))
            	return;
            XPathNodeIterator action = null;
            if (!(String.IsNullOrEmpty(args.LastCommandName)))
            	action = _config.Navigator.Select(String.Format("//c:action[@whenLastCommandName=\'{0}\' and @commandName=\'{1}\']", args.LastCommandName, args.CommandName), Resolver);
            else
            	action = _config.Navigator.Select(String.Format("//c:action[not(@whenLastCommandName!=\'\') and @commandName=\'{0}\']", args.CommandName), Resolver);
            if (action.MoveNext())
            {
                if (!(Controller.UserIsInRole(action.Current.GetAttribute("roles", String.Empty))))
                	throw new Exception(String.Format("You are not authorized to execute command \'{0}\'.", args.CommandName));
            }
            else
            {
                if ((args.LastCommandName == "Select") || (args.LastCommandName == "Cancel"))
                {
                    action = _config.Navigator.Select(String.Format("//c:action[not(@whenLastCommandName!=\'\') and @commandName=\'{0}\']", args.CommandName), Resolver);
                    if (action.MoveNext())
                    	return;
                }
                throw new Exception(String.Format("Unauthorized attempt to execute command \'{0}\' when last executed command name is " +
                            "\'{1}\'.", args.CommandName, args.LastCommandName));
            }
        }
        
        public static bool UserIsInRole(string roles)
        {
            bool inRole = true;
            if (!(String.IsNullOrEmpty(roles)))
            {
                if (HttpContext.Current == null)
                	return true;
                inRole = false;
                foreach (string role in roles.Split(','))
                	if (!(String.IsNullOrEmpty(role)) && HttpContext.Current.User.IsInRole(role))
                    {
                        inRole = true;
                        break;
                    }
            }
            return inRole;
        }
        
        private void ExecutePostActionCommands(ActionArgs args, ActionResult result, DbConnection connection)
        {
            if (!(TransactionManager.InTransaction(args)))
            {
                string eventName = String.Empty;
                if (args.CommandName.Equals("insert", StringComparison.OrdinalIgnoreCase))
                	eventName = "Inserted";
                else
                	if (args.CommandName.Equals("update", StringComparison.OrdinalIgnoreCase))
                    	eventName = "Updated";
                    else
                    	if (args.CommandName.Equals("delete", StringComparison.OrdinalIgnoreCase))
                        	eventName = "Deleted";
                XPathNodeIterator eventCommandIterator = _config.Navigator.Select(String.Format("/c:dataController/c:commands/c:command[@event=\'{0}\']", eventName), Resolver);
                while (eventCommandIterator.MoveNext())
                	ExecuteActionCommand(args, result, connection, eventCommandIterator.Current);
            }
            if ((args.SaveLEVs && (HttpContext.Current.Session != null)) && ((args.CommandName == "Insert") || (args.CommandName == "Update")))
            	HttpContext.Current.Session[String.Format("{0}$LEVs", args.Controller)] = args.Values;
        }
        
        private void ExecuteActionCommand(ActionArgs args, ActionResult result, DbConnection connection, XPathNavigator commandNavigator)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandType = ((CommandType)(TypeDescriptor.GetConverter(typeof(CommandType)).ConvertFromString(((string)(commandNavigator.Evaluate("string(@type)"))))));
            command.CommandText = ((string)(commandNavigator.Evaluate("string(c:text)", Resolver)));
            DbDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                int outputIndex = 0;
                XPathNodeIterator outputIterator = commandNavigator.Select("c:output/c:*", Resolver);
                while (outputIterator.MoveNext())
                {
                    if (outputIterator.Current.LocalName == "fieldOutput")
                    {
                        string name = ((string)(outputIterator.Current.Evaluate("string(@name)")));
                        string fieldName = ((string)(outputIterator.Current.Evaluate("string(@fieldName)")));
                        foreach (FieldValue v in args.Values)
                        	if (v.Name == fieldName)
                            {
                                if (String.IsNullOrEmpty(name))
                                	v.NewValue = reader[outputIndex];
                                else
                                	v.NewValue = reader[name];
                                v.Modified = true;
                                if (result != null)
                                	result.Values.Add(v);
                                break;
                            }
                    }
                    outputIndex++;
                }
            }
            reader.Close();
        }
        
        private void ExecutePreActionCommands(ActionArgs args, ActionResult result, DbConnection connection)
        {
            if (TransactionManager.InTransaction(args))
            	return;
            string eventName = String.Empty;
            if (args.CommandName.Equals("insert", StringComparison.OrdinalIgnoreCase))
            	eventName = "Inserting";
            else
            	if (args.CommandName.Equals("update", StringComparison.OrdinalIgnoreCase))
                	eventName = "Updating";
                else
                	if (args.CommandName.Equals("delete", StringComparison.OrdinalIgnoreCase))
                    	eventName = "Deleting";
            XPathNodeIterator eventCommandIterator = _config.Navigator.Select(String.Format("/c:dataController/c:commands/c:command[@event=\'{0}\']", eventName), Resolver);
            while (eventCommandIterator.MoveNext())
            	ExecuteActionCommand(args, result, connection, eventCommandIterator.Current);
        }
        
        protected virtual ControllerConfiguration CreateConfiguration(string controller)
        {
            return BUDI2_NS.Data.Controller.CreateConfigurationInstance(GetType(), controller);
        }
        
        public static ControllerConfiguration CreateConfigurationInstance(Type t, string controller)
        {
            ControllerConfiguration config = ((ControllerConfiguration)(HttpRuntime.Cache[("DataController_" + controller)]));
            if (config == null)
            {
                Stream res = t.Assembly.GetManifestResourceStream(String.Format("BUDI2_NS.Controllers.{0}.xml", controller));
                if (res == null)
                	res = t.Assembly.GetManifestResourceStream(String.Format("BUDI2_NS.{0}.xml", controller));
                if (res == null)
                {
                    string controllerPath = Path.Combine(Path.Combine(HttpRuntime.AppDomainAppPath, "Controllers"), (controller + ".xml"));
                    if (!(File.Exists(controllerPath)))
                    	throw new Exception(String.Format("Controller \'{0}\' does not exist.", controller));
                    config = new ControllerConfiguration(controllerPath);
                    HttpRuntime.Cache.Insert(("DataController_" + controller), config, new CacheDependency(controllerPath));
                }
                else
                {
                    config = new ControllerConfiguration(res);
                    HttpRuntime.Cache.Insert(("DataController_" + controller), config);
                }
            }
            bool requiresLocalization = config.RequiresLocalization;
            if (config.UsesVariables)
            	config = config.Clone();
            if (config.PlugIn != null)
            	config = config.PlugIn.Create(config);
            if (requiresLocalization)
            	config = config.Localize(controller);
            return config;
        }
        
        public void SelectView(string controller, string view)
        {
            _config = CreateConfiguration(controller);
            XPathNodeIterator iterator = null;
            if (String.IsNullOrEmpty(view))
            	iterator = _config.Navigator.Select("/c:dataController/c:views/c:view[1]", Resolver);
            else
            	iterator = _config.Navigator.Select(String.Format("/c:dataController/c:views/c:view[@id=\'{0}\']", view), Resolver);
            if (!(iterator.MoveNext()))
            	throw new Exception(String.Format("The view \'{0}\' does not exist.", view));
            _view = iterator.Current;
            _viewId = iterator.Current.GetAttribute("id", String.Empty);
            if (!(ViewOverridingDisabled))
            {
                XPathNodeIterator overrideIterator = _config.Navigator.Select(String.Format("/c:dataController/c:views/c:view[@virtualViewId=\'{0}\']", _viewId), Resolver);
                while (overrideIterator.MoveNext())
                {
                    string viewId = overrideIterator.Current.GetAttribute("id", String.Empty);
                    BusinessRules rules = _config.CreateBusinessRules();
                    if ((rules != null) && rules.IsOverrideApplicable(controller, viewId, _viewId))
                    {
                        _view = overrideIterator.Current;
                        return;
                    }
                }
            }
            _viewType = iterator.Current.GetAttribute("type", String.Empty);
        }
        
        protected virtual DbConnection CreateConnection()
        {
            ConnectionStringSettings settings = ConnectionStringSettingsFactory.Create(_config.ConnectionStringName);
            if (settings == null)
            	throw new Exception(String.Format("Connection string \'{0}\' is not defined in web.config of this application.", _config.ConnectionStringName));
            DbProviderFactory factory = DbProviderFactories.GetFactory(settings.ProviderName);
            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = settings.ConnectionString;
            connection.Open();
            _parameterMarker = SqlStatement.ConvertTypeToParameterMarker(settings.ProviderName);
            return connection;
        }
        
        protected virtual DbCommand CreateCommand(DbConnection connection)
        {
            return CreateCommand(connection, null);
        }
        
        protected virtual DbCommand CreateCommand(DbConnection connection, ActionArgs args)
        {
            string commandId = _view.GetAttribute("commandId", String.Empty);
            XPathNavigator commandNav = _config.Navigator.SelectSingleNode(String.Format("/c:dataController/c:commands/c:command[@id=\'{0}\']", commandId), Resolver);
            if ((args != null) && !(String.IsNullOrEmpty(args.CommandArgument)))
            {
                XPathNavigator commandNav2 = _config.Navigator.SelectSingleNode(String.Format("/c:dataController/c:commands/c:command[@id=\'{0}\']", args.CommandArgument), Resolver);
                if (commandNav2 != null)
                	commandNav = commandNav2;
            }
            if (commandNav == null)
            	throw new Exception(String.Format("Command {0} does not exist.", commandId));
            DbCommand command = connection.CreateCommand();
            if (SinglePhaseTransactionScope.Current != null)
            	SinglePhaseTransactionScope.Current.Enlist(command);
            command.CommandType = ((CommandType)(TypeDescriptor.GetConverter(typeof(CommandType)).ConvertFromString(commandNav.GetAttribute("type", String.Empty))));
            command.CommandText = ((string)(commandNav.Evaluate("string(c:text)", Resolver)));
            if (String.IsNullOrEmpty(command.CommandText))
            	command.CommandText = commandNav.InnerXml;
            IActionHandler handler = _config.CreateActionHandler();
            XPathNodeIterator parameterIterator = commandNav.Select("c:parameters/c:parameter", Resolver);
            SortedDictionary<string, string> missingFields = null;
            while (parameterIterator.MoveNext())
            {
                DbParameter parameter = command.CreateParameter();
                parameter.ParameterName = parameterIterator.Current.GetAttribute("name", String.Empty);
                string s = parameterIterator.Current.GetAttribute("type", String.Empty);
                if (!(String.IsNullOrEmpty(s)))
                	parameter.DbType = ((DbType)(TypeDescriptor.GetConverter(typeof(DbType)).ConvertFromString(s)));
                s = parameterIterator.Current.GetAttribute("direction", String.Empty);
                if (!(String.IsNullOrEmpty(s)))
                	parameter.Direction = ((ParameterDirection)(TypeDescriptor.GetConverter(typeof(ParameterDirection)).ConvertFromString(s)));
                command.Parameters.Add(parameter);
                s = parameterIterator.Current.GetAttribute("defaultValue", String.Empty);
                if (!(String.IsNullOrEmpty(s)))
                	parameter.Value = s;
                s = parameterIterator.Current.GetAttribute("fieldName", String.Empty);
                if ((args != null) && !(String.IsNullOrEmpty(s)))
                {
                    FieldValue v = args.SelectFieldValueObject(s);
                    if (v != null)
                    {
                        s = parameterIterator.Current.GetAttribute("fieldValue", String.Empty);
                        if (s == "Old")
                        	parameter.Value = v.OldValue;
                        else
                        	if (s == "New")
                            	parameter.Value = v.NewValue;
                            else
                            	parameter.Value = v.Value;
                    }
                    else
                    {
                        if (missingFields == null)
                        	missingFields = new SortedDictionary<string, string>();
                        missingFields.Add(parameter.ParameterName, s);
                    }
                }
                s = parameterIterator.Current.GetAttribute("propertyName", String.Empty);
                if (!(String.IsNullOrEmpty(s)) && (handler != null))
                {
                    object result = handler.GetType().InvokeMember(s, (System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.GetField), null, handler, new object[0]);
                    parameter.Value = result;
                }
                if (parameter.Value == null)
                	parameter.Value = DBNull.Value;
            }
            if (missingFields != null)
            {
                bool retrieveMissingValues = true;
                List<string> filter = new List<string>();
                ViewPage page = CreateViewPage();
                foreach (DataField field in page.Fields)
                	if (field.IsPrimaryKey)
                    {
                        FieldValue v = args.SelectFieldValueObject(field.Name);
                        if (v == null)
                        {
                            retrieveMissingValues = false;
                            break;
                        }
                        else
                        	filter.Add(String.Format("{0}:={1}", v.Name, v.Value));
                    }
                if (retrieveMissingValues)
                {
                    string editView = ((string)(_config.Navigator.Evaluate("string(//c:view[@type=\'Form\']/@id)", Resolver)));
                    if (!(String.IsNullOrEmpty(editView)))
                    {
                        PageRequest request = new PageRequest(0, 1, null, filter.ToArray());
                        request.RequiresMetaData = true;
                        page = ControllerFactory.CreateDataController().GetPage(args.Controller, editView, request);
                        if (page.Rows.Count > 0)
                        	foreach (string parameterName in missingFields.Keys)
                            {
                                int index = 0;
                                string fieldName = missingFields[parameterName];
                                foreach (DataField field in page.Fields)
                                {
                                    if (field.Name.Equals(fieldName))
                                    {
                                        object v = page.Rows[0][index];
                                        if (v != null)
                                        	command.Parameters[parameterName].Value = v;
                                    }
                                    index++;
                                }
                            }
                    }
                }
            }
            return command;
        }
        
        protected virtual bool ConfigureCommand(DbCommand command, ViewPage page, CommandConfigurationType commandConfiguration, FieldValue[] values)
        {
            if (page == null)
            	page = new ViewPage();
            PopulatePageFields(page);
            if (command.CommandType == CommandType.Text)
            {
                Match statementMatch = Regex.Match(command.CommandText, "/\\*<select>\\*/(?\'Select\'[\\S\\s]*)?/\\*</select>\\*[\\S\\s]*?/\\*<from>\\*/(?\'From\'[\\S\\s]" +
                        "*)?/\\*</from>\\*[\\S\\s]*?/\\*(<order-by>\\*/(?\'OrderBy\'[\\S\\s]*)?/\\*</order-by>\\*/)?", RegexOptions.IgnoreCase);
                if (!(statementMatch.Success))
                	statementMatch = Regex.Match(command.CommandText, @"\s*select\s*(?'Select'[\S\s]*)?\sfrom\s*(?'From'[\S\s]*)?\swhere\s*(?'Where'[\S\s]*)?\sorder\s+by\s*(?'OrderBy'[\S\s]*)?|\s*select\s*(?'Select'[\S\s]*)?\sfrom\s*(?'From'[\S\s]*)?\swhere\s*(?'Where'[\S\s]*)?|\s*select\s*(?'Select'[\S\s]*)?\sfrom\s*(?'From'[\S\s]*)?\sorder\s+by\s*(?'OrderBy'[\S\s]*)?|\s*select\s*(?'Select'[\S\s]*)?\sfrom\s*(?'From'[\S\s]*)?", RegexOptions.IgnoreCase);
                SelectClauseDictionary expressions = ParseSelectExpressions(statementMatch.Groups["Select"].Value);
                EnsurePageFields(page, expressions);
                AddComputedExpressions(expressions, page);
                AssignPivotParameterValues(command, page);
                if (statementMatch.Success)
                {
                    string fromClause = statementMatch.Groups["From"].Value;
                    string whereClause = statementMatch.Groups["Where"].Value;
                    string orderByClause = statementMatch.Groups["OrderBy"].Value;
                    string tableName = null;
                    if (!(commandConfiguration.ToString().StartsWith("Select")))
                    	tableName = ((string)(_config.Navigator.Evaluate(String.Format("string(/c:dataController/c:commands/c:command[@id=\'{0}\']/@tableName)", _view.GetAttribute("commandId", String.Empty)), Resolver)));
                    // table name regular expression:
                    // ^(?'Table'((\[|"|`)([\w\s]+)?(\]|"|`)|\w+)(\s*\.\s*((\[|"|`)([\w\s]+)?(\]|"|`)|\w+))*(\s*\.\s*((\[|"|`)([\w\s]+)?(\]|"|`)|\w+))*)(\s*(as|)\s*(\[|"|`|)([\w\s]+)?(\]|"|`|))
                    if (String.IsNullOrEmpty(tableName))
                    	tableName = Regex.Match(fromClause, "^(?\'Table\'((\\[|\"|`)([\\w\\s]+)?(\\]|\"|`)|\\w+)(\\s*\\.\\s*((\\[|\"|`)([\\w\\s]+)?(\\]|\"|`)|\\w" +
                                "+))*(\\s*\\.\\s*((\\[|\"|`)([\\w\\s]+)?(\\]|\"|`)|\\w+))*)(\\s*(as|)\\s*(\\[|\"|`|)([\\w\\s]+)?(" +
                                "\\]|\"|`|))", RegexOptions.IgnoreCase).Groups["Table"].Value;
                    if (commandConfiguration == CommandConfigurationType.Update)
                    	return ConfigureCommandForUpdate(command, page, expressions, tableName, values);
                    else
                    	if (commandConfiguration == CommandConfigurationType.Insert)
                        	return ConfigureCommandForInsert(command, page, expressions, tableName, values);
                        else
                        	if (commandConfiguration == CommandConfigurationType.Delete)
                            	return ConfigureCommandForDelete(command, page, expressions, tableName, values);
                            else
                            	ConfigureCommandForSelect(command, page, expressions, fromClause, whereClause, orderByClause, commandConfiguration);
                }
                else
                	if ((commandConfiguration == CommandConfigurationType.Select) && YieldsSingleRow(command))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("select ");
                        AppendSelectExpressions(sb, page, expressions, true);
                        command.CommandText = sb.ToString();
                    }
                return commandConfiguration != CommandConfigurationType.None;
            }
            return (command.CommandType == CommandType.StoredProcedure);
        }
        
        private void AddComputedExpressions(SelectClauseDictionary expressions, ViewPage page)
        {
            foreach (DataField field in page.Fields)
            	if (!(String.IsNullOrEmpty(field.Formula)))
                	expressions[field.ExpressionName()] = String.Format("({0})", field.Formula);
        }
        
        private void AssignPivotParameterValues(DbCommand command, ViewPage page)
        {
            foreach (DbParameter p in command.Parameters)
            {
                Match pivotMatch = Regex.Match(p.ParameterName, "^\\WPivotCol(\\d+)_(\\w+)$");
                if (pivotMatch.Success)
                {
                    DataTable pivotColumns = page.EnumeratePivotColumns();
                    int rowIndex = Convert.ToInt32(pivotMatch.Groups[1].Value);
                    string columnFieldName = pivotMatch.Groups[2].Value;
                    if (rowIndex < pivotColumns.Rows.Count)
                    	p.Value = pivotColumns.Rows[rowIndex][columnFieldName];
                }
            }
        }
        
        private bool ConfigureCommandForDelete(DbCommand command, ViewPage page, SelectClauseDictionary expressions, string tableName, FieldValue[] values)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("delete from {0}", tableName);
            AppendWhereExpressions(sb, command, page, expressions, values);
            command.CommandText = sb.ToString();
            return true;
        }
        
        private bool ConfigureCommandForInsert(DbCommand command, ViewPage page, SelectClauseDictionary expressions, string tableName, FieldValue[] values)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("insert into {0} (", tableName);
            bool firstField = true;
            bool hasPivotColumnKey = false;
            bool hasPivotRowKey = false;
            foreach (FieldValue v in values)
            {
                DataField field = page.FindField(v.Name);
                if ((field != null) && (v.Modified || ((field.Pivot == "ColumnKey") || (field.Pivot == "RowKey"))))
                {
                    sb.AppendLine();
                    if (firstField)
                    	firstField = false;
                    else
                    	sb.Append(",");
                    sb.AppendFormat(RemoveTableAliasFromExpression(expressions[v.Name]));
                    if (field.Pivot == "ColumnKey")
                    	hasPivotColumnKey = (v.Value != null);
                    if (field.Pivot == "RowKey")
                    	hasPivotRowKey = (v.Value != null);
                }
            }
            if (firstField)
            	return false;
            if (hasPivotColumnKey != hasPivotRowKey)
            	return false;
            sb.AppendLine(")");
            sb.AppendLine("values(");
            firstField = true;
            foreach (FieldValue v in values)
            {
                DataField field = page.FindField(v.Name);
                if ((field != null) && (v.Modified || ((field.Pivot == "ColumnKey") || (field.Pivot == "RowKey"))))
                {
                    sb.AppendLine();
                    if (firstField)
                    	firstField = false;
                    else
                    	sb.Append(",");
                    if ((v.NewValue == null) && field.HasDefaultValue)
                    	sb.Append(field.DefaultValue);
                    else
                    {
                        sb.AppendFormat("{0}p{1}", _parameterMarker, command.Parameters.Count);
                        DbParameter parameter = command.CreateParameter();
                        parameter.ParameterName = String.Format("{0}p{1}", _parameterMarker, command.Parameters.Count);
                        AssignParameterValue(parameter, field.Type, v.NewValue);
                        command.Parameters.Add(parameter);
                    }
                }
            }
            sb.AppendLine(")");
            command.CommandText = sb.ToString();
            return true;
        }
        
        private string RemoveTableAliasFromExpression(string expression)
        {
            // alias extraction regular expression:
            // "[\w\s]+".("[\w\s]+")
            Match m = Regex.Match(expression, "\"[\\w\\s]+\".(\"[\\w\\s]+\")");
            if (m.Success)
            	return m.Groups[1].Value;
            return expression;
        }
        
        private bool ConfigureCommandForUpdate(DbCommand command, ViewPage page, SelectClauseDictionary expressions, string tableName, FieldValue[] values)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("update {0} set ", tableName);
            bool firstField = true;
            foreach (FieldValue v in values)
            {
                DataField field = page.FindField(v.Name);
                if ((field != null) && v.Modified)
                {
                    sb.AppendLine();
                    if (firstField)
                    	firstField = false;
                    else
                    	sb.Append(",");
                    sb.AppendFormat(RemoveTableAliasFromExpression(expressions[v.Name]));
                    if ((v.NewValue == null) && field.HasDefaultValue)
                    	sb.Append(String.Format("={0}", field.DefaultValue));
                    else
                    {
                        sb.AppendFormat("={0}p{1}", _parameterMarker, command.Parameters.Count);
                        DbParameter parameter = command.CreateParameter();
                        parameter.ParameterName = String.Format("{0}p{1}", _parameterMarker, command.Parameters.Count);
                        AssignParameterValue(parameter, field.Type, v.NewValue);
                        command.Parameters.Add(parameter);
                    }
                }
            }
            if (firstField)
            	return false;
            AppendWhereExpressions(sb, command, page, expressions, values);
            command.CommandText = sb.ToString();
            return true;
        }
        
        private void AppendWhereExpressions(StringBuilder sb, DbCommand command, ViewPage page, SelectClauseDictionary expressions, FieldValue[] values)
        {
            sb.AppendLine();
            sb.Append("where");
            bool firstField = true;
            foreach (FieldValue v in values)
            {
                DataField field = page.FindField(v.Name);
                if ((field != null) && field.IsPrimaryKey)
                {
                    sb.AppendLine();
                    if (firstField)
                    	firstField = false;
                    else
                    	sb.Append("and ");
                    sb.AppendFormat(RemoveTableAliasFromExpression(expressions[v.Name]));
                    sb.AppendFormat("={0}p{1}", _parameterMarker, command.Parameters.Count);
                    DbParameter parameter = command.CreateParameter();
                    parameter.ParameterName = String.Format("{0}p{1}", _parameterMarker, command.Parameters.Count);
                    AssignParameterValue(parameter, field.Type, v.OldValue);
                    command.Parameters.Add(parameter);
                }
            }
            if (_config.ConflictDetectionEnabled)
            	foreach (FieldValue v in values)
                {
                    DataField field = page.FindField(v.Name);
                    if ((field != null) && (!((field.IsPrimaryKey || field.OnDemand)) && !(v.ReadOnly)))
                    {
                        sb.AppendLine();
                        sb.Append("and ");
                        sb.Append(RemoveTableAliasFromExpression(expressions[v.Name]));
                        if (v.OldValue == null)
                        	sb.Append(" is null");
                        else
                        {
                            sb.AppendFormat("={0}p{1}", _parameterMarker, command.Parameters.Count);
                            DbParameter parameter = command.CreateParameter();
                            parameter.ParameterName = String.Format("{0}p{1}", _parameterMarker, command.Parameters.Count);
                            AssignParameterValue(parameter, field.Type, v.OldValue);
                            command.Parameters.Add(parameter);
                        }
                    }
                }
            sb.AppendLine();
        }
        
        private void EnsureWhereKeyword(StringBuilder sb)
        {
            if (!(_hasWhere))
            {
                _hasWhere = true;
                sb.AppendLine("where");
            }
        }
        
        private void ConfigureCommandForSelect(DbCommand command, ViewPage page, SelectClauseDictionary expressions, string fromClause, string whereClause, string orderByClause, CommandConfigurationType commandConfiguration)
        {
            bool useServerPaging = (commandConfiguration != CommandConfigurationType.SelectDistinct && commandConfiguration != CommandConfigurationType.SelectAggregates);
            bool useLimit = command.ToString().Contains("MySql");
            if (useServerPaging)
            	page.AcceptAllRows();
            StringBuilder sb = new StringBuilder();
            if (useLimit)
            	useServerPaging = false;
            if (commandConfiguration == CommandConfigurationType.SelectCount)
            	sb.AppendLine("select count(*)");
            else
            {
                if (useServerPaging)
                	sb.AppendLine("with page_cte__ as (");
                sb.AppendLine("select");
                if (useServerPaging)
                	AppendRowNumberExpression(sb, page, expressions, orderByClause);
                if (commandConfiguration == CommandConfigurationType.SelectDistinct)
                {
                    DataField distinctField = page.FindField(page.DistinctValueFieldName);
                    string distinctExpression = expressions[distinctField.ExpressionName()];
                    if (distinctField.Type.StartsWith("Date"))
                    {
                        string commandType = command.GetType().ToString();
                        if (commandType == "System.Data.SqlClient.SqlCommand")
                        	distinctExpression = String.Format("DATEADD(dd, 0, DATEDIFF(dd, 0, {0}))", distinctExpression);
                        if (commandType == "MySql.Data.MySqlClient.MySqlCommand")
                        	distinctExpression = String.Format("cast({0} as date)", distinctExpression);
                    }
                    sb.AppendFormat("distinct {0} \"{1}\"\r\n", distinctExpression, page.DistinctValueFieldName);
                }
                else
                	if (commandConfiguration == CommandConfigurationType.SelectAggregates)
                    	AppendAggregateExpressions(sb, page, expressions);
                    else
                    	AppendSelectExpressions(sb, page, expressions, !(useServerPaging));
            }
            sb.AppendLine("from");
            sb.AppendLine(fromClause);
            _hasWhere = false;
            if (String.IsNullOrEmpty(_viewFilter))
            	_viewFilter = _view.GetAttribute("filter", String.Empty);
            if (!(String.IsNullOrEmpty(_viewFilter)))
            	_viewFilter = String.Format("({0})", _viewFilter);
            AppendRecursiveFilter(page);
            if (((page.Filter != null) && (page.Filter.Length > 0)) || !(String.IsNullOrEmpty(_viewFilter)))
            	AppendFilterExpressionsToWhere(sb, page, command, expressions, whereClause);
            else
            	if (!(String.IsNullOrEmpty(whereClause)))
                {
                    EnsureWhereKeyword(sb);
                    sb.AppendLine(whereClause);
                }
            if (commandConfiguration == CommandConfigurationType.Select)
            	if (useServerPaging)
                {
                    sb.AppendFormat(")\r\nselect * from page_cte__ where row_number__ > {0}PageRangeFirstRowNumber and r" +
                            "ow_number__ <= {0}PageRangeLastRowNumber", _parameterMarker);
                    DbParameter p = command.CreateParameter();
                    p.ParameterName = (_parameterMarker + "PageRangeFirstRowNumber");
                    p.Value = ((page.PageSize * page.PageIndex) 
                                + page.PageOffset);
                    command.Parameters.Add(p);
                    DbParameter p2 = command.CreateParameter();
                    p2.ParameterName = (_parameterMarker + "PageRangeLastRowNumber");
                    p2.Value = ((page.PageSize 
                                * (page.PageIndex + 1)) 
                                + page.PageOffset);
                    command.Parameters.Add(p2);
                }
                else
                {
                    AppendOrderByExpression(sb, page, expressions, orderByClause);
                    if (useLimit)
                    {
                        sb.AppendFormat("\r\nlimit {0}Limit_PageOffset, {0}Limit_PageSize", _parameterMarker);
                        DbParameter p = command.CreateParameter();
                        p.ParameterName = (_parameterMarker + "Limit_PageOffset");
                        p.Value = ((page.PageSize * page.PageIndex) 
                                    + page.PageOffset);
                        command.Parameters.Add(p);
                        DbParameter p2 = command.CreateParameter();
                        p2.ParameterName = (_parameterMarker + "Limit_PageSize");
                        p2.Value = page.PageSize;
                        command.Parameters.Add(p2);
                    }
                }
            else
            	if (commandConfiguration == CommandConfigurationType.SelectDistinct)
                	sb.Append("order by 1");
            command.CommandText = sb.ToString();
        }
        
        private bool FilterIsApplicable(ViewPage page)
        {
            if ((page.Filter == null) || (page.Filter.Length == 0))
            	return false;
            foreach (string expression in page.Filter)
            {
                Match m = Regex.Match(expression, "(\\w+):");
                if (m.Success && (page.FindField(m.Groups[1].Value) != null))
                	return true;
                return false;
            }
            return false;
        }
        
        private string ProcessViewFilter(ViewPage page, DbCommand command, SelectClauseDictionary expressions)
        {
            _currentCommand = command;
            _currentExpressions = expressions;
            string filter = Regex.Replace(_viewFilter, "(?\'Parameter\'(@|:)\\w+)|(?\'Other\'(\"|\'|\\[|`)\\s*\\w+)|(?\'Function\'\\$\\w+\\s*\\((?\'Argume" +
                    "nts\'[\\S\\s]*?)\\))|(?\'Name\'\\w+)", DoReplaceKnownNames);
            return filter;
        }
        
        private string DoReplaceKnownNames(Match m)
        {
            if (!(String.IsNullOrEmpty(m.Groups["Other"].Value)))
            	return m.Value;
            if (!(String.IsNullOrEmpty(m.Groups["Parameter"].Value)))
            	return AssignFilterParameterValue(m.Groups["Parameter"].Value);
            else
            	if (!(String.IsNullOrEmpty(m.Groups["Function"].Value)))
                	return FilterFunctions.Replace(_currentCommand, _currentExpressions, m.Groups["Function"].Value);
                else
                {
                    string s = null;
                    if (_currentExpressions.TryGetValue(m.Groups["Name"].Value, out s))
                    	return s;
                }
            return m.Value;
        }
        
        private string AssignFilterParameterValue(string qualifiedName)
        {
            char prefix = qualifiedName[0];
            string name = qualifiedName.Substring(1);
            if ((prefix.Equals('@') || prefix.Equals(':')) && !(_currentCommand.Parameters.Contains(qualifiedName)))
            {
                object result = null;
                if ((_parameters != null) && _parameters.ContainsKey(qualifiedName))
                	result = _parameters[qualifiedName];
                else
                {
                    IActionHandler handler = _config.CreateActionHandler();
                    if (handler == null)
                    	throw new Exception(String.Format("Filter \"{1}\" of view \"{0}\" requires the business rules class of the controller to" +
                                    " define property \"{2}\".", _viewId, _viewFilter, name));
                    result = handler.GetType().InvokeMember(name, (System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.GetField), null, handler, new object[0]);
                }
                IEnumerable<object> enumerable = null;
                if (typeof(IEnumerable<object>).IsInstanceOfType(result))
                	enumerable = ((IEnumerable<object>)(result));
                if (enumerable != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("(");
                    int parameterIndex = 0;
                    foreach (object o in enumerable)
                    {
                        DbParameter p = _currentCommand.CreateParameter();
                        _currentCommand.Parameters.Add(p);
                        p.ParameterName = (qualifiedName + parameterIndex.ToString());
                        p.Value = o;
                        if (parameterIndex > 0)
                        	sb.Append(",");
                        sb.Append(p.ParameterName);
                        parameterIndex++;
                    }
                    sb.Append(")");
                    return sb.ToString();
                }
                else
                {
                    DbParameter p = _currentCommand.CreateParameter();
                    _currentCommand.Parameters.Add(p);
                    p.ParameterName = qualifiedName;
                    p.Value = result;
                }
            }
            return qualifiedName;
        }
        
        protected virtual void AppendFilterExpressionsToWhere(StringBuilder sb, ViewPage page, DbCommand command, SelectClauseDictionary expressions, string whereClause)
        {
            bool firstCriteria = String.IsNullOrEmpty(_viewFilter);
            if (!(firstCriteria))
            {
                EnsureWhereKeyword(sb);
                sb.AppendLine("(");
                sb.Append(ProcessViewFilter(page, command, expressions));
            }
            if (page.Filter != null)
            	foreach (string filterExpression in page.Filter)
                {
                    Match filterMatch = Regex.Match(filterExpression, "(?\'Alias\'\\w+):(?\'Values\'[\\s\\S]*)");
                    if (filterMatch.Success)
                    {
                        // "ProductName:?g", "CategoryCategoryName:=Condiments\x00=Seafood"
                        bool firstValue = true;
                        string fieldOperator = " or ";
                        if (Regex.IsMatch(filterMatch.Groups["Values"].Value, ">|<"))
                        	fieldOperator = " and ";
                        Match valueMatch = Regex.Match(filterMatch.Groups["Values"].Value, "(?\'Operation\'\\*|\\$\\w+\\$|=|~|<(=|>){0,1}|>={0,1})(?\'Value\'[\\s\\S]*?)(\\0|$)");
                        while (valueMatch.Success)
                        {
                            string alias = filterMatch.Groups["Alias"].Value;
                            string operation = valueMatch.Groups["Operation"].Value;
                            string paramValue = valueMatch.Groups["Value"].Value;
                            DataField field = page.FindField(alias);
                            if (((field != null) && field.AllowQBE) && (page.DistinctValueFieldName != field.Name || (page.AllowDistinctFieldInFilter || page.CustomFilteredBy(field.Name))))
                            {
                                if (firstValue)
                                {
                                    if (firstCriteria)
                                    {
                                        EnsureWhereKeyword(sb);
                                        sb.AppendLine("(");
                                        firstCriteria = false;
                                    }
                                    else
                                    	sb.Append("and ");
                                    sb.Append("(");
                                    firstValue = false;
                                }
                                else
                                	sb.Append(fieldOperator);
                                if (operation == "~")
                                {
                                    bool firstWord = true;
                                    paramValue = Convert.ToString(StringToValue(paramValue));
                                    foreach (string paramValueWord in paramValue.Split(' '))
                                    {
                                        string pv = paramValueWord.Trim();
                                        DateTime paramValueAsDate;
                                        bool paramValueIsDate = DateTime.TryParse(pv, out paramValueAsDate);
                                        bool firstTry = true;
                                        DbParameter parameter = command.CreateParameter();
                                        parameter.ParameterName = String.Format("{0}p{1}", _parameterMarker, command.Parameters.Count);
                                        parameter.DbType = DbType.String;
                                        command.Parameters.Add(parameter);
                                        if (!(pv.Contains("%")))
                                        	pv = String.Format("%{0}%", pv);
                                        parameter.Value = pv;
                                        if (firstWord)
                                        	firstWord = false;
                                        else
                                        	sb.Append("and");
                                        sb.Append("(");
                                        foreach (DataField tf in page.Fields)
                                        	if (tf.AllowQBE && (!((tf.IsPrimaryKey && tf.Hidden)) && (!(tf.Type.StartsWith("Date")) || paramValueIsDate)))
                                            {
                                                if (firstTry)
                                                	firstTry = false;
                                                else
                                                	sb.Append(" or ");
                                                if (tf.Type.StartsWith("Date"))
                                                {
                                                    DbParameter dateParameter = command.CreateParameter();
                                                    dateParameter.ParameterName = String.Format("{0}p{1}", _parameterMarker, command.Parameters.Count);
                                                    dateParameter.DbType = DbType.String;
                                                    command.Parameters.Add(dateParameter);
                                                    dateParameter.Value = paramValueAsDate;
                                                    sb.AppendFormat("({0} = {1})", expressions[tf.ExpressionName()], dateParameter.ParameterName);
                                                }
                                                else
                                                	sb.AppendFormat("({0} like {1})", expressions[tf.ExpressionName()], parameter.ParameterName);
                                            }
                                        sb.Append(")");
                                    }
                                }
                                else
                                	if (operation.StartsWith("$"))
                                    	sb.Append(FilterFunctions.Replace(command, expressions, String.Format("{0}({1}$comma${2})", operation.TrimEnd('$'), alias, Convert.ToBase64String(Encoding.Default.GetBytes(paramValue)))));
                                    else
                                    {
                                        DbParameter parameter = command.CreateParameter();
                                        parameter.ParameterName = String.Format("{0}p{1}", _parameterMarker, command.Parameters.Count);
                                        AssignParameterDbType(parameter, field.Type);
                                        sb.Append(expressions[field.ExpressionName()]);
                                        bool requiresRangeAdjustment = ((operation == "=") && (field.Type.StartsWith("DateTime") && !(StringIsNull(paramValue))));
                                        if ((operation == "=") && StringIsNull(paramValue))
                                        	sb.Append(" is null ");
                                        else
                                        {
                                            if (operation == "*")
                                            {
                                                sb.Append(" like ");
                                                parameter.DbType = DbType.String;
                                                if (!(paramValue.Contains("%")))
                                                	paramValue = (paramValue + "%");
                                            }
                                            else
                                            	if (requiresRangeAdjustment)
                                                	sb.Append(">=");
                                                else
                                                	sb.Append(operation);
                                            try
                                            {
                                                parameter.Value = StringToValue(field, paramValue);
                                            }
                                            catch (Exception )
                                            {
                                                parameter.Value = DBNull.Value;
                                            }
                                            sb.Append(parameter.ParameterName);
                                            command.Parameters.Add(parameter);
                                            if (requiresRangeAdjustment)
                                            {
                                                DbParameter rangeParameter = command.CreateParameter();
                                                AssignParameterDbType(rangeParameter, field.Type);
                                                rangeParameter.ParameterName = String.Format("{0}p{1}", _parameterMarker, command.Parameters.Count);
                                                sb.Append(String.Format(" and {0} < {1}", expressions[field.ExpressionName()], rangeParameter.ParameterName));
                                                if (field.Type == "DateTimeOffset")
                                                {
                                                    DateTime d = Convert.ToDateTime(paramValue);
                                                    DateTimeOffset o = new DateTimeOffset(d, TimeSpan.FromHours(14));
                                                    parameter.Value = o;
                                                    o = new DateTimeOffset(d, TimeSpan.FromHours(-14));
                                                    rangeParameter.Value = o;
                                                }
                                                else
                                                	rangeParameter.Value = Convert.ToDateTime(parameter.Value).AddDays(1);
                                                command.Parameters.Add(rangeParameter);
                                            }
                                        }
                                    }
                            }
                            valueMatch = valueMatch.NextMatch();
                        }
                        if (!(firstValue))
                        	sb.AppendLine(")");
                    }
                }
            if (!(firstCriteria))
            {
                sb.AppendLine(")");
                if (!(String.IsNullOrEmpty(whereClause)))
                	sb.Append("and ");
            }
            if (!(String.IsNullOrEmpty(whereClause)))
            {
                sb.AppendLine("(");
                sb.AppendLine(whereClause);
                sb.AppendLine(")");
            }
        }
        
        protected virtual void AppendRecursiveFilter(ViewPage page)
        {
            if (this._viewType != "Tree")
            	return;
            DataField recursiveField = null;
            if (page.Filter != null)
            	foreach (string filterExpression in page.Filter)
                {
                    Match filterMatch = Regex.Match(filterExpression, "(?\'Alias\'\\w+):");
                    if (filterMatch.Success)
                    {
                        DataField f = page.FindField(filterMatch.Groups["Alias"].Value);
                        if ((f != null) && (f.ItemsDataController == page.Controller))
                        {
                            recursiveField = f;
                            break;
                        }
                    }
                }
                if (recursiveField == null)
                	foreach (DataField f in page.Fields)
                    	if (f.ItemsDataController == page.Controller)
                        {
                            if (!(String.IsNullOrEmpty(_viewFilter)))
                            	_viewFilter = String.Format("({0})and", _viewFilter);
                            _viewFilter = String.Format("{0}({1} is null)", _viewFilter, f.Name);
                            break;
                        }
        }
        
        public static bool StringIsNull(string s)
        {
            return ((s == "null") || (s == "%js%null"));
        }
        
        public static string ValueToString(object o)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
#pragma warning restore 0618
            return ("%js%" + serializer.Serialize(o));
        }
        
        public static object StringToValue(string s)
        {
            return StringToValue(null, s);
        }
        
        public static object StringToValue(DataField field, string s)
        {
            if (!(String.IsNullOrEmpty(s)) && s.StartsWith("%js%"))
            {
#pragma warning disable 0618
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
#pragma warning restore 0618
                object v = serializer.Deserialize<object>(s.Substring(4));
                if (!((v is string)) || ((field == null) || (field.Type == "String")))
                	return v;
                s = ((string)(v));
            }
            if (field != null)
            	return TypeDescriptor.GetConverter(Controller.TypeMap[field.Type]).ConvertFromString(s);
            return s;
        }
        
        private void AssignParameterDbType(DbParameter parameter, string systemType)
        {
            if (systemType == "SByte")
            	parameter.DbType = DbType.Int16;
            else
            	if (systemType == "TimeSpan")
                	parameter.DbType = DbType.String;
                else
                	parameter.DbType = ((DbType)(TypeDescriptor.GetConverter(typeof(DbType)).ConvertFrom(systemType)));
        }
        
        public void AssignParameterValue(DbParameter parameter, string systemType, object v)
        {
            AssignParameterDbType(parameter, systemType);
            if (v == null)
            	parameter.Value = DBNull.Value;
            else
            	if (v is string)
                	parameter.Value = TypeDescriptor.GetConverter(Controller.TypeMap[systemType]).ConvertFromString(((string)(v)));
                else
                	parameter.Value = v;
        }
        
        private void AppendSelectExpressions(StringBuilder sb, ViewPage page, SelectClauseDictionary expressions, bool firstField)
        {
            foreach (DataField field in page.Fields)
            {
                if (firstField)
                	firstField = false;
                else
                	sb.Append(",");
                try
                {
                    if (field.OnDemand)
                    	sb.Append(String.Format("case when {0} is not null then 1 else null end as ", expressions[field.ExpressionName()]));
                    else
                    	sb.Append(expressions[field.ExpressionName()]);
                }
                catch (Exception )
                {
                    throw new Exception(String.Format("Unknown data field \'{0}\'.", field.Name));
                }
                sb.Append(" \"");
                sb.Append(field.Name);
                sb.AppendLine("\"");
            }
        }
        
        void AppendAggregateExpressions(StringBuilder sb, ViewPage page, SelectClauseDictionary expressions)
        {
            bool firstField = true;
            foreach (DataField field in page.Fields)
            {
                if (firstField)
                	firstField = false;
                else
                	sb.Append(",");
                if (field.Aggregate == DataFieldAggregate.None)
                	sb.Append("null ");
                else
                {
                    string functionName = field.Aggregate.ToString();
                    if (functionName == "Average")
                    	functionName = "Avg";
                    string fmt = "{0}({1})";
                    if (functionName == "Count")
                    	fmt = "{0}(distinct {1})";
                    sb.AppendFormat(fmt, functionName, expressions[field.ExpressionName()]);
                }
                sb.Append(" \"");
                sb.Append(field.Name);
                sb.AppendLine("\"");
            }
        }
        
        private void AppendRowNumberExpression(StringBuilder sb, ViewPage page, SelectClauseDictionary expressions, string orderByClause)
        {
            sb.Append("row_number() over (");
            AppendOrderByExpression(sb, page, expressions, orderByClause);
            sb.AppendLine(") as row_number__");
        }
        
        private void AppendOrderByExpression(StringBuilder sb, ViewPage page, SelectClauseDictionary expressions, string orderByClause)
        {
            if (String.IsNullOrEmpty(page.SortExpression))
            	page.SortExpression = _view.GetAttribute("sortExpression", String.Empty);
            sb.Append("order by ");
            if (String.IsNullOrEmpty(page.SortExpression))
            	if (!(String.IsNullOrEmpty(orderByClause)))
                	sb.Append(orderByClause);
                else
                {
                    bool firstKey = true;
                    foreach (DataField field in page.Fields)
                    	if (field.IsPrimaryKey)
                        {
                            if (firstKey)
                            	firstKey = false;
                            else
                            	sb.Append(",");
                            sb.Append(expressions[field.ExpressionName()]);
                        }
                    if (firstKey)
                    	sb.Append(expressions[page.Fields[0].ExpressionName()]);
                }
            else
            {
                bool firstSortField = true;
                Match orderByMatch = Regex.Match(page.SortExpression, "\\s*(?\'Alias\'[\\s\\w]+?)\\s*(?\'Order\'\\s(ASC|DESC))?\\s*(,|$)", RegexOptions.IgnoreCase);
                while (orderByMatch.Success)
                {
                    if (firstSortField)
                    	firstSortField = false;
                    else
                    	sb.Append(",");
                    sb.Append(expressions[orderByMatch.Groups["Alias"].Value]);
                    sb.Append(" ");
                    sb.Append(orderByMatch.Groups["Order"].Value);
                    orderByMatch = orderByMatch.NextMatch();
                }
            }
        }
        
        private void EnsurePageFields(ViewPage page, SelectClauseDictionary expressions)
        {
            if (page.Fields.Count == 0)
            {
                XPathNodeIterator fieldIterator = _config.Navigator.Select("/c:dataController/c:fields/c:field[not(@hidden=\'true\')]", Resolver);
                while (fieldIterator.MoveNext())
                {
                    string fieldName = fieldIterator.Current.GetAttribute("name", String.Empty);
                    if (expressions.ContainsKey(fieldName))
                    	page.Fields.Add(new DataField(fieldIterator.Current, Resolver));
                }
            }
            XPathNodeIterator keyFieldIterator = _config.Navigator.Select("/c:dataController/c:fields/c:field[@isPrimaryKey=\'true\']", Resolver);
            while (keyFieldIterator.MoveNext())
            {
                string fieldName = keyFieldIterator.Current.GetAttribute("name", String.Empty);
                if (page.FindField(fieldName) == null)
                	page.Fields.Add(new DataField(keyFieldIterator.Current, Resolver, true));
            }
            XPathNodeIterator aliasIterator = _view.Select(".//c:dataFields/c:dataField/@aliasFieldName", Resolver);
            while (aliasIterator.MoveNext())
            	if (page.FindField(aliasIterator.Current.Value) == null)
                {
                    XPathNodeIterator fieldIterator = _config.Navigator.Select(String.Format("/c:dataController/c:fields/c:field[@name=\'{0}\']", aliasIterator.Current.Value), Resolver);
                    if (fieldIterator.MoveNext())
                    	page.Fields.Add(new DataField(fieldIterator.Current, Resolver, true));
                }
            XPathNodeIterator lookupFieldIterator = _config.Navigator.Select("/c:dataController/c:fields/c:field[c:items/@dataController]", Resolver);
            while (lookupFieldIterator.MoveNext())
            {
                string fieldName = lookupFieldIterator.Current.GetAttribute("name", String.Empty);
                if (page.FindField(fieldName) == null)
                	page.Fields.Add(new DataField(lookupFieldIterator.Current, Resolver, true));
            }
            int i = 0;
            while (i < page.Fields.Count)
            {
                DataField field = page.Fields[i];
                if ((!(field.FormatOnClient) && !(String.IsNullOrEmpty(field.DataFormatString))) && !(field.IsMirror))
                {
                    page.Fields.Insert((i + 1), new DataField(field));
                    i = (i + 2);
                }
                else
                	i++;
            }
            XPathNodeIterator dynamicConfigIterator = _config.Navigator.Select("/c:dataController/c:fields/c:field[c:configuration!=\'\']/c:configuration|/c:dataCo" +
                    "ntroller/c:fields/c:field/c:items[@copy!=\'\']/@copy", Resolver);
            while (dynamicConfigIterator.MoveNext())
            {
                Match dynamicConfig = Regex.Match(dynamicConfigIterator.Current.Value, "(\\w+)=(\\w+)");
                while (dynamicConfig.Success)
                {
                    int groupIndex = 2;
                    if (dynamicConfigIterator.Current.Name == "copy")
                    	groupIndex = 1;
                    if (page.FindField(dynamicConfig.Groups[groupIndex].Value) == null)
                    {
                        XPathNavigator nav = _config.Navigator.SelectSingleNode(String.Format("/c:dataController/c:fields/c:field[@name=\'{0}\']", dynamicConfig.Groups[1].Value), Resolver);
                        if (nav != null)
                        	page.Fields.Add(new DataField(nav, Resolver, true));
                    }
                    dynamicConfig = dynamicConfig.NextMatch();
                }
                if (page.InTransaction)
                {
                    XPathNodeIterator globalFieldIterator = _config.Navigator.Select("/c:dataController/c:fields/c:field", Resolver);
                    while (globalFieldIterator.MoveNext())
                    {
                        string fieldName = globalFieldIterator.Current.GetAttribute("name", String.Empty);
                        if (page.FindField(fieldName) == null)
                        	page.Fields.Add(new DataField(globalFieldIterator.Current, Resolver, true));
                    }
                }
            }
            if (!(ViewOverridingDisabled))
            {
                XPathNodeIterator pivotRowFields = _view.Select(".//c:dataFields/c:dataField[@pivot=\'RowKey\']", Resolver);
                bool resetPrimaryKeys = true;
                while (pivotRowFields.MoveNext())
                {
                    if (resetPrimaryKeys)
                    {
                        resetPrimaryKeys = false;
                        foreach (DataField field in page.Fields)
                        	field.IsPrimaryKey = false;
                    }
                    DataField pivotRowField = page.FindField(pivotRowFields.Current.GetAttribute("fieldName", String.Empty));
                    pivotRowField.IsPrimaryKey = true;
                }
            }
        }
        
        private SelectClauseDictionary ParseSelectExpressions(string selectClause)
        {
            SelectClauseDictionary expressions = new SelectClauseDictionary();
            Match fieldMatch = Regex.Match(selectClause, "\\s*(?\'Expression\'[\\S\\s]*?(\\([\\s\\S]*?\\)|(\\.((\"|\'|\\[|`)(?\'FieldName\'[\\S\\s]*?)(\"|\'|\\" +
                    "]|`))|(\"|\'|\\[|`|)(?\'FieldName\'[\\w\\s]*?)(\"|\'|\\]|)|)))((\\s+as\\s+|\\s+)(\"|\'|\\[|`|)(?" +
                    "\'Alias\'[\\S\\s]*?)|)(\"|\'|\\]|`|)\\s*(,|$)", RegexOptions.IgnoreCase);
            while (fieldMatch.Success)
            {
                string expression = fieldMatch.Groups["Expression"].Value;
                string fieldName = fieldMatch.Groups["FieldName"].Value;
                string alias = fieldMatch.Groups["Alias"].Value;
                if (!(String.IsNullOrEmpty(expression)))
                {
                    if (String.IsNullOrEmpty(alias))
                    	if (String.IsNullOrEmpty(fieldName))
                        	alias = expression;
                        else
                        	alias = fieldName;
                    if (!(expressions.ContainsKey(alias)))
                    	expressions.Add(alias, expression);
                }
                fieldMatch = fieldMatch.NextMatch();
            }
            return expressions;
        }
        
        protected void PopulatePageFields(ViewPage page)
        {
            if (page.Fields.Count > 0)
            	return;
            XPathNodeIterator dataFieldIterator = _view.Select(".//c:dataFields/c:dataField", Resolver);
            while (dataFieldIterator.MoveNext())
            {
                XPathNodeIterator fieldIterator = _config.Navigator.Select(String.Format("/c:dataController/c:fields/c:field[@name=\'{0}\']", dataFieldIterator.Current.GetAttribute("fieldName", String.Empty)), Resolver);
                if (fieldIterator.MoveNext())
                {
                    DataField field = new DataField(fieldIterator.Current, Resolver);
                    field.Hidden = (dataFieldIterator.Current.GetAttribute("hidden", String.Empty) == "true");
                    field.DataFormatString = fieldIterator.Current.GetAttribute("dataFormatString", String.Empty);
                    string formatOnClient = dataFieldIterator.Current.GetAttribute("formatOnClient", String.Empty);
                    if (!(String.IsNullOrEmpty(formatOnClient)))
                    	field.FormatOnClient = formatOnClient != "false";
                    if (String.IsNullOrEmpty(field.DataFormatString))
                    	field.DataFormatString = dataFieldIterator.Current.GetAttribute("dataFormatString", String.Empty);
                    field.HeaderText = ((string)(dataFieldIterator.Current.Evaluate("string(c:headerText)", Resolver)));
                    field.FooterText = ((string)(dataFieldIterator.Current.Evaluate("string(c:footerText)", Resolver)));
                    field.HyperlinkFormatString = dataFieldIterator.Current.GetAttribute("hyperlinkFormatString", String.Empty);
                    field.AliasName = dataFieldIterator.Current.GetAttribute("aliasFieldName", String.Empty);
                    if (!(String.IsNullOrEmpty(dataFieldIterator.Current.GetAttribute("allowQBE", String.Empty))))
                    	field.AllowQBE = (dataFieldIterator.Current.GetAttribute("allowQBE", String.Empty) == "true");
                    if (!(String.IsNullOrEmpty(dataFieldIterator.Current.GetAttribute("allowSorting", String.Empty))))
                    	field.AllowSorting = (dataFieldIterator.Current.GetAttribute("allowSorting", String.Empty) == "true");
                    field.CategoryIndex = Convert.ToInt32(dataFieldIterator.Current.Evaluate("count(parent::c:dataFields/parent::c:category/preceding-sibling::c:category)", Resolver));
                    string columns = dataFieldIterator.Current.GetAttribute("columns", String.Empty);
                    if (!(String.IsNullOrEmpty(columns)))
                    	field.Columns = Convert.ToInt32(columns);
                    string rows = dataFieldIterator.Current.GetAttribute("rows", String.Empty);
                    if (!(String.IsNullOrEmpty(rows)))
                    	field.Rows = Convert.ToInt32(rows);
                    string textMode = dataFieldIterator.Current.GetAttribute("textMode", String.Empty);
                    if (!(String.IsNullOrEmpty(textMode)))
                    	field.TextMode = ((TextInputMode)(TypeDescriptor.GetConverter(typeof(TextInputMode)).ConvertFromString(textMode)));
                    string maskType = fieldIterator.Current.GetAttribute("maskType", String.Empty);
                    if (!(String.IsNullOrEmpty(maskType)))
                    	field.MaskType = ((DataFieldMaskType)(TypeDescriptor.GetConverter(typeof(DataFieldMaskType)).ConvertFromString(maskType)));
                    field.Mask = fieldIterator.Current.GetAttribute("mask", String.Empty);
                    string readOnly = dataFieldIterator.Current.GetAttribute("readOnly", String.Empty);
                    if (!(String.IsNullOrEmpty(readOnly)))
                    	field.ReadOnly = (readOnly == "true");
                    string aggregate = dataFieldIterator.Current.GetAttribute("aggregate", String.Empty);
                    if (!(String.IsNullOrEmpty(aggregate)))
                    	field.Aggregate = ((DataFieldAggregate)(TypeDescriptor.GetConverter(typeof(DataFieldAggregate)).ConvertFromString(aggregate)));
                    string search = dataFieldIterator.Current.GetAttribute("search", String.Empty);
                    if (!(String.IsNullOrEmpty(search)))
                    	field.Search = ((FieldSearchMode)(TypeDescriptor.GetConverter(typeof(FieldSearchMode)).ConvertFromString(search)));
                    field.Pivot = dataFieldIterator.Current.GetAttribute("pivot", String.Empty);
                    string prefixLength = dataFieldIterator.Current.GetAttribute("autoCompletePrefixLength", String.Empty);
                    if (!(String.IsNullOrEmpty(prefixLength)))
                    	field.AutoCompletePrefixLength = Convert.ToInt32(prefixLength);
                    XPathNodeIterator itemsIterator = dataFieldIterator.Current.Select("c:items[c:item]", Resolver);
                    if (!(itemsIterator.MoveNext()))
                    {
                        itemsIterator = fieldIterator.Current.Select("c:items", Resolver);
                        if (!(itemsIterator.MoveNext()))
                        	itemsIterator = null;
                    }
                    if (itemsIterator != null)
                    {
                        field.ItemsDataController = itemsIterator.Current.GetAttribute("dataController", String.Empty);
                        field.ItemsDataView = itemsIterator.Current.GetAttribute("dataView", String.Empty);
                        field.ItemsDataValueField = itemsIterator.Current.GetAttribute("dataValueField", String.Empty);
                        field.ItemsDataTextField = itemsIterator.Current.GetAttribute("dataTextField", String.Empty);
                        field.ItemsStyle = itemsIterator.Current.GetAttribute("style", String.Empty);
                        field.ItemsNewDataView = itemsIterator.Current.GetAttribute("newDataView", String.Empty);
                        field.Copy = itemsIterator.Current.GetAttribute("copy", String.Empty);
                        XPathNodeIterator itemIterator = itemsIterator.Current.Select("c:item", Resolver);
                        while (itemIterator.MoveNext())
                        	field.Items.Add(new object[] {
                                        itemIterator.Current.GetAttribute("value", String.Empty),
                                        itemIterator.Current.GetAttribute("text", String.Empty)});
                        if (!(String.IsNullOrEmpty(field.ItemsNewDataView)))
                        {
                            Controller itemsController = ((Controller)(this.GetType().Assembly.CreateInstance(this.GetType().FullName)));
                            itemsController.SelectView(field.ItemsDataController, field.ItemsNewDataView);
                            string roles = ((string)(itemsController._config.Navigator.Evaluate(String.Format("string(//c:action[@commandName=\'New\' and @commandArgument=\'{0}\'][1]/@roles)", field.ItemsNewDataView), Resolver)));
                            if (!(Controller.UserIsInRole(roles)))
                            	field.ItemsNewDataView = null;
                            field.AutoSelect = (itemsIterator.Current.GetAttribute("autoSelect", String.Empty) == "true");
                            field.SearchOnStart = (itemsIterator.Current.GetAttribute("searchOnStart", String.Empty) == "true");
                            field.ItemsDescription = itemsIterator.Current.GetAttribute("description", String.Empty);
                        }
                    }
                    if (!(Controller.UserIsInRole(fieldIterator.Current.GetAttribute("writeRoles", String.Empty))))
                    	field.ReadOnly = true;
                    if (!(Controller.UserIsInRole(fieldIterator.Current.GetAttribute("roles", String.Empty))))
                    {
                        field.ReadOnly = true;
                        field.Hidden = true;
                    }
                    page.Fields.Add(field);
                }
            }
        }
        
        public static string LookupText(string controllerName, string filterExpression, string fieldNames)
        {
            string[] dataTextFields = fieldNames.Split(',');
            PageRequest request = new PageRequest(-1, 1, null, new string[] {
                        filterExpression});
            ViewPage page = ControllerFactory.CreateDataController().GetPage(controllerName, String.Empty, request);
            string result = String.Empty;
            if (page.Rows.Count > 0)
            	for (int i = 0; (i < page.Fields.Count); i++)
                {
                    DataField field = page.Fields[i];
                    if (Array.IndexOf(dataTextFields, field.Name) >= 0)
                    {
                        if (result.Length > 0)
                        	result = (result + "; ");
                        result = (result + Convert.ToString(page.Rows[0][i]));
                    }
                }
            return result;
        }
        
        public static string ConvertSampleToQuery(string sample)
        {
            Match m = Regex.Match(sample, "^\\s*(?\'Operation\'(<|>)={0,1}){0,1}\\s*(?\'Value\'.+)\\s*$");
            if (!(m.Success))
            	return null;
            string operation = m.Groups["Operation"].Value;
            sample = m.Groups["Value"].Value.Trim();
            if (String.IsNullOrEmpty(operation))
            {
                operation = "*";
                double doubleTest;
                if (Double.TryParse(sample, out doubleTest))
                	operation = "=";
                else
                {
                    bool boolTest;
                    if (Boolean.TryParse(sample, out boolTest))
                    	operation = "=";
                    else
                    {
                        DateTime dateTest;
                        if (DateTime.TryParse(sample, out dateTest))
                        	operation = "=";
                    }
                }
            }
            return String.Format("{0}{1}{2}", operation, sample, Convert.ToChar(0));
        }
        
        public static string LookupActionArgument(string controllerName, string commandName)
        {
            Controller c = new Controller();
            c.SelectView(controllerName, null);
            XPathNavigator action = c._config.Navigator.SelectSingleNode(String.Format("//c:action[@commandName=\'{0}\' and contains(@commandArgument, \'Form\')]", commandName), c.Resolver);
            if (action == null)
            	return null;
            if (!(UserIsInRole(action.GetAttribute("roles", String.Empty))))
            	return null;
            return action.GetAttribute("commandArgument", String.Empty);
        }
        
        string[] IAutoCompleteManager.GetCompletionList(string prefixText, int count, string contextKey)
        {
            if (contextKey == null)
            	return null;
            string[] arguments = contextKey.Split(',');
            if (arguments.Length != 3)
            	return null;
            DistinctValueRequest request = new DistinctValueRequest();
            request.FieldName = arguments[2];
            string filter = (request.FieldName + ":");
            foreach (string s in prefixText.Split(',', ';'))
            {
                string query = Controller.ConvertSampleToQuery(s);
                if (!(String.IsNullOrEmpty(query)))
                	filter = (filter + query);
            }
            request.Filter = new string[] {
                    filter};
            request.AllowFieldInFilter = true;
            request.MaximumValueCount = count;
            object[] list = ControllerFactory.CreateDataController().GetListOfValues(arguments[0], arguments[1], request);
            List<string> result = new List<string>();
            foreach (object o in list)
            	result.Add(Convert.ToString(o));
            return result.ToArray();
        }
        
        public static string CreateReportInstance(Type t, string name, string controller, string view)
        {
            if (String.IsNullOrEmpty(name))
            	name = "Template.xslt";
            bool isGeneric = (Path.GetExtension(name).ToLower() == ".xslt");
            string reportKey = ("Report_" + name);
            if (isGeneric)
            	reportKey = String.Format("Reports_{0}_{1}", controller, view);
            string report = ((string)(HttpRuntime.Cache[reportKey]));
            if (String.IsNullOrEmpty(report))
            {
                // try loading a report as a resource or from the folder ~/Reports/
                if (t == null)
                	t = typeof(BUDI2_NS.Data.Controller);
                Stream res = t.Assembly.GetManifestResourceStream(String.Format("BUDI2_NS.Reports.{0}", name));
                if (res == null)
                	res = t.Assembly.GetManifestResourceStream(String.Format("BUDI2_NS.{0}", name));
                CacheDependency dependency = null;
                if (res == null)
                {
                    string templatePath = Path.Combine(Path.Combine(HttpRuntime.AppDomainAppPath, "Reports"), name);
                    if (!(File.Exists(templatePath)))
                    	throw new Exception(String.Format("Report or report template \\\'{0}\\\' does not exist.", name));
                    report = File.ReadAllText(templatePath);
                    dependency = new CacheDependency(templatePath);
                }
                else
                {
                    StreamReader reader = new StreamReader(res);
                    report = reader.ReadToEnd();
                    reader.Close();
                }
                if (isGeneric)
                {
                    // transform a data controller into a report by applying the specified template
                    ControllerConfiguration config = BUDI2_NS.Data.Controller.CreateConfigurationInstance(t, controller);
                    XsltArgumentList arguments = new XsltArgumentList();
                    arguments.AddParam("ViewName", String.Empty, view);
                    XslCompiledTransform transform = new XslCompiledTransform();
                    transform.Load(new XPathDocument(new StringReader(report)));
                    MemoryStream output = new MemoryStream();
                    transform.Transform(config.Navigator, arguments, output);
                    output.Position = 0;
                    StreamReader sr = new StreamReader(output);
                    report = sr.ReadToEnd();
                    sr.Close();
                }
                HttpRuntime.Cache.Insert(reportKey, report, dependency);
            }
            report = Regex.Replace(report, "(<Language>)(.+?)(</Language>)", String.Format("$1{0}$3", System.Threading.Thread.CurrentThread.CurrentUICulture.Name));
            report = Localizer.Replace("Reports", name, report);
            return report;
        }
        
        public static object FindSelectedValueByTag(string tag)
        {
#pragma warning disable 0618
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
#pragma warning restore 0618
            object[] selectedValues = serializer.Deserialize<object[]>(HttpContext.Current.Request.Form["__WEB_DATAVIEWSTATE"]);
            if (selectedValues != null)
            {
                int i = 0;
                while (i < selectedValues.Length)
                {
                    string k = ((string)(selectedValues[i]));
                    i++;
                    if (k == tag)
                    {
                        object[] v = ((object[])(selectedValues[i]));
                        if ((v == null) || (v.Length == 0))
                        	return null;
                        if (v.Length == 1)
                        	return v[0];
                        return v;
                    }
                    i++;
                }
            }
            return null;
        }
        
        void IBusinessObject.AssignFilter(string filter, BusinessObjectParameters parameters)
        {
            _viewFilter = filter;
            _parameters = parameters;
        }
        
        protected virtual void Initialize()
        {
            CultureManager.Initialize();
        }
    }
    
    public class ControllerFactory
    {
        
        public static IDataController CreateDataController()
        {
            return new Controller();
        }
        
        public static IAutoCompleteManager CreateAutoCompleteManager()
        {
            return new Controller();
        }
        
        public static IDataEngine CreateDataEngine()
        {
            return new Controller();
        }
    }
}
