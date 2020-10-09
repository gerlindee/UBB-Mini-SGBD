using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniSGBD
{
    public static class Commands
    {
        // Database Commands
        public const string CREATE_DATABASE = "CREATE_DATABASE;";
        public const string DROP_DATABASE = "DROP_DATABASE;";

        // Table Commands
        public const string CREATE_TABLE = "CREATE_TABLE";
        public const string DROP_TABLE = "DROP_TABLE";

        // Index Commands
        public const string CREATE_INDEX = "CREATE_INDEX";

        public static string MapResponse(string responseCode)
        {
            switch (responseCode)
            {
                case "100": 
                    return "Database created succesfully";
                case "101":
                    return "A database with the same name already exists!";
                case "102":
                    return "Database deleted successfully!";

                case "200":
                    return "Table created succesfully";
                case "201":
                    return "A table with the same name already exists in the database";

            }
            return ""; 
        }
        
    }
}
