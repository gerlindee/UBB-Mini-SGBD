using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace ServerApp.Queries
{
    public abstract class AbstractQuery
    {
        public string QueryCommand;
        public string QueryAttributes;

        public AbstractQuery(string _queryType)
        {
            QueryCommand = _queryType;
        }

        public AbstractQuery()
        {

        }

        public abstract void ParseAttributes();

        // methods return String instead of Bool because they're meant to return specific response codes (see class Responses from Utils)
        public virtual string ValidateQuery()
        {
            return Commands.MapCommandToSuccessResponse(QueryCommand);
        }

        public virtual void PerformXMLActions()
        {
            
        }

        public string Execute()
        {
            ParseAttributes();
            var validationResult = ValidateQuery();
            if (validationResult == Commands.MapCommandToSuccessResponse(QueryCommand))
            {
                PerformXMLActions();
            }
            return validationResult;
        }
    }
}
