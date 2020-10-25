using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Utils;

namespace ServerApp.Queries
{
    class DropTableQuery : AbstractQuery
    {
        private string DBName;
        private string TableName;

        public DropTableQuery(string _queryAttributesDB, string _queryAttributesTB) : base(Commands.DROP_TABLE)
        {
            DBName = _queryAttributesDB;
            TableName = _queryAttributesTB;
        }

        public override void ParseAttributes()
        {

        }

        public override string ValidateQuery()
        {
            return Commands.MapCommandToSuccessResponse(QueryCommand);
        }

        public override void PerformXMLActions()
        {
            var xmlDocument = XDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");
            XElement deletedXMLTag = null;


            XElement givenDatabaseNode = null;
            XElement[] databasesNodes = xmlDocument.Element("Databases").Descendants("Database").ToArray();
            XElement givenDB = Array.Find(databasesNodes, elem => elem.Attribute("databaseName").Value.Equals(DBName));

            XElement[] databasesTables = givenDB.Descendants("Table").ToArray();
            deletedXMLTag = Array.Find(databasesTables, elem => elem.Attribute("tableName").Value.Equals(TableName));

            deletedXMLTag.Remove();
            xmlDocument.Save(Application.StartupPath + "\\SGBDCatalog.xml");
        }
    }
}
