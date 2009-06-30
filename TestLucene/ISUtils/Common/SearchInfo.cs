using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    [Serializable]
    public class SearchInfo
    {
        public const int DEFPAGESIZE=10;
        private QueryInfo queryInfo;
        public QueryInfo Query
        {
            get { return queryInfo; }
            set { queryInfo = value; }
        }
        private int pageSize = DEFPAGESIZE;
        public int PageSize
        {
            get 
            { 
                return pageSize; 
            }
            set 
            { 
                pageSize = value;
                if (pageSize < DEFPAGESIZE)
                    pageSize = DEFPAGESIZE;
            }
        }
        private int pageNum = 1;
        public int PageNum
        {
            get 
            { 
                return pageNum; 
            }
            set 
            { 
                pageNum = value;
                if (pageNum <= 0)
                    pageNum = 1;
            }
        }
        public override string ToString()
        {
            return queryInfo.ToString()+"\tPageSize="+pageSize.ToString()+"\tPageNum="+pageNum.ToString();
        }
    }
}
