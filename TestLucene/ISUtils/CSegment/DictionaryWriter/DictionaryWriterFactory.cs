using System;
using System.Collections.Generic;
using System.Text;

namespace Lwh.ChineseSegment.DictionaryWriter
{
    internal class DictionaryWriterFactory
    {
        IDictionaryWriter CreateDictionaryWriter(string writer)
        {
            switch (writer)
            {
                case "TextDictionaryWriter": return new TextDictionaryWriter();
                case "XmlDictionaryWriter": return new XmlDictionaryWriter();
                case "BinDictionaryWriter": return new BinDictionaryWriter();
                default: return new TextDictionaryWriter();

            }
        }
    }
}
