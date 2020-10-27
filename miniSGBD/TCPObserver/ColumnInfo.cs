using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Utils
{
    public class ColumnInfo
    {
        public ColumnInfo(string command)
        {
            var columnStruct = command.Split('#');
            ColumnName = columnStruct[0];

            for (var i = 1; i < columnStruct.Length; i++)
            {
                switch (columnStruct[i])
                {
                    case ColumnInformation.PK:
                        {
                            PK = true;
                        }
                        break;
                    case ColumnInformation.FK:
                        {
                            FK = true;
                        }
                        break;
                    case ColumnInformation.UNQ:
                        {
                            Unique = true;
                        }
                        break;
                    case ColumnInformation.NULL:
                        {
                            NonNull = true;
                        }
                        break;
                    default:
                        {
                            try
                            {
                                var types = columnStruct[i].Split('-');
                                Type = types[0];
                                int.TryParse(types[1], out int val);
                                Lenght = val;
                            }
                            catch (Exception)
                            {
                                Type = columnStruct[i];
                            }
                        }
                        break;
                }
            }
        }

        public string ColumnName {get; set;}
        public bool PK { get; set; } = false;
        public bool FK { get; set; } = false;
        public string FKReferenceTable { get; set; }
        public bool Unique { get; set; } = false;
        public bool NonNull { get; set; } = false;
        public string Type { get; set; }
        public int Lenght { get; set; } = -1;
    }
}
