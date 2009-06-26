using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    [Serializable]
    public sealed class FormatedResult
    {
        [Serializable]
        public sealed class Element
        {
            private string key="";
            private string value="";
            public string Key
            {
                get { return key; }
                set { key = value; }
            }
            public string Value
            {
                get { return value; }
                set { this.value = value; }
            }
            public Element() { }
            public Element(string key, string value)
            {
                this.key = key;
                this.value = value;
            }
            public override string ToString()
            {
                return key +":\t"+value;
            }
        }
        [Serializable]
        public sealed class FormatedDoc
        {
            private List<Element> elemList = new List<Element>();
            public List<Element> ElemList
            {
                get { return elemList; }
                set { elemList = value; }
            }
            public void AddElement(string key, string value)
            {
                if (elemList == null)
                    elemList = new List<Element>();
                elemList.Add(new Element(key, value));
            }
            public void AddElement(Element elem)
            {
                if (elemList == null)
                    elemList = new List<Element>();
                elemList.Add(elem);
            }
        }
        private List<FormatedDoc> fdocList = new List<FormatedDoc>();
        public List<FormatedDoc> FormatedDocList
        {
            get { return fdocList; }
            set { fdocList = value; }
        }
        public void AddFormatedDoc(FormatedDoc fd)
        {
            if (fdocList == null)
                fdocList = new List<FormatedDoc>();
            fdocList.Add(fd);
        }
    }
}
