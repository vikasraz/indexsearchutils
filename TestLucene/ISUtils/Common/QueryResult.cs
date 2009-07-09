using System;
using System.Collections.Generic;
using Lucene.Net.Documents;
using Lucene.Net.Search;

namespace ISUtils.Common
{
    [Serializable]
    public class QueryResult
    {
        [Serializable]
        public sealed class ExDocument
        {
            public Document doc;
            public float score;
            public ExDocument() { }
            public ExDocument(Document dc, float s)
            {
                if (dc == null)
                    throw new ArgumentNullException("dc", "should input a true doc for ExDocument.");
                doc=dc;
                score =s;
            }
        }
        [Serializable]
        public sealed class SearchInfo
        {
            private string indexName="";
            private List<string> searchFields = new List<string>();
            public string IndexName
            {
                get { return indexName; }
                set { indexName = value; }
            }
            public List<string> Fields
            {
                get 
                {
                    if (searchFields == null)
                        searchFields = new List<string>();
                    return searchFields; 
                }
                set { searchFields = value; }
            }
            public SearchInfo()
            { 
            }
            public SearchInfo(string name, List<string> fields)
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentNullException("name", "name error in SearchInfo.");
                if (fields == null)
                    throw new ArgumentNullException("fields", "fields error in SearchInfo.");
                if (fields==null)
                    throw new ArgumentNullException("fields", "fields error in SearchInfo.");
                IndexName = name;
                searchFields=fields;
            }
            public SearchInfo(string name, string[] fields)
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentNullException("name", "name error in SearchInfo.");
                if (fields==null)
                    throw new ArgumentNullException("fields", "fields error in SearchInfo.");
                if (fields.Length <=0)
                    throw new ArgumentNullException("fields", "fields error in SearchInfo.");
                IndexName = name;
                if (searchFields == null)
                    searchFields = new List<string>();
                searchFields.Clear();
                searchFields.AddRange(fields); 
            }            
        }
        public Dictionary<SearchInfo, List<ExDocument>> docs;
        public List<ExDocument> docList;
        public QueryResult()
        {
            docs = new Dictionary<SearchInfo, List<ExDocument>>();
            docList = new List<ExDocument>();
        }
        public void AddResult(Hits hits, int maxMatches)
        {
            if (docList == null)
                docList = new List<ExDocument>();
            if (hits == null)
                return;
            for (int i = 0; i < maxMatches && i < hits.Length(); i++)
            {
                docList.Add(new ExDocument(hits.Doc(i), hits.Score(i)));
            }
        }
        public void AddResult(SearchInfo info, Hits hits, int maxMatches)
        {
            if ( docs == null)
                docs = new Dictionary<SearchInfo, List<ExDocument>>();
            if (hits == null)
                return;
            if (info == null)
                return;
            List<ExDocument> exdl = new List<ExDocument>();
            for (int i = 0; i < maxMatches && i < hits.Length(); i++)
            {
                exdl.Add(new ExDocument(hits.Doc(i),hits.Score(i)));
            }
            if ( exdl.Count > 0)
               docs.Add(info, exdl);
        }
        public void AddResult(List<SearchInfo> infoList, List<Hits> hitsList, int maxMatches)
        {
            if (docs == null)
                docs = new Dictionary<SearchInfo, List<ExDocument>>();
            if (hitsList == null)
                return;
            if (infoList == null)
                return;
            for (int step = 0; step < infoList.Count; step++ )
            {
                Hits hits = hitsList[step];
                SearchInfo info = infoList[step];
                List<ExDocument> exdl = new List<ExDocument>();
                for (int i = 0; i < maxMatches && i < hits.Length(); i++)
                {
                    exdl.Add(new ExDocument(hits.Doc(i), hits.Score(i)));
                }
                if (exdl.Count > 0)
                    docs.Add(info, exdl);
            }
        }
    }
}
