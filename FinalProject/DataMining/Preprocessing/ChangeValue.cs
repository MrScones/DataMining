using DataMining.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Preprocessing
{

    public class ChangeValue : IDataProcessor
    {
        private string _columnName;
        private Func<string, string> _valueConverter;

        public ChangeValue(Func<string, string> valueConverter)
        {
            _valueConverter = valueConverter;
        }

        public ChangeValue(string columnName, Func<string, string> valueConverter) : this(valueConverter)
        {
            _columnName = columnName;
        }

        public void Process(DataCollection data)
        {
            foreach (DataRow row in data.Rows)
            {
                if (_columnName != null)
                    row[_columnName] = _valueConverter.Invoke(row[_columnName] as string);
                else
                    row.ItemArray = row.ItemArray.Cast<string>().Select(v => _valueConverter.Invoke(v)).ToArray();
            }
        }
    }
}
