using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    /*
     *  <Searchd>
     *    <Address>192.168.1.102</Address>
     *    <Port>3312</Port>
     *    <MaxChildren>30</MaxChildren>
     *    <MaxMatches>10000</MaxMatches>
     *    <MaxTrans>100</MaxTrans>
     *    <MinScore>0.0</MinScore>
     *  </Searchd>
     * /
    /**/
    /// <summary>
    /// Searchd设置
    /// </summary>
    [Serializable]
    public class SearchSet
    {
        #region Flags
        /**/
        /// <summary>
        /// IP地址的标志
        /// </summary>
        public const string AddressFlag = "ADDRESS";
        /**/
        /// <summary>
        /// 端口的标志
        /// </summary>
        public const string PortFlag = "PORT";
        /**/
        /// <summary>
        /// 结果信息的标志
        /// </summary>
        public const string LogPathFlag = "LOG";
        /**/
        /// <summary>
        /// 查询信息路径的标志
        /// </summary>
        public const string QueryLogPathFlag = "QUERY_LOG";
        /**/
        /// <summary>
        /// 超时时间的标志
        /// </summary>
        public const string TimeOutFlag = "READ_TIMEOUT";
        /**/
        /// <summary>
        /// 最大匹配记录数的标志
        /// </summary>
        public const string MaxChildrenFlag = "MAX_CHILDREN";
        /**/
        /// <summary>
        /// 最大匹配记录数的标志
        /// </summary>
        public const string MaxMatchesFlag = "MAX_MATCHES";
        public const string MaxTransFlag = "MAX_TRANS";
        #endregion
        #region Property
        /**/
        /// <summary>
        /// 存储IP地址
        /// </summary>
        private string address = "192.168.0.1";
        /**/
        /// <summary>
        /// 设定或返回IP地址
        /// </summary>
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        /**/
        /// <summary>
        /// 存储端口
        /// </summary>
        private int port = 3312;
        /**/
        /// <summary>
        /// 设定或返回端口
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }
        /**/
        /// <summary>
        /// 存储最大匹配记录数
        /// </summary>
        private int maxchildren = 30;
        /**/
        /// <summary>
        /// 设定或返回最大查询客户数
        /// </summary>
        public int MaxChildren
        {
            get { return maxchildren; }
            set { maxchildren = value; }
        }
        /**/
        /// <summary>
        /// 存储最大匹配记录数
        /// </summary>
        private int maxmatches = 10000;
        /**/
        /// <summary>
        /// 设定或返回最大匹配记录数
        /// </summary>
        public int MaxMatches
        {
            get { return maxmatches; }
            set { maxmatches = value; }
        }
        private int maxtrans = 100;
        public int MaxTrans
        {
            get { return maxtrans; }
            set { maxtrans = value; }
        }
        private float minscore = 0.0f;
        public float MinScore
        {
            get { return minscore; }
            set { minscore = value; }
        }
        #endregion
        #region Override
        /**/
        /// <summary>
        /// 获取SearchSet内容
        /// </summary>
        /// <returns>返回类型</returns>
        //public const string MaxTrans
        public override string ToString()
        {
            string ret = string.Format("Searchd:Address({0}),Port({1}),Max Children({2}),Max Matches({3}),Min Score({4})",
                                     address, port,maxchildren,maxmatches,minscore);
            return base.ToString() + "\t" + ret;
        }
        #endregion
    }
}
