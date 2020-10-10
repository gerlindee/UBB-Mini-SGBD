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

        // Database Responses
        public const string CREATE_DATABASE_SUCCESS = "100";
        public const string CREATE_DATABASE_ALREADY_EXISTS = "101";
        public const string DROP_DATABASE_SUCCESS = "110";

        public static string MapResponseToMessage(string responseCode)
        {
            switch (responseCode)
            {
                case CREATE_DATABASE_SUCCESS: 
                    return "Database created succesfully";
                case CREATE_DATABASE_ALREADY_EXISTS:
                    return "A database with the same name already exists!";
                case DROP_DATABASE_SUCCESS:
                    return "Database deleted successfully!";
            }
            return ""; 
        }

        public static string GetSuccessCodeForCommand(string command)
        {
            switch (command)
            {
                case CREATE_DATABASE:
                    return CREATE_DATABASE_SUCCESS;
                case DROP_DATABASE:
                    return DROP_DATABASE_SUCCESS;
            }
            return "";
        }
        
    }
}
