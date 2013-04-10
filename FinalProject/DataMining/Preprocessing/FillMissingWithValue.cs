using DataMining.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Preprocessing
{
    public class FillMissingWithValue : IDataProcessor
    {
        private Func<string> _valueGenerator;
        private string _columnName;

        public FillMissingWithValue(Func<string> valueGenerator)
        {
            _valueGenerator = valueGenerator;
        }

        public FillMissingWithValue(string columnName, Func<string> valueGenerator) : this(valueGenerator)
        {
            _columnName = columnName;
        }

        public void Process(DataCollection data)
        {
            foreach (DataRow row in data.Rows)
            {
                if (!string.IsNullOrEmpty(_columnName))
                {
                    if (string.IsNullOrEmpty(row[_columnName] as string))
                        row[_columnName] = _valueGenerator.Invoke();
                }
                else
                {
                    row.ItemArray = row.ItemArray.Cast<string>().Select(value =>
                        {
                            if (!string.IsNullOrEmpty(value))
                                return value;
                            else
                                return _valueGenerator.Invoke();
                        })
                        .ToArray();
                }
            }
        }
    }
}
