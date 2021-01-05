using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace ServerApp.Queries
{
    class SelectWithJoinQuery : AbstractQuery
    {
        private string DatabaseName;
        private MongoDBAcess MongoDB;
        private string Attributes;

        private bool SelectAllFlag; // set to true when the entire result of the JOIN operation needs to be displayed

        private List<Tuple<string, List<string>>> SelectedTablesStructure;
        private List<SelectJoinInfo> SelectJoinInfo;

        private List<SelectRowInfo> SelectConfigList;

        // Key = TableName, Value = list of (ColumnName, AliasName) 
        private List<KeyValuePair<string, List<Tuple<string, string>>>> OutputParamsAliasList;
        private List<KeyValuePair<string, List<Tuple<string, string>>>> WhereConditionsList;
        private List<KeyValuePair<string, List<Tuple<string, string>>>> AggregateFunctionList;
        private List<KeyValuePair<string, List<Tuple<string, string>>>> HavingParamsList;

        // Key = TableName, Value = list of ColumnName
        private List<KeyValuePair<string, List<string>>> GroupByList;

        public SelectWithJoinQuery(string _databaseName, string _attributes)
        {
            DatabaseName = _databaseName;
            Attributes = _attributes;
            MongoDB = new MongoDBAcess(DatabaseName);

            SelectAllFlag = false;
            SelectedTablesStructure = new List<Tuple<string, List<string>>>();
            SelectJoinInfo = new List<SelectJoinInfo>();

            SelectConfigList = new List<SelectRowInfo>();
            OutputParamsAliasList = new List<KeyValuePair<string, List<Tuple<string, string>>>>();
            WhereConditionsList = new List<KeyValuePair<string, List<Tuple<string, string>>>>();
            AggregateFunctionList = new List<KeyValuePair<string, List<Tuple<string, string>>>>();
            GroupByList = new List<KeyValuePair<string, List<string>>>();
            HavingParamsList = new List<KeyValuePair<string, List<Tuple<string, string>>>>();
        }

        public override void ParseAttributes()
        {
            var splitAttributes = Attributes.Split('|');
            var selectedTables = new List<string>();

            if (splitAttributes[0].Equals("SELECT_ALL"))
            {
                SelectAllFlag = true;

                for (int idx = 1; idx < splitAttributes.Length; idx++)
                {
                    SelectJoinInfo.Add(new SelectJoinInfo(splitAttributes[idx]));

                    var leftTable = splitAttributes[idx].Split('#')[0];
                    var rightTable = splitAttributes[idx].Split('#')[1];

                    if (!selectedTables.Contains(leftTable))
                    {
                        selectedTables.Add(leftTable);
                    }

                    if (!selectedTables.Contains(rightTable))
                    {
                        selectedTables.Add(rightTable);
                    }
                }
            }
            else
            {
                foreach (var selectAttribute in splitAttributes[0].Split('*'))
                {
                    if (selectAttribute != "")
                    {
                        SelectJoinInfo.Add(new SelectJoinInfo(selectAttribute));

                        var leftTable = selectAttribute.Split('#')[0];
                        var rightTable = selectAttribute.Split('#')[1];

                        if (!selectedTables.Contains(leftTable))
                        {
                            selectedTables.Add(leftTable);
                        }

                        if (!selectedTables.Contains(rightTable))
                        {
                            selectedTables.Add(rightTable);
                        }
                    }
                }

                foreach (var columnAttribute in splitAttributes[1].Split('*'))
                {
                    if (columnAttribute != "")
                    {
                        var newColumnConfig = new SelectRowInfo(columnAttribute);
                        SelectConfigList.Add(newColumnConfig);
                    }
                }

                ParseSelectConfig();
            }

            foreach (var table in selectedTables)
            {
                SelectedTablesStructure.Add(new Tuple<string, List<string>>(table, TableUtils.GetTableColumns(DatabaseName, table)));
            }
        }

        private void ParseSelectConfig()
        {
            foreach (var column in SelectConfigList)
            {
                if (column.Output)
                {
                    var outputOption = new KeyValuePair<string, List<Tuple<string, string>>>();
                    if (!OutputParamsAliasList.Any(elem => elem.Key == column.TableName))
                    {
                        OutputParamsAliasList.Add(new KeyValuePair<string, List<Tuple<string, string>>>(column.TableName, new List<Tuple<string, string>>()));
                    }
                    outputOption = OutputParamsAliasList.Find(elem => elem.Key == column.TableName);

                    if (column.Alias != "-")
                    {
                        outputOption.Value.Add(new Tuple<string, string>(column.ColumnName, column.Alias));
                    }
                    else
                    {
                        outputOption.Value.Add(new Tuple<string, string>(column.ColumnName, column.ColumnName));
                    }
                }

                if (column.Filter != "-")
                {
                    var filterOption = new KeyValuePair<string, List<Tuple<string, string>>>();
                    if (!WhereConditionsList.Any(elem => elem.Key == column.TableName))
                    {
                        WhereConditionsList.Add(new KeyValuePair<string, List<Tuple<string, string>>>(column.TableName, new List<Tuple<string, string>>()));
                    }
                    filterOption = WhereConditionsList.Find(elem => elem.Key == column.TableName);

                    filterOption.Value.Add(new Tuple<string, string>(column.ColumnName, column.Filter));
                }

                if (column.GroupBy)
                {
                    var groupByOption = new KeyValuePair<string, List<string>>();
                    if (!GroupByList.Any(elem => elem.Key == column.TableName))
                    {
                        GroupByList.Add(new KeyValuePair<string, List<string>>(column.TableName, new List<string>()));
                    }
                    groupByOption = GroupByList.Find(elem => elem.Key == column.TableName);

                    groupByOption.Value.Add(column.ColumnName);
                }

                if (column.Having != "-")
                {
                    var havingOption = new KeyValuePair<string, List<Tuple<string, string>>>();
                    if (!HavingParamsList.Any(elem => elem.Key == column.TableName))
                    {
                        HavingParamsList.Add(new KeyValuePair<string, List<Tuple<string, string>>>(column.TableName, new List<Tuple<string, string>>()));
                    }
                    havingOption = HavingParamsList.Find(elem => elem.Key == column.TableName);

                    havingOption.Value.Add(new Tuple<string, string>(column.ColumnName, column.Having));
                }

                if (column.Aggregate != "-")
                {
                    var aggregateOption = new KeyValuePair<string, List<Tuple<string, string>>>();
                    if (!AggregateFunctionList.Any(elem => elem.Key == column.TableName))
                    {
                        AggregateFunctionList.Add(new KeyValuePair<string, List<Tuple<string, string>>>(column.TableName, new List<Tuple<string, string>>()));
                    }
                    aggregateOption = AggregateFunctionList.Find(elem => elem.Key == column.TableName);

                    aggregateOption.Value.Add(new Tuple<string, string>(column.ColumnName, column.Aggregate));
                }
            }
        }

        public new string Execute()
        {
            try
            {
                ParseAttributes();
                if (SelectAllFlag)
                {
                    // SELECT * from the result of the join
                    return Commands.MapCommandToSuccessResponse(Commands.SELECT_RECORDS_WITH_JOIN) + ";" + SelectEntireJoinResult();
                }
                else
                {
                    // SELECT statement with some criteria and/or output options
                    return Commands.MapCommandToSuccessResponse(Commands.SELECT_RECORDS_WITH_JOIN) + ";" + SelectRecords();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string SelectEntireJoinResult()
        {
            try
            {
                var intermJoinStructure = new List<string>();
                var intermJoinRecords = new List<List<string>>();
                var joinedTables = new List<string>();

                for (int idx = 0; idx < SelectJoinInfo.Count; idx++)
                {
                    var leftTable = SelectedTablesStructure.Find(elem => elem.Item1 == SelectJoinInfo[idx].LeftTableName);
                    var rightTable = SelectedTablesStructure.Find(elem => elem.Item1 == SelectJoinInfo[idx].RightTableName);

                    if (intermJoinStructure.Count == 0)
                    {
                        intermJoinStructure = JoinTableStructures(leftTable.Item2, rightTable.Item2);
                        joinedTables.Add(leftTable.Item1);
                        joinedTables.Add(rightTable.Item1);

                        var keyValuePairs = MongoDB.GetEntireCollection(SelectJoinInfo[idx].LeftTableName);
                        var leftTableRecords = new List<List<string>>();
                        foreach (var keyValue in keyValuePairs)
                        {
                            var key = keyValue.GetElement("_id").Value.ToString().Split('#');
                            var value = keyValue.GetElement("value").Value.ToString().Split('#');
                            var tableRecord = new List<string>();
                            tableRecord.AddRange(key);
                            tableRecord.AddRange(value);
                            leftTableRecords.Add(tableRecord);
                        }

                        keyValuePairs = MongoDB.GetEntireCollection(SelectJoinInfo[idx].FKFileName);
                        var rightTableRecords = new List<List<string>>();
                        foreach (var keyValue in keyValuePairs)
                        {
                            var key = keyValue.GetElement("_id").Value.ToString().Split('#');
                            var value = keyValue.GetElement("value").Value.ToString().Split('#');
                            var tableRecord = new List<string>();
                            tableRecord.AddRange(key);
                            tableRecord.AddRange(value);
                            rightTableRecords.Add(tableRecord);
                        }

                        intermJoinRecords = PerformIndexedNestedLoopsJoin(intermJoinStructure, leftTableRecords, rightTableRecords, SelectJoinInfo[idx].FKColumn, SelectJoinInfo[idx].RightTableName, true);
                    }
                    else
                    {
                        intermJoinStructure = JoinTableStructures(intermJoinStructure, leftTable.Item2);
                        joinedTables.Add(leftTable.Item1);

                        var keyValuePairs = MongoDB.GetEntireCollection(leftTable.Item1);
                        var rightTableRecords = new List<List<string>>();
                        foreach (var keyValue in keyValuePairs)
                        {
                            var key = keyValue.GetElement("_id").Value.ToString().Split('#');
                            var value = keyValue.GetElement("value").Value.ToString().Split('#');
                            var tableRecord = new List<string>();
                            tableRecord.AddRange(key);
                            tableRecord.AddRange(value);
                            rightTableRecords.Add(tableRecord);
                        }

                        intermJoinRecords = PerformIndexedNestedLoopsJoin(intermJoinStructure, intermJoinRecords, rightTableRecords, SelectJoinInfo[idx].FKColumn, leftTable.Item1, false);
                    }
                }

                var outHeader = "";
                foreach (var outCol in intermJoinStructure)
                {
                    outHeader += outCol + "#";
                }
                outHeader = outHeader.Remove(outHeader.Length - 1);

                var outRecords = "";
                foreach (var outRec in intermJoinRecords)
                {
                    foreach (var outRecCol in outRec)
                    {
                        outRecords += outRecCol + '#';
                    }
                    outRecords = outRecords.Remove(outRecords.Length - 1) + '|';
                }

                return outHeader + ";" + outRecords.Remove(outRecords.Length - 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string SelectRecords()
        {
            try
            {
                var records = "";
                var joinedTables = new List<string>();
                var interimJoinStructure = new List<string>();
                var interimJoinRecords = new List<List<string>>();

                // Restrict the columns by the WHERE conditions, where it is possible
                for (int idx = 0; idx < SelectJoinInfo.Count; idx++)
                {
                    var leftTable = SelectedTablesStructure.Find(elem => elem.Item1 == SelectJoinInfo[idx].LeftTableName);
                    var rightTable = SelectedTablesStructure.Find(elem => elem.Item1 == SelectJoinInfo[idx].RightTableName);

                    if (interimJoinStructure.Count == 0)
                    {
                        interimJoinStructure = JoinTableStructures(leftTable.Item2, rightTable.Item2);
                        joinedTables.Add(leftTable.Item1);
                        joinedTables.Add(rightTable.Item1);

                        var leftTableRecords = new List<List<string>>();
                        var rightTableRecords = new List<List<string>>();

                        var indexInfo = CheckTableCanBeRestricted(leftTable.Item1).Split('|');
                        if (indexInfo[0] != "TABLE_SCAN")
                        {
                            if (indexInfo[0] == "WHERE")
                            {
                                leftTableRecords = SelectWithIndexWhere(indexInfo[1], leftTable.Item1);
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            var keyValuePairs = MongoDB.GetEntireCollection(SelectJoinInfo[idx].LeftTableName);
                            foreach (var keyValue in keyValuePairs)
                            {
                                var key = keyValue.GetElement("_id").Value.ToString().Split('#');
                                var value = keyValue.GetElement("value").Value.ToString().Split('#');
                                var tableRecord = new List<string>();
                                tableRecord.AddRange(key);
                                tableRecord.AddRange(value);
                                leftTableRecords.Add(tableRecord);
                            }
                        }

                        indexInfo = CheckTableCanBeRestricted(rightTable.Item1).Split('|');
                        if (indexInfo[0] != "TABLE_SCAN")
                        {
                            if (indexInfo[0] == "WHERE")
                            {
                                rightTableRecords = SelectWithIndexWhere(indexInfo[1], rightTable.Item1);
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            var keyValuePairs = MongoDB.GetEntireCollection(SelectJoinInfo[idx].FKFileName);
                            foreach (var keyValue in keyValuePairs)
                            {
                                var key = keyValue.GetElement("_id").Value.ToString().Split('#');
                                var value = keyValue.GetElement("value").Value.ToString().Split('#');
                                var tableRecord = new List<string>();
                                tableRecord.AddRange(key);
                                tableRecord.AddRange(value);
                                rightTableRecords.Add(tableRecord);
                            }
                        }

                        interimJoinRecords = PerformIndexedNestedLoopsJoin(interimJoinStructure, leftTableRecords, rightTableRecords, SelectJoinInfo[idx].FKColumn, SelectJoinInfo[idx].RightTableName, true);
                    }
                    else
                    {
                        interimJoinStructure = JoinTableStructures(interimJoinStructure, leftTable.Item2);
                        joinedTables.Add(leftTable.Item1);

                        var indexInfo = CheckTableCanBeRestricted(leftTable.Item1).Split('|');
                        var rightTableRecords = new List<List<string>>();
                        if (indexInfo[0] != "TABLE_SCAN")
                        {
                            if (indexInfo[0] == "WHERE")
                            {
                                rightTableRecords = SelectWithIndexWhere(indexInfo[1], leftTable.Item1);
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            var keyValuePairs = MongoDB.GetEntireCollection(SelectJoinInfo[idx].LeftTableName);
                            foreach (var keyValue in keyValuePairs)
                            {
                                var key = keyValue.GetElement("_id").Value.ToString().Split('#');
                                var value = keyValue.GetElement("value").Value.ToString().Split('#');
                                var tableRecord = new List<string>();
                                tableRecord.AddRange(key);
                                tableRecord.AddRange(value);
                                rightTableRecords.Add(tableRecord);
                            }
                        }

                        interimJoinRecords = PerformIndexedNestedLoopsJoin(interimJoinStructure, interimJoinRecords, rightTableRecords, SelectJoinInfo[idx].FKColumn, leftTable.Item1, false);
                    }
                }

                var filteredRecords = ApplyWhereConditions(interimJoinRecords, interimJoinStructure);

                return records;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string CheckTableCanBeRestricted(string tableName)
        {
            var whereConditionIndex = CheckForIndex(WhereConditionsList, tableName);
            var projectionConditionIndex = CheckForIndex(OutputParamsAliasList, tableName);

            if (whereConditionIndex == "" && projectionConditionIndex == "")
            {
                return "TABLE_SCAN";
            }

            if (whereConditionIndex != "")
            {
                return "WHERE" + "|" + whereConditionIndex;
            }
            else
            {
                return "OUTPUT" + "|" + projectionConditionIndex;
            }
        }

        private string CheckForIndex(List<KeyValuePair<string, List<Tuple<string, string>>>> conditionList, string tableName)
        {
            if (!conditionList.Any(elem => elem.Key == tableName))
            {
                return "";
            }

            var conditionsForTable = conditionList.Find(elem => elem.Key == tableName).Value;
            var columnNames = "";
            foreach (var column in conditionsForTable)
            {
                columnNames += column.Item1 + "_";
            }
            columnNames = columnNames.Remove(columnNames.Length - 1);

            // check if any unique key index files can be used
            var uniqueFiles = TableUtils.GetUniqueFiles(DatabaseName, tableName);
            foreach (var unique in uniqueFiles)
            {
                if (unique.Contains(columnNames))
                {
                    return unique;
                }
            }

            var searchedIndexName = "Index_" + tableName + "_" + columnNames;
            var indexFiles = TableUtils.GetIndexFiles(DatabaseName, tableName);
            foreach (var index in indexFiles)
            {
                if (index.IndexFileName.Contains(searchedIndexName))
                {
                    // an index is used in the selection condition if the attributes are a prefix of the attributes in an existing index 
                    return index.IndexFileName;
                }
            }

            return "";
        }

        private List<List<string>> SelectWithIndexWhere(string indexName, string tableName)
        {
            try
            {
                var records = new List<List<string>>();

                // only returns the names of the columns in the index, discarding the other parts of the index name (INDEX_<TableName>_....) 
                var columnsIndex = indexName.Split('_').Where(elem => elem != "Index" && !tableName.Contains(elem)).ToList();

                if (columnsIndex.Count == 1)
                {
                    // single-attribute index 
                    FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Empty;
                    foreach (var condition in WhereConditionsList.Find(elem => elem.Key == tableName).Value)
                    {
                        var conditionSplit = condition.Item2.Split(' ');
                        switch (conditionSplit[0])
                        {
                            case "=":
                                {
                                    filter &= Builders<BsonDocument>.Filter.Eq("_id", conditionSplit[1]);
                                }
                                break;
                            case "<":
                                {
                                    filter &= Builders<BsonDocument>.Filter.Lt("_id", conditionSplit[1]);
                                }
                                break;
                            case ">":
                                {
                                    filter &= Builders<BsonDocument>.Filter.Gt("_id", conditionSplit[1]);
                                }
                                break;
                            case "<=":
                                {
                                    filter &= Builders<BsonDocument>.Filter.Lte("_id", conditionSplit[1]);
                                }
                                break;
                            case ">=":
                                {
                                    filter &= Builders<BsonDocument>.Filter.Gte("_id", conditionSplit[1]);
                                }
                                break;
                            case "<>":
                                {
                                    filter &= Builders<BsonDocument>.Filter.Ne("_id", conditionSplit[1]);
                                }
                                break;
                            case "!=":
                                {
                                    filter &= Builders<BsonDocument>.Filter.Ne("_id", conditionSplit[1]);
                                }
                                break;
                        }
                    }

                    var keyValuePairs = MongoDB.GetCollectionFilteredByKey(indexName, filter);
                    foreach (var keyValue in keyValuePairs)
                    {
                        // if we use an index file for reading data, only the value needs to be kept, for key-lookup
                        var keys = keyValue.GetElement("value").Value.ToString().Split('#').ToList();
                        records.AddRange(PerformKeyLookup(keys, tableName));
                    }
                }
                else
                {
                    // multi-attribute index => needs separate filtering 
                }

                return records;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<List<string>> PerformKeyLookup(List<string> keys, string tableName)
        {
            var records = new List<List<string>>();

            FilterDefinition<BsonDocument> filter = default;
            foreach (var key in keys)
            {
                var keySplit = key.Split('#');

                foreach (var filterVal in keySplit)
                {
                    if (filter == null)
                    {
                        filter = Builders<BsonDocument>.Filter.Eq("_id", filterVal);
                    }
                    else
                    {
                        filter |= Builders<BsonDocument>.Filter.Eq("_id", filterVal);
                    }
                }
            }

            var keyValuePairs = MongoDB.GetCollectionFilteredByKey(tableName, filter);
            foreach (var keyValue in keyValuePairs)
            {
                records.Add((keyValue.GetElement("_id").Value + "#" + keyValue.GetElement("value").Value).Split('#').ToList());
            }

            return records;
        }

        private List<string> JoinTableStructures(List<string> leftTable, List<string> rightTable)
        {
            var intermStructure = new List<string>();

            foreach (var column in leftTable)
            {
                intermStructure.Add(column);
            }

            foreach (var column in rightTable)
            {
                if (!intermStructure.Contains(column))
                {
                    intermStructure.Add(column);
                }
            }

            return intermStructure;
        }

        private List<List<string>> ApplyWhereConditions(List<List<string>> joinResultRecords, List<string> joinResultStructure)
        {
            var records = new List<List<string>>();

            foreach (var record in joinResultRecords)
            {
                if (RecordMatchesCondition(record, joinResultStructure))
                {
                    records.Add(record);
                }
            }

            return records;
        }

        private List<List<string>> ApplyProjection(List<List<string>> joinResultRecords, List<string> joinResultStructure)
        {
            var outputColumns = new List<List<string>>();

            if (AggregateFunctionList.Count == 0)
            {
                // Select the values of the columns from the record and add them to the output
                foreach (var record in joinResultRecords)
                {
                    var outputLine = new List<string>();
                    foreach (var outputCol in OutputParamsAliasList)
                    {

                    }
                }
            }
            else
            {
                // Collect the result of the aggregations => This will always be one row 
            }

            return outputColumns;
        }

        private bool RecordMatchesCondition(List<string> record, List<string> structure)
        {
            foreach (var condition in GetAllWhereConditions())
            {
                var columnValue = record[structure.IndexOf(condition.Item1)];
                var conditionOperator = condition.Item2.Split(' ')[0];
                var conditionValue = condition.Item2.Split(' ')[1];

                if (conditionOperator == "=" && columnValue != conditionValue)
                {
                    return false;
                }

                if (conditionOperator == "<>" && columnValue == conditionValue)
                {
                    return false;
                }

                if (conditionOperator == "<")
                {
                    if (int.TryParse(columnValue, out int convertedColumn) && int.TryParse(conditionValue, out int convertedValue))
                    {
                        if (convertedColumn >= convertedValue)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (columnValue.CompareTo(conditionValue) >= 0)
                        {
                            return false;
                        }
                    }
                }

                if (conditionOperator == ">")
                {
                    if (int.TryParse(columnValue, out int convertedColumn) && int.TryParse(conditionValue, out int convertedValue))
                    {
                        if (convertedColumn <= convertedValue)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (columnValue.CompareTo(conditionValue) <= 0)
                        {
                            return false;
                        }
                    }
                }

                if (conditionOperator == "<=")
                {
                    if (int.TryParse(columnValue, out int convertedColumn) && int.TryParse(conditionValue, out int convertedValue))
                    {
                        if (convertedColumn > convertedValue)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (columnValue.CompareTo(conditionValue) > 0)
                        {
                            return false;
                        }
                    }
                }

                if (conditionOperator == ">=")
                {
                    if (int.TryParse(columnValue, out int convertedColumn) && int.TryParse(conditionValue, out int convertedValue))
                    {
                        if (convertedColumn < convertedValue)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (columnValue.CompareTo(conditionValue) < 0)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private List<Tuple<string, string>> GetAllWhereConditions()
        {
            var conditions = new List<Tuple<string, string>>();

            foreach (var conditionsTable in WhereConditionsList)
            {
                foreach (var condition in conditionsTable.Value)
                {
                    conditions.Add(condition);
                }
            }

            return conditions;
        }

        private List<List<string>> PerformIndexedNestedLoopsJoin(List<string> joinStructure, List<List<string>> leftTableRecords, List<List<string>> rightTableRecords, string fkColumn, string rightTableName, bool usesFK)
        {
            var records = new List<List<string>>();
            var rightTableStructure = SelectedTablesStructure.Find(elem => elem.Item1 == rightTableName).Item2;

            foreach (var leftRecord in leftTableRecords)
            {
                var fkColumnValue = leftRecord[joinStructure.IndexOf(fkColumn)];

                foreach (var rightRecord in rightTableRecords)
                {
                    if (rightRecord[0] == fkColumnValue)
                    {
                        if (usesFK == true)
                        {
                            for (int idx = 1; idx < rightRecord.Count; idx++)
                            {
                                var joinResultRecord = leftRecord.ToList();
                                var rightRecordColumns = (rightRecord[idx] + "#" + MongoDB.GetRecordValueWithKey(rightTableName, rightRecord[idx])).Split('#');
                                for (int jdx = 0; jdx < rightRecordColumns.Length; jdx++)
                                {
                                    if (jdx != rightTableStructure.IndexOf(fkColumn))
                                    {
                                        joinResultRecord.Add(rightRecordColumns[jdx]);
                                    }
                                }
                                records.Add(joinResultRecord);
                            }
                        }
                        else
                        {
                            var joinResultRecord = leftRecord.ToList();
                            for (int jdx = 0; jdx < rightRecord.Count; jdx++)
                            {
                                if (jdx != rightTableStructure.IndexOf(fkColumn))
                                {
                                    joinResultRecord.Add(rightRecord[jdx]);
                                }
                            }
                            records.Add(joinResultRecord);
                        }
                    }
                }
            }

            return records;
        }

        private List<string> PerformSortMergeJoin()
        {
            var records = new List<string>();

            return records;
        }
    }
}
