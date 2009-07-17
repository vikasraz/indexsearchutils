using System;
using System.Collections.Generic;
using System.Text;
using ISUtils.CSegment.Utility;

namespace ISUtils.CSegment.DictionaryLoader
{
    /// <summary>
    /// bin词库加载器,存于bin文件中。
    /// </summary>
    public class BinDictionaryLoader : IDictionaryLoader
    {
        public BinDictionaryLoader()
        {
        }

        #region IDictionaryLoader 成员


        public List<string> Load(string dictionaryPath)
        {
            if (!Validator.IsValidFile(dictionaryPath))
            {
                return new List<string>();
            }

            return Serialization.DeserializeBin(dictionaryPath, typeof(List<string>)) as List<string>;
        }

        #endregion
    }
}
