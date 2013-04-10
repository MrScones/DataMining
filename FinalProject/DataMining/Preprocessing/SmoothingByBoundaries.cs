using DataMining.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Preprocessing
{
    public class SmoothingByBoundaries : IDataProcessor
    {
        private string _columnName;

        public SmoothingByBoundaries(string columnName, int binSize = 3)
        {
            _columnName = columnName;
        }

        public void Process(DataCollection data)
        {
            throw new NotImplementedException();
        }
    }
}
