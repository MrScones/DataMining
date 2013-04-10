using System.Globalization;
using DataMining.Core;
using System.Data;
using System.Linq;

namespace DataMining.Preprocessing
{
    public class MinMaxNormalization : IDataProcessor
    {
        private string _columnName;
        private double _newMin, _newMax;
        public MinMaxNormalization(string columnName, double min, double max)
        {
            _columnName = columnName;
            _newMin = min;
            _newMax = max;
        }

        public void Process(DataCollection data)
        {
            var max = data[_columnName].Max(value => double.Parse(value));
            var min = data[_columnName].Min(value => double.Parse(value));

            foreach (DataRow row in data.Rows)
            {
                var value = double.Parse(row[_columnName].ToString());
                var newValue = ((value - min) / (max - min)) * (_newMax - _newMin) + _newMin;
                row[_columnName] = newValue.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
