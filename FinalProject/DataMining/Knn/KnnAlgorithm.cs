using System.Threading;
using DataMining.Preprocessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMining.Core;

namespace DataMining.Knn
{
    public class KnnAlgorithm
    {
        private DataCollection _trainingSet;
        private DataCollection _testSet;
        private string _classColumn;
        private string[] _processColumns;
        private int _k;
        private int _maxNominalDistance;
        private int _correct;
        private int _wrong;

        public int CorrectPredictions { get { return _correct; } }
        public int WrongPredictions { get { return _wrong; } }

        public KnnAlgorithm(int k,DataCollection trainingSet, DataCollection testSet, string classColumn, int maxNominalDistance, params string[] processColumns)
        {
            _k = k;
            _trainingSet = trainingSet;
            _testSet = testSet;
            _classColumn = classColumn;
            _processColumns = processColumns;
            _maxNominalDistance = maxNominalDistance;
            _correct = 0;
            _wrong = 0;
        }

        public void Calculate()
        {
            if (_k < 1) throw new Exception("k must be a positive number");
            if (!_testSet.ColumnNames.Contains(_classColumn)) 
                _testSet.Columns.Add(_classColumn);
            Parallel.ForEach(_testSet.Rows.Cast<DataRow>(), row =>
                {
                    var predictedValue = CalculateClass(row, _trainingSet);

                    if (row[_classColumn] as string == predictedValue)
                        Interlocked.Increment(ref _correct);
                    else
                        Interlocked.Increment(ref _wrong);
                    lock (this)
                    {
                        row[_classColumn] = predictedValue;
                    }
                    if ((_correct + _wrong) % 100 == 0)
                    {
                        Console.WriteLine(string.Format("Progress... {0}/{1}", _correct + _wrong, _testSet.Rows.Count));
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                    }
                });
        }

        private string CalculateClass(DataRow row, DataCollection data)
        {
            var list = new List<KeyValuePair<double, DataRow>>();
            double distance;
            foreach (DataRow dataRow in data.Rows)
            {
                distance = 0;
                foreach (var column in _processColumns)
                {
                    //For nominal values, use 0 if equal and highest nominal distance if not.
                    if (data.GetColumnType(column) == DataType.Nominal)
                    {
                        distance = (row[column].ToString() == dataRow[column].ToString())
                                        ? 0
                                        : _maxNominalDistance;
                    }
                        //For numeric values, use euclidean distance formula.
                    else
                    {
                        double x1 = double.Parse(row[column].ToString());
                        double x2 = double.Parse(dataRow[column].ToString());
                        distance += Math.Pow(x1 - x2, 2);
                    }
                }
                distance = Math.Sqrt(distance);
                
                list.Add(new KeyValuePair<double, DataRow>(distance, dataRow));
            }

            //Group class and order by occurence desc.
            list = list.OrderBy(kvp => kvp.Key).Take(_k).ToList();
            var groups = (from kvp in list
                          group kvp by kvp.Value[_classColumn].ToString() into grp
                          orderby grp.Count() descending
                          select new { Count = grp.Count(), Class = grp.Key }).ToList();

            //Check if the maximum class count is unique. If not recalc with k - 1.
            if (groups.Count > 1 && groups[0].Count == groups[1].Count)
            {
                _k = _k - 1;
                return CalculateClass(row, data);
            }
            else return groups[0].Class;
        }
    }
}
