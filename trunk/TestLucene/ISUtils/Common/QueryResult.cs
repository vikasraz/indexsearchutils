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
                doc=dc;
                score =s;
            }
        }
        [Serializable]
        public sealed class SearchInfo
        {
            public string IndexName;
            public string[] Fields;
            public SearchInfo()
            { 
            }
            public SearchInfo(string name, string[] fields)
            {
                IndexName = name;
                Fields = (string[])fields.Clone(); 
            }
        }
        public Dictionary<SearchInfo, List<ExDocument>> docs;
        public QueryResult()
        {
            docs = new Dictionary<SearchInfo, List<ExDocument>>();
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
