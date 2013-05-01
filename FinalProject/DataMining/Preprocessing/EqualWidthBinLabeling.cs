using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Preprocessing
{
    public class EqualWidthBinLabeling : IDataProcessor
    {
        private readonly string _columnName;
        private readonly int _binWidth;

        public EqualWidthBinLabeling(string columnName, int binWidth)
        {
            _columnName = columnName;
            _binWidth = binWidth;
        }

        public void Process(Core.DataCollection data)
        {
            foreach (DataRow row in data.Rows)
                row[_columnName] = GenerateBinLabel(row[_columnName] as string);
        }

        private string GenerateBinLabel(string value)
        {
            //Check if strings are int or double and convert then to integers.
            double dummy;
            int val = double.TryParse(value, out dummy)
                            ? (int)Math.Round(double.Parse(value))
                            : int.Parse(value);

            //Calculate bin bounds .
            int lowBound = val - (val % _binWidth);
            int highBound = lowBound + _binWidth - 1;
            return string.Format("{0} - {1}", lowBound, highBound);
        }
    }
}
