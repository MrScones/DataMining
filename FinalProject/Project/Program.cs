using DataMining.Core;
using DataMining.Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMining.Knn;
using DataMining.Preprocessing;
using System.Collections.Concurrent;
using DataMining.Apriori;

namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            #region Input

            const string inputFile = "adult.data";
            const string inputTestFile = "adult.test";

            //Input data set.
            var input = new CsvFileIO(
                filepath: inputFile,
                hasHeaders: true,
                delimiter: ","
            );

            //Input test set for KNN
            var inputTest = new CsvFileIO(
                filepath: inputTestFile,
                hasHeaders: true,
                delimiter: ","
            );

            #endregion

            #region Preprocessing

            var manager = new PreprocessingManager(input);
            //Declare which attributes are numeric.
            manager.Data.SetColumnType("capital-loss", DataType.Numeric);
            manager.Data.SetColumnType("capital-gain", DataType.Numeric);
            manager.Data.SetColumnType("hours-per-week", DataType.Numeric);
            

            //Give labels to age ranges using equal width binning technique.
            manager.AddProcessor(new EqualWidthBinLabeling("age", 5));
            //Give missing values in occupation column the label 'Unknown'.
            manager.AddProcessor(new ChangeValue("occupation", value => value == "?" ? "Unknown" : value));
            //Give missing values in native-country column the label 'Unknown'.
            manager.AddProcessor(new ChangeValue("native-country", value => value == "?" ? "Unknown" : value));
            //Remove rows in workclass with values: ?, Never-worked, Without-pay.
            manager.AddProcessor(new DeleteRows("workclass", 
                value => value == "?" || value == "Never-worked" || value == "Without-pay"));
            //Normalize captital-gain
            manager.AddProcessor(new MinMaxNormalization("capital-gain", 0, 1));
            //Normalize capital-loss
            manager.AddProcessor(new MinMaxNormalization("capital-loss", 0, 1));
            //Normalize hours-per-week
            manager.AddProcessor(new MinMaxNormalization("hours-per-week", 0, 1));
            //Discretize workclass into public sector  and private sector.
            manager.AddProcessor(new ChangeValue("workclass", value => 
                value == "State-gov" || value == "Federal-gov" || value == "Local-gov"
                ? "Public Sector"
                : "Private Sector"));

            //Preprocess data.
            manager.Run();

            #endregion

            #region Preprocessing Test Set

            var managerTest = new PreprocessingManager(inputTest);
            //Declare which attributes are numeric.
            managerTest.Data.SetColumnType("capital-loss", DataType.Numeric);
            managerTest.Data.SetColumnType("capital-gain", DataType.Numeric);
            managerTest.Data.SetColumnType("hours-per-week", DataType.Numeric);


            //Give labels to age ranges using equal width binning technique.
            managerTest.AddProcessor(new EqualWidthBinLabeling("age", 5));
            //Give missing values in occupation column the label 'Unknown'.
            managerTest.AddProcessor(new ChangeValue("occupation", value => value == "?" ? "Unknown" : value));
            //Give missing values in native-country column the label 'Unknown'.
            managerTest.AddProcessor(new ChangeValue("native-country", value => value == "?" ? "Unknown" : value));
            //Remove rows in workclass with values: ?, Never-worked, Without-pay.
            managerTest.AddProcessor(new DeleteRows("workclass",
                value => value == "?" || value == "Never-worked" || value == "Without-pay"));
            //Normalize captital-gain
            managerTest.AddProcessor(new MinMaxNormalization("capital-gain", 0, 1));
            //Normalize capital-loss
            managerTest.AddProcessor(new MinMaxNormalization("capital-loss", 0, 1));
            //Normalize hours-per-week
            managerTest.AddProcessor(new MinMaxNormalization("hours-per-week", 0, 1));
            //Discretize workclass into public sector  and private sector.
            managerTest.AddProcessor(new ChangeValue("workclass", value =>
                value == "State-gov" || value == "Federal-gov" || value == "Local-gov"
                ? "Public Sector"
                : "Private Sector"));

            //Preprocess data.
            managerTest.Run();

            #endregion

            #region Apriori

            Console.WriteLine("########## Apriori ##########");

            //Calculate Apriori threshold from a 1 % support.
            var threshold = (int)Math.Round(manager.Data.Rows.Count * 0.3);
            var apriori = new AprioriAlgorithm(threshold, "age", "sex", "income", "marital-status");
            var results = apriori.Calculate(manager.Data);

            foreach (var set in results)
            {   
                //Print Equation 6.1 page. 245 in DM book + lift stuff
                var confidenceResults = apriori.CalculateConfidence(manager.Data, set, 0.90);
                foreach (var confidence in confidenceResults)
                {
                    //get right side (what is actually implied by some other item/items.)
                    Set rightsideOfRule = new Set();
                    rightsideOfRule.Union(set.Items);
                    rightsideOfRule.Except(confidence.Items);
                    rightsideOfRule = apriori.CalculateSupport(manager.Data, rightsideOfRule);
                    
                    //lift calculations:
                    double probabilityAUnionB = (double)set.SupportCount / manager.Data.Rows.Count;
                    double probabilityA = (double)confidence.SupportCount / manager.Data.Rows.Count;
                    double probabilityB =(double)rightsideOfRule.SupportCount / manager.Data.Rows.Count;

                    double lift = (double)probabilityAUnionB / (probabilityA * probabilityB);
                    Console.WriteLine(string.Format("###{0}  --> {1}[Support = {2:P2}, Confidence = {3:P2}, Lift = {4:0.00}]", confidence.ToString(), rightsideOfRule.ToString(), (double)confidence.SupportCount / manager.Data.Rows.Count, confidence.Confidence, lift));
                }
            }

            #endregion

            Console.WriteLine();

            #region K-nearest-neighbors

            Console.WriteLine("########## K-nearest-neighbors ##########");

            var knn = new KnnAlgorithm(1, manager.Data, managerTest.Data, "workclass", 1,
                        "age", "education", "occupation", "sex", "capital-gain", 
                        "capital-loss", "hours-per-week", "income");
            
            knn.Calculate();

            Console.WriteLine("Correct Predictions: " +knn.CorrectPredictions);
            Console.WriteLine("Wrong Predictions: " + knn.WrongPredictions);
            Console.WriteLine("Model Accuracy: " + ((double)knn.CorrectPredictions / (double)manager.Data.Rows.Count));

            #endregion

            Console.Read();
        }

    }
}