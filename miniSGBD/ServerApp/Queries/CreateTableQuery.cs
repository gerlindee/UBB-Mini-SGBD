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
        private List<string> ReferencedTables;
        private string QueryAttributes;

        public CreateTableQuery(string _queryAttributes) : base(Commands.CREATE_TABLE)
        {
            QueryAttributes = _queryAttributes;
        }

        public override void ParseAttributes()
        {
            var tableAttributes = QueryAttributes.Split('#');
            DatabaseName = tableAttributes[0];
            TableName = tableAttributes[1];
            Columns = new List<TableColumn>();
            ReferencedTables = new List<string>();
            for (int idx = 2; idx < tableAttributes.Length - 1; idx++)
            {
                var columnAttributes = tableAttributes[idx].Split('|');
                var columnName = columnAttributes[0];
                var columnPK = bool.Parse(columnAttributes[1]);
                var columnType = columnAttributes[2];
                int.TryParse(columnAttributes[3], out var columnLength);
                var columnUnique = bool.Parse(columnAttributes[4]);
                var columnAllowNull = bool.Parse(columnAttributes[5]);
                Columns.Add(new TableColumn(columnName, columnPK, columnType, columnLength, columnUnique, columnAllowNull));
            }

            if (tableAttributes[tableAttributes.Length - 1] != "")
            {
                var refTables = tableAttributes[tableAttributes.Length - 1].Split('|');
                foreach (string tableName in refTables)
                {
                    if (tableName != "")
                    {
                        ReferencedTables.Add(tableName);
                    }
                }
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

            XElement newTableNode = new XElement("Table");
            XElement structureNode = new XElement("Structure");
            XElement primaryKeyNode = new XElement("PrimaryKey");
            XElement indexFilesAttribute = new XElement("IndexFiles");
            XElement foreignKeysNode = new XElement("ForeignKeys"); 
            XElement uniqueKeysNode = new XElement("UniqueKeys"); ;
            newTableNode.Add(structureNode);
            newTableNode.Add(primaryKeyNode);
            if (AreThereForeignKeys())
            {
                newTableNode.Add(foreignKeysNode);
            }
            if (AreThereUniqueKeys())
            {
                newTableNode.Add(uniqueKeysNode);
            }
            newTableNode.Add(indexFilesAttribute);

            int rowLength = 0;
            foreach(TableColumn tableColumn in Columns)
            {
                XElement columnNode = new XElement("Column",
                                            new XAttribute("allowsNulls", tableColumn.AllowsNulls),
                                            new XAttribute("type", tableColumn.Type),
                                            new XAttribute("columnName", tableColumn.Name));

                if (tableColumn.Length != 0)
                {
                    columnNode.SetAttributeValue("length", tableColumn.Length);
                }

                structureNode.Add(columnNode);
                rowLength += tableColumn.Length;

                if (tableColumn.IsPrimaryKey)
                {
                    XElement pkColumnNode = new XElement("PrimaryKeyColumn", tableColumn.Name);
                    primaryKeyNode.Add(pkColumnNode);
                }

                if (tableColumn.IsUnique)
                {
                    XElement uniqueColumnNode = new XElement("UniqueKeyColumn", tableColumn.Name);
                    uniqueKeysNode.Add(uniqueColumnNode);
                }
            }

            // Create the Foreign Key tags 
            if (AreThereForeignKeys())
            {
                foreach (string reference in ReferencedTables)
                {
                    XElement foreignKeyNode = new XElement("ForeignKey");
                    foreignKeysNode.Add(foreignKeyNode);

                    foreignKeyNode.Add(new XElement("ReferencedTable", reference));

                    foreach (TableColumn foreignKey in GetPrimaryKeysOfGivenTable(reference, givenDatabaseNode))
                    {
                        XElement referencedColumn = new XElement("ReferencedColumn", foreignKey.Name);
                        foreignKeyNode.Add(referencedColumn);

                        // Also add the PK field to the table structure as a column
                        structureNode.Add( new XElement("Column",
                                                new XAttribute("allowsNulls", foreignKey.AllowsNulls),
                                                new XAttribute("length", foreignKey.Length),
                                                new XAttribute("type", foreignKey.Type),
                                                new XAttribute("columnName", foreignKey.Name)));
                    }
                }
            }

            newTableNode.SetAttributeValue("rowLength", rowLength);
            newTableNode.SetAttributeValue("fileName", DatabaseName + "." + TableName);
            newTableNode.SetAttributeValue("tableName", TableName);
            givenDatabaseNode.Add(newTableNode);
            xmlDocument.Save(Application.StartupPath + "\\SGBDCatalog.xml");
        }

        private bool AreThereUniqueKeys()
        {
            foreach(TableColumn tableColumn in Columns)
            {
                if (tableColumn.IsUnique)
                    return true;
            }
            return false;
        }

        public bool AreThereForeignKeys()
        {
            return ReferencedTables.Count > 0; 
        }

        public List<TableColumn> GetPrimaryKeysCurrentTable()
        {
            var primaryKeys = new List<TableColumn>();
            foreach (TableColumn tableColumn in Columns)
            {
                if (tableColumn.IsPrimaryKey)
                    primaryKeys.Add(tableColumn);
            }
            return primaryKeys;
        }

        public List<TableColumn> GetPrimaryKeysOfGivenTable(string tableName, XElement databaseNode)
        {
            var primaryKeys = new List<string>();
            
            XElement givenTable = null;
            XElement[] tableNodes = databaseNode.Descendants("Table").ToArray();
            for (int i = 0; i < tableNodes.Length; i++)
            {
                if (tableNodes[i].Attribute("tableName").Value.Equals(tableName))
                {
                    givenTable = tableNodes[i];
                    break;
                }
            }

            XElement[] primaryKeyNodes = givenTable.Descendants("PrimaryKey").Descendants("PrimaryKeyColumn").ToArray();
            foreach (XElement pkNode in primaryKeyNodes)
            {
                primaryKeys.Add(pkNode.Value);
            }

            List<TableColumn> primaryKeysObjects = new List<TableColumn>();
            XElement[] tableColumnsNodes = givenTable.Descendants("Structure").Descendants("Column").ToArray();
            foreach (XElement column in tableColumnsNodes)
            {
                var columnName = column.Attribute("columnName").Value;
                if (primaryKeys.Contains(columnName))
                {
                    var columnLength = column.Attribute("length").Value;
                    var columnType = column.Attribute("type").Value;
                    var columnAllowsNulls = column.Attribute("allowsNulls").Value;
                    primaryKeysObjects.Add(new TableColumn(columnName, false, columnType, int.Parse(columnLength), false, bool.Parse(columnAllowsNulls)));
                }
            }
            return primaryKeysObjects;
        }
    }
}
