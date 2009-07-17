using System;
using System.Collections.Generic;
using System.Text;

namespace Lwh.ChineseSegment.DictionaryWriter
{
    public interface IDictionaryWriter
    {
        bool Write(string dictionaryPath, List<string> segmentList);
        bool Append(string dictionaryPath, List<string> segmentList);
    }
}
