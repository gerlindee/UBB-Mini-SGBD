using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class ForeignKeyData
    {
        public List<string> ReferencedColumns;
        public string ReferencedTable;
        public string ForeignKeyFile;

        public ForeignKeyData(List<string> _column, string _table, string _file)
        {
            ReferencedColumns = _column;
            ReferencedTable = _table;
            ForeignKeyFile = _file;
        }
    }

    public class ForeignKeyInsertData
    {
        public string MongoDBFilename;
        public KeyValuePair<string, string> ForeignKeyRecord;

        public ForeignKeyInsertData(string _file, string _key, string _value)
        {
            MongoDBFilename = _file;
            ForeignKeyRecord = new KeyValuePair<string, string>(_key, _value);
        }
    }
}
