using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils;

namespace ServerApp
{
    class TableUtils
    {
        public static bool IsTableReferenced(string databaseName, string tableName)
        {
            var xmlDocument = XDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");

            XElement[] databasesNodes = xmlDocument.Element("Databases").Descendants("Database").ToArray();
            XElement givenDB = Array.Find(databasesNodes, elem => elem.Attribute("databaseName").Value.Equals(databaseName));

            var referencedTables = Array.FindAll(givenDB.Descendants("ReferencedTable").ToArray(), elem => elem.Value == tableName);
            if (referencedTables.Count() != 0)
            {
                // Table references exist
                return true;
            }

            return false;
        }

        public static List<string> GetForeignKeyFiles(string databaseName, string tableName)
        {
            var fkFiles = new List<string>();
            var xmlDocument = XDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");

            XElement[] databasesNodes = xmlDocument.Element("Databases").Descendants("Database").ToArray();
            XElement givenDB = Array.Find(databasesNodes, elem => elem.Attribute("databaseName").Value.Equals(databaseName));

            var foreignKeyNodes = givenDB.Descendants("ForeignKey").ToArray();
            foreach (var foreignKey in foreignKeyNodes)
            {
                var referencedTable = foreignKey.Descendants("ReferencedTable").ToArray()[0].Value;
                if (referencedTable == tableName)
                {
                    fkFiles.Add(foreignKey.Attribute("fileName").Value);
                }
            }

            return fkFiles;
        }

        public static List<string> GetOwnForeignKeyFiles(string databaseName, string tableName)
        {
            var fkFiles = new List<string>();
            var xmlDocument = XDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");

            XElement[] databasesNodes = xmlDocument.Element("Databases").Descendants("Database").ToArray();
            XElement givenDB = Array.Find(databasesNodes, elem => elem.Attribute("databaseName").Value.Equals(databaseName));
            var givenTable = Array.Find(givenDB.Descendants("Table").ToArray(), elem => elem.Attribute("tableName").Value == tableName);
            var foreignKeyFiles = givenTable.Descendants("ForeignKey").ToArray();

            foreach (var fkFile in foreignKeyFiles)
            {
                fkFiles.Add(fkFile.Attribute("fileName").Value);
            }

            return fkFiles;
        }

        public static List<IndexFileData> GetIndexFiles(string databaseName, string tableName)
        {
            var indexFiles = new List<IndexFileData>();
            var xmlDocument = XDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");

            XElement givenDB = Array.Find(xmlDocument.Element("Databases").Descendants("Database").ToArray(), 
                                            elem => elem.Attribute("databaseName").Value.Equals(databaseName));
            XElement givenTable = Array.Find(givenDB.Descendants("Table").ToArray(), 
                                            elem => elem.Attribute("tableName").Value == tableName);
            XElement[] indexFileNodes = givenTable.Descendants("IndexFiles").Descendants("IndexFile").ToArray();

            foreach (var indexFile in indexFileNodes)
            {
                var indexName = indexFile.Attribute("indexName").Value;
                var indexUnique = indexFile.Attribute("isUnique").Value;
                var indexColumns = new List<string>();

                foreach (var column in indexFile.Descendants("IndexAttribute").ToArray())
                {
                    indexColumns.Add(column.Value);
                }

                indexFiles.Add(new IndexFileData(indexName, bool.Parse(indexUnique), indexColumns));
            }

            return indexFiles;
        }
    }
}
