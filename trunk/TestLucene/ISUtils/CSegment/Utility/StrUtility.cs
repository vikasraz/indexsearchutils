using System;
using System.Collections.Generic;
using System.Text;
using Lwh.ChineseSegment.DictionaryLoader;

namespace Lwh.ChineseSegment.Utility
{
    internal static class StrUtility
    {
        private static string spliters = " 　\t\r\n,.;'?/\\+-=@&*，。<>《》？、{}[]$￥：:\"“”";
        /// <summary>
        /// 根据指定的过滤字符过滤文本。
        /// </summary>
        /// <param name="text">待处理文本</param>
        /// <param name="filters">过滤字符</param>
        /// <returns></returns>
        public static string Filter(string text, List<string> filters)
        {
            return Filter(text, filters, false);
        }
        /// <summary>
        /// 根据指定的过滤字符过滤文本。
        /// </summary>
        /// <param name="text">待处理文本</param>
        /// <param name="filters">过滤字符</param>
        /// <param name="filterSpaces">是否过滤空格</param>
        /// <returns></returns>
        public static string Filter(string text, List<string> filters, bool filterSpaces)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            if (filters == null || filters.Count <= 0)
            {
                return text;
            }

            StringBuilder tempBuilder = new StringBuilder(text);
            if (filterSpaces)
            {
                tempBuilder.Replace(" ", "");
            }

            foreach (string filter in filters)
            {
                tempBuilder.Replace(filter, "\t");
            }

            return tempBuilder.ToString();
        }
        /// <summary>
        /// 根据指定的过滤字符字典文件过滤文本。
        /// </summary>
        /// <param name="text">待处理文本</param>
        /// <param name="filterFilePath"></param>
        /// <param name="loader"></param>
        /// <param name="filterSpaces">是否过滤空格</param>
        /// <returns></returns>
        public static string Filter(string text, string filterFilePath, IDictionaryLoader loader, bool filterSpaces)
        {
            if (string.IsNullOrEmpty(filterFilePath))
            {
                return "";
            }
            if (loader == null)
            {
                return text;
            }

            List<string> filters = loader.Load(filterFilePath);
            return Filter(text, filters, filterSpaces);
        }
        /// <summary>
        /// 根据指定的过滤字符字典文件过滤文本。
        /// </summary>
        /// <param name="text">待处理文本</param>
        /// <param name="filterFilePath"></param>
        /// <param name="loader"></param>
        /// <returns></returns>
        public static string Filter(string text, string filterFilePath, IDictionaryLoader loader)
        {
            return Filter(text, filterFilePath, loader, false);
        }
        public static string[] SplitSentences(string text, List<string> filterList, bool filterSpaces)
        {
            return SplitSentences(Filter(text, filterList, filterSpaces));
        }
        /// <summary>
        /// 按标点符号切分成句子。
        /// </summary>
        /// <param name="text">待处理文本</param>
        /// <returns></returns>
        public static string[] SplitSentences(string text)
        {
            return text.Split(spliters.ToCharArray(),StringSplitOptions.RemoveEmptyEntries); 
        }
        /// <summary>
        /// 根据指定的过滤字符过滤文本。
        /// </summary>
        /// <param name="text">待处理文本</param>
        /// <param name="filters">过滤字符</param>
        /// <returns></returns>
        public static string FilterEx(string text, List<string> filters)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            if (filters == null || filters.Count <= 0)
            {
                return text;
            }
            StringBuilder tempBuilder = new StringBuilder(text);
            foreach (string filter in filters)
            {
                tempBuilder.Replace(filter, Space(filter.Length));
            }
            char[] splits = spliters.ToCharArray();
            foreach (char split in splits)
            {
                tempBuilder.Replace(split, ' ');
            }
            return tempBuilder.ToString();
        }
        public static string Space(int len)
        {
            if (len <= 0) return "";
            StringBuilder spacer = new StringBuilder();
            spacer.Append(' ', len);
            return spacer.ToString();
        }
    }
}
