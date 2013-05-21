using DataMining.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Preprocessing
{
    public class DeleteRows : IDataProcessor
    {
        private string _columnName;
        private Func<string, bool> _predicate; 

        public DeleteRows(string columnName, Func<string, bool> predicate)
        {
            _columnName = columnName;
            _predicate = predicate;
        }

        public void Process(DataCollection data)
        {
            for (int i = data.Rows.Count - 1; i > 0; i--)
            {
                if (_predicate(data[i, _columnName] as string))
                    data[i].Delete();
            }
        }
    }
}
