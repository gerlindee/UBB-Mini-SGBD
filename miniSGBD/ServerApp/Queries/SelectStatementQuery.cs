using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace ServerApp.Queries
{
    class SelectStatementQuery : AbstractQuery
    {
        private string DatabaseName;
        private string TableName;
        private string Attributes;
        private MongoDBAcess MongoDB;

        private List<SelectRowInfo> SelectRows = new List<SelectRowInfo>();
        private List<Tuple<string, string>> OutputParamsAndAlias = new List<Tuple<string, string>>();
        private List<Tuple<string, string>> FilterParamsAndCondition = new List<Tuple<string, string>>();
        private List<Tuple<string, string>> HavingParamsAndCondition = new List<Tuple<string, string>>();
        private List<string> GroupByList = new List<string>();

        public SelectStatementQuery(string _databaseName, string _tableName, string _attributes) : base(Commands.SELECT_RECORDS)
        {
            DatabaseName = _databaseName;
            TableName = _tableName;
            Attributes = _attributes;
            MongoDB = new MongoDBAcess(DatabaseName);
        }

        public override void ParseAttributes()
        {
            var splitAttributes = Attributes.Split('|');

            foreach (var attribute in splitAttributes)
            {
                if (attribute != "")
                {
                    SelectRows.Add(new SelectRowInfo(attribute));
                }
            }
            parseRows();
        }

        public void parseRows()
        {
            foreach (var row in SelectRows)
            {
                if (row.Output)
                    OutputParamsAndAlias.Add(new Tuple<string, string>(row.ColumnName, row.Alias));
                if (row.Filter != "-")
                    FilterParamsAndCondition.Add(new Tuple<string, string>(row.ColumnName, row.Filter));
                if (row.GroupBy)
                    GroupByList.Add(row.ColumnName);
                if (row.Having != "-")
                    HavingParamsAndCondition.Add(new Tuple<string, string>(row.ColumnName, row.Having));
            }
        }

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

        public override void PerformXMLActions()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
