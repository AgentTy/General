using System;
using General;
using General.Utilities.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Utilities.Text
{
    public class StringFormatter
    {

        #region Format
        public static string FormatString(string strInput, string strFormat)
        {
            if (!Test(strInput, strFormat))
                return strInput;

            string strResult = string.Empty;
            /*  
                 # for number, @ for letter, ? for letter/number, _ for any character, * multiplier  
            */
            System.Collections.ArrayList aryFormat = ParseFormatString(strFormat);
            int i = 0;
            foreach (string strToken in aryFormat)
            {
                switch (strToken)
                {
                    case "#":
                        strResult += strInput[i];
                        i++;
                        break;
                    case "#*":
                        while (StringFunctions.IsNumeric(strInput[i].ToString()))
                        {
                            strResult += strInput[i];
                            i++;
                            if (i == strInput.Length)
                                break;
                        }
                        break;
                    case "@":
                        strResult += strInput[i];
                        i++;
                        break;
                    case "@*":
                        while (StringFunctions.IsAlphaOnly(strInput[i].ToString()))
                        {
                            strResult += strInput[i];
                            i++;
                            if (i == strInput.Length)
                                break;
                        }
                        break;
                    case "?":
                        strResult += strInput[i];
                        i++;
                        break;
                    case "?*":
                        while (StringFunctions.IsAlphaNumeric(strInput[i].ToString()))
                        {
                            strResult += strInput[i];
                            i++;
                            if (i == strInput.Length)
                                break;
                        }
                        break;
                    case "_":
                        strResult += strInput[i];
                        i++;
                        break;
                    case "_*":
                        while (i <= strInput.Length)
                        {
                            strResult += strInput[i];
                            i++;
                        }
                        break;
                    default:
                        strResult += strToken;
                        if (strInput[i].ToString() == strToken)
                            i++;
                        break;
                }
            }
            if (i < strInput.Length)
                strResult += StringFunctions.Right(strInput, strInput.Length - i);
            return strResult;
        }
        #endregion

        #region Test
        public static bool Test(string strInput, string strFormat)
        {
            return Test(strInput, strFormat, false) || Test(strInput, strFormat, true);
        }

        public static bool Test(string strInput, string strFormat, bool blnStrict)
        {
            bool blnResult = true;
            /*  
                # for number, @ for letter, ? for letter/number, _ for any character, * multiplier
            */
            System.Collections.ArrayList aryFormat = ParseFormatString(strFormat);
            int i = 0;
            foreach (string strToken in aryFormat)
            {
                switch (strToken)
                {
                    case "#":
                        if (i >= strInput.Length)
                            return false;
                        if (!StringFunctions.IsNumeric(strInput[i].ToString()))
                            return false;
                        break;
                    case "#*":
                        if (i >= strInput.Length)
                            return false;
                        if (!StringFunctions.IsNumeric(strInput[i].ToString()))
                            return false;
                        if (i < strInput.Length - 1)
                        {
                            while (StringFunctions.IsNumeric(strInput[i + 1].ToString()))
                            {
                                i++;
                                if (i == strInput.Length - 1)
                                    break;
                            }
                        }
                        break;
                    case "@":
                        if (i >= strInput.Length)
                            return false;
                        if (!StringFunctions.IsAlphaOnly(strInput[i].ToString()))
                            return false;
                        break;
                    case "@*":
                        if (i >= strInput.Length)
                            return false;
                        if (!StringFunctions.IsAlphaOnly(strInput[i].ToString()))
                            return false;
                        if (i < strInput.Length - 1)
                        {
                            while (StringFunctions.IsAlphaOnly(strInput[i + 1].ToString()))
                            {
                                i++;
                                if (i == strInput.Length - 1)
                                    break;
                            }
                        }
                        break;
                    case "?":
                        if (i >= strInput.Length)
                            return false;
                        if (!StringFunctions.IsAlphaNumeric(strInput[i].ToString()))
                            return false;
                        break;
                    case "?*":
                        if (i >= strInput.Length)
                            return false;
                        if (!StringFunctions.IsAlphaNumeric(strInput[i].ToString()))
                            return false;
                        if (i < strInput.Length - 1)
                        {
                            while (StringFunctions.IsAlphaNumeric(strInput[i + 1].ToString()))
                            {
                                i++;
                                if (i == strInput.Length - 1)
                                    break;
                            }
                        }
                        break;
                    case "_":
                        break;
                    case "_*":
                        if (i >= strInput.Length)
                            return false;
                        if (i < strInput.Length - 1)
                        {
                            while (!IsWildCard(strInput[i + 1]))
                            {
                                i++;
                                if (i == strInput.Length - 1)
                                    break;
                            }
                        }
                        break;
                    default:
                        if (!blnStrict)
                            i--;
                        break;
                }
                i++;
            }
            return blnResult;
        }
        #endregion

        #region ParseFormatString
        private static System.Collections.ArrayList ParseFormatString(string strFormat)
        {
            if (!ValidateFormat(strFormat))
                throw new ArgumentException("Format String is not Valid: " + strFormat);

            System.Collections.ArrayList aryFormat = new System.Collections.ArrayList();
            int i = 0;
            while (i < strFormat.Length)
            {
                if (i == strFormat.Length - 1)
                    aryFormat.Add(strFormat[i].ToString());
                else if (strFormat[i + 1] == '*')
                {
                    aryFormat.Add(strFormat[i].ToString() + "*");
                    i++;
                }
                else
                    aryFormat.Add(strFormat[i].ToString());
                i++;
            }
            return aryFormat;
        }
        #endregion

        #region ValidateFormat
        public static bool ValidateFormat(string strFormat)
        {
            if (strFormat.Contains("#*#"))
                return false;
            if (strFormat.Contains("@*@"))
                return false;
            if (strFormat.Contains("?*?"))
                return false;
            if (strFormat.Contains("?*#"))
                return false;
            if (strFormat.Contains("?*@"))
                return false;
            if (strFormat.Contains("_*_"))
                return false;
            if (strFormat.Contains("_*#"))
                return false;
            if (strFormat.Contains("_*@"))
                return false;
            if (strFormat.Contains("_*?"))
                return false;
            return true;
        }
        #endregion

        #region IsWildCard
        private static bool IsWildCard(char c)
        {
            switch (c)
            {
                case '#':
                case '@':
                case '?':
                case '_':
                case '*':
                    return true;
                default:
                    return false;
            }
        }
        #endregion

    }
}
