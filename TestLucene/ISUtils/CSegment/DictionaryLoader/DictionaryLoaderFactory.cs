using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.CSegment.DictionaryLoader
{
    internal static class DictionaryLoaderFactory
    {
        public static IDictionaryLoader CreateDictionaryLoader(string loader)
        {
            switch (loader)
            {
                case "TextDictionaryLoader": return new TextDictionaryLoader();
                case "XmlDictionaryLoader": return new XmlDictionaryLoader();
                case "BinDictionaryLoader": return new BinDictionaryLoader();
                default: return new TextDictionaryLoader();

            }
        }
    }
}
