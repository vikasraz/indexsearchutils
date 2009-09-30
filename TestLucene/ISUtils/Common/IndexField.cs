using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    [Serializable]
    public sealed class IndexField:FieldBase
    {
        #region 属性
        #endregion
        #region 构造函数
        public IndexField()
        {
        }
        public IndexField(string field, float boost)
            :this(field,field,boost,false)
        {
        }
        public IndexField(string field, float boost,bool isTitle)
            :this(field,field,boost,isTitle)
        {
        }
        public IndexField(string field, string caption, float boost, bool isTitle)
            :base(field,caption,boost,isTitle,true,0)
        {
        }
        public IndexField(string field,string caption, string szBoost, string szIsTitle)
            :base(field,caption,szBoost,szIsTitle)
        {
        }
        public IndexField(string srcFB)
        {
            if (string.IsNullOrEmpty(srcFB))
                throw new ArgumentNullException("srcFB", "srcFB not valid for FieldBoost.");
            //srcFB:field(caption,boost) or field(caption) or field or field(caption,boost,true or false)
            if (srcFB.StartsWith(","))
                srcFB = srcFB.Substring(1);
            srcFB =srcFB.Replace('(',',');
            srcFB=srcFB.Replace(')',',');
            string[] strArray = srcFB.Split(new char[]{','});
            if (strArray.Length <1 || strArray.Length>4)
                throw new ArgumentException("srcFB has a bad format.", "srcFB");
            name = strArray[0];
            if (strArray.Length > 1)
                if (string.IsNullOrEmpty(strArray[1]))
                    caption = "";
                else
                    caption = strArray[1];
            if (strArray.Length > 2)
                if (string.IsNullOrEmpty(strArray[2]))
                    boost = 1.0f;
                else
                    boost = float.Parse(strArray[2]);
            if (strArray.Length > 3)
                if (string.IsNullOrEmpty(strArray[3]))
                    isTitle = false;
                else
                    isTitle = bool.Parse(strArray[3]);
        }
        #endregion
        #region 重写
        public override string ToString()
        {
            return name+"("+caption+","+boost.ToString()+","+isTitle.ToString()+")";
        }
        #endregion
        #region 全局方法
        public static implicit operator IndexField(string szFB)
        {
            return new IndexField(szFB);
        }
        public static IndexField[] ToArray(params string[] szFbs)
        {
            List<IndexField> fbList = new List<IndexField>();
            foreach(string szFb in szFbs)
                fbList.Add(new IndexField(szFb));
            return fbList.ToArray();
        }
        public static IndexField[] ToArray(string szFbs)
        {
            List<IndexField> fpList = new List<IndexField>();
            if (szFbs.IndexOf(')') > 0)
            {
                string[] split = SupportClass.String.Split(szFbs, ")");
                foreach (string token in split)
                    fpList.Add(new IndexField(token));
            }
            else
            {
                string[] split = SupportClass.String.Split(szFbs, ",");
                foreach (string token in split)
                    fpList.Add(new IndexField(token));
            }
            return fpList.ToArray();
        }
        public static List<IndexField> ToList(params string[] szFbs)
        {
            List<IndexField> fbList = new List<IndexField>();
            foreach (string szFb in szFbs)
                fbList.Add(new IndexField(szFb));
            return fbList;
        }
        public static List<IndexField> ToList(string szFbs)
        {
            List<IndexField> fpList = new List<IndexField>();
            if (szFbs.IndexOf(')') > 0)
            {
                string[] split = SupportClass.String.Split(szFbs, ")");
                foreach (string token in split)
                    fpList.Add(new IndexField(token));
            }
            else
            {
                string[] split = SupportClass.String.Split(szFbs, ",");
                foreach (string token in split)
                    fpList.Add(new IndexField(token));
            }
            return fpList;
        }
        #endregion
    }
}
