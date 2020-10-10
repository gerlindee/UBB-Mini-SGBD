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
    public class CreateDatabaseQuery : AbstractQuery
    {
        private string DatabaseName; 
        public CreateDatabaseQuery(string _queryAttributes) : base(Commands.CREATE_DATABASE, _queryAttributes)
        {
            
        }

        public override void ParseAttributes()
        {
            DatabaseName = base.QueryAttributes;
        }

        public override string ValidateQuery()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");
            XmlElement xmlRoot = xmlDocument.DocumentElement;

            foreach(XmlNode childNode in xmlRoot.ChildNodes)
            {
                if (childNode.Attributes.GetNamedItem("databaseName").Value.Equals(DatabaseName))
                    return Commands.CREATE_DATABASE_ALREADY_EXISTS;
            }
            return Commands.GetSuccessCodeForCommand(QueryCommand);
        }

        public override void PerformXMLActions()
        {
            var xmlDocument = XDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");
            XElement xmlElement = xmlDocument.Element("Databases");
            xmlElement.Add(new XElement("Database", new XAttribute("databaseName", DatabaseName)));
            xmlDocument.Save(Application.StartupPath + "\\SGBDCatalog.xml");
        }


    }
}
