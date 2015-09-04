using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace General
{
    /// <summary>
    /// Summary description for WordpressFunctions
    /// </summary>
    public class WordpressFunctions
    {


        #region wpautop (Formatting Content)
        /**
         * Replaces double line-breaks with paragraph elements.
         *
         * A group of regex replaces used to identify text formatted with newlines and
         * replace double line-breaks with HTML paragraph tags. The remaining
         * line-breaks after conversion become <<br />> tags, unless $br is set to '0'
         * or 'false'.
         *
         * @since 0.71
         *
         * @param string $pee The text which has to be formatted.
         * @param bool $br Optional. If set, this will convert all remaining line-breaks after paragraphing. Default true.
         * @return string Text which has been converted into correct paragraph tags.
        */
        /* 
         * 
         * This is a conversion of the wpautop function from PHP to C#
         * Manually compiled by Ty Hansen on 12/18/2013
         * ... fun it was ... LOL ...
         * 
         * 
        */
        public static string wpautop(string pee)
        {
            return wpautop(pee, true);
        }

        public static string wpautop(string pee, bool br)
        {
            Dictionary<string, string> pre_tags = new Dictionary<string, string>();
            if (String.IsNullOrEmpty(pee.Trim()))
                return "";

            pee = pee + "\n"; // just to make things a little easier, pad the end


            if (pee.Contains("<pre"))
            {
                Stack<string> pee_parts = new Stack<string>(pee.Split(new string[] { "</pre>" }, StringSplitOptions.None));
                string last_pee = pee_parts.Pop();
                pee = "";
                int i = 0;

                foreach (string pee_part in pee_parts)
                {
                    int start = pee_part.IndexOf("<pre");

                    // Malformed html?
                    if (start < 0)
                    {
                        pee += pee_part;
                        continue;
                    }

                    string name = "<pre wp-pre-tag-" + i.ToString() + "></pre>";
                    pre_tags[name] = pee_part.Substring(start) + "</pre>";
                    pee += pee_part.Substring(0, start) + name;
                    i++;
                }
                pee += last_pee;
            }

            pee = Regex.Replace(pee, @"<br />\s*<br />", "\n\n"); // Space things out a little
            string allblocks = "(?:table|thead|tfoot|caption|col|colgroup|tbody|tr|td|th|div|dl|dd|dt|ul|ol|li|pre|select|option|form|map|area|blockquote|address|math|style|p|h[1-6]|hr|fieldset|noscript|legend|section|article|aside|hgroup|header|footer|nav|figure|figcaption|details|menu|summary)";
            pee = pee = Regex.Replace(pee, @"(<" + allblocks + "[^>]*>)", "\n$1");
            pee = pee = Regex.Replace(pee, @"(</" + allblocks + ">)", "$1\n\n");
            pee = pee.Replace("\r\n", "\n"); // cross-platform newlines
            pee = pee.Replace("\r", "\n"); // cross-platform newlines
            if (pee.Contains("<object"))
            {
                pee = pee = Regex.Replace(pee, @"\s*<param([^>]*)>\s*", "<param1>"); // no pee inside object/embed
                pee = pee = Regex.Replace(pee, @"\s*</embed>\s*", "</embed>");
            }

            pee = pee = Regex.Replace(pee, @"\n\n+", "\n\n"); // take care of duplicates
            // make paragraphs, including one at the end
            var pees = Regex.Split(pee, @"\n\s*\n", RegexOptions.None);
            pee = "";

            foreach (string tinkle in pees)
                if (!String.IsNullOrEmpty(tinkle.Trim()))
                    pee += "<p>" + tinkle.Trim(new char[] { '\n' }) + "</p>\n";

            pee = pee = Regex.Replace(pee, @"<p>\s*</p>", ""); // under certain strange conditions it could create a P of entirely whitespace
            pee = pee = Regex.Replace(pee, @"<p>([^<]+)</(div|address|form)>", "<p>$1</p></$2>");
            pee = pee = Regex.Replace(pee, @"<p>\s*(</?" + allblocks + @"[^>]*>)\s*</p>", "$1"); // don't pee all over a tag
            pee = pee = Regex.Replace(pee, @"<p>(<li.+?)</p>", "$1"); // problem with nested lists
            pee = pee = Regex.Replace(pee, @"<p><blockquote([^>]*)>", "<blockquote$1><p>");
            pee = pee.Replace("</blockquote></p>", "</p></blockquote>");
            pee = pee = Regex.Replace(pee, @"<p>\s*(</?" + allblocks + "[^>]*>)", "$1");
            pee = pee = Regex.Replace(pee, @"(</?" + allblocks + @"[^>]*>)\s*</p>", "$1");
            if (br)
            {
                //pee = preg_replace_callback(@"/<(script|style).*?<\/\\1>/s", "_autop_newline_preservation_helper");
                pee = Regex.Replace(pee, @"<(script|style).*?<\/\\1>", new MatchEvaluator(_autop_newline_preservation_helper));
                pee = pee = Regex.Replace(pee, @"(?<!<br />)\s*\n", "<br />\n"); // optionally make line breaks
                pee = pee.Replace("<WPPreserveNewline />", "\n");
            }

            pee = pee = Regex.Replace(pee, @"(</?" + allblocks + @"[^>]*>)\s*<br />", "$1");
            pee = pee = Regex.Replace(pee, @"<br />(\s*</?(?:p|li|div|dl|dd|dt|th|pre|td|ul|ol)[^>]*>)", "$1");
            pee = pee = Regex.Replace(pee, @"\n</p>$", "</p>");

            if (pre_tags.Count > 0)
            {
                foreach (var item in pre_tags)
                    pee = pee.Replace(item.Key, item.Value);
            }

            pee = pee.Replace("<pre>", "");
            pee = pee.Replace("</pre>", "");
            return pee;
        }


        /*
         * Newline preservation help function for wpautop
         *
         * @since 3.1.0
         * @access private
         *
         * @param array $matches preg_replace_callback matches array
         * @return string
        */
        private static string _autop_newline_preservation_helper(Match m)
        {
            return m.Value.Replace("\n", "<WPPreserveNewline />");
        }

        #endregion

    }
}