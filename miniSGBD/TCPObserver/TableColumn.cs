using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class TableColumn
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
