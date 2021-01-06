using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Utils;

namespace ServerApp.Queries
{
    class SelectWithJoinQuery : AbstractQuery
    {
        private string DatabaseName;
        private MongoDBAcess MongoDB;
        private string Attributes;

        private List<Tuple<string, List<string>>> SelectedTablesStructure;
        private List<SelectJoinInfo> SelectJoinInfo;

        private List<SelectRowInfo> SelectConfigList;

        // Key = TableName, Value = list of (ColumnName, AliasName) 
        private List<KeyValuePair<string, List<Tuple<string, string>>>> OutputParamsAliasList;
        private List<KeyValuePair<string, List<Tuple<string, string>>>> WhereConditionsList;
        private List<Tuple<string, string>> AggregateFunctionList = new List<Tuple<string, string>>();
        private List<Tuple<Tuple<string, string>, string>> HavingParamsList = new List<Tuple<Tuple<string, string>, string>>();

        // Key = TableName, Value = list of ColumnName
        private List<Tuple<Tuple<string, string>, string>> GroupByList = new List<Tuple<Tuple<string, string>, string>>();

        public SelectWithJoinQuery(string _databaseName, string _attributes)
        {
            DatabaseName = _databaseName;
            Attributes = _attributes;
            MongoDB = new MongoDBAcess(DatabaseName);

            SelectedTablesStructure = new List<Tuple<string, List<string>>>();
            SelectJoinInfo = new List<SelectJoinInfo>();

            SelectConfigList = new List<SelectRowInfo>();
            OutputParamsAliasList = new List<KeyValuePair<string, List<Tuple<string, string>>>>();
            WhereConditionsList = new List<KeyValuePair<string, List<Tuple<string, string>>>>();
            AggregateFunctionList = new List<Tuple<string, string>>();
            GroupByList = new List<Tuple<Tuple<string, string>, string>>();
            HavingParamsList = new List<Tuple<Tuple<string, string>, string>>();
        }

        public override void ParseAttributes()
        {
            var splitAttributes = Attributes.Split('|');
            var selectedTables = new List<string>();

            foreach (var selectAttribute in splitAttributes[0].Split('*'))
            {
                if (selectAttribute != "")
                {
                    SelectJoinInfo.Add(new SelectJoinInfo(selectAttribute));

                    var leftTable = selectAttribute.Split('#')[0];
                    var rightTable = selectAttribute.Split('#')[2];

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

            for (int idx = 1; idx < splitAttributes.Length; idx++)
            {
                if (splitAttributes[idx] != "")
                {
                    var newColumnConfig = new SelectRowInfo(splitAttributes[idx]);
                    SelectConfigList.Add(newColumnConfig);
                }
            }

            ParseSelectConfig();

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
                    GroupByList.Add(new Tuple<Tuple<string, string>, string>(new Tuple<string, string>(column.TableName, column.ColumnName), column.ColumnName));
                }

                if (column.Having != "-")
                {
                    HavingParamsList.Add(new Tuple<Tuple<string, string>, string>(new Tuple<string, string>(column.TableName, column.ColumnName), column.Having));
                }

                if (column.Aggregate != "-")
                {
                    if (column.Aggregate != "-")
                    {
                        AggregateFunctionList.Add(new Tuple<string, string>(column.ColumnName, column.Aggregate));
                    }
                }
            }
        }

        public new string Execute()
        {
            try
            {
                ParseAttributes();
                return Commands.MapCommandToSuccessResponse(Commands.SELECT_RECORDS_WITH_JOIN) + ";" + SelectRecords();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string SelectRecords()
        {
            try
            {
                var interimJoinStructure = new List<string>();
                var interimJoinRecords = new List<List<string>>();

                for (int idx = 0; idx < SelectJoinInfo.Count; idx++)
                {
                    var leftTable = SelectedTablesStructure.Find(elem => elem.Item1 == SelectJoinInfo[idx].LeftTableName);
                    var rightTable = SelectedTablesStructure.Find(elem => elem.Item1 == SelectJoinInfo[idx].RightTableName);

                    var leftIndex = JoinColumnUsesIndex(SelectJoinInfo[idx].LeftTableName, SelectJoinInfo[idx].LeftTableColumn);
                    var rightIndex = JoinColumnUsesIndex(SelectJoinInfo[idx].RightTableName, SelectJoinInfo[idx].RightTableColumn);

                    if (rightIndex != "")
                    {
                        // Index Nested Join Loops using the right table index as the inner 
                        interimJoinStructure = JoinTableStructures(leftTable.Item2, rightTable.Item2);
                        var outerTableRecords = GetRecordsForSelection(leftTable.Item1);
                        var innerTableRecords = GetRecordsForSelectionIndex(rightIndex);
                        interimJoinRecords = PerformIndexedNestedLoopsJoin(interimJoinStructure, outerTableRecords, innerTableRecords, SelectJoinInfo[idx].LeftTableColumn, SelectJoinInfo[idx].RightTableName, true);
                    }
                    else
                    {
                        if (leftIndex != "")
                        {
                            // Perform Index Nested Loops with the left table as an inner 
                            interimJoinStructure = JoinTableStructures(interimJoinStructure, rightTable.Item2);
                            var outerTableRecords = GetRecordsForSelection(rightTable.Item1);
                            interimJoinRecords = PerformIndexedNestedLoopsJoin(interimJoinStructure, interimJoinRecords, outerTableRecords, SelectJoinInfo[idx].RightTableColumn, SelectJoinInfo[idx].RightTableName, false);
                        }
                        else
                        {
                            // No Index on the join column => Perform Hash Join
                            if (interimJoinRecords.Count != 0)
                            {
                                interimJoinStructure = JoinTableStructures(interimJoinStructure, rightTable.Item2);
                            }
                            else
                            {
                                interimJoinRecords = GetRecordsForSelection(leftTable.Item1);
                                interimJoinStructure = JoinTableStructures(leftTable.Item2, rightTable.Item2);
                            }
                            var outerTableRecords = GetRecordsForSelection(rightTable.Item1);
                            interimJoinRecords = PerformHashJoin(interimJoinStructure, interimJoinRecords, outerTableRecords, SelectJoinInfo[idx].LeftTableColumn, SelectJoinInfo[idx].RightTableColumn, SelectedTablesStructure.Find(elem => elem.Item1 == SelectJoinInfo[idx].RightTableName).Item2);
                        }
                    }
                }

                if (GroupByList.Count > 0)
                {
                    var groupByRecords = new List<string>();
                    foreach (var record in interimJoinRecords)
                    {
                        var stringRecord = "";
                        foreach (var col in record)
                        {
                            stringRecord += col + "#";
                        }
                        groupByRecords.Add(stringRecord.Remove(stringRecord.Length - 1));
                    }

                    var groups = ApplyGroupBy(groupByRecords, interimJoinStructure);
                    var output = "";
                    foreach (var group in groups)
                    {
                        output += group + "|";
                    }

                    return ReturnOutputHeader() + ";" + output.Remove(output.Length - 1);
                }

                return ReturnOutputHeader() + ";" + PerformProjection(interimJoinRecords, interimJoinStructure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        private List<List<string>> GetRecordsForSelection(string tableName)
        {
            var records = new List<List<string>>();

            var indexInfo = CheckTableCanBeRestricted(tableName).Split('|');
            if (indexInfo[0] == "WHERE")
            {
                records = SelectWithIndexWhere(indexInfo[1], tableName);
            }
            else
            {
                var keyValuePairs = MongoDB.GetEntireCollection(tableName);
                foreach (var keyValue in keyValuePairs)
                {
                    var key = keyValue.GetElement("_id").Value.ToString().Split('#');
                    var value = keyValue.GetElement("value").Value.ToString().Split('#');
                    var tableRecord = new List<string>();
                    tableRecord.AddRange(key);
                    tableRecord.AddRange(value);
                    records.Add(tableRecord);
                }
            }

            // Further restrict by the rest of the conditions for that table 
            return ApplyWhereConditions(records, tableName);
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

        private List<List<string>> GetRecordsForSelectionIndex(string indexName)
        {
            var records = new List<List<string>>();

            var keyValuePairs = MongoDB.GetEntireCollection(indexName);
            foreach (var keyValue in keyValuePairs)
            {
                var key = keyValue.GetElement("_id").Value.ToString().Split('#');
                var value = keyValue.GetElement("value").Value.ToString().Split('#');
                var tableRecord = new List<string>();
                tableRecord.AddRange(key);
                tableRecord.AddRange(value);
                records.Add(tableRecord);
            }

            return records;
        }

        private List<List<string>> ApplyWhereConditions(List<List<string>> tableRecords, string table)
        {
            var records = new List<List<string>>();

            foreach (var record in tableRecords)
            {
                if (RecordMatchesCondition(table, record))
                {
                    records.Add(record);
                }
            }

            return records;
        }

        private bool RecordMatchesCondition(string table, List<string> record)
        {
            if (WhereConditionsList.Any(elem => elem.Key == table))
            {
                foreach (var condition in WhereConditionsList.Find(elem => elem.Key == table).Value)
                {
                    var columnValue = record[SelectedTablesStructure.Find(elem => elem.Item1 == table).Item2.IndexOf(condition.Item1)];
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
            }

            return true;
        }

        private string JoinColumnUsesIndex(string tableName, string columnName)
        {
            var indexFiles = TableUtils.GetForeignKeyData(DatabaseName, tableName);

            foreach (var file in indexFiles)
            {
                if (file[1] == columnName)
                {
                    return "FK_" + tableName + "_" + file[0];
                }
            }

            return "";
        }

        private string CheckTableCanBeRestricted(string tableName)
        {
            var whereConditionIndex = CheckForIndex(WhereConditionsList, tableName);

            if (whereConditionIndex == "")
            {
                return "TABLE_SCAN";
            }
            else
            {
                return "WHERE" + "|" + whereConditionIndex;
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

        private List<List<string>> PerformIndexedNestedLoopsJoin(List<string> joinStructure, List<List<string>> leftTableRecords, List<List<string>> rightTableRecords, string fkColumn, string rightTableName, bool indexOnRight)
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
                        if (indexOnRight == true)
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

        private List<List<string>> PerformHashJoin(List<string> joinStructure, List<List<string>> leftTableRecords, List<List<string>> rightTableRecords, string leftTableColumn, string rightTableColumn, List<string> rightTableStructure)
        {
            var records = new List<List<string>>();

            // Building phase
            var hashTable = new Hashtable();
            foreach (var record in leftTableRecords)
            {
                var joinColumnValue = record[joinStructure.IndexOf(leftTableColumn)];
                if (!hashTable.ContainsKey(joinColumnValue))
                {
                    hashTable.Add(joinColumnValue, new List<string>());
                }
                (hashTable[joinColumnValue] as List<string>).Add(record[0]);
            }

            // Probing phase  
            foreach (var rightRecord in rightTableRecords)
            {
                var joinColumnValue = rightRecord[rightTableStructure.IndexOf(rightTableColumn)];
                if (hashTable.ContainsKey(joinColumnValue))
                {
                    foreach (var leftRecordKey in hashTable[joinColumnValue] as List<string>)
                    {
                        var joinResultRecord = leftTableRecords.Find(elem => elem[0] == leftRecordKey).ToList();
                        for (int jdx = 0; jdx < rightRecord.Count; jdx++)
                        {
                            joinResultRecord.Add(rightRecord[jdx]);
                        }
                        records.Add(joinResultRecord);
                    }
                }
            }

            return records;
        }

        private string PerformProjection(List<List<string>> records, List<string> structure)
        {
            var outputRecords = new List<List<string>>();
            var outRecords = "";

            foreach (var record in records)
            {
                var outputRecord = new List<string>();
                foreach (var output in GetAllOutputOptions())
                {
                    outputRecord.Add(record[structure.IndexOf(output.Item1)]);
                }

                // Apply DISTINCT as well 
                if (!outputRecords.Contains(outputRecord))
                {
                    outputRecords.Add(outputRecord);
                }
            }

            foreach (var outRec in outputRecords)
            {
                foreach (var outCol in outRec)
                {
                    outRecords += outCol + "#";
                }
                outRecords = outRecords.Remove(outRecords.Length - 1) + "|";
            }

            return outRecords.Remove(outRecords.Length - 1);
        }

        private string ReturnOutputHeader()
        {
            var outHead = "";
            foreach (var output in GetAllOutputOptions())
            {
                outHead += output.Item2 + "#";
            }
            return outHead.Remove(outHead.Length - 1);
        }

        private List<Tuple<string, string>> GetAllOutputOptions()
        {
            var conditions = new List<Tuple<string, string>>();

            foreach (var conditionsTable in OutputParamsAliasList)
            {
                foreach (var condition in conditionsTable.Value)
                {
                    conditions.Add(condition);
                }
            }

            return conditions;
        }

        private List<string> ApplyGroupBy(List<string> records, List<string> structure)
        {
            var group = GroupByList[0].Item2;
            var groupByPK = new List<KeyValuePair<string, List<string>>>();
            var result = new List<string>();

            foreach (var rec in records)
            {
                var recordSplit = rec.Split('#');
                var value = recordSplit[structure.IndexOf(group)];
                if (!groupByPK.Any(elem => elem.Key == value))
                {
                    groupByPK.Add(new KeyValuePair<string, List<string>>(value, new List<string>()));
                }
                groupByPK.Find(elem => elem.Key == value).Value.Add(rec);
            }

            foreach (var record in groupByPK)
            {
                var aggregate = ApplyAggregate(record.Value, structure);

                var havingCond = HavingParamsList[0].Item2;
                if (RecordMatchesHaving(aggregate[0], havingCond))
                {
                    var outputColums = record.Value[0];

                    var outputRecord = aggregate[0] + '#';
                    var recordSplit = outputColums.Split('#');

                    outputRecord += recordSplit[structure.IndexOf(OutputParamsAliasList[1].Value[0].Item2)] + "#";
                    outputRecord += recordSplit[structure.IndexOf(OutputParamsAliasList[1].Value[1].Item2)] + "#";

                    result.Add(outputRecord.Remove(outputRecord.Length - 1));
                }
            }

            return result;
        }

        private List<string> ApplyAggregate(List<string> records, List<string> structure)
        {
            var outputColumns = new List<string>();
            var groups = new List<List<Tuple<string, string>>>();
            // Collect the result of the aggregations => This will always be one row 
            foreach (var output in AggregateFunctionList)
            {
                var selectedColumns = new List<Tuple<string, string>>();

                foreach (var record in records)
                {
                    var recordSplit = record.Split('#');
                    selectedColumns.Add(new Tuple<string, string>(recordSplit[0], recordSplit[structure.IndexOf(output.Item1)]));
                }
                groups.Add(selectedColumns);
            }

            const string COUNT = "COUNT";
            const string MIN = "MIN";
            const string MAX = "MAX";

            for (var i = 0; i < AggregateFunctionList.Count; i++)
            {
                switch (AggregateFunctionList[i].Item2)
                {
                    case COUNT:
                        var distinct = groups[i].Select(x => x.Item2).Distinct().ToList();
                        outputColumns.Add(distinct.Count.ToString());
                        break;
                    case MIN:
                        var min = groups[i].Min(x => Int32.Parse(x.Item2));
                        outputColumns.Add(min.ToString());
                        break;
                    case MAX:
                        var max = groups[i].Max(x => Int32.Parse(x.Item2));
                        outputColumns.Add(max.ToString());
                        break;
                }
            }
            return outputColumns;
        }

        private bool RecordMatchesHaving(string record, string condition)
        {
            var conditionOperator = condition.Split(' ')[0];
            var conditionValue = condition.Split(' ')[1];

            if (conditionOperator == "=" && record != conditionValue)
            {
                return false;
            }

            if (conditionOperator == "<>" && record == conditionValue)
            {
                return false;
            }

            if (conditionOperator == "<")
            {
                if (int.TryParse(record, out int convertedColumn) && int.TryParse(conditionValue, out int convertedValue))
                {
                    if (convertedColumn >= convertedValue)
                    {
                        return false;
                    }
                }
                else
                {
                    if (record.CompareTo(conditionValue) >= 0)
                    {
                        return false;
                    }
                }
            }

            if (conditionOperator == ">")
            {
                if (int.TryParse(record, out int convertedColumn) && int.TryParse(conditionValue, out int convertedValue))
                {
                    if (convertedColumn <= convertedValue)
                    {
                        return false;
                    }
                }
                else
                {
                    if (record.CompareTo(conditionValue) <= 0)
                    {
                        return false;
                    }
                }
            }

            if (conditionOperator == "<=")
            {
                if (int.TryParse(record, out int convertedColumn) && int.TryParse(conditionValue, out int convertedValue))
                {
                    if (convertedColumn > convertedValue)
                    {
                        return false;
                    }
                }
                else
                {
                    if (record.CompareTo(conditionValue) > 0)
                    {
                        return false;
                    }
                }
            }

            if (conditionOperator == ">=")
            {
                if (int.TryParse(record, out int convertedColumn) && int.TryParse(conditionValue, out int convertedValue))
                {
                    if (convertedColumn < convertedValue)
                    {
                        return false;
                    }
                }
                else
                {
                    if (record.CompareTo(conditionValue) < 0)
                    {
                        return false;
                    }
                }

            }
            return true;
        }
    }
}

