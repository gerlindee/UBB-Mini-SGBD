using ServerApp.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Utils;

namespace ServerApp
{
    static class DatabaseManager
    {
        public static string ExecuteCommand(string command)
        {
            var commandSplit = command.Split(';');
            var executionResponse = ""; 
            switch (commandSplit[0])
            {
                case Commands.GET_ALL_DATABASES:
                    {
                        executionResponse = FetchDatabases();
                    }
                    break;
                case Commands.CREATE_DATABASE:
                    {
                        executionResponse = new CreateDatabaseQuery(commandSplit[1]).Execute();
                    }
                    break;
                case Commands.DROP_DATABASE:
                    {
                        executionResponse = new DropDatabaseQuery(commandSplit[1]).Execute();
                    }
                    break;
                case Commands.CREATE_TABLE:
                    {
                        executionResponse = new CreateTableQuery(commandSplit[1]).Execute();
                    }
                    break;
                case Commands.DROP_TABLE:
                    {
                        executionResponse = new DropTableQuery(commandSplit[1], commandSplit[2]).Execute();
                    }
                    break;
                case Commands.GET_ALL_TABLES:
                    {
                        executionResponse = FetchTables(commandSplit[1]);
                    }
                    break;
                case Commands.CREATE_INDEX:
                    {
                        executionResponse = FetchColumns(commandSplit[1], commandSplit[2]);
                    }
                    break;
                case Commands.CREATE_NONUNIQUE_INDEX:
                    {
                        executionResponse = new CreateIndexQuery(commandSplit[0], false, commandSplit[1], commandSplit[2], commandSplit[3], commandSplit[4]).Execute();
                    }
                    break;
                case Commands.CREATE_UNIQUE_INDEX:
                    {
                        executionResponse = new CreateIndexQuery(commandSplit[0], true, commandSplit[1], commandSplit[2], commandSplit[3], commandSplit[4]).Execute();
                    }
                    break;
            }
            return executionResponse;
        }

        private static string FetchDatabases()
        {
            var databaseNames = Responses.GENERAL_DISPLAY_ENTRIES + ';';
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");
            XmlElement xmlRoot = xmlDocument.DocumentElement;

            foreach (XmlNode childNode in xmlRoot.ChildNodes)
            {
                databaseNames += childNode.Attributes.GetNamedItem("databaseName").Value + '|';
            }
            return databaseNames.Remove(databaseNames.Length - 1);
        }

        private static string FetchTables(string dbName)
        {
            var tableNames = Responses.GENERAL_DISPLAY_ENTRIES + ';';
            var xmlDocument = XDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");

            XElement[] databasesNodes = xmlDocument.Element("Databases").Descendants("Database").ToArray();
            XElement givenDB = Array.Find(databasesNodes, elem => elem.Attribute("databaseName").Value.Equals(dbName));

            XElement[] databasesTables = givenDB.Descendants("Table").ToArray();

            foreach(var table in databasesTables)
            {
                tableNames += table.Attribute("tableName").Value + "|";
            }

            return tableNames.Remove(tableNames.Length -1);
        }

        private static string FetchColumns(string dbName, string tbName)
        {
            var columnNames = Responses.GENERAL_DISPLAY_ENTRIES + ';';
            var xmlDocument = XDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");

            XElement[] databasesNodes = xmlDocument.Element("Databases").Descendants("Database").ToArray();
            XElement givenDB = Array.Find(databasesNodes, elem => elem.Attribute("databaseName").Value.Equals(dbName));
            XElement[] databasesTables = givenDB.Descendants("Table").ToArray();
            XElement givenTable = Array.Find(databasesTables, elem => elem.Attribute("tableName").Value.Equals(tbName));
            XElement[] tableColumnsNodes = givenTable.Descendants("Structure").Descendants("Column").ToArray();

            foreach (var col in tableColumnsNodes)
            {
                columnNames += col.Attribute("columnName").Value + "|";
            }

            return columnNames.Remove(columnNames.Length - 1);
        }

        private static string FetchTableInformation(string databaseName, string tableName)
        {
            var columnInfo = Responses.GENERAL_DISPLAY_ENTRIES + ';';
            var xmlDocument = XDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");

            XElement[] databasesNodes = xmlDocument.Element("Databases").Descendants("Database").ToArray();
            XElement givenDatabase = Array.Find(databasesNodes, elem => elem.Attribute("databaseName").Value.Equals(databaseName));
            XElement[] databasesTables = givenDatabase.Descendants("Table").ToArray();
            XElement givenTable = Array.Find(databasesTables, elem => elem.Attribute("tableName").Value.Equals(tableName));

            XElement[] tableColumnsNodes = givenTable.Descendants("Structure").Descendants("Column").ToArray();

            // Get the names of the columns that are primary keys
            var primaryKeyNames = new List<string>();
            XElement[] primaryKeyNodes = givenTable.Descendants("PrimaryKey").Descendants("PrimaryKeyColumn").ToArray();


            // Get the names of the columns that are unique 

            // Get column structure information


            return columnInfo;
        }
    }
}
