using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Async
{
    public class IndexProgressChangedEventArgs
    {
        private int percent = 0;
        public int Percentage
        {
            get { return percent; }
        }
        public IndexProgressChangedEventArgs(int percentage)
        {
            percent = percentage;
        }
    }
}
