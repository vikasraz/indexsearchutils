using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Async
{
    public delegate void WriteDbProgressChangedEventHandler(object sender, WriteDbProgressChangedEventArgs e);
    public class WriteDbProgressChangedEventArgs
    {
        private int rowNum = 0;
        private int current = 0;
        public int RowNum
        {
            get
            {
                return rowNum;
            }
        }
        public int CurrentRow
        {
            get
            {
                return current;
            }
        }
        public WriteDbProgressChangedEventArgs(int rownum, int cur)
        {
            rowNum=rownum;
            current=cur;
        }
    }
}
