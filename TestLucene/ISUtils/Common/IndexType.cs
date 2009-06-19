using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    public enum IndexTypeEnum
    {
        Ordinary,
        Increment
    }
    public class IndexType
    {
        /**/
        /// <summary>
        /// 返回指定索引类型的索引字符串
        /// </summary>
        /// <param name="type">索引类型</param>
        /// <returns>返回类型</returns>
        public static string GetIndexTypeStr(IndexTypeEnum type)
        {
            switch (type)
            {
                case IndexTypeEnum.Increment:
                    return "Increment";
                case IndexTypeEnum.Ordinary:
                    return "Ordinary";
                default:
                    return "Ordinary";
            }
        }
        /**/
        /// <summary>
        /// 返回字符串的索引类型
        /// </summary>
        /// <param name="typestr">索引类型字符串</param>
        /// <returns>返回类型</returns>
        public static IndexTypeEnum GetIndexType(string typestr)
        {
            string upper = typestr.ToUpper();
            if (upper.CompareTo(GetIndexTypeStr(IndexTypeEnum.Ordinary).ToUpper()) == 0)
                return IndexTypeEnum.Ordinary;
            else if (upper.CompareTo(GetIndexTypeStr(IndexTypeEnum.Increment).ToUpper()) == 0)
                return IndexTypeEnum.Increment;
            else
                throw new ArgumentException("typestr must be index type!", "typestr");
        }
    }
}
