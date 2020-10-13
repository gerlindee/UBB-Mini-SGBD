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
    class DropDatabaseQuery : AbstractQuery
    {
        private string DatabaseName;
        public DropDatabaseQuery(string _queryAttributes) : base(Commands.DROP_DATABASE)
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

            foreach (XmlNode childNode in xmlRoot.ChildNodes)
            {
                if (childNode.Attributes.GetNamedItem("databaseName").Value.Equals(DatabaseName))
                    return Commands.MapCommandToSuccessResponse(QueryCommand);
            }
            return Responses.DROP_DATABASE_DOESNT_EXIST;
        }

        public override void PerformXMLActions()
        {
            var xmlDocument = XDocument.Load(Application.StartupPath + "\\SGBDCatalog.xml");
            XElement deletedXMLTag = null;
            var databasesTags = xmlDocument.Element("Databases").Descendants("Database").ToArray();
            foreach (XElement tag in databasesTags)
            {
                if (tag.Attribute("databaseName").Value.Equals(DatabaseName))
                {
                    deletedXMLTag = tag;
                    break;
                }
            }
            deletedXMLTag.Remove();
            xmlDocument.Save(Application.StartupPath + "\\SGBDCatalog.xml");
        }

    }
}
