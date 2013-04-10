using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class DataCollection
    {   
        private DataTable _data;
        private Dictionary<string, DataType> _columnTypes;

        #region Constructors

        public DataCollection()
        {
            _data = new DataTable();
            _columnTypes = new Dictionary<string, DataType>();
        }

        public DataCollection(DataTable data)
        {
            _data = data;
            _columnTypes = new Dictionary<string,DataType>();
            foreach (DataColumn column in data.Columns)
                _columnTypes.Add(column.ColumnName, DataType.Nominal);
        }

        #endregion

        #region Properties

        public DataRowCollection Rows
        {
            get { return _data.Rows; }
        }

        public DataColumnCollection Columns
        {
            get { return _data.Columns; }
        }

        public string[] ColumnNames
        {
            get { return _data.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray(); }
        }

        #endregion

        #region Indexers

        public DataRow this[int row]
        {
            get
            {
                return _data.Rows[row];
            }
        }

        public string this[int row, int column]
        {
            get
            {
                return _data.Rows[row][column] as string;
            }
        }

        public string this[int row, string columnName]
        {
            get
            {
                return _data.Rows[row][columnName] as string;
            }
        }

        public List<string> this[string columnName]
        {
            get
            {
                return _data.Select().Select(dr => dr[columnName] as string).ToList();
            }
        }

        #endregion

        #region Methods

        public void SetColumnType(string columnName, DataType dataType)
        {
            if (!_columnTypes.ContainsKey(columnName))
                _columnTypes.Add(columnName, dataType);
            else
                _columnTypes[columnName] = dataType;
        }

        public DataType GetColumnType(string columnName)
        {
            return _columnTypes[columnName];
        }

        public void AddColumn(string columnName, DataType dataType = DataType.Nominal)
        {
            _data.Columns.Add(columnName);
            SetColumnType(columnName, dataType);
        }

        public void AddRow(params string[] fields)
        {
            _data.Rows.Add(fields);
        }

        #endregion
    }
}
