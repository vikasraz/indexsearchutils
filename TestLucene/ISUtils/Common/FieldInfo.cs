using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    public sealed class FieldInfo
    {
        private bool primaryKey = false;
        public bool PrimaryKey
        {
            get { return primaryKey; }
            set { primaryKey = value; }
        }
        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private Type type="".GetType();
        public Type Type
        {
            get { return type; }
            set { type = value; }
        }
        public FieldInfo()
        { 
        }
        public FieldInfo(string name, Type type)
        {
            this.name = name;
            this.type = type;
        }
        public FieldInfo(string name, Type type,bool primaryKey)
        {
            this.name = name;
            this.type = type;
            this.primaryKey = primaryKey;
        }
    }
}
