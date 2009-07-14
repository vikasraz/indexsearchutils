using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    public class FileIndexSet
    {
        #region Public Const Flag
        /**/
        /// <summary>
        /// 基本文件路径的标志
        /// </summary>
        public const string PathFlag = "PATH";
        /**/
        /// <summary>
        /// 要索引目录的标志
        /// </summary>
        public const string DirectoryFlag = "DIRECTORY";
        #endregion
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
        #region Static Function
        public static void WriteToFile(ref System.IO.StreamWriter sw, FileIndexSet fileSet)
        {
            sw.WriteLine("#############################################");
            sw.WriteLine("#FileIndex");
            sw.WriteLine("#############################################");
            sw.WriteLine(Config.FileIndexSetFlag.ToLower());
            sw.WriteLine(Config.Prefix);
            sw.WriteLine("\t#文件索引存储路径");
            sw.WriteLine("\t" + FileIndexSet.PathFlag.ToLower() + "=" + fileSet.path);
            foreach (string dir in fileSet.baseDirs)
            {
                sw.WriteLine();
                sw.WriteLine("\t#索引目录");
                sw.WriteLine("\t" + FileIndexSet.DirectoryFlag.ToLower() + "=" + dir);
            }
            sw.WriteLine(Config.Suffix);
        }
        #endregion
    }
}
