using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    [Serializable]
    class CommonSet
    {
        #region 属性
        /**/
        /// <summary>
        /// 存储数据库类型
        /// </summary>
        private DBTypeEnum dbtype = DBTypeEnum.SQL_Server;
        /**/
        /// <summary>
        /// 设定或返回数据库类型
        /// </summary>
        public DBTypeEnum DBType
        {
            get { return dbtype; }
            set { dbtype = value; }
        }
        /**/
        /// <summary>
        /// 存储数据库路径
        /// </summary>
        private string hostname = "";
        /**/
        /// <summary>
        /// 设定或返回数据库路径
        /// </summary>
        public string HostName
        {
            get { return hostname; }
            set { hostname = value; }
        }
        /**/
        /// <summary>
        /// 存储数据库名称
        /// </summary>
        private string database = "";
        /**/
        /// <summary>
        /// 设定或返回数据库名称
        /// </summary>
        public string DataBase
        {
            get { return database; }
            set { database = value; }
        }
        /**/
        /// <summary>
        /// 存储用户名
        /// </summary>
        private string username = "";
        /**/
        /// <summary>
        /// 设定或返回用户名
        /// </summary>
        public string UserName
        {
            get { return username; }
            set { username = value; }
        }
        /**/
        /// <summary>
        /// 存储密码
        /// </summary>
        private string password = "";
        /**/
        /// <summary>
        /// 设定或返回密码
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        /**/
        /// <summary>
        /// 存储密码
        /// </summary>
        private string indexPath = "";
        /**/
        /// <summary>
        /// 设定或返回密码
        /// </summary>
        public string IndexPath
        {
            get { return indexPath; }
            set { indexPath = value; }
        }
        /**/
        /// <summary>
        /// 存储密码
        /// </summary>
        private string config = "";
        /**/
        /// <summary>
        /// 设定或返回密码
        /// </summary>
        public string Config
        {
            get { return config; }
            set { config = value; }
        }
        #endregion
    }
}
