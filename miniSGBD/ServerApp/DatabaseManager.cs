using ServerApp.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
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
                databaseNames = databaseNames + childNode.Attributes.GetNamedItem("databaseName").Value + '|';
            }
            return databaseNames;
        }
    }
}
