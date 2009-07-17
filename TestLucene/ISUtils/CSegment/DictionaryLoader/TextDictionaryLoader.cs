using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lwh.ChineseSegment.Utility;

namespace Lwh.ChineseSegment.DictionaryLoader
{
    /// <summary>
    /// 文本词库加载器,存于文本文件中。
    /// </summary>
    public class TextDictionaryLoader : IDictionaryLoader
    {
        public TextDictionaryLoader()
        {
        }

        #region IDictionaryLoader 成员

        /// <summary>
        /// 从文本文件中读取分词，存于分词列表中；文本中一行存储一个词。
        /// </summary>
        /// <param name="dictionaryPath"></param>
        /// <returns></returns>
        public List<string> Load(string dictionaryPath)
        {
            if (!Validator.IsValidFile(dictionaryPath))
            {
                return new List<string>();
            }

            StreamReader reader = new StreamReader(dictionaryPath, System.Text.Encoding.UTF8);

            List<string> segmentList = new List<string>();
            string word = reader.ReadLine();
            while (word != null)
            {
                if (word.Trim()=="")
                {
                    word = reader.ReadLine();
                    continue;
                }
       
                segmentList.Add(word);
                word = reader.ReadLine();
            }
            try
            {
                reader.Close();
                return segmentList;
            }
            catch
            {
                return segmentList;
            }

        }

        #endregion
    }
}
