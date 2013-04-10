using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Apriori
{
    public class Item : IComparable<Item>
    {
        public string Column { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Column, Value);
        }

        public int CompareTo(Item other)
        {
            return ToString().CompareTo(other.ToString());
        }
    }
}
