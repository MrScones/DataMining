using DataMining.Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMining.Preprocessing;

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

            //TODO: Preprocessing

        }
    }
}
