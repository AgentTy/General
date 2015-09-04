using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Globalization;
using System.Xml;

namespace General
{
    public class XML
    {

        #region SerializeToXml
        public static string SerializeToXml(object value)
        {
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            XmlSerializer serializer = new XmlSerializer(value.GetType());
            serializer.Serialize(writer, value);
            return writer.ToString();
        }

        public static string SerializeToXml(object value, Type type)
        {
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            XmlSerializer serializer = new XmlSerializer(type);
            serializer.Serialize(writer, value);
            return writer.ToString();
        }

        public static string SerializeToXml(System.Collections.Specialized.NameValueCollection list, string strDocumentName)
        {
            return NameValueCollectionToXmlDocument(list, strDocumentName).OuterXml;
        }


        public static XmlDocument NameValueCollectionToXmlDocument(System.Collections.Specialized.NameValueCollection list, string strDocumentName)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-16", String.Empty));

            XmlElement root = doc.CreateElement(strDocumentName);
            //root.SetAttribute("name", strDocumentName);

            foreach (string strKey in list.AllKeys)
            {
                XmlElement child = doc.CreateElement(strKey);
                child.InnerText = list[strKey];
                root.AppendChild(child);
            }

            doc.AppendChild(root);
            return doc;
        }

        public static System.Collections.Specialized.NameValueCollection XmlToNameValueCollection(XmlDocument doc)
        {
            System.Collections.Specialized.NameValueCollection list = new System.Collections.Specialized.NameValueCollection();
            foreach (XmlNode node in doc.ChildNodes)
            {
                if (!String.IsNullOrEmpty(node.Value))
                    list.Add(node.Name, node.Value);
                else
                    list.Add(node.Name, node.InnerText);
            }
            return list;
        }
        #endregion

        #region DeserializeXML
        public static T DeserializeXML<T>(string xmlString)
        {
            //This code will catch an xml definition with nothing inside of it.
            if (xmlString.EndsWith("/>"))
                if (General.StringFunctions.Count(xmlString, "<") == 1 && General.StringFunctions.Count(xmlString, ">") == 1)
                    return default(T);

            using (MemoryStream memStream = new MemoryStream(Encoding.Unicode.GetBytes(xmlString)))
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(memStream);
            }
        }
        #endregion

    }
}
