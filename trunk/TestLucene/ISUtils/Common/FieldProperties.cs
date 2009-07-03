using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    [Serializable]
    public sealed class FieldProperties
    {
        #region 属性
        private string field = "";
        private string caption = "";
        private float boost = 1.0f;
        private bool titleOrContent = false;//True for title ,false for content
        public string Field
        {
            get { return field; }
            set { field = value; }
        }
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }
        public float Boost
        {
            get { return boost; }
            set { boost = value; }
        }
        //True for title ,false for content
        public bool TitleOrContent
        {
            get { return titleOrContent; }
            set { titleOrContent = value; }
        }
        #endregion
        #region 构造函数
        public FieldProperties()
        {
        }
        public FieldProperties(string field, float boost)
            :this(field,field,boost,false)
        {
        }
        public FieldProperties(string field, float boost,bool isTitle)
            :this(field,field,boost,isTitle)
        {
        }
        public FieldProperties(string field, string caption, float boost, bool isTitle)
        {
            if (string.IsNullOrEmpty(field))
                throw new ArgumentNullException("field", "field not valid for FieldBoost.");
            this.field = field;
            this.caption = caption;
            this.boost = boost;
            this.titleOrContent = isTitle;
        }
        public FieldProperties(string field,string caption, string szBoost, string szIsTitle)
        {
            if (string.IsNullOrEmpty(field))
                throw new ArgumentNullException("field", "field not valid for FieldBoost.");
            this.field = field;
            this.caption = caption;
            this.boost = float.Parse(szBoost);
            this.titleOrContent = bool.Parse(szIsTitle);
        }
        public FieldProperties(string srcFB)
        {
            if (string.IsNullOrEmpty(srcFB))
                throw new ArgumentNullException("srcFB", "srcFB not valid for FieldBoost.");
            //srcFB:field(caption,boost) or field(caption) or field or field(caption,boost,true or false)
            string[] strArray = SupportClass.String.Split(srcFB, "(,) ");
            if (strArray.Length <1 || strArray.Length>4)
                throw new ArgumentException("srcFB has a bad format.", "srcFB");
            field = strArray[0];
            if (strArray.Length > 1)
                caption = strArray[1];
            if (strArray.Length > 2)
                boost = float.Parse(strArray[2]);
            if (strArray.Length > 3)
                titleOrContent = bool.Parse(strArray[3]);
        }
        #endregion
        #region 重写
        public override string ToString()
        {
            return field+"("+caption+","+boost.ToString()+","+titleOrContent.ToString()+")";
        }
        #endregion
        #region 全局方法
        public static implicit operator FieldProperties(string szFB)
        {
            return new FieldProperties(szFB);
        }
        public static FieldProperties[] ToArray(params string[] szFbs)
        {
            List<FieldProperties> fbList = new List<FieldProperties>();
            foreach(string szFb in szFbs)
                fbList.Add(new FieldProperties(szFb));
            return fbList.ToArray();
        }
        #endregion
    }
}
