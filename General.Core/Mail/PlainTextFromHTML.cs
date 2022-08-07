using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mshtml;
using System.Text.RegularExpressions;

namespace General.Mail
{
    public class PlainText
    {

        #region GetPlainTextFromHTML
        public static string GetPlainTextFromHTML(string strHTML)
        {
            string strPlainText;

            try
            {
                HTMLDocument htmldoc = new HTMLDocument();
                IHTMLDocument2 htmldoc2 = (IHTMLDocument2)htmldoc;
                htmldoc2.write(new object[] { strHTML });
                strPlainText = htmldoc2.body.outerText;
            }
            catch(Exception ex)
            {
                strPlainText = Regex.Replace(strHTML, @"<p>|</p>|<br>|<br />", "\r\n");
                strPlainText = Regex.Replace(strPlainText, @"\<[^\>]*\>", string.Empty);
            }

            return strPlainText;
        }
        #endregion

    }
}
