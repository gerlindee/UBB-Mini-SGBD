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

        private string SelectAllTableName;
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
                        SelectConfigList.Add(new SelectRowInfo(attribute));
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
                    OutputParamsAliasList.Add(new Tuple<Tuple<string, string>, string>(new Tuple<string, string>(column.TableName, column.ColumnName), column.Alias));
                }

                if (column.Filter != "-")
                {
                    WhereConditionsList.Add(new Tuple<Tuple<string, string>, string>(new Tuple<string, string>(column.TableName, column.ColumnName), column.Filter));
                }

                if (column.GroupBy)
                {
                    GroupByList.Add(new Tuple<string, string>(column.TableName, column.ColumnName));
                }

                if (column.Having != "")
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
                return "TODO";
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

        /*
        private void checkWhereInIndexFiles()
        {
            var mongoDB = new MongoDBAcess(DatabaseName);

            string whereAttributes = "";//Where has priority
            foreach (var whereCondition in FilterParamsAndCondition)
            {
                whereAttributes += whereCondition.Item1 + '#';
            }
            whereAttributes = whereAttributes.Remove(whereAttributes.Length - 1);

            var pkOfWhere = MongoDB.GetRecordValueWithKey(TableName, whereAttributes);

            if (pkOfWhere.Length != 0)
            {
                try
                {
                    var listOfPk = pkOfWhere.Split('#');
                    foreach (var pk in listOfPk)
                    {
                        var valueFromMainList = MongoDB.GetRecordValueWithKey(TableName, pk);
                    }
                }
                catch (Exception e)
                {

                }

            }
        }
        */
    }
}
