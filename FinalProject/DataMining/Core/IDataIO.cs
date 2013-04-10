using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataIO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        DataCollection Load();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        void Save(DataCollection data);
    }
}
