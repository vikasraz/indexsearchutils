using System;
using System.Collections.Generic;
using System.Text;
using Lwh.ChineseSegment.Utility;

namespace Lwh.ChineseSegment.DictionaryLoader
{
    /// <summary>
    /// Xml词库加载器,存于Xml文件中。
    /// </summary>
    public class XmlDictionaryLoader : IDictionaryLoader
    {
        public XmlDictionaryLoader()
        {
        }

        #region IDictionaryLoader 成员


        public List<string> Load(string dictionaryPath)
        {
            if (!Validator.IsValidFile(dictionaryPath))
            {
                return new List<string>();
            }

            return Serialization.DeserializeXml(dictionaryPath, typeof(List<string>)) as List<string>;
        }

        #endregion
    }
}
