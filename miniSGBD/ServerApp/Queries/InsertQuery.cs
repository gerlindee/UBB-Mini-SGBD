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
        private List<ForeignKeyInsertData> NewForeignKeyEntries;
        private List<KeyValuePair<string, int>> PrimaryKeyPositions; 


        public InsertQuery(string _databaseName, string _tableName, string _records) : base(Commands.INSERT_INTO_TABLE)
        {
            DatabaseName = _databaseName;
            TableName = _tableName;
            RecordsString = _records;
            Records = new List<KeyValuePair<string, string>>();
            ColumnsInfo = new List<ColumnInfo>();
            PrimaryKeyPositions = new List<KeyValuePair<string, int>>();
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

            // Get a list of Primary Key names + the positions within the table structure of the PK column
            for (int idx = 0; idx < ColumnsInfo.Count; idx++)
            {
                if (ColumnsInfo[idx].PK)
                {
                    PrimaryKeyPositions.Add(new KeyValuePair<string, int>(ColumnsInfo[idx].ColumnName, idx));
                }
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
                    NewForeignKeyEntries = new List<ForeignKeyInsertData>();

                    if (CheckDuplicatePK(keyValuePairs.Key))
                    {
                        throw new Exception("Table " + TableName + " already contains a record with Primary Key " + keyValuePairs.Key + "!");
                    }

                    CheckFKConstraints(keyValuePairs.Key + "#" + keyValuePairs.Value);
                    CheckUniqueKeyConstraint(keyValuePairs.Key + "#" + keyValuePairs.Value);
                    CheckUniqueIndexConstraint(keyValuePairs.Key + '#' + keyValuePairs.Value);
                    // All checks have passed => Insert new record into all relevant files

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

        private void CheckFKConstraints(string record)
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

                var pkValue = "";
                foreach (var primaryKey in PrimaryKeyPositions)
                {
                    pkValue += columnValues.ElementAt(primaryKey.Value) + '#';
                }
                pkValue = pkValue.Remove(pkValue.Length - 1);

                // current FK correct => add it to the Index File to-insert list
                NewForeignKeyEntries.Add(new ForeignKeyInsertData(referenceData.ForeignKeyFile, fkValue, pkValue));
            }
        }

        private bool CheckUniqueKeyConstraint(string record)
        {
            var columnValues = record.Split('#');
           
            foreach (var referenceData in UniqueKeyData)
            {
                var uqValue = columnValues.ElementAt(UniqueKeyPositions.Find(elem => elem.Key == referenceData.Item1.ToString()).Value);

                if (MongoDB.CollectionContainsKey(referenceData.Item2, uqValue))
                {
                    throw new Exception("A record with Unique Key " + uqValue + " already exists in table " + TableName + "!");
                }
            }
            return true;

        }

        private void CheckUniqueIndexConstraint(string record)
        {
            var columnValues = record.Split('#');
            var mongoDB = new MongoDBAcess(DatabaseName);
            var indexFiles = TableUtils.GetIndexFiles(DatabaseName, TableName);
            
            foreach(var file in indexFiles)
            {
                if (file.IsUnique)
                {
                    var createKeyOfUqIndex = "";
                    var pk = "";
                    for (int idx = 0; idx < ColumnsInfo.Count; idx++)
                    {
                        if(file.IndexColumns.Exists(elem => elem == ColumnsInfo[idx].ColumnName))
                        {
                            createKeyOfUqIndex += columnValues[idx] + '#';
                        }

                        if (ColumnsInfo[idx].PK)
                            pk = columnValues[idx] + "#";
                    }
                    
                    createKeyOfUqIndex = createKeyOfUqIndex.Remove(createKeyOfUqIndex.Length - 1);
                    pk = pk.Remove(pk.Length - 1);

                    MongoDB.InsertKVIntoCollection(file.IndexFileName, createKeyOfUqIndex, pk);
                }
            }
        }

        private void InsertRecord(string key, string value)
        {
            try
            {
                // Insert into main table data file 
                MongoDB.InsertKVIntoCollection(TableName, key, value);

                // Insert into FK index file 
                foreach (var newFKRecords in NewForeignKeyEntries)
                {
                    // Check if the Foreign Key from the referenced table has any other assigned records from the current table 
                    if (MongoDB.CollectionContainsKey(newFKRecords.MongoDBFilename, newFKRecords.ForeignKeyRecord.Key))
                    {
                        var existingReferences = MongoDB.GetRecordValueWithKey(newFKRecords.MongoDBFilename, newFKRecords.ForeignKeyRecord.Key);
                        existingReferences += "#" + newFKRecords.ForeignKeyRecord.Value;

                        // who needs update when you can just delete and add back 
                        MongoDB.RemoveKVFromCollection(newFKRecords.MongoDBFilename, newFKRecords.ForeignKeyRecord.Key);
                        MongoDB.InsertKVIntoCollection(newFKRecords.MongoDBFilename, newFKRecords.ForeignKeyRecord.Key, existingReferences);
                    }
                    // Otherwise just add a new Key-Value entry
                    else
                    {
                        MongoDB.InsertKVIntoCollection(newFKRecords.MongoDBFilename, newFKRecords.ForeignKeyRecord.Key, newFKRecords.ForeignKeyRecord.Value);
                    }
                }

                // Insert into Unique Key files
                var columnValues = (key + '#' + value).Split('#');
                foreach (var uniqueKey in UniqueKeyData)
                {
                    var uqValue = columnValues.ElementAt(UniqueKeyPositions.Find(elem => elem.Key == uniqueKey.Item1.ToString()).Value);
                    MongoDB.InsertKVIntoCollection(uniqueKey.Item2, uqValue, key);
                }

                // Insert into any index files 
                foreach (var indexFile in TableUtils.GetIndexFiles(DatabaseName, TableName))
                {
                    var indexKey = "";
                    var recordColumns = (key + '#' + value).Split('#'); 

                    // Build the key from the specified Index KV file 
                    foreach (var indexColumn in indexFile.IndexColumns)
                    {
                        indexKey += recordColumns[ColumnsInfo.FindIndex(elem => elem.ColumnName == indexColumn)] + '#';
                    }
                    indexKey = indexKey.Remove(indexKey.Length - 1);
                    
                    if (indexFile.IsUnique)
                    {
                        MongoDB.InsertKVIntoCollection(indexFile.IndexFileName, indexKey, key);
                    }
                    else
                    {
                        if (MongoDB.CollectionContainsKey(indexFile.IndexFileName, indexKey))
                        {
                            // Append the new record PK to the value of the index key 
                            var indexValue = MongoDB.GetRecordValueWithKey(indexFile.IndexFileName, indexKey) + '#' + key;
                            MongoDB.RemoveKVFromCollection(indexFile.IndexFileName, indexKey);
                            MongoDB.InsertKVIntoCollection(indexFile.IndexFileName, indexKey, indexValue);
                        }
                        else
                        {
                            MongoDB.InsertKVIntoCollection(indexFile.IndexFileName, indexKey, key);
                        }
                    }
                }
  
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
            // Return a list with all the data about all unique keys on the table 
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
}
