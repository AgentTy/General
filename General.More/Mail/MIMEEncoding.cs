using System;
using System.Collections.Generic;
using System.Text;

namespace General.Mail
{

    public enum TransferEncoding
    {
        MultiPart,
        QuotedPrintable,
        Base64,
        SevenBit,
        EightBit,
        Unknown
    }

    public class MIMEEncoding
    {

        #region Encode/Decode
        public static string Decode(string content, TransferEncoding transferEncoding, System.Text.Encoding binaryEncoding = null)
        {
            if (binaryEncoding == null)
                binaryEncoding = System.Text.Encoding.UTF8;

            switch (transferEncoding)
            {
                case TransferEncoding.QuotedPrintable: return QuotedPrintableDecode(content);
                case TransferEncoding.Base64: return Base64Decode(content, binaryEncoding);
                default: return content;
            }
        }

        public static string Encode(string content, TransferEncoding transferEncoding, System.Text.Encoding binaryEncoding = null)
        {
            if (binaryEncoding == null)
                binaryEncoding = System.Text.Encoding.UTF8;

            switch (transferEncoding)
            {
                case TransferEncoding.QuotedPrintable: return QuotedPrintableEncode(content);
                case TransferEncoding.Base64: return Base64Encode(content, binaryEncoding);
                default: return content;
            }
        }
        #endregion

        #region GetEncodingFromString
        public static General.Mail.TransferEncoding GetEncodingFromString(string strContentType, string strEncoding)
        {
            strContentType = strContentType.ToLowerInvariant().Trim();
            strEncoding = strEncoding.ToLowerInvariant().Trim();

            if (!string.IsNullOrEmpty(strEncoding))
            {
                switch (strEncoding)
                {
                    case "quoted-printable": return General.Mail.TransferEncoding.QuotedPrintable;
                    case "base64": return General.Mail.TransferEncoding.Base64;
                    case "7bit": return General.Mail.TransferEncoding.SevenBit;
                    case "8bit": return General.Mail.TransferEncoding.EightBit;
                }
            }

            switch (strContentType)
            {
                case "text/plain": return General.Mail.TransferEncoding.SevenBit;
                case "text/html": return General.Mail.TransferEncoding.SevenBit;
                case "multipart/alternative": return General.Mail.TransferEncoding.MultiPart;
                default: return General.Mail.TransferEncoding.Unknown;
            }

        }
        #endregion

        #region QuotedPrintableEncode
        public static string QuotedPrintableEncode(string strInput)
        {
            return QuotedPrintableEncode(strInput.ToCharArray());
        }

        private static string QuotedPrintableEncode(char[] Chars)
        {
            int TargetLineLength = 73; //Quoted-Printable allows up to 76 characters per line, I use less to be safe.
            int MaxLineLength = 76;
            int LinePosition = 1;
            int i = 0;
            int Ascii = 0;
            int AsciiWhiteSpace = Convert.ToInt32(' ');
            string EncodedChar = "";
            StringBuilder ReturnString = new StringBuilder();

            for (i = 0; i <= Chars.Length - 1; i++)
            {
                Ascii = Convert.ToInt32(Chars[i]);
                if ((Ascii < 32 | Ascii == 61 | Ascii > 126) && Ascii != 10 && Ascii != 13) //If < 32 OR = OR > 126 AND NOT /r OR /n
                {
                    EncodedChar = String.Format("{0:X}", Ascii).ToUpper();
                    if (EncodedChar.Length == 1)
                        EncodedChar = "0" + EncodedChar;
                    ReturnString.Append("=" + EncodedChar);
                    LinePosition += 2;
                }
                else
                {
                    ReturnString.Append(Chars[i]);
                }

                LinePosition++;
                if (Chars[i] == '\n')
                {
                    LinePosition = 1; //Reset for new line
                    if(i - 2 > 0)
                        if(Chars[i - 2] == ' ')
                        {
                            ReturnString.Remove(ReturnString.Length - 3, 1);
                            ReturnString.Insert(ReturnString.Length - 2, "=20"); //Add =20 if whitespace is at end of line
                        }
                }

                if (LinePosition >= TargetLineLength)
                {
                    if (i + 1 < Chars.Length)
                    {
                        int AsciiNextChar = Convert.ToInt32(Chars[i + 1]);
                        if ((AsciiNextChar != AsciiWhiteSpace || LinePosition >= MaxLineLength) && Chars[i + 1] != '\r' && Chars[i + 1] != '\n')// This causes instability
                        {
                            ReturnString.Append("=\r\n");
                            LinePosition = 1;
                        }
                    }
                }
            }
            return ReturnString.ToString();
        }
        #endregion

        #region QuotedPrintableDecode
        public static string QuotedPrintableDecode(string strInput)
        {
            return QuotedPrintableDecode(strInput.ToCharArray());
        }

        private static string QuotedPrintableDecode(char[] Chars)
        {
            string strTest = new string(Chars);
            int i = 0;
            StringBuilder ReturnString = new StringBuilder();
            for (i = 0; i <= Chars.Length - 1; i++)
            {
                if (Chars[i] == '=')
                {
                    string TheValue = null;
                    if (i + 2 >= Chars.Length)
                    {
                        TheValue = ""; // = is at end of content
                        goto next;
                    }
                    else if (Chars[i + 1] == '0')
                    {
                        TheValue = Chars[i + 2].ToString();
                    }
                    else
                    {
                        TheValue = Chars[i + 1].ToString() + Chars[i + 2].ToString();
                    }
                    if (TheValue == "\r\n")
                    {
                        i += 2;
                    }
                    else
                    {
                        int IntValue = Convert.ToInt32(TheValue, 16);
                        if (TheValue == String.Format("{0:X}", IntValue))
                        {
                            ReturnString.Append(Convert.ToChar(IntValue));
                            i += 2;
                        }
                        else
                        {
                            ReturnString.Append(Chars[i]);
                        }
                    }
                }
                else
                {
                    ReturnString.Append(Chars[i]);
                }
            next: bool GoingToNext = true;
            }
            return ReturnString.ToString();
        }
        #endregion

        #region Base64Encode
        public static string Base64Encode(string input, System.Text.Encoding binaryEncoding)
        {
            return Convert.ToBase64String(binaryEncoding.GetBytes(input));
        }
        #endregion

        #region Base64Decode
        public static string Base64Decode(string input, System.Text.Encoding binaryEncoding)
        {
            return binaryEncoding.GetString(Convert.FromBase64String(input));
        }
        #endregion

    }


}
