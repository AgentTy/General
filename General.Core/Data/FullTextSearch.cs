using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace General.Data
{
    public class FullTextSearch
    {
        public static string[] Tokenize(string strSearch)
        {
            strSearch = strSearch.Replace("'", "\"");
            var tokens = Regex.Matches(strSearch.Trim(), @"[\""].+?[\""]|[^ ]+")
               .Cast<Match>()
               .Select(m => m.Value.Replace("\"", "").ToLower().Trim())
               .OrderBy(m => !m.StartsWith("-")).ToArray();

            return tokens;
        }

        public static bool TokenContains(string strText, string[] aryTokens, out bool blnHardBlock)
        {
            bool blnTempBlock = false;
            if (aryTokens.Any(t => strText.ToLower().Contains(CheckForNot(t, out blnTempBlock))))
            {
                blnHardBlock = blnTempBlock;
                return true;
            }
            else
            {
                blnHardBlock = false;
                return false;
            }

        }

        public static bool TokenEquals(string strText, string[] aryTokens, out bool blnHardBlock)
        {
            bool blnTempBlock = false;
            if (aryTokens.Any(t => strText.ToLower().Equals(CheckForNot(t, out blnTempBlock))))
            {
                blnHardBlock = blnTempBlock;
                return true;
            }
            else
            {
                blnHardBlock = false;
                return false;
            }

        }

        public static bool TokenEquals(string[] aryValues, string[] aryTokens, out bool blnHardBlock)
        {
            bool blnTempBlock = false;
            if (aryTokens.Any(t => aryValues.Contains<string>(CheckForNot(t, out blnTempBlock), new CaseInsensitiveComparer())))
            {
                blnHardBlock = blnTempBlock;
                return true;
            }
            else
            {
                blnHardBlock = false;
                return false;
            }
        }

        public static string CheckForNot(string strToken, out bool blnHardBlock)
        {
            if (String.IsNullOrWhiteSpace(strToken))
            {
                blnHardBlock = false;
                return "";
            }
            if (strToken.StartsWith("-"))
            {
                blnHardBlock = true;
                return strToken.Substring(1, strToken.Length - 1);
            }
            else
            {
                blnHardBlock = false;
                return strToken;
            }
        }

        #region CaseInsensitiveComparer
        internal class CaseInsensitiveComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return String.Equals(x, y, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(string obj)
            {
                return obj.GetHashCode();
            }
        }
        #endregion
    }
}
