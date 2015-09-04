using System;
using System.Text;
using System.Xml;
using System.Collections;

namespace General
{
    public class XMLNameValueCollection : GenericNameValueCollection<object>
    {

        #region Private Variables
        private XmlDocument _xmlData;
        private bool blnReading = false;
        private bool blnSerializeObjects = true; //True to handle complex objects and stong data typing
        #endregion

        #region Constructors
        public XMLNameValueCollection()
        {
            _xmlData = new XmlDocument();
            WriteXML();
            base.AnnounceChange = new GenericNameValueCollection<object>.ChangeDetectedDelegate(WriteXML);
        }

        public XMLNameValueCollection(XmlDocument xmlData)
        {
            _xmlData = xmlData;
            ReadXML();
            base.AnnounceChange = new GenericNameValueCollection<object>.ChangeDetectedDelegate(WriteXML);
        }
        #endregion

        #region Public Properties
        public XmlDocument XMLDocument
        {
            get { return _xmlData; }
        }
        #endregion

        #region Read XML

        private void ReadXML()
        {
            if (_xmlData.SelectSingleNode("NameValueCollection") != null)
            {
                blnReading = true;
                //GenericNameValueCollection<object> objCol = new GenericNameValueCollection<object>(new GenericNameValueCollection<object>.ChangeDetectedDelegate(WriteXML));
                foreach (object objNode in _xmlData.SelectSingleNode("NameValueCollection"))
                {
                    if (objNode.GetType() == typeof(XmlElement))
                    {
                        XmlElement objElement = (XmlElement)objNode;

                        if (blnSerializeObjects)
                        {
                            //try
                            //{
                            base.Add(objElement.Attributes["Name"].Value, General.Utilities.Serialization.SerializationTools.DeserializeObject(objElement.Attributes["Type"].Value, objElement.Attributes["Value"].Value));
                            //}
                            //catch(Exception ex)
                            //{
                            // base.Add(objElement.Attributes["name"].Value, objElement.Attributes["value"].Value);
                            //}
                        }
                        else
                            this.Add(objElement.Attributes["Name"].Value, objElement.Attributes["Value"].Value);

                    }
                }
               // _objSpecs = objCol;
                blnReading = false;
            }

        }
        #endregion

        #region Write XML
        private void WriteXML()
        {
            if (!blnReading)
            {
                if (_xmlData != null && this != null)
                {
                    /*foreach (XmlElement objElement in _xmlSpecs.SelectSingleNode("NameValueCollection"))
                    {
                        objElement.Attributes["value"].Value = _objSpecs[objElement.Attributes["name"].Value].ToString();
                    }
                    */

                    if (_xmlData.SelectSingleNode("NameValueCollection") == null)
                        _xmlData.AppendChild(_xmlData.CreateNode(XmlNodeType.Element, "NameValueCollection", ""));
                    else
                        _xmlData.SelectSingleNode("NameValueCollection").RemoveAll();

                    XmlNode objNodeTemplate = _xmlData.CreateNode(XmlNodeType.Element, "Item", "");
                    objNodeTemplate.Attributes.Append(_xmlData.CreateAttribute("Name"));
                    objNodeTemplate.Attributes.Append(_xmlData.CreateAttribute("Type"));
                    objNodeTemplate.Attributes.Append(_xmlData.CreateAttribute("Value"));

                    foreach (System.Collections.DictionaryEntry obj in this)
                    {


                        XmlNode objNewNode = objNodeTemplate.Clone();
                        objNewNode.Attributes["Name"].Value = obj.Key.ToString();
                        if (blnSerializeObjects)
                        {
                            General.Utilities.Serialization.SerializationTools.SerializedObjectArgs objPacket = General.Utilities.Serialization.SerializationTools.SerializeObjectForXML(obj.Value);
                            objNewNode.Attributes["Type"].Value = objPacket.Type;
                            objNewNode.Attributes["Value"].Value = objPacket.Value;
                        }
                        else
                        {
                            objNewNode.Attributes["Type"].Value = obj.Value.GetType().Name;
                            objNewNode.Attributes["Value"].Value = obj.Value.ToString();
                        }
                        _xmlData.SelectSingleNode("NameValueCollection").AppendChild(objNewNode);

                        /*XmlNode objNode = _xmlSpecs.SelectSingleNode("NameValueCollection/add[@name='" + obj.Key + "']");
                        if (objNode == null)
                        {
                            XmlNode objNewNode = _xmlSpecs.SelectSingleNode("NameValueCollection").FirstChild.Clone();
                            objNewNode.Attributes["name"].Value = obj.Key.ToString();
                            objNewNode.Attributes["value"].Value = obj.Value.ToString();
                            _xmlSpecs.SelectSingleNode("NameValueCollection").AppendChild(objNewNode);
                        }*/
                    }
                }
            }
        }
        #endregion

    }

    #region SimpleXMLNameValueCollection
    public class SimpleXMLNameValueCollection : IEnumerable, IEnumerator
    {

        #region Private Variables
        private XMLNameValueCollection obj;
        #endregion

        #region Constructors
        public SimpleXMLNameValueCollection(XmlDocument xmlData)
        {
            obj = new XMLNameValueCollection(xmlData);
        }

        public SimpleXMLNameValueCollection()
        {
            obj = new XMLNameValueCollection();
        }
        #endregion

        #region Public Properties
        public XmlDocument XMLDocument
        {
            get { return obj.XMLDocument; }
        }

        /// <summary>
        /// Gets a value using an index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get { return obj[index]; }
        }

        /// <summary>
        /// Gets or sets a value for the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get { return obj[key]; }
            set { obj[key] = value; }
        }
        #endregion

        #region IEnumerable Implementation
        private int _intIndex;
        #region IEnumerable Members
        /// <summary>
        /// Gets the Enumerator object
        /// </summary>
        /// <returns>IEnumerator</returns>
        public IEnumerator GetEnumerator() { Reset(); return (IEnumerator)this; }
        #endregion

        #region IEnumerator Members
        #region Public Properties
        /// <summary>
        /// Gets the current object
        /// </summary>
        /// <returns>object</returns>
        public object Current
        {
            get
            {
                //try { return obj[_intIndex]; }
                try { return new DictionaryEntry(obj.AllKeys[_intIndex], obj.AllValues[_intIndex]); }
                catch { return null; }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Resets the enumeration index
        /// </summary>
        public void Reset()
        {
            _intIndex = -1;
        }

        /// <summary>
        /// Moves to the next object in the enumeration
        /// </summary>
        /// <returns>bool</returns>
        public bool MoveNext()
        {
            if (_intIndex < obj.Count - 1)
            {
                _intIndex++;
                return true;
            }

            Reset();
            return false;
        }
        #endregion
        #endregion

        #endregion IEnumerable Implementation

    }
    #endregion

}
