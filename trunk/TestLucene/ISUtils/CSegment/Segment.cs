using System;
using System.Collections.Generic;
using System.Text;
using ISUtils.CSegment.Utility;
using ISUtils.CSegment.SegmentDictionary;
using ISUtils.CSegment.DictionaryLoader;

namespace ISUtils.CSegment
{
    public static class Segment
    {
        private static string _basePath = "";
        private static string _namePath = "";
        private static string _numberPath = "";
        private static string _filterPath = "";
        private static List<string> _customPaths = new List<string>();
        private static IWordSegment wordSegment=new ForwardMatchSegment();
        private static IDictionaryLoader dictLoader=new TextDictionaryLoader();
        private static bool initPath = false;
        private static bool initDefs = false;
        public static void SetPaths(string basePath, string namePath, 
                                 string numberPath, string filterPath,
                                 List<string> customPaths)
        {
            if (basePath == null)
                throw new ArgumentNullException("basePath", "basePath must no be null!");
            if (Validator.IsValidFile(basePath) == false)
                throw new ArgumentException("basePath must be exists!", "basePath");
            _basePath = basePath;
            _namePath = namePath;
            _numberPath = numberPath;
            _filterPath = filterPath;
#if DEBUG
            System.Console.WriteLine("BasePath:" + basePath);
            System.Console.WriteLine("NamePath:" + namePath);
            System.Console.WriteLine("NumberPath:" +numberPath);
            System.Console.WriteLine("FilterPath:" + filterPath);
#endif
            foreach (string path in customPaths)
            {
                _customPaths.Add(path);
#if DEBUG
                System.Console.WriteLine("CustomPath:" + path);
#endif
            }
            initPath = true;
        }
        public static void SetPaths(string basePath, string namePath,
                                 string numberPath, string filterPath,
                                 params string[] customPaths)
        {
            if (basePath == null)
                throw new ArgumentNullException("basePath", "basePath must no be null!");
            if (Validator.IsValidFile(basePath) == false)
                throw new ArgumentException("basePath must be exists!", "basePath");
            _basePath = basePath;
            _namePath = namePath;
            _numberPath = numberPath;
            _filterPath = filterPath;
#if DEBUG
            System.Console.WriteLine("BasePath:" + basePath);
            System.Console.WriteLine("NamePath:" + namePath);
            System.Console.WriteLine("NumberPath:" + numberPath);
            System.Console.WriteLine("FilterPath:" + filterPath);
#endif
            foreach (string path in customPaths)
            {
                _customPaths.Add(path);
#if DEBUG
                System.Console.WriteLine("CustomPath:" + path);
#endif
            }
            initPath = true;
        }
        public static void SetDefaults(IDictionaryLoader loader, IWordSegment wordseg)
        {
            dictLoader = loader;
            wordSegment = wordseg;
            wordSegment.LoadDictionary(loader, _basePath);
            wordSegment.LoadNameDictionary(loader, _namePath);
            wordSegment.LoadNumberDictionary(loader, _numberPath);
            foreach (string path in _customPaths)
                wordSegment.AppendDictionary(loader, path);
            wordSegment.LoadFilterDictionary(loader, _filterPath);
            initDefs = true;
        }
        public static bool IsInit()
        {
            return initDefs && initPath;
        }
        public static string SegmentString(string text)
        {
            if (!initDefs || !initPath)
                throw new ApplicationException("Segment has not init!");
            return wordSegment.Segment(text);
        }
        public static List<string> SegmentStringEx(string text)
        {
            if (!initDefs || !initPath)
                throw new ApplicationException("Segment has not init!");
            return wordSegment.SegmentEx(text);
        }
        public static string SegmentString(string text, string separator)
        {
            if (!initDefs || !initPath)
                throw new ApplicationException("Segment has not init!");
            wordSegment.Separator = separator;
            return wordSegment.Segment(text);
        }
        public static List<string> SegmentStringEx(string text, string separator)
        {
            if (!initDefs || !initPath)
                throw new ApplicationException("Segment has not init!");
            wordSegment.Separator = separator;
            return wordSegment.SegmentEx(text);
        }
#if DEBUG
        public static void OutputDictionary()
        {
            wordSegment.SegmentDictionary.Output();
        }
#endif
    }
}
