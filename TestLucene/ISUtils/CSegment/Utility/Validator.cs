using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.CSegment.Utility
{
    internal static class Validator
    {
        public static bool IsValidFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            if (!System.IO.File.Exists(filePath))
            {
                return false;
            }

            return true;
        }
    }
}
