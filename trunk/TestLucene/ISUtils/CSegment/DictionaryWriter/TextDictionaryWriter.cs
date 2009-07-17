using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lwh.ChineseSegment.DictionaryWriter
{
    internal class TextDictionaryWriter : IDictionaryWriter
    {
         //private string _dictionaryPath;

         public TextDictionaryWriter()
        {
            
        }

        #region IDictionaryWriter 成员

         public bool Write(string dictionaryPath, List<string> segmentList)
        {
            return WriteToFile(dictionaryPath, false, segmentList);
        }

         public bool Append(string dictionaryPath, List<string> segmentList)
        {
            return WriteToFile(dictionaryPath, true, segmentList);
        }

         private bool WriteToFile(string dictionaryPath, bool append, List<string> segmentList)
        {
            if (string.IsNullOrEmpty(dictionaryPath))
            {
                return false;
            }

            if (segmentList == null || segmentList.Count <= 0)
            {
                return false;
            }

            StreamWriter writer = new StreamWriter(dictionaryPath, append, System.Text.Encoding.UTF8);

            for (int i = 0; i < segmentList.Count; i++)
            {
                writer.WriteLine(segmentList[i]);
            }

            writer.Close();

            return true;
        }

        #endregion
    }
}
