using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Xml;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace CreateUnicodeDB
{
    class Program
    {
        static void Main(string[] args)
        {
            //Parse command line parameters
            Dictionary<string, string> param = new Dictionary<string, string>();
            foreach (var v in args)
            {
                int p = v.IndexOf("=");
                if (p > 0)
                {
                    param.Add(v.Substring(0, p), v.Substring(p + 1, v.Length - p - 1));
                }
            }

            if (param.Count == 0)
            {
                //just show help
                ShowUsage();
                WriteLine("Press any key to close");
                Console.ReadKey();
                Environment.Exit(0);
            }

            string server;
            string database;
            string newDatabase = null;
            string user = null;
            string password = null;
            string databaseFileName = "";
            if (!param.TryGetValue("server", out server)) server = ".";
            if (!param.TryGetValue("db", out database)) WarnAndExit("\"db\" command line parameter was not specified");
            if (!param.TryGetValue("newdb", out newDatabase)) newDatabase = null;
            if (!param.TryGetValue("user", out user)) user = null;
            if (!param.TryGetValue("password", out password)) password = null;
            if (!param.TryGetValue("filename", out databaseFileName)) databaseFileName = null;
            if (string.IsNullOrEmpty(newDatabase)) newDatabase = database + "_Unicode";

            //Connect to the specified server
            ServerConnection connection = new ServerConnection(server);
            Database db = null;
            Server svr = null;
            if (string.IsNullOrEmpty(user))
            {
                connection.LoginSecure = true; //Windows authentication
            }
            else
            {
                connection.LoginSecure = false;
                connection.Login = user;
                connection.Password = password;
            }
            try
            {
                svr = new Server(connection);
                db = svr.Databases[database];
            }
            catch (Exception e)
            {
                WarnAndExit(string.Format("Could not connect to server \"{0}\": {1}", server, e.Message));
            }
            if (db == null) WarnAndExit(string.Format("Database \"{0}\" does not exist on the server \"{1}\"", database, server));

            //create new database

            Database newDB = svr.Databases[newDatabase];
            if (newDB != null)
            {
                try
                {
                    Notify(string.Format("Dropping the existing \"{0}\" database", newDatabase));
                    svr.KillAllProcesses(newDatabase);
                    svr.KillDatabase(newDatabase);
                }
                catch (Exception e)
                {
                    ReportException(e);
                }
                newDB = svr.Databases[newDatabase];
            }
            if (newDB != null)
            {
                WriteLine(string.Format("Target database \"{0}\" already exists. All existing data will be deleted", newDatabase), OutputKind.Warning);
                //store views/tables/triggers in a list
                List<View> oldViews = new List<View>();
                foreach (View v in newDB.Views) if (!v.IsSystemObject) oldViews.Add(v);
                List<Table> oldTables = new List<Table>();
                foreach (Table t in newDB.Tables) if (!t.IsSystemObject) oldTables.Add(t);
                List<Trigger> oldTriggers = new List<Trigger>();
                foreach (Trigger t in newDB.Triggers) if (!t.IsSystemObject) oldTriggers.Add(t);
                //delete 'em
                foreach (Trigger t in oldTriggers) t.Drop();
                foreach (View v in oldViews) v.Drop();
                foreach (Table t in oldTables) t.Drop();
            }
            else
            {
                WriteLine(string.Format("Creating new database \"{0}\" ", newDatabase), OutputKind.Info);
                newDB = new Database(svr, newDatabase);
                newDB.Collation = db.Collation;
                newDB.DefaultSchema = db.DefaultSchema; //should it be "sysdba"?
                FileGroup dbFG = new FileGroup(newDB, "PRIMARY");
                newDB.FileGroups.Add(dbFG);

                //Now add primary db file to file group
                if (string.IsNullOrEmpty(databaseFileName))
                {
                    string oldDatabaseFilename = db.FileGroups[0].Files[0].FileName;
                    string directory = Path.GetDirectoryName(oldDatabaseFilename);
                    databaseFileName = directory + @"\" + newDatabase + ".mdf";
                }

                DataFile df1 = new DataFile(dbFG, "SalesLogix_Data");
                dbFG.Files.Add(df1);
                df1.FileName = databaseFileName;
                df1.Size = 10.0*1024.0;
                df1.GrowthType = FileGrowthType.Percent;
                df1.Growth = 25.0;
                try
                {
                    newDB.Create();
                }
                catch (Exception e)
                {
                    WriteLine(string.Format("Could not create database \"{0}\"", newDatabase), OutputKind.Error);
                    ReportException(e);
                    WarnAndExit("");
                }
            }

            //copy the users
            foreach (User oldUser in db.Users)
            {
                User newUser = newDB.Users[oldUser.Name];
                if (newUser == null)
                {
                    Notify("Processing user  " + oldUser.Name);
                    try
                    {
                        newUser = new User(newDB, oldUser.Name);
                        newUser.DefaultSchema = oldUser.DefaultSchema;
                        newUser.UserType = oldUser.UserType;
                        newUser.Login = oldUser.Login;
                        newDB.Users.Add(newUser);
                        newUser.Create();

                        StringCollection roles = oldUser.EnumRoles();
                        foreach (string role in roles) newUser.AddToRole(role);
                        newUser.Alter();
                    }
                    catch (Exception e)
                    {
                        ReportException(e);
                    }
                }
            }

            //copy schemas
            foreach (Schema oldSchema in db.Schemas)
            {
                Schema newSchema = newDB.Schemas[oldSchema.Name];
                if (newSchema == null)
                {
                    Notify("Processing schema  " + oldSchema.Name);
                    try
                    {
                        newSchema = new Schema(newDB, oldSchema.Name);
                        newSchema.Owner = oldSchema.Owner;
                        newDB.Schemas.Add(newSchema);
                        newSchema.Create();
                    }
                    catch (Exception e)
                    {
                        ReportException(e);
                    }
                }
            }

            //copy datatype
            foreach (UserDefinedDataType oldType in db.UserDefinedDataTypes)
            {
                UserDefinedDataType newType = newDB.UserDefinedDataTypes[string.Format("[{0}].[{1}]", oldType.Owner, oldType.Name)];
                if (newType == null) newType = newDB.UserDefinedDataTypes[oldType.Name];
                if (newType == null)
                {
                    Notify("Processing user data type  " + oldType.Name);
                    try
                    {
                        newType = new UserDefinedDataType(newDB, oldType.Name, oldType.Schema);
                        newType.Owner = oldType.Owner;
                        //adjust the type correctly
                        string systemType = oldType.SystemType;
                        switch (systemType.ToUpper())
                        {
                            case "VARCHAR":
                                {
                                    systemType = "NVARCHAR";
                                    break;
                                }
                            case "CHAR":
                                {
                                    systemType = "NCHAR";
                                    break;
                                }
                            case "TEXT":
                                {
                                    systemType = "NTEXT";
                                    break;
                                }
                        }
                        newType.SystemType = systemType;
                        newType.Length = oldType.Length;
                        newType.Nullable = oldType.Nullable;
                        newType.Default = oldType.Default;
                        newType.DefaultSchema = oldType.DefaultSchema;
                        newType.NumericPrecision = oldType.NumericPrecision;
                        newType.NumericScale = oldType.NumericScale;
                        newType.Rule = oldType.Rule;
                        newType.RuleSchema = oldType.RuleSchema;
                        newDB.UserDefinedDataTypes.Add(newType);
                        newType.Create();
                    }
                    catch (Exception e)
                    {
                        ReportException(e);
                    }
                }
            }

            //copy the schema
            StringBuilder sb = new StringBuilder();
            ScriptingOptions options = new ScriptingOptions();
            options.ClusteredIndexes = true;
            options.Default = true;
            options.DriAll = true;
            options.Indexes = true;
            options.IncludeHeaders = true;
            options.DriAllConstraints = true;
            options.DriIndexes = true;
            options.FullTextIndexes = true;
            options.ExtendedProperties = true;
            options.NoCollation = true; //we will convert TEXT to NTEXT anyway
            options.NoCommandTerminator = false; //we do need this - otherwise CREATE VIEW has problems
            options.ClusteredIndexes = true;
            options.NonClusteredIndexes = true;
            options.SchemaQualify = true;
            options.ScriptSchema = true;
            options.SchemaQualifyForeignKeysReferences = true;
            options.IncludeDatabaseContext = false; //since we wil be executing in a different DB context

            options.Triggers = true;

            //copy tables
            foreach (Table table in db.Tables)
            {
                if (!table.IsSystemObject)
                {
                    Table oldTable = newDB.Tables[table.Name];
                    if (oldTable != null)
                    {
                        try
                        {
                            Notify("Dropping existing table " + oldTable.Name);
                            oldTable.Drop();
                        }
                        catch (Exception e)
                        {
                            ReportException(e);
                        }
                    }

                    sb.Length = 0;
                    //don't script inserts - we run out of memory
                    options.ScriptData = false;
                    StringCollection coll = table.Script(options);

                    foreach (string str in coll)
                    {
                        sb.AppendLine(str);
                    }
                    Notify("Creating table " + table.Name);
                    string sql = ChangeAnsiToUnicode(sb.ToString());
                    try
                    {
                        newDB.ExecuteNonQuery(sql);
                    }
                    catch (Exception e)
                    {
                        ReportException(e, sql);
                    }
                    var x = Missing.Value;
                }
            }

            //copy views
            foreach (View view in db.Views)
            {
                if (!view.IsSystemObject)
                {
                    View oldView = newDB.Views[view.Name];
                    if (oldView != null)
                    {
                        try
                        {
                            Notify("Dropping existing view " + oldView.Name);
                            oldView.Drop();
                        }
                        catch (Exception e)
                        {
                            ReportException(e);
                        }
                    }

                    sb.Length = 0;
                    StringCollection coll = view.Script(options);
                    foreach (string str in coll)
                    {
                        string line = str; //make it a local avriable since we change it below
                        if (line.Trim().StartsWith("create view", true, CultureInfo.CurrentCulture))
                        {
                            //'CREATE VIEW' must be the first statement in a query batch.
                            sb.AppendLine("GO");
                            //make sure we have the right schema when we call CREATE VIEW
                            if ((line.ToUpper().IndexOf(".[" + view.Name.ToUpper()) < 0) && //if there is no preceeing "]." before [viewname], the schema is not specified
                                (line.ToUpper().IndexOf("SYSDBA." + view.Name.ToUpper()) < 0))
                            {
                                int p = line.ToUpper().IndexOf(string.Format("[{0}]", view.Name.ToUpper()));
                                if (p >= 0)
                                {
                                    line = line.Replace(string.Format("[{0}]", view.Name), string.Format("[sysdba].[{0}]", view.Name));
                                }
                                else
                                {
                                    p = line.ToUpper().IndexOf(string.Format("CREATE VIEW {0}", view.Name.ToUpper()));
                                    if (p >= 0)
                                    {
                                        line = line.ToUpper().Replace(string.Format("CREATE VIEW {0}", view.Name), string.Format("CREATE VIEW [sysdba].[{0}]", view.Name));
                                    }
                                }
                            }
                        }
                        sb.AppendLine(line);
                    }
                    try
                    {
                        Notify("Creating view " + view.Name);
                        newDB.ExecuteNonQuery(sb.ToString());
                    }
                    catch (Exception e)
                    {
                        ReportException(e, sb.ToString());
                    }
                }
            }

            //now stuff existing data into the new db
            string connectionString = "";
            if (string.IsNullOrEmpty(user))
            {
                //Windows Auth
                connectionString = string.Format("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog={0};Data Source={1}",
                                                newDatabase, server);
            }
            else
            {
                //SQL Auth
                connectionString = string.Format("Password={0};Persist Security Info=True;User ID={1};Initial Catalog={2};Data Source={3}",
                                                 password, user, newDatabase, server);
            }
            SqlConnection newConnection = new SqlConnection(connectionString);
            newConnection.Open();
            SqlBulkCopy bulkCopy = new SqlBulkCopy(newConnection);
            bulkCopy.BulkCopyTimeout = int.MaxValue;

            foreach (Table table in db.Tables)
            {
                if (!table.IsSystemObject)
                {
                    try
                    {
                        Notify("Copying data to table " + table.Name);
                        string sql = string.Format("SELECT * FROM {0}.{1}", table.Owner, table.Name);
                        using (DataSet dataset = db.ExecuteWithResults(sql))
                        {
                            bulkCopy.DestinationTableName = string.Format("{0}.{1}", table.Owner, table.Name);
                            if (dataset.Tables.Count > 0)
                            {
                                DataTable t = dataset.Tables[0];
                                bulkCopy.WriteToServer(t);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ReportException(e);
                    }
                }
                GC.Collect(); //we can use lots of memory above!
            }

            //make sure the DB is marked as Unicode
            string markAsUnicodeSQL = "UPDATE SYSDBA.SYSTEMINFO SET UNICODE = 'T' ";
            try
            {
                Notify("Marking the new database as Unicode enabled");
                newDB.ExecuteNonQuery(markAsUnicodeSQL);
            }
            catch (Exception e)
            {
                ReportException(e, markAsUnicodeSQL);
            }

            //convert the virtual file system
            Notify("Updating VIRTUALFILESYSTEM table...");
            try
            {
                //we need a conneciton for updates since SMO won't let us use parameters for updates
                //when we update large binary fields (VIRTUALFILESYSTEM.ITEMDATA)
                string updateConnectionString = "";
                if (string.IsNullOrEmpty(user))
                {
                    //Windows Auth
                    updateConnectionString = string.Format("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog={0};Data Source={1}",
                                                    newDatabase, server);
                }
                else
                {
                    //SQL Auth
                    updateConnectionString = string.Format("Password={0};Persist Security Info=True;User ID={1};Initial Catalog={2};Data Source={3}",
                                                     password, user, newDatabase, server);
                }
                SqlConnection updateConnection = new SqlConnection(updateConnectionString);
                updateConnection.Open();

                string vfsSql =
                    "SELECT VIRTUALFILESYSTEMID, ITEMNAME, ITEMDATA, ISCOMPRESSED FROM SYSDBA.VIRTUALFILESYSTEM WHERE ITEMNAME LIKE '%.entity.xml' ";
                using (DataSet vfsDataset = newDB.ExecuteWithResults(vfsSql))
                {
                    if (vfsDataset.Tables.Count > 0)
                    {
                        DataTable t = vfsDataset.Tables[0];
                        foreach (DataRow row in t.Rows)
                        {
                            string itemName = (string) row["ITEMNAME"];
                            string[] arrItemName = itemName.Split(new string[] {"."}, StringSplitOptions.None);
                            //Name.TABLE.entity.xml
                            if (arrItemName.Length > 2)
                            {
                                string entityName = arrItemName[0];
                                string tableName = arrItemName[1];
                                Notify(string.Format("Updating entity {0} (Table {1})", entityName, tableName));
                                try
                                {
                                    Table table = null;
                                    foreach (Table currTable in newDB.Tables)
                                    {
                                        if ((String.Compare(currTable.Name, tableName, true) == 0) && (String.Compare(currTable.Owner, "sysdba", true) == 0))
                                        {
                                            table = currTable;
                                            break;
                                        }
                                    }
                                    if (table != null)
                                    {
                                        object ItemData = row["ITEMDATA"]; //we expect an array
                                        if (ItemData != null)
                                        {
                                            object strIsCompressed = row["ISCOMPRESSED"];
                                            Boolean bIsCompressed = (strIsCompressed != null) &&
                                                                    (string.Compare((string) strIsCompressed, "T", true) ==
                                                                     0);
                                            using (var memoryStream = UnpackItemData((byte[]) ItemData, bIsCompressed))
                                            {
                                                XmlDocument xmlEntityDocument = new XmlDocument();
                                                memoryStream.Position = 0;
                                                xmlEntityDocument.Load(memoryStream);
                                                Boolean XmlModified = false;
                                                foreach (Column column in table.Columns)
                                                {
                                                    //is this a string derived type?
                                                    SqlDataType dataType = column.DataType.SqlDataType;
                                                    if (column.DataType.SqlDataType == SqlDataType.UserDefinedDataType)
                                                    {
                                                        UserDefinedDataType colType = newDB.UserDefinedDataTypes[string.Format("[{0}].[{1}]", column.DataType.Schema, column.DataType.Name)];
                                                        if (colType == null) colType = newDB.UserDefinedDataTypes[column.DataType.Name];
                                                        if (colType != null)
                                                        {
                                                            switch (colType.SystemType.ToUpper())
                                                            {
                                                                case "NVARCHAR":
                                                                    {
                                                                        dataType = SqlDataType.NVarChar;
                                                                        break;
                                                                    }
                                                                case "NCHAR":
                                                                    {
                                                                        dataType = SqlDataType.NChar;
                                                                        break;
                                                                    }
                                                                case "NTEXT":
                                                                    {
                                                                        dataType = SqlDataType.NText;
                                                                        break;
                                                                    }
                                                            }
                                                        }
                                                    }

                                                    if ((dataType == SqlDataType.NChar) ||
                                                        (dataType == SqlDataType.NText) ||
                                                        (dataType == SqlDataType.NVarChar) ||
                                                        (dataType == SqlDataType.NVarCharMax)
                                                        )
                                                    {
                                                        XmlModified |= ModifyEntityColumn(xmlEntityDocument, column.Name);
                                                    }
                                                }
                                                if (XmlModified)
                                                {
                                                    //now save it
                                                    memoryStream.SetLength(0);
                                                    xmlEntityDocument.Save(memoryStream);
                                                    memoryStream.Position = 0;
                                                    byte[] newData = PackItemData(memoryStream, true, ref bIsCompressed);

                                                    string updateSql =
                                                        string.Format(
                                                            "UPDATE sysdba.VIRTUALFILESYSTEM SET ITEMDATA = @DATA, ISCOMPRESSED = @COMPRESSED WHERE VIRTUALFILESYSTEMID = '{0}'",
                                                            row["VIRTUALFILESYSTEMID"]
                                                            );
                                                    SqlParameter parameter;
                                                    SqlCommand command = new SqlCommand(updateSql, updateConnection);
                                                    parameter = command.Parameters.Add("@DATA", SqlDbType.Image);
                                                    parameter.Value = newData;
                                                    parameter = command.Parameters.Add("@COMPRESSED", SqlDbType.NVarChar);
                                                    parameter.Value = bIsCompressed ? "T" : "F";
                                                    command.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    ReportException(e);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ReportException(e);
            }
            GC.Collect();
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private static bool ModifyEntityColumn(XmlNode xmlDocument, string columnName)
        {
            const string GUID_ANSI_TEXT = "ccc0f01d-7ba5-408e-8526-a3f942354b3a";
            const string GUID_ANSI_LONGTEXT = "f4ca6023-9f5f-4e41-8571-50ba94e8f233";
            const string GUID_UNICODE_TEXT = "76c537a8-8b08-4b35-84cf-fa95c6c133b0";
            const string GUID_UNICODE_LONGTEXT = "b2ed309e-ea89-4eef-8051-6244987953a4";
            const string GUID_GUID = "3ca925e1-4b76-4621-a39c-a0d4cb7327d5";
            const string GUID_ENUM = "8edd8fce-2be5-4d3d-bedd-ea667e78a8af";
            const string GUID_PICKLIST = "b71918bf-fac1-4b62-9ed5-0b0294bc9900";

            if (xmlDocument == null) return false;
            if (string.IsNullOrEmpty(columnName)) return false;
            XmlNodeList xmlPropertyList = xmlDocument.SelectNodes("entity/properties/property");
            if (xmlPropertyList != null)
            {
                foreach (XmlNode xmlPropertyNode in xmlPropertyList)
                {
                    XmlAttribute xmlXsiTypeAttribute = xmlPropertyNode.Attributes["xsi:type"];
                    if (xmlXsiTypeAttribute != null)
                    {
                        string strType = xmlXsiTypeAttribute.Value;
                        if (!string.IsNullOrEmpty(strType) && (strType.ToUpper().Equals("ORMFIELDPROPERTY")))
                        {
                            XmlAttribute xmlColumnNameAttribute = xmlPropertyNode.Attributes["columnName"];
                            if (xmlColumnNameAttribute != null)
                            {
                                string strColumnName = xmlColumnNameAttribute.Value;
                                if (!string.IsNullOrEmpty(strColumnName) && (strColumnName.ToUpper().Equals(columnName.ToUpper())))
                                {
                                    /* Found the column */
                                    XmlNode xmlSystemDataTypeNode =
                                        xmlPropertyNode.SelectSingleNode("SystemDataType");
                                    if (xmlSystemDataTypeNode != null)
                                    {
                                        XmlAttribute xmlGuidAttribute = xmlSystemDataTypeNode.Attributes["guid"];
                                        if ((xmlGuidAttribute != null) && (!string.IsNullOrEmpty(xmlGuidAttribute.Value)))
                                        {
                                            string strGuid = xmlGuidAttribute.Value;
                                            XmlNode xmlANSIDataTypeNode;
                                            switch (strGuid.ToLower())
                                            {
                                                case GUID_ANSI_TEXT:
                                                    xmlANSIDataTypeNode =
                                                        xmlSystemDataTypeNode.SelectSingleNode("TextDataType");
                                                    if (xmlANSIDataTypeNode != null)
                                                    {
                                                        xmlSystemDataTypeNode.InnerXml =
                                                            xmlSystemDataTypeNode.InnerXml.Replace(
                                                                "TextDataType", "UnicodeTextDataType");
                                                        xmlGuidAttribute.Value = GUID_UNICODE_TEXT;
                                                        return true;
                                                    }
                                                    break;
                                                case GUID_ANSI_LONGTEXT:
                                                    xmlANSIDataTypeNode =
                                                        xmlSystemDataTypeNode.SelectSingleNode("LongTextDataType");
                                                    if (xmlANSIDataTypeNode != null)
                                                    {
                                                        xmlSystemDataTypeNode.InnerXml =
                                                            xmlSystemDataTypeNode.InnerXml.Replace(
                                                                "LongTextDataType", "LongUnicodeTextDataType");
                                                        xmlGuidAttribute.Value = GUID_UNICODE_LONGTEXT;
                                                        return true;
                                                    }
                                                    break;
                                                case GUID_GUID:
                                                    xmlANSIDataTypeNode = xmlSystemDataTypeNode.SelectSingleNode("GuidDataType");
                                                    if (xmlANSIDataTypeNode != null)
                                                    {
                                                        var storageAttr = xmlANSIDataTypeNode.Attributes["storage"];
                                                        if (storageAttr != null && storageAttr.Value == "String")
                                                        {
                                                            storageAttr.Value += "Unicode";
                                                            return true;
                                                        }
                                                    }
                                                    break;
                                                case GUID_ENUM:
                                                    xmlANSIDataTypeNode = xmlSystemDataTypeNode.SelectSingleNode("EnumDataType");
                                                    if (xmlANSIDataTypeNode != null)
                                                    {
                                                        var storageAttr = xmlANSIDataTypeNode.Attributes["storage"];
                                                        if (storageAttr == null)
                                                        {
                                                            storageAttr = ((XmlDocument) xmlDocument).CreateAttribute("storage");
                                                            storageAttr.Value = "Name";
                                                            xmlANSIDataTypeNode.AppendChild(storageAttr);
                                                        }
                                                        if (storageAttr.Value == "Name" || storageAttr.Value == "Code")
                                                        {
                                                            storageAttr.Value += "Unicode";
                                                            return true;
                                                        }
                                                    }
                                                    break;
                                                case GUID_PICKLIST:
                                                    xmlANSIDataTypeNode = xmlSystemDataTypeNode.SelectSingleNode("PickListDataType");
                                                    if (xmlANSIDataTypeNode != null)
                                                    {
                                                        var storageElem = xmlANSIDataTypeNode.SelectSingleNode("Storage");
                                                        if (storageElem == null)
                                                        {
                                                            storageElem = ((XmlDocument) xmlDocument).CreateElement("Storage");
                                                            storageElem.InnerText = "Text";
                                                            xmlANSIDataTypeNode.AppendChild(storageElem);
                                                        }
                                                        if (storageElem.InnerText == "Text" || storageElem.InnerText == "Code")
                                                        {
                                                            storageElem.InnerText += "Unicode";
                                                            return true;
                                                        }
                                                    }
                                                    break;
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private static MemoryStream UnpackItemData(byte[] itemData, bool compressed)
        {
            if (itemData == null)
            {
                return null;
            }

            if (compressed)
            {
                var compressedStream = new MemoryStream();

                using (var memoryStream = new MemoryStream(itemData))
                using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
                {
                    var buffer = new byte[0x100];
                    int count;

                    while ((count = deflateStream.Read(buffer, 0, 0x100)) > 0)
                    {
                        compressedStream.Write(buffer, 0, count);
                    }
                }

                return compressedStream;
            }

            return new MemoryStream(itemData);
        }

        private static byte[] PackItemData(MemoryStream itemData, bool smart, ref bool compressed)
        {
            if (itemData == null)
            {
                return null;
            }

            var data = itemData.ToArray();

            if (smart || compressed)
            {
                byte[] compressedData;

                using (var memoryStream = new MemoryStream())
                {
                    using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress))
                    {
                        deflateStream.Write(data, 0, data.Length);
                    }

                    compressedData = memoryStream.ToArray();
                }

                compressed = !smart || compressedData.Length < data.Length;

                if (compressed)
                {
                    return compressedData;
                }
            }

            return data;
        }

        //takes a line from the CREATE TABLE script and converts all ANSI columsn to Unicode
        private static string ChangeAnsiToUnicode(string value)
        {
            /*
            this is lame - poor man' parser..
            We assume that the data type is always the second element on a line describing a column
            CREATE TABLE Persons
            (
               P_Id int,
               LastName varchar(255),
             )
             * if this is not the case, we are screwed...
             * We also assume that case does not omatter and that there is always one column per line....
             */

            string line;
            Boolean convert = false;
            List<string> lst = new List<string>(value.Split(new string[] {"\r\n"}, StringSplitOptions.None));
            for (int i = 0; i < lst.Count; i++)
            {
                line = lst[i].TrimStart().ToUpper();
                if (!convert)
                {
                    if (line.StartsWith("CREATE TABLE")) convert = true;
                }
                else
                {
                    //parse the line
                    string[] elements = line.Split(new string[] {" "}, StringSplitOptions.None);
                    if (elements.Length >= 2)
                    {
                        var res = elements[1];
                        if ((res.IndexOf("NVARCHAR") < 0) && (res.IndexOf("NCHAR") < 0) && (res.IndexOf("NTEXT") < 0) && (res.IndexOf("TEXTBLOB") < 0))
                        {
                            if (res.IndexOf("VARCHAR") >= 0) res = res.Replace("VARCHAR", "NVARCHAR");
                            else if (res.IndexOf("CHAR") >= 0) res = res.Replace("CHAR", "NCHAR");
                            else if (res.IndexOf("TEXT") >= 0) res = res.Replace("TEXT", "NTEXT");
                            elements[1] = res;
                        }
                        lst[i] = string.Join(" ", elements);
                    }
                }
            }
            return string.Join("\r\n", lst.ToArray());
        }

        private enum OutputKind { None = 0, Info = 1, Warning = 2, Error = 3 }

        static void WriteLine(string value, OutputKind kind = OutputKind.None)
        {
            switch (kind)
            {
                case OutputKind.None:
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                case OutputKind.Info:
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    }
                case OutputKind.Warning:
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    }
                case OutputKind.Error:
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    }
            }
            Console.WriteLine(value);
        }

        static void WarnAndExit(string warning)
        {
            if (!string.IsNullOrEmpty(warning))
            {
                WriteLine(warning, OutputKind.Warning);
                WriteLine("");
            }
            ShowUsage();
            WriteLine("");
            WriteLine("Press any key to close");

            Console.Read();
            Environment.Exit(1);
        }

        static void ShowUsage()
        {
            WriteLine("This utility creates a new Unicode SalesLogix database from an existing (ANSI) database");
            WriteLine("Command line usage:");
            WriteLine("CreateUnicodeDB.exe server=value db=value newdb=value user=value password=value");
            WriteLine("db       - (required). The name of the database to convert, e.g. \"saleslogix_eval\"");
            WriteLine("server   - (optional). The name of the SQL server instance, e.g. \"MyMachine\\SQLEXPRESS\". ");
            WriteLine("           If not specified, local machine is assumed");
            WriteLine("newdb    - (optional). The name of the new Unicode database. If not specified, the new database");
            WriteLine("           name will be the same as the old database plus \"_Unicode\", e.g. \"saleslogix_eval_Unicode\"");
            WriteLine("filename - (optional). The file name of the new Unicode database. If not specified, the new database will");
            WriteLine("           be created in the same folder as the source data base and its file name will be the bew database name");
            WriteLine("user     - (optional). SQL user name. If not specified, the connection will be performed using the current");
            WriteLine("           Windows user credentials");
            WriteLine("password - (optional). SQL user password. Used only if the \"user\" parameter is specified.");
        }

        static void Notify(string value)
        {
            WriteLine(value, OutputKind.Info);
        }

        static void ReportException(Exception e, string sql = null)
        {
            Exception currentException = e;
            while (currentException != null)
            {
                WriteLine(currentException.GetType().Name + ": " + currentException.Message, OutputKind.Error);
                currentException = currentException.InnerException;
            }
            if (!string.IsNullOrEmpty(sql))
            {
                WriteLine("Offending SQL statement:", OutputKind.None);
                WriteLine(sql, OutputKind.Warning);
            }
        }
    }
}