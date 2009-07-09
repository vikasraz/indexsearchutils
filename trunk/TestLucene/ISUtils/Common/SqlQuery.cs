using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Search;
using Lucene.Net.Analysis;
using Lucene.Net.QueryParsers;

namespace ISUtils.Common
{
    #region "精确查询"
    [Serializable]
    public sealed class SqlQuery
    {
        #region "Properties"
        private string indexnames = "";
        public string IndexNames
        {
            get { return indexnames; }
            set { indexnames = value; }
        }
        private List<TableField> resultFields=new List<TableField>();
        public List<TableField> ResultFields
        {
            get { return resultFields; }
            set { resultFields = value; }
        }
        private List<string> tables=new List<string>();
        public List<string> SearchTables
        {
            get { return tables; }
            set { tables = value; }
        }
        private List<FilterCondition> filterConList = new List<FilterCondition>();
        public List<FilterCondition> FilterList
        {
            get { return filterConList; }
            set { filterConList = value; }
        }
        private List<ExcludeCondition> excludeList = new List<ExcludeCondition>();
        public List<ExcludeCondition> ExcludeList
        {
            get { return excludeList; }
            set { excludeList = value; }
        }
        private List<RangeCondition> rangeConList = new List<RangeCondition>();
        public List<RangeCondition> RangeList
        {
            get { return rangeConList; }
            set { rangeConList = value; }
        }
        #endregion
        #region "Functions"
        public void SetResultFields(params string[] fieldArray)
        {
            List<TableField> tfList = new List<TableField>();
            foreach (string result in fieldArray)
            {
                tfList.Add(new TableField(result));
            }
            resultFields = tfList;
        }
        public void SetResultFileds(List<string> fieldList)
        {
            List<TableField> tfList = new List<TableField>();
            foreach (string result in fieldList)
            {
                tfList.Add(new TableField(result));
            }
            resultFields = tfList;
        }
        public void SetSearchTables(params string[] tableArray)
        {
            if (tables == null)
                tables = new List<string>();
            tables.Clear();
            tables.AddRange(tableArray);
        }
        public void SetSearchTables(List<string> tableList)
        {
            tables = tableList;
        }
        public void SetFilters(params FilterCondition[] filters)
        {
            if (filterConList == null)
                filterConList = new List<FilterCondition>();
            filterConList.Clear();
            filterConList.AddRange(filters);
        }
        public void AddFilter(FilterCondition fc)
        {
            if (filterConList == null)
                filterConList = new List<FilterCondition>();
            filterConList.Add(fc);
        }
        public void AddFilters(params FilterCondition[] fcArray)
        {
            if (filterConList == null)
                filterConList = new List<FilterCondition>();
            filterConList.AddRange(fcArray);
        }
        public void AddFilters(List<FilterCondition> fcList)
        {
            if (filterConList == null)
                filterConList = new List<FilterCondition>();
            filterConList.AddRange(fcList);
        }
        public void SetRemoves(params ExcludeCondition[] excludes)
        {
            if (excludeList == null)
                excludeList = new List<ExcludeCondition>();
            excludeList.Clear();
            excludeList.AddRange(excludes);
        }
        public void AddRemove(ExcludeCondition ec)
        {
            if (excludeList == null)
                excludeList = new List<ExcludeCondition>();
            excludeList.Add(ec);
        }
        public void AddRemoves(params ExcludeCondition[] ecArray)
        {
            if (excludeList == null)
                excludeList = new List<ExcludeCondition>();
            excludeList.AddRange(ecArray);
        }
        public void AddRemoves(List<ExcludeCondition> ecList)
        {
            if (excludeList == null)
                excludeList = new List<ExcludeCondition>();
            excludeList.AddRange(ecList);
        }
        public void SetRanges(params RangeCondition[] ranges)
        {
            if (rangeConList == null)
                rangeConList = new List<RangeCondition>();
            rangeConList.Clear();
            rangeConList.AddRange(ranges);
        }
        public void AddRange(RangeCondition rc)
        {
            if (rangeConList == null)
                rangeConList = new List<RangeCondition>();
            rangeConList.Add(rc);
        }
        public void AddRanges(params RangeCondition[] rangeArray)
        {
            if (rangeConList == null)
                rangeConList = new List<RangeCondition>();
            rangeConList.AddRange(rangeArray);
        }
        public void AddRanges(List<RangeCondition> rcList)
        {
            if (rangeConList == null)
                rangeConList = new List<RangeCondition>();
            rangeConList.AddRange(rcList);
        }
        #endregion
        #region "override"
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append(indexnames + ":select ");
            if (resultFields != null)
            {
                foreach (TableField tf in resultFields)
                {
                    ret.Append(tf.ToString() + ",");
                }
                if (resultFields.Count > 0)
                    ret.Remove(ret.Length - 1, 1);
            }
            else
            {
                ret.Append("*");
            }
            ret.Append(" from ");
            if (tables != null)
            {
                foreach (string table in tables)
                {
                    ret.Append(table + ",");
                }
                if (tables.Count > 0)
                    ret.Remove(ret.Length - 1, 1);
            }
            else
            {
                ret.Append("*");
            }
            ret.Append(" where ");
            foreach (FilterCondition fc in filterConList)
            {
                ret.Append(fc.ToString() + " and ");
            }
            if (filterConList.Count > 0)
                ret.Append(" and ");
            foreach (RangeCondition rc in rangeConList)
            {
                ret.Append(rc.ToString() + " and ");
            }
            if (rangeConList.Count > 0)
                ret.Remove(ret.Length - 5, 1);
            if (excludeList.Count > 0)
                ret.Append(" and ");
            foreach (ExcludeCondition ec in excludeList)
            {
                ret.Append(ec.ToString() + " and ");
            }
            if (excludeList.Count > 0)
                ret.Remove(ret.Length - 5, 1);
            return ret.ToString();
        }
        #endregion
    }
    #endregion
}
