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
        private MongoDBAcess MongoDB;

        public DropTableQuery(string _queryAttributesDB, string _queryAttributesTB) : base(Commands.DROP_TABLE)
        {
            DBName = _queryAttributesDB;
            TableName = _queryAttributesTB;
            MongoDB = new MongoDBAcess(DBName);
        }

        public override void ParseAttributes()
        {

        }

        public override string ValidateQuery()
        {
            // Check if the table is used as a reference in any other table 
            if (TableUtils.IsTableReferenced(DBName, TableName))
            {
                // Table references exist => error message 
                return Responses.DROP_TABLE_REFERENCED;
            }

            return Commands.MapCommandToSuccessResponse(QueryCommand);
        }

        public override void PerformXMLActions()
        {   
            try
            {
                var xmlDocument = XDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");

                XElement[] databasesNodes = xmlDocument.Element("Databases").Descendants("Database").ToArray();
                XElement givenDB = Array.Find(databasesNodes, elem => elem.Attribute("databaseName").Value.Equals(DBName));

                XElement[] databasesTables = givenDB.Descendants("Table").ToArray();
                XElement deletedXMLTag = Array.Find(databasesTables, elem => elem.Attribute("tableName").Value.Equals(TableName));

                // Delete the records from the table, stored in the MongoDB collection
                new DeleteRowsQuery(DBName, TableName, "ALL").Execute();

                // Delete unique key collections from MongoDB
                XElement[] uniqueKeysNodes = deletedXMLTag.Descendants("UniqueKeys").Descendants("UniqueKeyColumn").ToArray(); 
                foreach (var uniqueKey in uniqueKeysNodes)
                {
                    MongoDB.RemoveAllKVFromCollection(uniqueKey.Attribute("fileName").Value);
                }

                // Delete index file collections from MongoDB
                XElement[] indexNodes = deletedXMLTag.Descendants("IndexFiles").Descendants("IndexFile").ToArray();
                foreach (var index in indexNodes)
                {
                    MongoDB.RemoveAllKVFromCollection(index.Attribute("fileName").Value);
                }

                // Delete foreign key file collection from MongoDB 
                XElement[] foreignKeysNodes = deletedXMLTag.Descendants("ForeignKeys").Descendants("ForeignKey").ToArray();
                foreach (var foreignKey in foreignKeysNodes)
                {
                    MongoDB.RemoveAllKVFromCollection(foreignKey.Attribute("fileName").Value);
                }

                // Delete the XML content for the table  
                deletedXMLTag.Remove();
                xmlDocument.Save(Application.StartupPath + "\\SGBDCatalog.xml");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
