using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    /*
  <DbMointor>
  	<DbSuffix>_SSB</DbSuffix>
  	<TvSuffix>_log</TvSuffix>
  </DbMonitor>
    */
    [Serializable]
    public class DbMonitorSet
    {
        #region 属性
        private string dbSuffix = "_SSB";
        public string DbSuffix
        {
            get { return dbSuffix; }
            set { dbSuffix = value; }
        }
        private string monitorTable = "log";
        public string MonitorTable
        {
            get { return monitorTable; }
            set { monitorTable = value; }
        }
        #endregion
    }
}
