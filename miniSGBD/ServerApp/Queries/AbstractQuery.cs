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
        public string QueryType;
        public string ResponseCode;

        // methods return String instead of Bool because they're meant to return specific response codes (see class Responses from Utils)
        public string ValidateQuery()
        {
            return "Correct";
        }

        public string PerformXMLActions()
        {
            return "Correct";
        }

        public void Execute()
        {
            var validationResult = ValidateQuery();
            if (validationResult == "Correct")
            {
                var xmlResult = PerformXMLActions();
                if (xmlResult == "Correct")
                {
                    ResponseCode = Commands.GetSuccessCodeForCommand(QueryType);
                } 
                else
                {
                    ResponseCode = xmlResult;
                }
            }
            else
            {
                ResponseCode = validationResult;
            }
        }
    }
}
