using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace General.Utilities.Data.Conversion
{
	public class WorkbookEngine
	{
	// you could have other overloads if you want to get creative...
	public static string CreateWorkbook(DataSet ds)
	{
		XmlDataDocument xmlDataDoc = new XmlDataDocument(ds);
			XslTransform xt = new XslTransform();
			StreamReader reader =
				new StreamReader(typeof (WorkbookEngine).Assembly.GetManifestResourceStream(typeof (WorkbookEngine), "Excel.xsl"));
			XmlTextReader xRdr = new XmlTextReader(reader);
			xt.Load(xRdr, null, null);
			StringWriter sw = new StringWriter();
			xt.Transform(xmlDataDoc, null, sw, null);
			return sw.ToString();
		}
	}
}