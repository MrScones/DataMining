using DataMining.Core;
using DataMining.Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMining.Preprocessing;
using System.Collections.Concurrent;
using DataMining.Apriori;

namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputFile = "adult.data";

            //Input and Output CSV file handlers
            var input = new CsvFileIO(
                filepath: inputFile,
                hasHeaders: true,
                delimiter: ","
            );

            var manager = new PreprocessingManager(input);

            //Give labels to age ranges using equal width binning technique.
            manager.AddProcessor(new EqualWidthBinLabeling("age", 5));
            //Give missing values in occupation column the label 'Unknown'.
            manager.AddProcessor(new ChangeValue("occupation", value => value == "?" ? "Unknown" : value));
            //Give missing values in native-country column the label 'Unknown'.
            manager.AddProcessor(new ChangeValue("native-country", value => value == "?" ? "Unknown" : value));

            //Preprocess data.
            manager.Run();

            foreach (var ageRange in manager.Data["age"]
                .GroupBy(a => a)
                .Select(group => new { group.Key, Count = group.Count() })
                .OrderBy(group => group.Key))
            {
                Console.WriteLine(ageRange.Key + ": " + ageRange.Count);
            }
            Console.Read();

            //Calculate Apriori threshold from a 1 % support.
            var threshold = (int)Math.Round(manager.Data.Rows.Count * 0.01);
            var apriori = new AprioriAlgorithm(threshold, "age", "sex", "income", "marital-status");
            var results = apriori.Calculate(manager.Data);

            foreach (var set in results)
            {
                Console.WriteLine(string.Format("FS: {0}  SupportCount: {1}", set.ToString(), set.SupportCount));
                
                //Print Equation 6.1 page. 245 in DM book.
                var confidenceResults = apriori.CalculateConfidence(manager.Data, set, 0.70);
                foreach (var confidence in confidenceResults)
                {
                    Set rightsideOfRule = set;
                    rightsideOfRule.Except(confidence.Items);
                    Console.WriteLine(string.Format("   - {0}  --> {1}[Support = {2:P2}, Confidence = {3:P2}]", confidence.ToString(), rightsideOfRule.ToString(), (double)confidence.SupportCount / manager.Data.Rows.Count, confidence.Confidence));
                }
            }

            Console.Read();
            Console.Read();
        }

    }
}
