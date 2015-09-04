using System;
using System.Web;
using System.Text.RegularExpressions;
using General.Utilities.Text;

namespace General.Utilities.Web
{
	/// <summary>
	/// Removes line breaks, tabs, and comments from a text document consisting of HTML and/or Javascript
	/// </summary>
	public class Compression
	{
		/// <summary>
		/// Compression Log
		/// </summary>
		public static TextLog Log = new TextLog();

		/// <summary>
		/// Compression
		/// </summary>
		public Compression()
		{

		}

		/// <summary>
		/// Compress a string
		/// </summary>
		public static string WebCompress(string Body)
		{
			Log.Clear();
			return(WebCompress(Body,EnumContentType.Mixed));
		}

		/// <summary>
		/// Compress a string
		/// </summary>
		public static string WebCompress(string Body, EnumContentType ContentType)
		{
			Log.Clear();
			Log.Write("Starting Compression");
			Log.Write("Content type: " + ContentType.ToString());


            if (ContentType == EnumContentType.Javascript)
            {
                #region Remove Javascript Comments (2014 new method... may be buggy)
                //var blockComments = @"/\*(.*?)\*/";
                //var lineComments = @"//(.*?)\r?\n";
                //var strings = @"""((\\[^\n]|[^""\n])*)""";
                //var verbatimStrings = @"@(""[^""]*"")+";
                /*
                string strBodyNoComments = Regex.Replace(Body,
                blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
                me =>
                {
                    if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                        return me.Value.StartsWith("//") ? "\r\n" : "";
                    // Keep the literal strings
                    return me.Value;
                },
                RegexOptions.Singleline);

                Body = strBodyNoComments;
                */
                #endregion
            }

			string[] lines;
			string line;
			int char_pos = 0;
			string buffer = "";
			lines = Body.Split('\n');
			EnumLineClass line_class;
			if(ContentType == EnumContentType.Javascript)
				line_class = EnumLineClass.Javascript;
			else
				line_class = EnumLineClass.HTML;

			for(int i = 0; i<lines.Length; i++)
			{
				line = lines[i];
                line_class = GetLineClass(line, line_class);
                buffer += WebCompressLine(line, line_class, i + 1, char_pos); //+ "\n"
				char_pos+= line.Length;
			}
			Log.Write("Compression Complete");
			return(buffer);
		}

		/// <summary>
		/// Compress a string and send to HtmlTextWriter
		/// </summary>
		public static void WebCompress(string Body, System.Web.UI.HtmlTextWriter Writer)
		{
			Log.Clear();
			WebCompress(Body,Writer,EnumContentType.Mixed);
		}

		/// <summary>
		/// Compress a string and send to HtmlTextWriter
		/// </summary>
		public static void WebCompress(string Body, System.Web.UI.HtmlTextWriter Writer, EnumContentType ContentType)
		{
			Log.Clear();
			Log.Write("Starting Compression");
			Log.Write("Content type: " + ContentType.ToString());

            string[] lines;
			string line;
			int char_pos = 0;
			lines = Body.Split('\n');
			EnumLineClass line_class;
			if(ContentType == EnumContentType.Javascript)
				line_class = EnumLineClass.Javascript;
			else
				line_class = EnumLineClass.HTML;

			for(int i = 0; i<lines.Length; i++)
			{
				line = lines[i];
				line_class = GetLineClass(line,line_class);
				Writer.Write(WebCompressLine(line,line_class,i + 1,char_pos));
				char_pos+= line.Length;
			}
			Log.Write("Compression Complete");
		}

		private static string WebCompressLine(string line, EnumLineClass line_class, int line_number, int col_number)
		{
			string index = "l" + line_number.ToString() + " c" + col_number.ToString();
			//string temp;
			line = line.Replace("\r",""); //REMOVE RETURNS
			line = line.Replace("\t",""); //REMOVE TABS
			line = line.Trim();
			if(line_class == EnumLineClass.HTML)
			{
				if(line.IndexOf("<!--") != -1)
					if(line.IndexOf("-->") != -1)
						line = StringFunctions.Left(line,line.IndexOf("<!--")) + StringFunctions.AllAfter(line,"-->");
					else
						line = StringFunctions.Left(line,line.IndexOf("<!--"));
				else if(line.IndexOf("-->") != -1)
					line = StringFunctions.AllAfter(line,"-->");				
			}
			else if(line_class == EnumLineClass.Javascript)
			{
				if(line.IndexOf("/*") != -1)
					if(line.IndexOf("*/") != -1)
						line = StringFunctions.Left(line,line.IndexOf("/*")) + StringFunctions.AllAfter(line,"*/");
					else
						line = StringFunctions.Left(line,line.IndexOf("/*"));
				else if(line.IndexOf("*/") != -1)
					line = StringFunctions.AllAfter(line,"*/");

				if(line.IndexOf("-->") == -1 && line.IndexOf("-->") == -1)
					if(line.Length >= 2)
					{
						if(StringFunctions.Left(line,2) == "//")
							line = "";
						else
						{
							if(line.IndexOf("://") != -1)
								line = line.Replace("://","~~temporaryvalue~~");
							line = Regex.Replace(line,"(?si)(//).*",""); //REMOVE JAVASCRIPT COMMENT <!--COMMENT-->
							if(line.IndexOf("~~temporaryvalue~~") != -1)
								line = line.Replace("~~temporaryvalue~~","://");
						}
					}

				line = Regex.Replace(line,"(?si)else","else "); //ADD SPACE AFTER ELSE
				if(Regex.IsMatch(line,"\\s*(((if\\s*\\()|else|switch\\s*\\(|(/\\*)|(\\*/)|(try)|(catch)|(while)|(case.*$)|(<!--)|(-->)|(.*[^:]*//)|(<script)|(<style)|(function)|(\\{)|(\\}).*)|(.*;))\\s*") == false && line.Trim().Length > 0)
				{
					Log.Write(index + " - MISSING SEMICOLON: " + line);
                    //line = line + ";\r\n";
					//System.Web.HttpContext.Current.Trace.Write(index + " - MISSING SEMICOLON: " + line);
				}
			}
			else if(line_class == EnumLineClass.HTMLComment)
			{
				//System.Web.HttpContext.Current.Trace.Write(index + " - HTML COMMENT: " + line);
				if(line.IndexOf("<!--") != -1)
				{
					line = StringFunctions.Left(line,line.IndexOf("<!--"));
				}
				else
				{
					line = "";
				}

			}
			else if(line_class == EnumLineClass.JavascriptComment)
			{
				//System.Web.HttpContext.Current.Trace.Write(index + " - JS COMMENT: " + line);
				if(line.IndexOf("/*") != -1)
				{
					line = StringFunctions.Left(line,line.IndexOf("/*"));
				}
				else
				{
					line = "";
				}
			}

			return(line);
		}

		private static EnumLineClass GetLineClass(string line)
		{
			return(GetLineClass(line,EnumLineClass.HTML));
		}

		private static EnumLineClass GetLineClass(string line,EnumLineClass previous)
		{
			line = line.ToLower();
			if(previous == EnumLineClass.HTML)
			{
				if(line.IndexOf("<script") != -1 && line.IndexOf("</script>") == -1)
					return(EnumLineClass.Javascript);
				else if(line.IndexOf("<style") != -1 && line.IndexOf("</style>") == -1)
					return(EnumLineClass.Javascript);
				else if(line.IndexOf("<!--") != -1 && line.IndexOf("-->") == -1)
					return(EnumLineClass.HTMLComment);
				else
					return(previous);
			}
			else if(previous == EnumLineClass.Javascript)
			{
				if(line.IndexOf("</script>") != -1)
					return(EnumLineClass.HTML);
				else if(line.IndexOf("</style>") != -1)
					return(EnumLineClass.HTML);
				else if(line.IndexOf("/*") != -1 && line.IndexOf("*/") == -1)
					return(EnumLineClass.JavascriptComment);
				else
					return(previous);
			}
			else if(previous == EnumLineClass.HTMLComment)
			{
				if(line.IndexOf("-->") != -1)
					return(EnumLineClass.HTML);
				else
					return(previous);
			}
			else if(previous == EnumLineClass.JavascriptComment)
			{
				if(line.IndexOf("*/") != -1)
					return(EnumLineClass.Javascript);
				else
					return(previous);
			}
			return(EnumLineClass.HTML);
		}

		private enum EnumLineClass
		{
			HTML = 1,
			Javascript = 2,
			HTMLComment = 3,
			JavascriptComment = 4
		}

		/// <summary>
		/// Enumerates possible content types for web compression
		/// </summary>
		public enum EnumContentType
		{
			/// <summary>
			/// HTML and Javascript
			/// </summary>
			Mixed = 1,
			/// <summary>
			/// Javascript only
			/// </summary>
			Javascript = 2
		}
	}
}
