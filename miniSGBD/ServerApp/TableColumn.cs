using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    class TableColumn
    {
        public string Name { get; } 
        public bool IsPrimaryKey { get; }
        public string Type { get; }
        public int Length { get; }
        public bool IsUnique { get; }
        public bool AllowsNulls { get; }

        public TableColumn(string _name, bool _isPrimaryKey, string _type, int _length, bool _isUnique, bool _allowsNulls)
        {
            Name = _name;
            IsPrimaryKey = _isPrimaryKey;
            Type = _type;
            Length = _length;
            IsUnique = _isUnique;
            AllowsNulls = _allowsNulls;
        }
        
    }
}
