using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Apriori
{
    public class Set
    {
        private SortedSet<Item> _set;

        public int SupportCount { get; set; }
        public double Confidence { get; set; }

        public Set()
        {
            _set = new SortedSet<Item>();
            SupportCount = 0;
            Confidence = 0;
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

        public bool Contains(Set other)
        {
            bool contains;
            foreach (var otherItem in other.Items)
            {
                contains = false;
                foreach (var item in Items)
                {
                    if (item == otherItem) contains = true;
                }
                if (!contains) return false;
            }
            return true;
        }

        public List<Set> GenerateNonEmptySubset()
        {
            List<Set> result = new List<Set>();
            Set set = null;
            foreach (var item in GetPowerSet(_set))
	        {
                set = new Set();
                foreach (var item1 in item.ToList())
	            {
                    set.SetAdd(item1);
	            }
                result.Add(set);
	        }
            return result;
        }

        private IEnumerable<IEnumerable<Item>> GetPowerSet(IEnumerable<Item> input)
        {
            var seed = new List<IEnumerable<Item>>(){Enumerable.Empty<Item>()};

            return input.Aggregate(seed.AsEnumerable(), (a, b) =>
              a.Concat(a.Select(x => x.Concat(new List<Item>() { b }))));
        }

        public void Except(ICollection<Item> items)
        {
            _set.ExceptWith(items);
        }
    }
}
