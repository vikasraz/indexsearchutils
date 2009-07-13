using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    public class FileIndexSet
    {
        #region Property
        private string path = "";
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        private List<string> baseDirs = new List<string>();
        public List<string> BaseDirs
        {
            get
            {
                if (baseDirs == null)
                    baseDirs = new List<string>();
                return baseDirs;
            }
            set 
            {
                baseDirs = value;
                if (baseDirs == null)
                    baseDirs = new List<string>();
            }
        }
        #endregion
        #region Constructor
        public FileIndexSet()
        {
            if(baseDirs==null)
                baseDirs=new List<string>();
        }
        public FileIndexSet(string path, List<string> dirs)
        {
            Path = path;
            BaseDirs = dirs;
        }
        public FileIndexSet(string path, params string[] dirs)
        {
            this.path = path;
            if (baseDirs == null)
                baseDirs = new List<string>();
            baseDirs.Clear();
            baseDirs.AddRange(dirs);
        }
        #endregion
        #region Function
        public void AddDirectory(string dir)
        {
            if (baseDirs == null)
                baseDirs = new List<string>();
            baseDirs.Add(dir);
        }
        public void AddDirectory(params string[] dirs)
        {
            if (baseDirs == null)
                baseDirs = new List<string>();
            baseDirs.AddRange(dirs);
        }
        public void AddDirectory(List<string> dirs)
        {
            if (baseDirs == null)
                baseDirs = new List<string>();
            baseDirs.AddRange(dirs);
        }
        #endregion
    }
}
