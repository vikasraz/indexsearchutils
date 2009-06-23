using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Async
{
    public class IndexProgressChangedEventArgs
    {
        private int total = 0;
        public int Total
        {
            get { return total; }
        }
        private int current = 0;
        public int Current
        {
            get { return current; }
        }
        public IndexProgressChangedEventArgs(int totalnum,int curnum)
        {
            total =totalnum;
            current = curnum;
        }
    }
}
