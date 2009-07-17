using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ISUtils.CSegment.Utility
{
    internal static class Character
    {
        public static bool IsChinese(string text)
        {
            string regExpression = "[\u4e00-\u9fa5]";
            return Regex.IsMatch(text, regExpression);
        }

        public static bool IsLetter(string text)
        {
            string regExpression = "[a-z|A-Z]";
            return Regex.IsMatch(text, regExpression);
        }

        public static bool IsNumber(string text)
        {
            string regExpression = "[0-9]";
            return Regex.IsMatch(text, regExpression);
        }
        public static CharType TypeOf(string text)
        {
            if (Regex.IsMatch(text, "[\u4e00-\u9fa5]"))
                return CharType.Chinese;
            else if (Regex.IsMatch(text, "[a-z|A-Z]"))
                return CharType.Letter;
            else if (Regex.IsMatch(text, "[0-9]"))
                return CharType.Number;
            else
                return CharType.Other;
        }
    }
    internal enum CharType
    {
        Chinese,
        Letter,
        Number,
        Other
    }
}
