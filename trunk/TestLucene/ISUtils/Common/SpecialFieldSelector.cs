using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Documents;

namespace ISUtils.Common
{
    public class SpecialFieldSelector:FieldSelector
    {
        protected string m_szFieldName="ItemID";
        public string FieldName
        {
            get { return m_szFieldName; }
            set { m_szFieldName = value; }
        }
        public SpecialFieldSelector(string szFieldName)
        {
            m_szFieldName = szFieldName;
        }
        public FieldSelectorResult Accept(string fieldName)
        {
            if (fieldName.Equals(m_szFieldName,StringComparison.OrdinalIgnoreCase))
            {
                return FieldSelectorResult.LOAD;
            }
            else
            {
                return FieldSelectorResult.NO_LOAD;
            }
        }
    }
}
