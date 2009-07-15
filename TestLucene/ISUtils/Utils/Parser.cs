using System;
using System.Collections.Generic;
using System.Collections;
using ISUtils.Common;

namespace ISUtils.Utils
{
    public class Parser
    {
        /**/
        /// <summary>
        /// 存储索引源列表
        /// </summary>
        private List<Source> sourceList;
        /**/
        /// <summary>
        /// 返回索引源列表
        /// </summary>
        public List<Source> SourceList
        {
            get { return sourceList; }
        }
        /**/
        /// <summary>
        /// 存储索引列表
        /// </summary>
        private List<IndexSet> indexList;
        /**/
        /// <summary>
        /// 返回索引列表
        /// </summary>
        public List<IndexSet> IndexList
        {
            get { return indexList; }
        }
        /**/
        /// <summary>
        /// 存储索引器
        /// </summary>
        private IndexerSet indexer;
        /**/
        /// <summary>
        /// 返回索引器
        /// </summary>
        public IndexerSet Indexer
        {
            get { return indexer; }
        }
        /**/
        /// <summary>
        /// 存储搜索引擎设置
        /// </summary>
        private SearchSet set;
        /**/
        /// <summary>
        /// 返回搜索引擎设置
        /// </summary>
        public SearchSet Searchd
        {
            get { return set; }
        }
        /**/
        /// <summary>
        /// 存储词库设置
        /// </summary>
        private DictionarySet dictSet;
        /**/
        /// <summary>
        /// 返回词库设置
        /// </summary>
        public DictionarySet DictionarySet
        {
            get { return dictSet; }
        }
        /**/
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        public Parser(string configFilePath)
        {
            if (configFilePath == null)
                throw new ArgumentNullException("configFilePath", "configFilePath must no be null!");
            if (SupportClass.FileUtil.IsFileExists(configFilePath) == false)
                throw new ArgumentException("configFilePath must be exists!", "configFilePath");
            List<string> src = SupportClass.FileUtil.GetFileText(configFilePath);
            sourceList = Source.GetSourceList(src);
            indexList = IndexSet.GetIndexList(src);
            indexer = IndexerSet.GetIndexer(src);
            set = SearchSet.GetSearchSet(src);
            dictSet = DictionarySet.GetDictionarySet(src);
        }
        /**/
        /// <summary>
        /// 获取索引源列表
        /// </summary>
        /// <returns>返回类型</returns>
        public List<Source> GetSourceList()
        {
            return sourceList;
        }
        /**/
        /// <summary>
        /// 获取索引列表
        /// </summary>
        /// <returns>返回类型</returns>
        public List<IndexSet> GetIndexList()
        {
            return indexList;
        }
        /**/
        /// <summary>
        /// 获取索引器
        /// </summary>
        /// <returns>返回类型</returns>
        public IndexerSet GetIndexer()
        {
            return indexer;
        }
        /**/
        /// <summary>
        /// 获取搜索引擎设置
        /// </summary>
        /// <returns>返回类型</returns>
        public SearchSet GetSearchd()
        {
            return set;
        }
        /**/
        /// <summary>
        /// 获取词库设置
        /// </summary>
        /// <returns>返回类型</returns>
        public DictionarySet GetDictionarySet()
        {
            return dictSet;
        }
    }
}
