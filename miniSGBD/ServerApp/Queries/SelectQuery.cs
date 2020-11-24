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
        private string TableName;
        private string Attributes;
        private MongoDBAcess MongoDB;

        public SelectQuery(string _databaseName, string _tableName, string _attributes) : base(Commands.SELECT_RECORDS)
        {
            DatabaseName = _databaseName;
            TableName = _tableName;
            Attributes = _attributes;
            MongoDB = new MongoDBAcess(DatabaseName);
        }

        public override void ParseAttributes()
        {
            var rows = new List<SelectRowInfo>();
            var splitAttributes = Attributes.Split('|');

            foreach(var attribute in splitAttributes)
            {
                var selectRowInfo = new SelectRowInfo(attribute);
                rows.Add(selectRowInfo);
            }
        }

        private string SelectEntireTable()
        {
            try
            {
                var records = "";
                var keyValuePairs = MongoDB.GetEntireCollection(TableName);
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
