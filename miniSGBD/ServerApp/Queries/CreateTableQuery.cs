using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils;

namespace ServerApp.Queries
{
    class CreateTableQuery : AbstractQuery
    {
        private string DatabaseName;
        private string TableName;
        private List<TableColumn> Columns;

        public CreateTableQuery(string _queryAttributes) : base(Commands.CREATE_TABLE, _queryAttributes)
        {

        }

        public override void ParseAttributes()
        {
            var tableAttributes = base.QueryAttributes.Split('#');
            DatabaseName = tableAttributes[0];
            TableName = tableAttributes[1];
            Columns = new List<TableColumn>();
            for (int idx = 2; idx < tableAttributes.Length - 1; idx++)
            {
                var columnAttributes = tableAttributes[idx].Split('|');
                var columnName = columnAttributes[0];
                var columnPK = bool.Parse(columnAttributes[1]);
                var columnType = columnAttributes[2];
                int.TryParse(columnAttributes[3], out var columnLength);
                var columnUnique = bool.Parse(columnAttributes[4]);
                var columnNotNull = bool.Parse(columnAttributes[5]);
                var columnFK = columnAttributes[6];
                Columns.Add(new TableColumn(columnName, columnPK, columnType, columnLength, columnUnique, columnNotNull, columnFK));
            }
        }

        public override string ValidateQuery()
        {
            var xmlDocument = XDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");
            XElement givenDatabaseNode = null;
            XElement[] databasesNodes = xmlDocument.Element("Databases").Descendants("Database").ToArray();
            for (int i = 0; i < databasesNodes.Length; i++)
            {
                if (databasesNodes[i].Attribute("databaseName").Value.Equals(DatabaseName))
                {
                    givenDatabaseNode = databasesNodes[i];
                    break;
                }
            }

            bool tableExists = false;
            XElement[] tableNodes = givenDatabaseNode.Descendants("Table").ToArray();
            for (int i = 0; i < tableNodes.Length; i++)
            {
                if (tableNodes[i].Attribute("tableName").Value.Equals(TableName))
                {
                    tableExists = true;
                    break;
                }
            }

            if (tableExists)
            {
                return Responses.CREATE_TABLE_ALREADY_EXISTS;
            }

            return Responses.CREATE_TABLE_SUCCESS;
        }

        public override void PerformXMLActions()
        {
            var xmlDocument = XDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");
            XElement givenDatabaseNode = null;
            XElement[] databasesNodes = xmlDocument.Element("Databases").Descendants("Database").ToArray();
            for (int i = 0; i < databasesNodes.Length; i++)
            {
                if (databasesNodes[i].Attribute("databaseName").Value.Equals(DatabaseName))
                {
                    givenDatabaseNode = databasesNodes[i];
                    break;
                }
            }

            XElement newTableNode = new XElement("Table", new XAttribute("tableName", TableName));
            givenDatabaseNode.Add(newTableNode);
            xmlDocument.Save(Application.StartupPath + "\\SGBDCatalog.xml");
        }
    }
}
