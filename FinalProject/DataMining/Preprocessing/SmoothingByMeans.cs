using DataMining.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Preprocessing
{
    public class SmoothingByMeans : IDataProcessor
    {
        private string _columnName;
        private int _binSize;

        public SmoothingByMeans(string columnName, int binSize)
        {
            _columnName = columnName;
            _binSize = binSize;
        }

        public void Process(DataCollection data)
        {
            int count = 0;
            double total = 0;
            double mean;
            double tempResult;
            List<DataRow> rows = new List<DataRow>();
            DataRow[] bin = new DataRow[_binSize];
            
            //Extract rows to list for sorting
            foreach(DataRow row in data.Rows)
                rows.Add(row);
            //Sort rows by target column
            rows = rows
                .Where(row => Double.TryParse(row[_columnName].ToString(), out tempResult))
                .OrderBy(row => Double.Parse(row[_columnName].ToString()))
                .ToList();

            //Partition into equal-sized bins
            foreach (DataRow row in rows)
            {
                bin[count % _binSize] = row;
                total += Double.Parse(row[_columnName] as string);
                count++;
                if (count % _binSize == 0)
                {
                    mean = total / _binSize;
                    foreach (DataRow binRow in bin)
                        binRow[_columnName] = mean.ToString();
                    total = 0;
                }
            }

            //Process possible remainder.
            int remainder = rows.Count % _binSize;
            if(remainder != 0)
            {
                mean = total / remainder;
                for (int i = 0; i < remainder; i++)
                    bin[i][_columnName] = mean.ToString();
            }
        }
    }
}
