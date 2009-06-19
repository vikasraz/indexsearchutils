using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    [Serializable]
    public class Message
    {
        private string result="";
        private bool success=false;
        private List<string> infoList=new List<string>();
        private bool exception = false;
        public string Result
        {
            get { return result; }
            set { result = value; }
        }
        public bool Success
        {
            get { return success; }
            set { success = value; }
        }
        public List<string> Infos
        {
            get { return infoList; }
        }
        public bool ExceptionOccur
        {
            get { return exception; }
            set { exception = value; }
        }
        public void AddInfo(string info)
        {
            if (infoList == null)
                infoList = new List<string>();
            infoList.Add(info);
        }
        public override string ToString()
        {
            string ret = success.ToString() + "\t" + result;
            foreach (string s in infoList)
            {
                ret += "\t" + s;
            }
            return ret;
        }
    }
}
