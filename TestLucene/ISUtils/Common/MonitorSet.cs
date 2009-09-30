using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    /*
     *   <DbMointor>
     *  	<DbSuffix>_SSB</DbSuffix>
     *  	<TvSuffix>_log</TvSuffix>
     *  </DbMonitor>
     * */
    [Serializable]
    public class MonitorSet
    {
        #region 常数
        public const string DBSUFFIX = "_SSB";
        public const string TVSUFFIX = "_log";
        #endregion
        #region 属性
        /**/
        /// <summary>
        /// 数据库后缀
        /// </summary>
        private string dbSuffix = DBSUFFIX;
        public string DbSuffix
        {
            get { return dbSuffix; }
            set { dbSuffix = value; }
        }
        /**/
        /// <summary>
        /// 库表和视图后缀
        /// </summary>
        private string tvSuffix = TVSUFFIX;
        public string TvSuffix
        {
            get { return tvSuffix; }
            set { tvSuffix = value; }
        }
        #endregion
    }
}
