using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    class TableColumn
    {
        private string Name { get; } 
        private bool IsPrimaryKey { get; }
        private string Type { get; }
        private int Length { get; }
        private bool IsUnique { get; }
        private bool IsNotNull { get; }
        private string ForeignKey { get; }

        public TableColumn(string _name, bool _isPrimaryKey, string _type, int _length, bool _isUnique, bool _isNotNull, string _foreignKey)
        {
            Name = _name;
            IsPrimaryKey = _isPrimaryKey;
            Type = _type;
            Length = _length;
            IsUnique = _isUnique;
            IsNotNull = _isNotNull;
            ForeignKey = _foreignKey;
        }
        
    }
}
