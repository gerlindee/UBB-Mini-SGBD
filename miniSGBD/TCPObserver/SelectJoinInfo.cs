using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class SelectJoinInfo
    {
        public string LeftTableName { get; set; }
        public string LeftTableColumn { get; set; }
        public string RightTableName { get; set; }
        public string RightTableColumn { get; set; }

        public SelectJoinInfo(string attributes)
        {
            var splitAttributes = attributes.Split('#');
            LeftTableName = splitAttributes[0];
            LeftTableColumn = splitAttributes[1];
            RightTableName = splitAttributes[2];
            RightTableColumn = splitAttributes[3];
        }
    }
}
