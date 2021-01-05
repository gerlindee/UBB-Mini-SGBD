using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class SelectJoinInfo
    {
        public string LeftTableName { get; set; }
        public string RightTableName { get; set; }
        public string FKFileName { get; set; }
        public string FKColumn { get; set; }

        public SelectJoinInfo(string attributes)
        {
            var splitAttributes = attributes.Split('#');
            LeftTableName = splitAttributes[0];
            RightTableName = splitAttributes[1];
            FKFileName = splitAttributes[2];
            FKColumn = splitAttributes[3];
        }
    }
}
