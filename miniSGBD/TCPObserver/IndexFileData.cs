﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class IndexFileData
    {
        public string IndexFileName;
        public bool IsUnique;
        public List<string> IndexColumns;

        public IndexFileData(string _fileName, bool _unique, List<string> _columns)
        {
            IndexFileName = _fileName;
            IsUnique = _unique;
            IndexColumns = _columns;
        }
    }
}
