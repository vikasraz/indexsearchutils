using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    public enum RangeType
    {
        Date,
        Time,
        DateTime,
        Int,
        Double,
        Other
    }
    [Serializable]
    public class RangeCondition : TableField
    {
        //true for open interval,else closed interval
        private bool interval = false;
        public bool IntervalType
        {
            get { return interval; }
            set { interval = value; }
        }
        private string rangeFrom = "";
        private string rangeTo = "";
        private RangeType type = RangeType.Other;
        public string RangeFrom
        {
            get { return rangeFrom; }
            set { rangeFrom = value; }
        }
        public string RangeTo
        {
            get { return rangeTo; }
            set { rangeTo = value; }
        }
        public RangeType Type
        {
            get { return type; }
            set 
            { 
                type = value; 
            }
        }
        public RangeCondition()
        { 
        }
        public RangeCondition(string tablename, string fieldname, string from, string to)
            : base(tablename, fieldname)
        {
            if (string.IsNullOrEmpty(from))
                throw new ArgumentNullException("from", "Do not input any from");
            if (string.IsNullOrEmpty(to))
                throw new ArgumentNullException("to", "Do not input any to");
            rangeFrom = from;
            rangeTo = to;
        }
        public RangeCondition(string tablename, string fieldname, string from, string to,RangeType type)
            : base(tablename, fieldname)
        {
            if (string.IsNullOrEmpty(from))
                throw new ArgumentNullException("from", "Do not input any from");
            if (string.IsNullOrEmpty(to))
                throw new ArgumentNullException("to", "Do not input any to");
            rangeFrom = from;
            rangeTo = to;
            this.type = type;
        }
        public RangeCondition(string tablename, string fieldname, DateTime from, DateTime to)
            : base(tablename, fieldname)
        {
            rangeFrom = SupportClass.Time.GetLuceneDate(from);
            rangeTo = SupportClass.Time.GetLuceneDate(to);
            type = RangeType.Date;
        }
        public RangeCondition(string tablename, string fieldname, DateTime from, DateTime to,RangeType type)
            : base(tablename, fieldname)
        {
            if (type != RangeType.Date && type != RangeType.Time && type != RangeType.DateTime)
                throw new ArgumentException("Type is unvalid!", "type");
            if (type == RangeType.Date)
            {
                rangeFrom = SupportClass.Time.GetLuceneDate(from);
                rangeTo = SupportClass.Time.GetLuceneDate(to);
                this.type = RangeType.Date;
            }
            else if (type == RangeType.DateTime)
            {
                rangeFrom = SupportClass.Time.GetLuceneDateTime(from);
                rangeTo = SupportClass.Time.GetLuceneDateTime(to);
                this.type = RangeType.DateTime;
            }
            else
            {
                rangeFrom = SupportClass.Time.GetLuceneTime(from);
                rangeTo = SupportClass.Time.GetLuceneTime(to);
                this.type = RangeType.Time;
            }
        }
        public RangeCondition(string tablename, string fieldname, int from, int to)
            : base(tablename, fieldname)
        {
            rangeFrom = from.ToString();
            rangeTo = to.ToString();
            type = RangeType.Int;
        }
        public RangeCondition(string tablename, string fieldname, double from, double to)
        {
            rangeFrom = from.ToString();
            rangeTo = to.ToString();
            type = RangeType.Double;
        }
        public override string ToString()
        {
            return base.ToString()+":["+rangeFrom+" TO "+rangeTo+"]";
        }
        //public override bool Equals(object obj)
        //{
        //    if (!(obj is RangeCondition))
        //        return false;
        //    return table == ((RangeCondition)obj).table && 
        //          field == ((RangeCondition)obj).field && 
        //          rangeFrom == ((RangeCondition)obj).rangeFrom &&
        //          rangeTo == ((RangeCondition)obj).rangeTo &&
        //          type == ((RangeCondition)obj).type ;
        //}
    }
}
