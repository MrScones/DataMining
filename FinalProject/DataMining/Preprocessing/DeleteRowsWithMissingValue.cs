using DataMining.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Preprocessing
{
    public class DeleteRowsWithMissingValue : IDataProcessor
    {
        private string _columnName;

        public DeleteRowsWithMissingValue(string columnName)
        {
            _columnName = columnName;
        }

        public void Process(DataCollection data)
        {
            for (int i = data.Rows.Count - 1; i > 0; i--)
            {
                if (string.IsNullOrEmpty(data[i, _columnName] as string))
                    data[i].Delete();
            }
        }
    }
}
