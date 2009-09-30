using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    /*
     *   <Source Name="IndexView_Monitoring_HCOV"  Caption="权属争议调查处理">
     *    <PrimaryKey>ItemID</PrimaryKey>
     *    <Fields>
     *      <Field Name="AY" Caption="案由" Boost="3" IsTitle="False" Order="0" />
     *    </Fields>
     *  </Source>
     * */
    [Serializable]
    public class Source
    {
        #region 标志设置
        /**/
        /// <summary>
        /// 数据库类型的标志
        /// </summary>
        public const string DBTypeFlag = "DBTYPE";
        /**/
        /// <summary>
        /// 数据库路径的标志
        /// </summary>
        public const string HostNameFlag = "HOSTNAME";
        /**/
        /// <summary>
        /// 数据库名的标志
        /// </summary>
        public const string DataBaseFlag = "DATABASE";
        /**/
        /// <summary>
        /// 查询语句的标志
        /// </summary>
        public const string QueryFlag = "QUERY";
        /**/
        /// <summary>
        /// 索引字段的标志
        /// </summary>
        public const string FieldsFlag = "FIELDS";
        /**/
        /// <summary>
        /// 索引权重的标志
        /// </summary>
        public const string BoostsFlag = "BOOSTS";
        /**/
        /// <summary>
        /// 用户名的标志
        /// </summary>
        public const string UserNameFlag = "USERNAME";
        /**/
        /// <summary>
        /// 密码的标志
        /// </summary>
        public const string PasswordFlag = "PASSWORD";
        /**/
        /// <summary>
        ///主键的标志
        /// </summary>
        public const string PrimaryKeyFlag = "PRIMARYKEY";
        #endregion
        #region 属性
        /**/
        /// <summary>
        /// 存储Source的名称
        /// </summary>
        private string name = "";
        /**/
        /// <summary>
        /// Gets and Sets SourceName
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /**/
        /// <summary>
        /// 存储Caption的名称
        /// </summary>
        private string caption = "";
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }
        /**/
        /// <summary>
        /// 存储索引字段名称
        /// </summary>
        private List<IndexField> fields=new List<IndexField>();
        /**/
        /// <summary>
        /// 设定或返回索引字段名称
        /// </summary>
        public List<IndexField> Fields
        {
            get 
            {
                if (fields == null)
                    fields = new List<IndexField>();
                return fields; 
            }
            set 
            { 
                fields = value;
                if (fields != null)
                { 
                    if (fieldDict ==null)
                        fieldDict = new Dictionary<string, IndexField>();
                    if (boostDict == null)
                        boostDict = new Dictionary<string, float>();
                    fieldDict.Clear();
                    boostDict.Clear();
                    foreach (IndexField fb in fields)
                    {
                        if(!fieldDict.ContainsKey(fb.Name))
                            fieldDict.Add(fb.Name, fb);
                        if(!boostDict.ContainsKey(fb.Name))
                            boostDict.Add(fb.Name, fb.Boost);
                    }
                }
            }
        }
        private Dictionary<string, IndexField> fieldDict = new Dictionary<string, IndexField>();
        public List<string> StringFields
        {
            get
            {
                List<string> szFields = new List<string>();
                szFields.AddRange(fieldDict.Keys);
                return szFields;
            }
        }
        public Dictionary<string, IndexField> FieldDict
        {
            get
            {
                return fieldDict;
            }
        }
        private Dictionary<string, float> boostDict = new Dictionary<string, float>();
        public Dictionary<string, float> FieldBoostDict
        {
            get { return boostDict; }
        }
        /**/
        /// <summary>
        /// 存储主键
        /// </summary>
        private string primaryKey = "";
        /**/
        /// <summary>
        /// 设定或返回主键
        /// </summary>
        public string PrimaryKey
        {
            get { return primaryKey; }
            set { primaryKey = value; }
        }
        #endregion
        #region 方法
        public string GetFields()
        {
            if (fields == null)
                return "";
            string ret=" ";
            foreach (IndexField fb in fields)
            {
                ret += "," + fb.ToString();
            }
            return ret.Substring(ret.IndexOf(',')+1).Trim();
        }
        public bool AllContains(params string[] fieldArray)
        {
            bool ret = true;
            foreach (string field in fieldArray)
            {
                ret = ret && fieldDict.ContainsKey(field);
            }
            return ret;
        }
        #endregion
    }
}
