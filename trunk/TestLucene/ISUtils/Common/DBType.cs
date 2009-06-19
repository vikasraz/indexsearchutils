using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    public enum DBTypeEnum
    {
        SQL_Server,
        OLE_DB,
        ODBC,
        Oracle
    }
    public class DbType
    {
        public static string[] GetAllDbTypeStr()
        {
            return new string[] { "SqlServer", "Oracle", "OleDB", "ODBC" };
        }
        /**/
        /// <summary>
        /// 返回指定数据库类型的数据库字符串
        /// </summary>
        /// <param name="dbtype">数据库类型</param>
        /// <returns>返回类型</returns>
        public static string GetDbTypeStr(DBTypeEnum dbtype)
        {
            switch (dbtype)
            {
                case DBTypeEnum.ODBC:
                    return "ODBC";
                case DBTypeEnum.OLE_DB:
                    return "OleDB";
                case DBTypeEnum.Oracle:
                    return "Oracle";
                case DBTypeEnum.SQL_Server:
                    return "SqlServer";
                default:
                    return "SqlServer";
            }
        }
        /**/
        /// <summary>
        /// 返回字符串的数据库类型
        /// </summary>
        /// <param name="typestr">数据库类型字符串</param>
        /// <returns>返回类型</returns>
        public static DBTypeEnum GetDbType(string typestr)
        {
            string upper = typestr.ToUpper();
            if (upper.CompareTo(GetDbTypeStr(DBTypeEnum.ODBC).ToUpper()) == 0)
                return DBTypeEnum.ODBC;
            else if (upper.CompareTo(GetDbTypeStr(DBTypeEnum.OLE_DB).ToUpper()) == 0)
                return DBTypeEnum.OLE_DB;
            else if (upper.CompareTo(GetDbTypeStr(DBTypeEnum.Oracle).ToUpper()) == 0)
                return DBTypeEnum.Oracle;
            else if (upper.CompareTo(GetDbTypeStr(DBTypeEnum.SQL_Server).ToUpper()) == 0)
                return DBTypeEnum.SQL_Server;
            else
                throw new ArgumentException("typestr must be database type!", "typestr");
        }
    }
}
