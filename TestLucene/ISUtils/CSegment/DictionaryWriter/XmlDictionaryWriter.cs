using System;
using System.Collections.Generic;
using System.Text;
using ISUtils.CSegment.Utility;

namespace ISUtils.CSegment.DictionaryWriter
{
    internal class XmlDictionaryWriter : IDictionaryWriter
    {
        //private string _dictionaryPath;

        public XmlDictionaryWriter()
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

            return Serialization.SerializeXml(dictionaryPath, segmentList);
        }

        public bool Append(string dictionaryPath, List<string> segmentList)
        {
            if (!IsValid(dictionaryPath, segmentList))
            {
                return false;
            }

            List<string> srcList = Serialization.DeserializeXml(dictionaryPath, typeof(List<string>)) as List<string>;
            if (srcList != null)
            {
                srcList.AddRange(segmentList);
                return Serialization.SerializeXml(dictionaryPath, srcList);
            }

            return Serialization.SerializeXml(dictionaryPath, segmentList);
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
