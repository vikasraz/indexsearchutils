using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    public sealed class FileContent
    {
        #region Property
        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string path = "";
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                name = GetName(path);
            }
        }
        private string content = "";
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        #endregion
        #region Constructor
        public FileContent()
        { 
        }
        public FileContent(string name, string path, string content)
        {
            this.name = name;
            this.path = path;
            this.content = content;
        }
        public FileContent(string path, string content)
        {
            Path = path;
            this.content = content;
        }
        public FileContent(string path)
        {
            Path = path;
        }
        #endregion
        #region Static Public Function
        public static string GetName(string path)
        {
            int pos1,pos2,pos3,pos;
            pos1=path.LastIndexOf("\\");
            pos2=path.LastIndexOf("/");
            pos3=path.LastIndexOf(":");
            pos=SupportClass.Numerical.Max(pos1,pos2,pos3);
            if (pos >= 0)
                return path.Substring(pos + 1);
            else
                return path;
        }
        #endregion
    }
}
