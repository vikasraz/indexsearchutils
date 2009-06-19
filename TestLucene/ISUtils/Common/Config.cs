using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    public class Config
    {
        /**/
        /// <summary>
        /// Config文件中Config项目的开始标志
        /// </summary>
        public const string Prefix = "{";
        /**/
        /// <summary>
        /// Config文件中Config项目的结束标志
        /// </summary>
        public const string Suffix = "}";
        /**/
        /// <summary>
        /// Config文件中Config条目的忽略标志
        /// </summary>
        public const string Ignore = "#";
        /**/
        /// <summary>
        /// Config文件中Config条目的分割字符串
        /// </summary>
        public const string Devider = "=\t\n,";
        /**/
        /// <summary>
        /// Config文件中SOURCE项目的标志
        /// </summary>
        public const string SourceFlag = "SOURCE";
        /**/
        /// <summary>
        /// Config文件中INDEX项目的标志
        /// </summary>
        public const string IndexFlag = "INDEX";
        /**/
        /// <summary>
        /// Config文件中INDEXER项目的标志
        /// </summary>
        public const string IndexerFlag = "INDEXER";
        /**/
        /// <summary>
        /// Config文件中Searchd项目的标志
        /// </summary>
        public const string SearchdFlag = "SEARCHD";
        /**/
        /// <summary>
        /// Config文件中Dictionary项目的标志
        /// </summary>
        public const string DictionaryFlag = "DICTIONARY";
    }
}
