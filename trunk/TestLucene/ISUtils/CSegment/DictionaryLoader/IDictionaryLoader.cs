using System;
using System.Collections.Generic;
using System.Text;

namespace Lwh.ChineseSegment.DictionaryLoader
{
    /// <summary>
    /// 词库加载器,存于文件中。
    /// </summary>
    public interface IDictionaryLoader
    {
        List<string> Load(string dictionaryPath);
    }
}
