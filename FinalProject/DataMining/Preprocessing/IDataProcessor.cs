using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMining.Core;

namespace DataMining.Preprocessing
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        void Process(DataCollection data);
    }
}
