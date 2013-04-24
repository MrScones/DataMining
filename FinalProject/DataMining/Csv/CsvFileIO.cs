using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMining.Core;
using System.Data;
using System.IO;

namespace DataMining.Csv
{
    public class CsvFileIO : IDataIO
    {
        private string _filepath;
        private bool _hasHeaders;
        private string _delimiter;

        public Encoding Encoding { get; set; }

        public CsvFileIO(string filepath, bool hasHeaders, string delimiter)
        {
            _filepath = filepath;
            _hasHeaders = hasHeaders;
            _delimiter = delimiter;
            Encoding = Encoding.GetEncoding(1252);
        }

        public DataCollection Load()
        {
            var data = new DataTable();

            using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(_filepath, Encoding))
            {
                parser.SetDelimiters(_delimiter);
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.HasFieldsEnclosedInQuotes = false;
                if (parser.EndOfData) throw new ApplicationException("Input file error");
                
                string[] fields = parser.ReadFields();

                if (_hasHeaders)
                {
                    for (int i = 0; i < fields.Length; i++)
                        data.Columns.Add(new DataColumn(fields[i]));
                    fields = parser.ReadFields();
                }
                else
                {
                    for (int i = 0; i < fields.Length; i++)
                        data.Columns.Add(new DataColumn("Column" + i));
                }

                while (!parser.EndOfData)
                {
                    data.Rows.Add(fields);
                    fields = parser.ReadFields();
                }

                data.Rows.Add(fields);

                parser.Close();
            }

            return new DataCollection(data);
        }

        public void Save(DataCollection data)
        {
            var sb = new StringBuilder();

            if(_hasHeaders)
                sb.AppendLine(string.Join(_delimiter, data.ColumnNames));

            foreach (DataRow row in data.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(_delimiter, fields));
            }

            File.WriteAllText(_filepath, sb.ToString(), Encoding);
        }
    }
}
