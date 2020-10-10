using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class Commands
    {
        // Database Commands
        public const string CREATE_DATABASE = "CREATE_DATABASE";
        public const string DROP_DATABASE = "DROP_DATABASE";
        public const string GET_ALL_DATABASES = "GET_ALL_DATABASES";

        public static string MapCommandToSuccessResponse(string command)
        {
            switch (command)
            {
                case CREATE_DATABASE:
                    return Responses.CREATE_DATABASE_SUCCESS;
                case DROP_DATABASE:
                    return Responses.DROP_DATABASE_SUCCESS;
            }
            return "";
        }

    }

    public static class Responses
    {
        // General Responses
        public const string GENERAL_SERVER_ERROR = "000";
        public const string GENERAL_DISPLAY_ENTRIES = "001";

        // Database Responses
        public const string CREATE_DATABASE_SUCCESS = "100";
        public const string CREATE_DATABASE_ALREADY_EXISTS = "101";
        public const string DROP_DATABASE_SUCCESS = "110";
        public const string DROP_DATABASE_DOESNT_EXIST = "111";

        public static string MapResponseToMessage(string responseCode)
        {
            switch (responseCode)
            {
                case GENERAL_SERVER_ERROR:
                    return "Server disconnected due to internal error!";
                case CREATE_DATABASE_SUCCESS:
                    return "Database created succesfully";
                case CREATE_DATABASE_ALREADY_EXISTS:
                    return "A database with the same name already exists!";
                case DROP_DATABASE_SUCCESS:
                    return "Database deleted successfully!";
                case DROP_DATABASE_DOESNT_EXIST:
                    return "A database with the given name does not exist!";
            }
            return "";
        }
    }
}
