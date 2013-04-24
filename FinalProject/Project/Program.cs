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
            //Treat age column as numeric values.
            manager.Data.SetColumnType("age", DataType.Numeric);

            //Give missing values in occupation column the label 'Unknown'.
            manager.AddProcessor(new ChangeValue("occupation", value => value == "?" ? "Unknown" : value));
            //Give missing values in native-country column the label 'Unknown'.
            manager.AddProcessor(new ChangeValue("native-country", value => value == "?" ? "Unknown" : value));

            //Preprocess data.
            manager.Run();

            //Calculate Apriori threshold from a 1 % support count.
            var threshold = (int)Math.Round(manager.Data.Rows.Count * 0.01);
            var apriori = new AprioriAlgorithm(threshold, "age", "sex", "income", "marital-status");
            var results = apriori.Calculate(manager.Data);

            foreach (var set in results)
            {
                Console.WriteLine(string.Format("FS: {0}  SupportCount: {1}", set.ToString(), set.SupportCount));
            }

            Console.Read();
        }

    }
}
