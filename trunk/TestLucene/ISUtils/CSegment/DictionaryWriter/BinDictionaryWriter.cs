using System;
using System.Collections.Generic;
using System.Text;
using Lwh.ChineseSegment.Utility;

namespace Lwh.ChineseSegment.DictionaryWriter
{
    internal class BinDictionaryWriter : IDictionaryWriter
    {
        //private string _dictionaryPath;

        public BinDictionaryWriter()
        {
            //_dictionaryPath = dictionaryPath;
        }

        #region IDictionaryWriter 成员

        public bool Write(string dictionaryPath, List<string> segmentList)
        {
            if (!IsValid(dictionaryPath, segmentList))
            {
                return false;
            }
            
            return Serialization.SerializeBin(dictionaryPath, segmentList);
        }

        public bool Append(string dictionaryPath, List<string> segmentList)
        {
            if (!IsValid(dictionaryPath, segmentList))
            {
                return false;
            }

            List<string> srcList = Serialization.DeserializeBin(dictionaryPath, typeof(List<string>)) as List<string>;
            if (srcList != null)
            {
                srcList.AddRange(segmentList);
                return Serialization.SerializeBin(dictionaryPath, srcList);
            }

            return Serialization.SerializeBin(dictionaryPath, segmentList);
        }

        private bool IsValid(string dictionaryPath, List<string> segmentList)
        {
            if (string.IsNullOrEmpty(dictionaryPath))
            {
                return false;
            }

            if (segmentList == null || segmentList.Count <= 0)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
