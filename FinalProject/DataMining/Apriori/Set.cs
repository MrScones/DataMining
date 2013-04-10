using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Apriori
{
    public class Set
    {
        private SortedSet<Item> _set;

        public int Count { get; set; }

        public Set()
        {
            _set = new SortedSet<Item>();
            Count = 0;
        }

        public Set(Item item) : this()
        {
            _set.Add(item);
        }

        public void SetAdd(Item item)
        {
            if (!_set.Contains(item))
                _set.Add(item);
        }

        public void Union(ICollection<Item> items)
        {
            _set.UnionWith(items);
        }

        public List<Item> Items { get { return _set.ToList(); } } 

        public override string ToString()
        {
            return string.Join(",", _set.Select(item => item.ToString()));
        }

        public override bool Equals(object obj)
        {
            return ToString() == obj.ToString();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
