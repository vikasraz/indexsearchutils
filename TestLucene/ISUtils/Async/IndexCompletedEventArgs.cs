using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Async
{
    public delegate void IndexCompletedEventHandler(object sender, IndexCompletedEventArgs e);
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
