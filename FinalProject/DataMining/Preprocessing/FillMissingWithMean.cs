using DataMining.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Preprocessing
{
    public class FillMissingWithMean : IDataProcessor
    {
        private string _columnName;

        public FillMissingWithMean(string columnName)
        {
            _columnName = columnName;
        }

        public void Process(DataCollection data)
        {
            double total = 0;
            int count = 0;
            double tempResult;
            data[_columnName].ForEach(value => 
                {
                    if (!string.IsNullOrEmpty(value) && Double.TryParse(value, out tempResult))
                    {
                        total += tempResult;
                        count++;
                    }
                });
            double mean = total / count;

            foreach (DataRow row in data.Rows)
                if (string.IsNullOrEmpty(row[_columnName] as string) ||
                    !Double.TryParse(row[_columnName] as string, out tempResult))
                    row[_columnName] = mean;
        }
    }
}
