using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMining.Core;

namespace DataMining.Preprocessing
{
    public class PreprocessingManager
    {
        #region Properties and Attributes

        private Queue<IDataProcessor> _processors;

        public DataCollection Data { get; set; }
        public IDataIO InputHandler { get; set; }
        public IDataIO OutputHandler { get; set; }

        #endregion

        #region Constructors

        public PreprocessingManager(IDataIO inputHandler)
        {
            InputHandler = inputHandler;
            _processors = new Queue<IDataProcessor>();
            Data = InputHandler.Load();
        }

        public PreprocessingManager(IDataIO inputHandler, IDataIO outputHandler) : this(inputHandler)
        {
            OutputHandler = outputHandler;
        }

        #endregion

        #region Methods

        public void AddProcessor(IDataProcessor processor)
        {
            _processors.Enqueue(processor);
        }

        public void Run()
        {
            //Execute all preprocessing units.
            while (_processors.Count > 0)
            {
                var processor = _processors.Dequeue();
                processor.Process(Data);
            }

            //Save output to file.
            if (OutputHandler != null)
                OutputHandler.Save(Data);
        }

        #endregion
    }
}
