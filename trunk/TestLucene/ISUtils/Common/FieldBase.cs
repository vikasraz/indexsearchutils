using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    [Serializable]
    public class FieldBase
    {
        #region 属性
        protected string name = "";
        protected string caption = "";
        protected float boost = 1.0f;        
        protected bool isTitle = false;
        protected bool visible = false;
        protected int order=0;
        protected Dictionary<string, string> properties = new Dictionary<string, string>();
        public string Name
        {
            get { return name; }
            set { name = value; }
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
        public bool IsTitle
        {
            get { return isTitle; }
            set { isTitle = value; }
        }
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
        public int Order
        {
            get { return order; }
            set { order = value; }
        }
        public Dictionary<string, string> Properties
        {
            get
            {
                if (properties==null)
                    properties =new Dictionary<string,string>();
                return properties;
            }
            set
            {
                properties = value;
                if (properties == null)
                    properties = new Dictionary<string, string>();
            }
        }
        #endregion
        #region 构造函数
        public FieldBase()
        { 
        }
        public FieldBase(string name, string caption, float boost, bool isTitle, bool visible, int order, Dictionary<string, string> propertyDict)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "name not valid for field.");
            this.name = name;
            this.caption = caption;
            this.isTitle = isTitle;
            this.visible = visible;
            this.boost = boost;
            this.order = order;
            this.Properties = propertyDict;
        }
        public FieldBase(string name, string caption, float boost, bool isTitle, bool visible, int order)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "name not valid for field.");
            this.name = name;
            this.caption = caption;
            this.isTitle = isTitle;
            this.visible = visible;
            this.boost = boost;
            this.order = order;
            if (properties == null)
                properties = new Dictionary<string, string>();
        }
        public FieldBase(string name, string caption, string szBoost, string szIsTitle)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "name not valid for field.");
            this.name = name;
            this.caption = caption;
            this.boost = float.Parse(szBoost);
            this.isTitle = bool.Parse(szIsTitle);
            this.visible = true;
            this.order = 0;
        }
        public FieldBase(string name, string caption, float boost, bool isTitle)
            : this(name, caption, boost, isTitle, true, 0)
        { 
        }
        public FieldBase(string name, float boost, bool isTitle)
            : this(name, name, boost, isTitle, true, 0)
        {
        }
        public FieldBase(string name, string caption, float boost)
            : this(name, caption, boost, false, true, 0)
        {
        }
        public FieldBase(string name, float boost)
            : this(name, name, boost, false, true, 0)
        {
        }
        public FieldBase(string name, string caption)
            : this(name, caption, 1.0f, false, false, 0)
        {
        }
        public FieldBase(string name)
            : this(name, name, 1.0f, false, false, 0)
        {
        }

        #endregion
    }
}
