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
            ParseAttributes();

            if (SelectAllTableName != null)
            {
                return SelectEntireTable();
            }
            else
            {
                if (TablesUsed.Count == 1)
                {
                    var whereConditionPrimaryKey = CheckForPrimaryKey(WhereConditionsList);
                    var projectionConditionPrimaryKey = CheckForPrimaryKey(OutputParamsAliasList);

                    if (whereConditionPrimaryKey.Count != 0)
                    {
                        return "Primary key subset used in where condition";
                    }

                    if (projectionConditionPrimaryKey.Count != 0)
                    {
                        return "Primary key subsent used in projection";
                    }

                    var whereConditionIndex = CheckForIndex(WhereConditionsList);
                    var projectionConditionIndex = CheckForIndex(OutputParamsAliasList);

                    if (whereConditionIndex == "" && projectionConditionIndex == "")
                    {
                        return "Does not use Index";
                    }

                    if (whereConditionIndex != "")
                    {
                        return "Where Index has priority";
                    }
                    else
                    {
                        return "Projection Index";
                    }
                }
                else
                {
                    // TODO: when JOIN algorithms are implemented 
                    //              => methods that check if the JOIN tables can be restricted using index files 
                }

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
                return ex.Message + ";";
            }
        }

        private List<string> CheckForPrimaryKey(List<Tuple<Tuple<string, string>, string>> conditionList)
        {
            var primaryKeyColumnsUsed = new List<string>();
            var primaryKeyColumns = TableUtils.GetPrimaryKey(DatabaseName, TablesUsed[0]);

            foreach (var column in conditionList)
            {
                if (primaryKeyColumns.Contains(column.Item1.Item2))
                {
                    primaryKeyColumnsUsed.Add(column.Item1.Item2);
                }
            }

            return primaryKeyColumnsUsed;
        }

        private string CheckForIndex(List<Tuple<Tuple<string, string>, string>> conditionList)
        {
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
