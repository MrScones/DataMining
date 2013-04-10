﻿using DataMining.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Apriori
{
    public class AprioriAlgorithm
    {
        private readonly int _threshold;
        private readonly string[] _columnNames;

        public AprioriAlgorithm(int threshold, params string[] columnNames)
        {
            _columnNames = columnNames;
            _threshold = threshold;
        }

        public List<Set> Calculate(DataCollection data)
        {
            List<Set> sets = GenerateInitialSets(data);
            List<Set> oldSets = null;
            while (sets.Count > 0)
            {
                oldSets = sets;
                sets = GenerateFrequentSets(sets);
                sets = CalculateSupport(data, sets);
                //Remove sets according to threshold
                sets = sets
                    .Where(set => set.Count >= _threshold)
                    .ToList();
            }
            return oldSets;
        }

        private List<Set> GenerateFrequentSets(List<Set> sets)
        {
            var newSets = new List<Set>();

            //Combine Sets
            for (int i = 0; i < sets.Count - 1; i++)
            {   
                for (int j = i + 1; j < sets.Count; j++)
                {
                    Set set;
                    bool combine = true;
                    
                    for (int k = 0; k < sets[i].Items.Count - 1; k++)
                        if (sets[i].Items[k] != sets[j].Items[k]) combine = false;
                    if (combine)
                    {
                        set = new Set();
                        set.Union(sets[i].Items);
                        set.Union(sets[j].Items);
                        newSets.Add(set);
                    }
                }
            }
            return newSets;
        }

        private List<Set> CalculateSupport(DataCollection data, List<Set> sets)
        {
            foreach (DataRow row in data.Rows)
            {
                foreach (var set in sets)
                {
                    bool hasSupport = true;
                    foreach (var item in set.Items)
                    {
                        if (row[item.Column].ToString() != item.Value) hasSupport = false;
                    }
                    if (hasSupport) set.Count++;
                }
            }
            return sets;
        }

        private List<Set> GenerateInitialSets(DataCollection data)
        {
            var sets = new List<Set>();
            //Generate initial sets.
            foreach (var column in _columnNames)
            {
                foreach (var value in data[column])
                {
                    var item = new Item {Column = column, Value = value};
                    var set = new Set(item);
                    if (!sets.Contains(set))
                    {
                        set.Count++;
                        sets.Add(set);
                    } 
                    else
                        sets.First(s => s.Equals(set)).Count++;
                }
            }

            //Remove sets according to threshold
            return sets
                .Where(set => set.Count >= _threshold)
                .ToList();
        }
    }
}
