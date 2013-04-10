using DataMining.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Preprocessing
{
    public class FillMissingWithMedian : IDataProcessor
    {
        private string _columnName;

        public FillMissingWithMedian(string columnName)
        {
            _columnName = columnName;
        }

        public void Process(DataCollection data)
        {
            List<double> values = new List<double>();
            double tempResult;
            double median;

            data[_columnName].ForEach(value =>
            {
                if (!string.IsNullOrEmpty(value) && Double.TryParse(value, out tempResult))
                    values.Add(tempResult);
            });

            values.Sort();

            if (values.Count % 2 == 0)
            {
                double value1 = values.ElementAt((values.Count / 2 - 1));
                double value2 = values.ElementAt((values.Count / 2));
                median = (value1 + value2) / 2;
            }
            else
                median = values.ElementAt((values.Count / 2));

            foreach (DataRow row in data.Rows)
                if (string.IsNullOrEmpty(row[_columnName] as string) ||
                    !Double.TryParse(row[_columnName] as string, out tempResult))
                    row[_columnName] = median;
        }
    }
}
