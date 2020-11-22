using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils;

namespace ServerApp.Queries
{
    class InsertQuery : AbstractQuery
    {
        private string DatabaseName;
        private string TableName;
        private string RecordsString;
        private List<KeyValuePair<string, string>> Records;
        private List<ColumnInfo> ColumnsInfo;
        private MongoDBAcess MongoDB;

        // Attributes used for Foreign Key constraints check
        private List<KeyValuePair<string, int>> ForeignKeyPositions; // stores the name of the FK + the position of the column within a record
        private List<KeyValuePair<string, int>> UniqueKeyPositions; // stores the name of the UqK + the position of the column within a record
        private List<ForeignKeyData> ForeignKeyData; // stores the data about FK relations 
        private List<Tuple<string, string>> UniqueKeyData;

        public InsertQuery(string _databaseName, string _tableName, string _records) : base(Commands.INSERT_INTO_TABLE)
        {
            DatabaseName = _databaseName;
            TableName = _tableName;
            RecordsString = _records;
            Records = new List<KeyValuePair<String, String>>();
            ColumnsInfo = new List<ColumnInfo>();
            ForeignKeyPositions = new List<KeyValuePair<string, int>>();
            UniqueKeyPositions = new List<KeyValuePair<string, int>>();
            ForeignKeyData = new List<ForeignKeyData>();
            UniqueKeyData = new List<Tuple<string, string>>();
        }

        public override void ParseAttributes()
        {
            var recordsArray = RecordsString.Split('|');

            // Create Key-Value pairs that will be added to the MongoDB 
            foreach (var newRecord in recordsArray)
            {
                var keyValuePair = newRecord.Split('*');

                // Special handling for tables with one column, where key = value bc I said so 
                if (keyValuePair.Length == 1)
                {
                    Records.Add(new KeyValuePair<string, string>(keyValuePair[0], keyValuePair[0]));
                } 
                else
                {
                    Records.Add(new KeyValuePair<string, string>(keyValuePair[0], keyValuePair[1]));
                }
            }

            // Get the information about the table columns 
            var columnInfoString = DatabaseManager.FetchTableColumns(DatabaseName, TableName);
            foreach (var columnInfo in columnInfoString.Split(';')[1].Split('|'))
            {
                ColumnsInfo.Add(new ColumnInfo(columnInfo));
            }

            // Get a list of Foreign Key names + the position within the table struture of the FK column 
            for (int idx = 0; idx < ColumnsInfo.Count; idx++)
            {
                if (ColumnsInfo[idx].FK)
                {
                    ForeignKeyPositions.Add(new KeyValuePair<string, int>(ColumnsInfo[idx].ColumnName, idx));
                }
            }
            
            for (int idx = 0; idx < ColumnsInfo.Count; idx++)
            {
                if (ColumnsInfo[idx].Unique)
                {
                    UniqueKeyPositions.Add(new KeyValuePair<string, int>(ColumnsInfo[idx].ColumnName, idx));
                }
            }

            ForeignKeyData = GetReferencesInformation();

            UniqueKeyData = GetUniqueKeyInformation();

            // Initialize MongoDB Access class
            MongoDB = new MongoDBAcess(DatabaseName);
        }

        public override void PerformXMLActions()
        {
            try
            {
                foreach (var keyValuePairs in Records)
                {
                    if (CheckDuplicatePK(keyValuePairs.Key))
                    {
                        throw new Exception("Table " + TableName + " already contains a record with Primary Key " + keyValuePairs.Key + "!");
                    }

                    CheckFKConstraints(keyValuePairs.Key + "#" + keyValuePairs.Value);

                    CheckUniqueKeyConstraint(keyValuePairs.Key + "#" + keyValuePairs.Value);
                    // All checks have passed => Insert new record 
                    InsertRecord(keyValuePairs.Key, keyValuePairs.Value);

                }
            }
            catch (Exception ex)
            {
                // Exception can come from different checks (PK, UK, FK, Index) => just throw back the message to the DatabaseManager
                throw new Exception(ex.Message);
            }
        }

        private bool CheckDuplicatePK(string key)
        {
            try
            {
                return MongoDB.CollectionContainsKey(TableName, key);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool CheckFKConstraints(string record)
        {
            var columnValues = record.Split('#');
            
            // Check each FK constraint one by one
            foreach (var referenceData in ForeignKeyData)
            {
                var fkValue = "";

                if (referenceData.ReferencedColumns.Count > 1)
                {
                    // Special handling for composite foreign key
                    foreach (var referencedColumn in referenceData.ReferencedColumns)
                    {
                        fkValue += columnValues.ElementAt(ForeignKeyPositions.Find(elem => elem.Key == referencedColumn).Value) + '#';
                    }
                    fkValue = fkValue.Remove(fkValue.Length - 1);
                }
                else
                {
                    // Foreign Key is made up of only one column 
                    fkValue = columnValues.ElementAt(ForeignKeyPositions.Find(elem => elem.Key == referenceData.ReferencedColumns[0]).Value);
                }

                // Check if the value of the Foreign Key exists in the referenced table, if not => exception
                if (!MongoDB.CollectionContainsKey(referenceData.ReferencedTable, fkValue))
                {
                    throw new Exception("Invalid reference! No record with Primary Key " + fkValue + " could be found in referenced table " + referenceData.ReferencedTable + "!");
                }
            }

            return true; // if no exception has been thrown so far => all FK constraints are respected for the current row 
        }

        private bool CheckUniqueKeyConstraint(string record)
        {
            var columnValues = record.Split('#');
            foreach (var referenceData in UniqueKeyData)
            {
                var uqValue = columnValues.ElementAt(UniqueKeyPositions.Find(elem => elem.Key == referenceData.Item1.ToString()).Value);

                if (MongoDB.CollectionContainsKey(referenceData.Item2, uqValue))
                {
                    throw new Exception("A record with Unique Key " + uqValue + " is already in referenced table " + referenceData.Item2 + "!");
                }
            }
            return true;

        }
        private void InsertRecord(string key, string value)
        {
            try
            {
                // Insert into main table data file 
                MongoDB.InsertKVIntoCollection(TableName, key, value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<ForeignKeyData> GetReferencesInformation()
        {
            // Return a list with all the data about all foreign keys on the table (list of FKs, Referenced table, mongoDB filename)
            var fkData = new List<ForeignKeyData>();

            var xmlDocument = XDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");
            XElement databaseNode = Array.Find(xmlDocument.Element("Databases").Descendants("Database").ToArray(), 
                                                                elem => elem.Attribute("databaseName").Value.Equals(DatabaseName));
            XElement tableNode = Array.Find(databaseNode.Descendants("Table").ToArray(),
                                                                elem => elem.Attribute("tableName").Value.Equals(TableName));
            XElement[] fkNodes = tableNode.Descendants("ForeignKeys").Descendants("ForeignKey").ToArray();

            foreach (var foreignKeyNode in fkNodes)
            {
                var mongoDBFilename = foreignKeyNode.Attribute("fileName").Value;
                var referencedTable = foreignKeyNode.Descendants("ReferencedTable").ToArray()[0].Value;
                var referencedKeys = new List<string>();
                
                foreach (var foreignKey in foreignKeyNode.Descendants("ReferencedColumn"))
                {
                    referencedKeys.Add(foreignKey.Value);
                }

                fkData.Add(new ForeignKeyData(referencedKeys, referencedTable, mongoDBFilename));
            }

            return fkData;
        }

        private List<Tuple<string, string>> GetUniqueKeyInformation()
        {
            // Return a list with all the data about all foreign keys on the table (list of FKs, Referenced table, mongoDB filename)
            var uqData = new List<Tuple<string,string>>();

            var xmlDocument = XDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");
            XElement databaseNode = Array.Find(xmlDocument.Element("Databases").Descendants("Database").ToArray(),
                                                                elem => elem.Attribute("databaseName").Value.Equals(DatabaseName));
            XElement tableNode = Array.Find(databaseNode.Descendants("Table").ToArray(),
                                                                elem => elem.Attribute("tableName").Value.Equals(TableName));
            XElement[] uqNodes = tableNode.Descendants("UniqueKeys").Descendants("UniqueKeyColumn").ToArray();

            foreach (var uniqueKeyNode in uqNodes)
            {
                var mongoDBFilename = uniqueKeyNode.Attribute("fileName").Value;
                var keyName = uniqueKeyNode.Value;

                uqData.Add(new Tuple<string, string>(keyName, mongoDBFilename));
            }

            return uqData;
        }
    }

    class ForeignKeyData
    {
        public List<string> ReferencedColumns;
        public string ReferencedTable;
        public string ForeignKeyFile;

        public ForeignKeyData(List<string> _column, string _table, string _file)
        {
            ReferencedColumns = _column;
            ReferencedTable = _table;
            ForeignKeyFile = _file;
        }
    }
}
