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
        public bool AllowNull { get; }
        public string ForeignKey { get; }

        public TableColumn(string _name, bool _isPrimaryKey, string _type, int _length, bool _isUnique, bool _isNotNull, string _foreignKey)
        {
            Name = _name;
            IsPrimaryKey = _isPrimaryKey;
            Type = _type;
            Length = _length;
            IsUnique = _isUnique;
            AllowNull = _isNotNull;
            ForeignKey = _foreignKey;
        }
        
    }
}
