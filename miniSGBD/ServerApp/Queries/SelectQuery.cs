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
    class SelectQuery : AbstractQuery
    {
        private string DatabaseName;
        private string Attributes;
        private MongoDBAcess MongoDB;

        // only used for SELECT * from <table> 
        private string SelectAllTableName;

        private List<string> TablesUsed = new List<string>();
        private List<SelectRowInfo> SelectConfigList = new List<SelectRowInfo>();
        private List<Tuple<Tuple<string, string>, string>> OutputParamsAliasList = new List<Tuple<Tuple<string, string>, string>>();
        private List<Tuple<Tuple<string, string>, string>> WhereConditionsList = new List<Tuple<Tuple<string, string>, string>>();
        private List<Tuple<Tuple<string, string>, string>> HavingParamsList = new List<Tuple<Tuple<string, string>, string>>();
        private List<Tuple<string, string>> GroupByList = new List<Tuple<string, string>>();

        public SelectQuery(string _databaseName, string _attributes) : base(Commands.SELECT_RECORDS)
        {
            DatabaseName = _databaseName;
            Attributes = _attributes;
            MongoDB = new MongoDBAcess(DatabaseName);
        }

        public override void ParseAttributes()
        {
            var splitAttributes = Attributes.Split('|');

            if (splitAttributes[0].Contains("SELECT_ALL"))
            {
                SelectAllTableName = splitAttributes[0].Split('#')[1];
            }
            else
            {
                foreach (var attribute in splitAttributes)
                {
                    if (attribute != "")
                    {
                        var newColumnConfig = new SelectRowInfo(attribute);
                        SelectConfigList.Add(newColumnConfig);

                        if (!TablesUsed.Contains(newColumnConfig.TableName))
                        {
                            TablesUsed.Add(newColumnConfig.TableName);
                        }
                    }
                }

                ParseSelectConfig();
            }
        }

        private void ParseSelectConfig()
        {
            foreach (var column in SelectConfigList)
            {
                if (column.Output)
                {
                    if (column.Alias == "-")
                    {
                        OutputParamsAliasList.Add(new Tuple<Tuple<string, string>, string>(new Tuple<string, string>(column.TableName, column.ColumnName), column.ColumnName));
                    }
                    else
                    {
                        OutputParamsAliasList.Add(new Tuple<Tuple<string, string>, string>(new Tuple<string, string>(column.TableName, column.ColumnName), column.Alias));
                    }
                }

                if (column.Filter != "-")
                {
                    WhereConditionsList.Add(new Tuple<Tuple<string, string>, string>(new Tuple<string, string>(column.TableName, column.ColumnName), column.Filter));
                }

                if (column.GroupBy)
                {
                    GroupByList.Add(new Tuple<string, string>(column.TableName, column.ColumnName));
                }

                if (column.Having != "-")
                {
                    HavingParamsList.Add(new Tuple<Tuple<string, string>, string>(new Tuple<string, string>(column.TableName, column.ColumnName), column.Having));
                }
            }
        }

        public new string Execute()
        {
            try
            {
                ParseAttributes();

                var selectionResult = "";

                if (SelectAllTableName != null)
                {
                    selectionResult = SelectEntireTable();
                }
                else
                {
                    if (TablesUsed.Count == 1)
                    {
                        var records = new List<string>();

                        var whereConditionPrimaryKey = CheckForPrimaryKey(WhereConditionsList);
                        var projectionConditionPrimaryKey = CheckForPrimaryKey(OutputParamsAliasList);

                        if (whereConditionPrimaryKey != "")
                        {
                            records = SelectWithIndexWhere(TablesUsed[0]);
                        }

                        if (projectionConditionPrimaryKey != "" && records.Count == 0)
                        {
                            records = SelectWithIndexProjection(TablesUsed[0]);
                        }

                        var whereConditionIndex = CheckForIndex(WhereConditionsList);
                        var projectionConditionIndex = CheckForIndex(OutputParamsAliasList);

                        if (whereConditionIndex == "" && projectionConditionIndex == "" && records.Count == 0)
                        {
                            records = SelectWithTableScan();
                        }

                        if (whereConditionIndex != "" && records.Count == 0)
                        {
                            records = SelectWithIndexWhere(whereConditionIndex);
                        }
                        else
                        {
                            if (records.Count != 0)
                            {
                                records = SelectWithIndexProjection(projectionConditionIndex);
                            }         
                        }
                    }
                    else
                    {
                        // TODO: when JOIN algorithms are implemented 
                        //              => methods that check if the JOIN tables can be restricted using index files 
                    }
                }

                return selectionResult;
            }
            catch (Exception ex)
            {
                return ex.Message + ";";
            }
        }

        private string SelectEntireTable()
        {
            try
            {
                var records = "";
                var keyValuePairs = MongoDB.GetEntireCollection(SelectAllTableName);
                foreach (var keyValue in keyValuePairs)
                {
                    records += keyValue.GetElement("_id").Value + "#" + keyValue.GetElement("value").Value + "|";
                }

                return Commands.MapCommandToSuccessResponse(Commands.SELECT_RECORDS) + ";" + records;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<string> SelectWithTableScan()
        {
            try
            {
                var records = new List<string>();

                return records; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<string> SelectWithIndexProjection(string indexName)
        {
            try
            {
                var records = new List<string>();
                var keyValuePairs = MongoDB.GetEntireCollection(indexName);
                foreach (var keyValue in keyValuePairs)
                {
                    records.Add(keyValue.GetElement("_id").Value.ToString());
                }
                return records;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<string> SelectWithIndexWhere(string indexName)
        {
            try
            {
                var records = new List<string>();

                var columnsIndex = indexName.Split('_').Where(elem => elem != "Index" && !TablesUsed.Contains(elem)).ToList();

                if (columnsIndex.Count == 1 || !indexName.Contains("_"))
                {
                    // single-attribute index or selecting using a primary key condition 
                    FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Empty;
                    foreach (var condition in WhereConditionsList)
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
                        records.Add(keyValue.GetElement("_id").Value + "#" + keyValue.GetElement("value").Value);
                    }
                }
                else
                {
                    // multi-attribute index => needs separate filtering 
                    var unfilteredRecords = MongoDB.GetEntireCollection(indexName);

                    var groupedConditions = new List<KeyValuePair<string, List<string>>>();
                    foreach (var condition in WhereConditionsList)
                    {
                        if (groupedConditions.Find(elem => elem.Key == condition.Item1.Item2).Value == null)
                        {
                            groupedConditions.Add(new KeyValuePair<string, List<string>>(condition.Item1.Item2, new List<string>()));
                        }

                        groupedConditions.Find(elem => elem.Key == condition.Item1.Item2).Value.Add(condition.Item2);
                    }

                    foreach (var indexRecord in unfilteredRecords)
                    {
                        var indRec = indexRecord.GetElement("_id").Value + "#" + indexRecord.GetElement("value").Value;
                        if (RecordMatchesConditions(indRec, groupedConditions))
                        {
                            records.Add(indRec);
                        }
                    }
                }

                return records;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool RecordMatchesConditions(string record, List<KeyValuePair<string, List<string>>> conditions)
        {
            var recordMatches = true;
            var columns = record.Split('#');

            for (int idx = 0; idx < conditions.Count; idx++)
            {
                var conditionsForColumn = conditions[idx].Value;
                foreach (var condition in conditionsForColumn)
                {
                    var conditionOperator = condition.Split(' ')[0];
                    var conditionValue = condition.Split(' ')[1];

                    if (conditionOperator == "=" && columns[idx] != conditionValue)
                    {
                        return false;
                    }

                    if (conditionOperator == "<>" && columns[idx] == conditionValue)
                    {
                        return false; 
                    }

                    if (conditionOperator == "<")
                    {
                        if (int.TryParse(columns[idx], out int convertedColumn) && int.TryParse(conditionValue, out int convertedValue))
                        {
                            if (convertedColumn >= convertedValue)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (columns[idx].CompareTo(conditionValue) >= 0)
                            {
                                return false;
                            }
                        }
                    }

                    if (conditionOperator == ">")
                    {
                        if (int.TryParse(columns[idx], out int convertedColumn) && int.TryParse(conditionValue, out int convertedValue))
                        {
                            if (convertedColumn <= convertedValue)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (columns[idx].CompareTo(conditionValue) <= 0)
                            {
                                return false;
                            }
                        }
                    }

                    if (conditionOperator == "<=")
                    {
                        if (int.TryParse(columns[idx], out int convertedColumn) && int.TryParse(conditionValue, out int convertedValue))
                        {
                            if (convertedColumn > convertedValue)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (columns[idx].CompareTo(conditionValue) > 0)
                            {
                                return false;
                            }
                        }
                    }

                    if (conditionOperator == ">=")
                    {
                        if (int.TryParse(columns[idx], out int convertedColumn) && int.TryParse(conditionValue, out int convertedValue))
                        {
                            if (convertedColumn < convertedValue)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (columns[idx].CompareTo(conditionValue) < 0)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return recordMatches; 
        }

        private string CheckForPrimaryKey(List<Tuple<Tuple<string, string>, string>> conditionList)
        {
            if (conditionList.Count == 0)
            {
                return "";
            }

            var columnsUsed = "";
            var primaryKeyColumns = TableUtils.GetPrimaryKey(DatabaseName, TablesUsed[0]);
            var primaryKeyColumnsString = "";

            foreach (var primaryKey in primaryKeyColumns)
            {
                primaryKeyColumnsString += primaryKey + "_";
            }
            primaryKeyColumnsString = primaryKeyColumnsString.Remove(primaryKeyColumnsString.Length - 1);

            foreach (var column in conditionList)
            {
                columnsUsed += column.Item1.Item2 + "_";
            }

            if (columnsUsed != "")
            {
                columnsUsed = columnsUsed.Remove(columnsUsed.Length - 1);

                if (primaryKeyColumnsString.Contains(columnsUsed))
                {
                    // the primary key of the table can be used if the attributes are a prefix of the attributes in the primary key structure
                    return columnsUsed;
                }
            }

            return "";
        }

        private string CheckForIndex(List<Tuple<Tuple<string, string>, string>> conditionList)
        {
            if (conditionList.Count == 0)
            {
                return "";
            }

            // build the index name containing the attributes from the condition 
            var searchedIndexName = "";
            foreach (var column in conditionList)
            {
                searchedIndexName += column.Item1.Item2 + "_";
            }
            searchedIndexName = searchedIndexName.Remove(searchedIndexName.Length - 1);

            var indexFiles = TableUtils.GetIndexFiles(DatabaseName, TablesUsed[0]);

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
    }
}
