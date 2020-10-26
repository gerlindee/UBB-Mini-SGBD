using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Utils
{
    public class ColumnInfo
    {
        public string ColumnName {get; set;}
        public bool PK { get; set; } = false;
        public bool FK { get; set; } = false;
        public string FKReferenceTable { get; set; }
        public bool unique { get; set; } = false;
        public bool nonNull { get; set; } = false;
        public string type { get; set; }
        public int lenght { get; set; } = -1;
    }
}
