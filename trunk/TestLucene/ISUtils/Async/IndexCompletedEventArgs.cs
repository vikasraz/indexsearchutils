using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Async
{
    public class IndexCompletedEventArgs
    {
        private string indexName = "";
        public string IndexName
        {
            get { return indexName; }
        }
        public IndexCompletedEventArgs(string name)
        {
            indexName = name;
        }
    }
}
