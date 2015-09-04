using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Mail
{
    public class DirtyMIMEReader
    {
        public string MIMEBody { get; set; }
        public string From { get; set; }
        public string FromName { get; set; }
        public string To { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public Dictionary<int, General.Mail.TransferEncoding> MimePartsWithEncoding { get; set; }

        public DirtyMIMEReader(string strMIMEBody)
        {
            MIMEBody = strMIMEBody;

            #region Fix Line Breaks
            int countCR = MIMEBody.Count(f => f == '\r');
            int countLF = MIMEBody.Count(f => f == '\n');
            if (countCR != countLF) //Inconsistent Line Endings Detected
            {
                MIMEBody = StringFunctions.NormalizeLineBreaks(MIMEBody);
            }
            #endregion

            #region Check Body For Attachments
            //bool blnBodyToLarge = false;
            string strBody = MIMEBody.ToString();
            if (MIMEBody.Contains("Content-Disposition: attachment"))
            {
                //strBody = StringFunctions.AllBefore(strBody, "Content-Disposition: attachment");
                MIMEBody = MIMEBody.Substring(0, MIMEBody.IndexOf("Content-Disposition: attachment"));
            }
            if (MIMEBody.Contains("Content-Type: application/octet-stream;"))
            {
                //strBody = StringFunctions.AllBefore(strBody, "Content-Type: application/octet-stream;");
                MIMEBody = MIMEBody.Substring(0, MIMEBody.IndexOf("Content-Type: application/octet-stream;"));
            }
            /*
            if (MIMEBody.Length > 500000)
            {
                blnBodyToLarge = true;
            }
            */
            #endregion

            #region Get Encoding Information
            MimePartsWithEncoding = new Dictionary<int, TransferEncoding>();
            int intTypeIndex = 0;
            int intEncodingIndex = 0;
            try
            {
                //Content-Type: text/html; charset="us-ascii"
                /* Content-Type: text/plain;
                   charset="us-ascii"*/
                //Content-Type: text/html; charset=ISO-8859-1
                while (MIMEBody.IndexOf("\nContent-Type:", intTypeIndex + 1, StringComparison.OrdinalIgnoreCase) > 0)
                {
                    //Get the index of Content-Type
                    intTypeIndex = MIMEBody.IndexOf("\nContent-Type:", intTypeIndex + 1, StringComparison.OrdinalIgnoreCase) + 1;
                    int intEndIndex = MIMEBody.IndexOf(";", intTypeIndex);
                    if (intEndIndex == -1)
                        intEndIndex = MIMEBody.IndexOf("\r\n", intTypeIndex);
                    if (intEndIndex == -1)
                        intEndIndex = MIMEBody.IndexOf("\r", intTypeIndex);
                    if (intEndIndex == -1)
                        intEndIndex = MIMEBody.IndexOf("\n", intTypeIndex);

                    string strContentTypeTemp = MIMEBody.Substring(intTypeIndex + 13, intEndIndex - (intTypeIndex + 13));

                    //Find a blank line indicating then end of this header set
                    int intEndOfHeaders = MIMEBody.IndexOf("\r\n\r\n", intTypeIndex);

                    //Find Content-Transfer-Encoding
                    intEncodingIndex = MIMEBody.IndexOf("Content-Transfer-Encoding:", intTypeIndex, StringComparison.OrdinalIgnoreCase);
                    if (intEncodingIndex > intEndOfHeaders && intEndOfHeaders > -1) //If this is outside of current scope
                        intEncodingIndex = -1;

                    if (intEncodingIndex == -1) //I didn't find Content-Transfer-Encoding
                    {
                        //I'll try again by moving UP to the -- (double dash) MIME Part delimiter
                        int intStartIndex = MIMEBody.LastIndexOf("--", intTypeIndex);
                        if (intStartIndex > -1)
                        {
                            intEncodingIndex = MIMEBody.IndexOf("Content-Transfer-Encoding:", intStartIndex, StringComparison.OrdinalIgnoreCase);
                            if (intEncodingIndex > intEndOfHeaders && intEndOfHeaders > -1) //If this is outside of current scope
                                intEncodingIndex = -1;
                        }
                    }

                    string strEncodingTemp = String.Empty;
                    if (intEncodingIndex > -1)
                    {
                        intEndIndex = MIMEBody.IndexOf("\r\n", intEncodingIndex);
                        if (intEndIndex == -1)
                            intEndIndex = MIMEBody.IndexOf("\r", intEncodingIndex);
                        if (intEndIndex == -1)
                            intEndIndex = MIMEBody.IndexOf("\n", intEncodingIndex);
                        strEncodingTemp = MIMEBody.Substring(intEncodingIndex + 27, intEndIndex - (intEncodingIndex + 27));
                    }
                    MimePartsWithEncoding.Add(intTypeIndex, General.Mail.MIMEEncoding.GetEncodingFromString(strContentTypeTemp, strEncodingTemp));
                }
            }
            catch (Exception)
            {
                /*
                try
                {
                    try
                    {
                        string strSnippet = MIMEBody.Substring(intTypeIndex, 150);
                        General.Debugging.Report.SendError("Could not read message encoding markers: " + strFromEmail, strSubject + "\r\n\r\n" + "BEGIN SNIPPET\r\n" + strSnippet + "\r\nEND SNIPPET\r\n\r\n" + ex.ToString() + "\r\n\r\n" + ex.StackTrace);
                    }
                    catch
                    {
                        General.Debugging.Report.SendError("Could not read message encoding markers or snippet: " + strFromEmail, ex);
                    }
                }
                catch { }
                */
            }


            #endregion

            this.To = DirtyReadHeader("To");
            if (this.To.Contains("<"))
            {
                this.ToName = General.StringFunctions.AllBefore(this.To, "<").Trim().Trim('\"');
                this.To = General.StringFunctions.AllBetween(this.To, "<", ">");
            }
            this.From = DirtyReadHeader("From");
            if (this.From.Contains("<"))
            {
                this.FromName = General.StringFunctions.AllBefore(this.From, "<").Trim().Trim('\"');
                this.From = General.StringFunctions.AllBetween(this.From, "<", ">");
            }
            this.Subject = DirtyReadSubject();
        }

        protected string DirtyReadSubject()
        {

            string strSubject = "";
            string strSubjectBefore = "";
            string strSubjectStage2 = "";
            try
            {
                if (!String.IsNullOrEmpty(MIMEBody))
                    strSubject = StringFunctions.AllBetween(MIMEBody, "Subject: ", "\r\n");

                strSubjectBefore = strSubject;

                if (strSubject.Contains("?Q?") || strSubject.Contains("?B?"))
                    strSubject = System.Net.Mail.Attachment.CreateAttachmentFromString("", strSubject).Name;
                strSubjectStage2 = strSubject;

                strSubject = strSubject.Trim();
            }
            catch (Exception)
            {
                strSubject = "No Subject";
                try
                {
                    //General.Debugging.Report.SendError("No Subject Line Found", ex.Message + "\r\n\r\n" + MIMEBody);
                }
                catch { }
            }
            return strSubject;
        }

        protected string DirtyReadHeader(string Name)
        {
            string strValue = "";
            if (!String.IsNullOrEmpty(MIMEBody))
                strValue = StringFunctions.AllBetween(MIMEBody, Name + ": ", "\r\n");
            strValue = strValue.Trim();
            return strValue;
        }

    }
}
