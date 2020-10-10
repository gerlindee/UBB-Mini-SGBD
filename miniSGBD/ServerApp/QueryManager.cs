using ServerApp.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace ServerApp
{
    static class QueryManager
    {
        public static string DispatchQuery(string clientQuery)
        {
            var clientQuerySplit = clientQuery.Split(';');
            var executionResponse = "";
            switch (clientQuerySplit[0])
            {
                case Commands.CREATE_DATABASE:
                    {
                        executionResponse = new CreateDatabaseQuery(clientQuerySplit[1]).Execute();
                    }
                    break;
            }   
            return executionResponse;
        }
    }
}
