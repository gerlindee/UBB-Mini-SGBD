using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace ServerApp.Queries
{
    class SelectWithJoinQuery : AbstractQuery
    {
        private string DatabaseName;
        private MongoDBAcess MongoDB;
        private string Attributes;

        private bool SelectAllFlag; // set to true when the entire result of the JOIN operation needs to be displayed

        private List<string> SelectedTables;
        private List<Tuple<string, List<string>>> SelectedTablesStructure;
        private List<SelectJoinInfo> SelectJoinInfo;

        public SelectWithJoinQuery(string _databaseName, string _attributes)
        {
            DatabaseName = _databaseName;
            Attributes = _attributes;
            MongoDB = new MongoDBAcess(DatabaseName);

            SelectAllFlag = false;
            SelectedTables = new List<string>();
            SelectedTablesStructure = new List<Tuple<string, List<string>>>();
            SelectJoinInfo = new List<SelectJoinInfo>();
        }

        public override void ParseAttributes()
        {
            var splitAttributes = Attributes.Split('|');

            if (splitAttributes[0].Equals("SELECT_ALL"))
            {
                SelectAllFlag = true;

                for (int idx = 1; idx < splitAttributes.Length; idx++)
                {
                    SelectJoinInfo.Add(new SelectJoinInfo(splitAttributes[idx]));

                    var leftTable = splitAttributes[idx].Split('#')[0];
                    var rightTable = splitAttributes[idx].Split('#')[1];

                    if (!SelectedTables.Contains(leftTable))
                    {
                        SelectedTables.Add(leftTable);
                    }

                    if (!SelectedTables.Contains(rightTable))
                    {
                        SelectedTables.Add(rightTable);
                    }
                }
            }
            else
            {

            }

            foreach (var table in SelectedTables)
            {
                SelectedTablesStructure.Add(new Tuple<string, List<string>>(table, TableUtils.GetTableColumns(DatabaseName, table)));
            }
        }

        public new string Execute()
        {
            try
            {
                ParseAttributes();
                if (SelectAllFlag)
                {
                    // SELECT * from the result of the join
                    return Commands.MapCommandToSuccessResponse(Commands.SELECT_RECORDS_WITH_JOIN) + ";" + SelectEntireJoinResult();
                }
                else
                {
                    // SELECT statement with some criteria and/or output options
                    return Commands.MapCommandToSuccessResponse(Commands.SELECT_RECORDS_WITH_JOIN) + ";";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string SelectEntireJoinResult()
        {
            try
            {
                var intermJoinStructure = new List<string>();
                var intermJoinRecords = new List<List<string>>();
                var joinedTables = new List<string>();

                for (int idx = 0; idx < SelectJoinInfo.Count; idx++)
                {
                    var leftTable = SelectedTablesStructure.Find(elem => elem.Item1 == SelectJoinInfo[idx].LeftTableName);
                    var rightTable = SelectedTablesStructure.Find(elem => elem.Item1 == SelectJoinInfo[idx].RightTableName);

                    if (intermJoinStructure.Count == 0)
                    {
                        intermJoinStructure = JoinTableStructures(leftTable.Item2, rightTable.Item2);
                        joinedTables.Add(leftTable.Item1);
                        joinedTables.Add(rightTable.Item1);

                        var keyValuePairs = MongoDB.GetEntireCollection(SelectJoinInfo[idx].LeftTableName);
                        var leftTableRecords = new List<List<string>>();
                        foreach (var keyValue in keyValuePairs)
                        {
                            var key = keyValue.GetElement("_id").Value.ToString().Split('#');
                            var value = keyValue.GetElement("value").Value.ToString().Split('#');
                            var tableRecord = new List<string>();
                            tableRecord.AddRange(key);
                            tableRecord.AddRange(value);
                            leftTableRecords.Add(tableRecord);
                        }

                        keyValuePairs = MongoDB.GetEntireCollection(SelectJoinInfo[idx].FKFileName);
                        var rightTableRecords = new List<List<string>>();
                        foreach (var keyValue in keyValuePairs)
                        {
                            var key = keyValue.GetElement("_id").Value.ToString().Split('#');
                            var value = keyValue.GetElement("value").Value.ToString().Split('#');
                            var tableRecord = new List<string>();
                            tableRecord.AddRange(key);
                            tableRecord.AddRange(value);
                            rightTableRecords.Add(tableRecord);
                        }

                        intermJoinRecords = PerformIndexedNestedLoopsJoin(intermJoinStructure, leftTableRecords, rightTableRecords, SelectJoinInfo[idx].FKColumn, SelectJoinInfo[idx].RightTableName, true);
                    }
                    else
                    {
                        intermJoinStructure = JoinTableStructures(intermJoinStructure, leftTable.Item2);
                        joinedTables.Add(leftTable.Item1);

                        var keyValuePairs = MongoDB.GetEntireCollection(leftTable.Item1);
                        var rightTableRecords = new List<List<string>>();
                        foreach (var keyValue in keyValuePairs)
                        {
                            var key = keyValue.GetElement("_id").Value.ToString().Split('#');
                            var value = keyValue.GetElement("value").Value.ToString().Split('#');
                            var tableRecord = new List<string>();
                            tableRecord.AddRange(key);
                            tableRecord.AddRange(value);
                            rightTableRecords.Add(tableRecord);
                        }

                        intermJoinRecords = PerformIndexedNestedLoopsJoin(intermJoinStructure, intermJoinRecords, rightTableRecords, SelectJoinInfo[idx].FKColumn, leftTable.Item1, false);
                    }
                }

                var outHeader = "";
                foreach (var outCol in intermJoinStructure)
                {
                    outHeader += outCol + "#";
                }
                outHeader = outHeader.Remove(outHeader.Length - 1);

                var outRecords = "";
                foreach (var outRec in intermJoinRecords)
                {
                    foreach (var outRecCol in outRec)
                    {
                        outRecords += outRecCol + '#';
                    }
                    outRecords = outRecords.Remove(outRecords.Length - 1) + '|';
                }

                return outHeader + ";" + outRecords.Remove(outRecords.Length - 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<string> JoinTableStructures(List<string> leftTable, List<string> rightTable)
        {
            var intermStructure = new List<string>();

            foreach (var column in leftTable)
            {
                intermStructure.Add(column);
            }

            foreach (var column in rightTable)
            {
                if (!intermStructure.Contains(column))
                {
                    intermStructure.Add(column);
                }
            }

            return intermStructure;
        }

        private List<List<string>> PerformIndexedNestedLoopsJoin(List<string> joinStructure, List<List<string>> leftTableRecords, List<List<string>> rightTableRecords, string fkColumn, string rightTableName, bool usesFK)
        {
            var records = new List<List<string>>();
            var rightTableStructure = SelectedTablesStructure.Find(elem => elem.Item1 == rightTableName).Item2;

            foreach (var leftRecord in leftTableRecords)
            {
                var fkColumnValue = leftRecord[joinStructure.IndexOf(fkColumn)];

                foreach (var rightRecord in rightTableRecords)
                {
                    if (rightRecord[0] == fkColumnValue)
                    {
                        if (usesFK == true)
                        {
                            for (int idx = 1; idx < rightRecord.Count; idx++)
                            {
                                var joinResultRecord = leftRecord.ToList();
                                var rightRecordColumns = (rightRecord[idx] + "#" + MongoDB.GetRecordValueWithKey(rightTableName, rightRecord[idx])).Split('#');
                                for (int jdx = 0; jdx < rightRecordColumns.Length; jdx++)
                                {
                                    if (jdx != rightTableStructure.IndexOf(fkColumn))
                                    {
                                        joinResultRecord.Add(rightRecordColumns[jdx]);
                                    }
                                }
                                records.Add(joinResultRecord);
                            }
                        }
                        else
                        {
                            var joinResultRecord = leftRecord.ToList();
                            for (int jdx = 0; jdx < rightRecord.Count; jdx++)
                            {
                                if (jdx != rightTableStructure.IndexOf(fkColumn))
                                {
                                    joinResultRecord.Add(rightRecord[jdx]);
                                }
                            }
                            records.Add(joinResultRecord);
                        }
                    }
                }
            }

            return records;
        }

        private List<string> PerformSortMergeJoin()
        {
            var records = new List<string>();

            return records;
        }
    }
}
