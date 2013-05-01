using System;
using DataMining.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace DataMining.Statistics
{
    public class Statistics
    {
        private String columnName;

        Statistics (String columnName){
            this.columnName = columnName;
        }


        public double calculateMean(DataCollection data){
            double total = 0;
            int count = 0;
            double tempResult;
            double mean;

            data[columnName].ForEach(value =>
            {
                if (!string.IsNullOrEmpty(value) && Double.TryParse(value, out tempResult))
                {
                    total += tempResult;
                    count++;
                }
            });

            mean = total / count;
            return mean;
        }


        public double calculateMedian(DataCollection data){
            List<double> values = new List<double>();
            double tempResult;
            double median;

            data[columnName].ForEach(value =>
            {
                if (!string.IsNullOrEmpty(value) && Double.TryParse(value, out tempResult))
                    values.Add(tempResult);
            });

            values.Sort();

            if (values.Count % 2 == 0)
            {
                double value1 = values.ElementAt((values.Count / 2 - 1));
                double value2 = values.ElementAt((values.Count / 2));
                median = (value1 + value2) / 2;
            } else {
                median = values.ElementAt((values.Count / 2));
            }

            return median;
        }



        public double calculateMode(DataCollection data)
        {
            double mode;
            double number;
            List<double> currentMax = new List<double>();
            List<List<double>> values =  new List<List<double>>();

            data[columnName].ForEach(value =>
            {
                if (!string.IsNullOrEmpty(value) && Double.TryParse(value, out number))
                {
                    foreach (List<double> currentValue in values)
                    {
                        if (compDoubles(currentValue[0], number))
                        {
                            currentValue[1]++;
                            break;
                        }
                        else
                        {   
                            List<double> tmpList = new List<double>();
                            tmpList.Add(number);
                            tmpList.Add(1);
                            values.Add(tmpList);
                        }
                    }
                }
            });

            currentMax.Add(0);
            currentMax.Add(0);

            foreach (List<double> currentValue in values)
            {
                if (currentValue[1] > currentMax[1])
                {
                    currentMax = currentValue;
                }
            }

            mode = currentMax[0];

            return mode;
        }


        public double maxValue(DataCollection data)
        {
            double number;
            double currentMax = 0;

            data[columnName].ForEach(value =>
            {
                if (!string.IsNullOrEmpty(value) && Double.TryParse(value, out number))
                {
                    currentMax = Math.Max(number, currentMax);
                }
            });

            return currentMax;
        }

        public double minValue(DataCollection data)
        {
            double number;
            double currentMin = 0;

            data[columnName].ForEach(value =>
            {
                if (!string.IsNullOrEmpty(value) && Double.TryParse(value, out number))
                {
                    currentMin = Math.Min(number, currentMin);
                }
            });

            return currentMin;
        }

        public double calculateDeviation(DataCollection data)
        {
            double mean = calculateMean(data);
            double variance;
            double runningSum = 0;
            double number;
            int count = 0;

            data[columnName].ForEach(value =>
            {
                if (!string.IsNullOrEmpty(value) && Double.TryParse(value, out number))
                {
                    runningSum += Math.Pow((number - mean), 2);
                    count++;
                }
            });
            variance = runningSum / count;

            return Math.Sqrt(variance);
        }

        public double lowerQuartile(DataCollection data){
            List<double> values = new List<double>();
            double tempResult;
            int mid;
            int lQ;

            data[columnName].ForEach(value =>
            {
                if (!string.IsNullOrEmpty(value) && Double.TryParse(value, out tempResult))
                    values.Add(tempResult);
            });

            values.Sort();

            mid = (int)Math.Ceiling(values.Count / 2.0);
            lQ = (int)Math.Ceiling(mid / 2.0);

            return values[lQ];
        }

        public double upperQuartile(DataCollection data)
        {
            List<double> values = new List<double>();
            double tempResult;
            int mid;
            int uQ;

            data[columnName].ForEach(value =>
            {
                if (!string.IsNullOrEmpty(value) && Double.TryParse(value, out tempResult))
                    values.Add(tempResult);
            });

            values.Sort();

            mid = (int)Math.Ceiling(values.Count / 2.0);
            uQ = (int)Math.Ceiling((values.Count - mid) / 2.0);

            return values[values.Count - uQ];
        }

        public double calculateSkew(DataCollection data)
        {
            double mean = calculateMean(data);
            double sDev = calculateDeviation(data);
            double runningSum = 0;
            double number;
            int count = 0;

            data[columnName].ForEach(value =>
            {
                if (!string.IsNullOrEmpty(value) && Double.TryParse(value, out number))
                {
                    runningSum += Math.Pow((number - mean), 3);
                    count++;
                }
            });

            count--;

            sDev = Math.Pow(sDev, 3);
            return runningSum / (sDev * count);
        }


        private bool compDoubles(double a, double b)
        {
            return Math.Abs(a - b) < 0.001;
        }
    }
}
