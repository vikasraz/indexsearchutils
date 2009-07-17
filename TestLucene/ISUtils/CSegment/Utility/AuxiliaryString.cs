using System;
using System.Collections.Generic;
using System.Text;

namespace Lwh.ChineseSegment.Utility
{
    public class AuxiliaryString
    {
        #region Static Property
        private static char separator = '/';
        public static char Separator
        {
            get { return separator; }
            set { separator = value; }
        }
        #endregion
        #region Private Vars
        private StringBuilder buffer = new StringBuilder();
        private List<int> startList = new List<int>();
        #endregion
        #region Property
        public List<int> StartList
        {
            get { return startList; }
        }
        public StringBuilder Builder
        {
            get { return buffer; }
        }
        public List<string> Tokens
        {
            get
            {
                List<string> tokens = new List<string>();
                tokens.AddRange(buffer.ToString().Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries));
                return tokens;
            }
        }
        public bool NeedAddPos
        {
            get 
            {
                if (buffer.Length <= 0)
                    return false;
                return buffer[buffer.Length-1]==separator;
            }
        }
        #endregion
        #region Override
        public override string ToString()
        {
            return buffer.ToString();
        }
        #endregion
        #region Function
        public void Append(string str, int pos)
        {
            if (string.IsNullOrEmpty(str))
                return;
            if (buffer.Length == 0)
            {
                buffer.Append(str.Trim());
                startList.Add(0);
            }
            else
            {
                if (buffer[buffer.Length - 1] == separator)
                {
                    buffer.Append(str.Trim());
                    startList.Add(pos);
                }
                else
                {
                    buffer.Append(str.Trim());
                }
            }
        }
        public void Append(string str)
        {
            if (string.IsNullOrEmpty(str))
                return;
            if (buffer.Length > 0 && buffer[buffer.Length - 1] == separator && !str.StartsWith("/"))
                throw new ArgumentException("Append Error:" + str, "separator");
            if (buffer.Length > 0 && buffer[buffer.Length - 1] == separator && str.StartsWith("/"))
            {
                buffer.Append(str.Substring(1).Trim());
            }
            else
            {
                buffer.Append(str.Trim());
            }
        }
        #endregion
    }
}
