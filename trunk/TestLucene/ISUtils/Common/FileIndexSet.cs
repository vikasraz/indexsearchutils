using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    /*
     *  <FileIndex>
     *  	<Directories>
     *  		<Directory >
     *  			<Path>W:\津国土房东丽</Path>
     *  			<VRoot>files</VRoot>
     *  		</Directory>
     *  	</Directories>
     *  </FileIndex>
     * */
    [Serializable]
    public class FileIndexSet
    {
        [Serializable]
        public sealed class Path
        {
            #region 属性
            /**/
            /// <summary>
            /// 文件真实路径
            /// </summary>
            private string realPath = "";
            public string RealPath
            {
                get { return realPath; }
                set { realPath = value; }
            }
            /**/
            /// <summary>
            /// 文件虚拟路径
            /// </summary>
            private string virtualPath = "";
            public string VirtualPath
            {
                get { return virtualPath; }
                set { virtualPath = value; }
            }
            #endregion
        }
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
        /**/
        /// <summary>
        /// 索引文件路径列表
        /// </summary>
        private List<string> baseDirs = new List<string>();
        public List<string> BaseDirs
        {
            get
            {
                if (baseDirs == null)
                    baseDirs = new List<string>();
                return baseDirs;
            }
        }
        /**/
        /// <summary>
        /// 虚拟文件路径列表
        /// </summary>
        private List<string> virtualDirs = new List<string>();
        public List<string> VirtualDirs
        {
            get
            {
                if (virtualDirs == null)
                    virtualDirs = new List<string>();
                return virtualDirs;
            }
        }
        /**/
        /// <summary>
        /// 文件路径列表
        /// </summary>
        private List<Path> dirs = new List<Path>();
        public List<Path> Dirs
        {
            get 
            {
                if (dirs == null)
                    dirs = new List<Path>();
                return dirs;
            }
            set 
            {
                if (dirs == null)
                    dirs = new List<Path>();
                dirs.Clear();
                dirs.AddRange(value);
                if (virtualDirs == null)
                    virtualDirs = new List<string>();
                virtualDirs.Clear();
                if (baseDirs == null)
                    baseDirs = new List<string>();
                baseDirs.Clear();
                foreach (Path path in dirs)
                {
                    baseDirs.Add(path.RealPath);
                    virtualDirs.Add(path.VirtualPath);
                }
            }
        }
        #endregion
    }
}
