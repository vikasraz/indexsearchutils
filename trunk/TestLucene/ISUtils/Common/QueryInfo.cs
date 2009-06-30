using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;

namespace ISUtils.Common
{
    [Serializable]
    public class QueryInfo
    {
        private bool isFuzzySearch = true;
        public bool IsFuzzySearch
        {
            get { return isFuzzySearch; }
            set { isFuzzySearch = value; }
        }
        private SqlQuery sqlQuery=new SqlQuery();
        private FuzzyQuery fuzzyQuery = new FuzzyQuery();
        public SqlQuery SQuery
        {
            get
            {
                if (!isFuzzySearch)
                    return sqlQuery;
                else
                    return null;
            }
            set
            {
                if (!isFuzzySearch)
                    sqlQuery = value;
            }
        }
        public FuzzyQuery FQuery
        {
            get
            {
                if (isFuzzySearch)
                    return fuzzyQuery;
                else
                    return null;
            }
            set
            {
                if (isFuzzySearch)
                    fuzzyQuery = value;
            }
        }
        public override string ToString()
        {
            if (isFuzzySearch)
                return fuzzyQuery.ToString();
            else
                return sqlQuery.ToString();
        }
    }
}
