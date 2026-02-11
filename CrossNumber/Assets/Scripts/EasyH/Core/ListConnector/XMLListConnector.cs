using System.Collections.Generic;
using System.Xml;
using System;

namespace EasyH
{
    public class XMLListConnector<T> : IListConnector<T>
    {
        class XMLValue {
            public T value;

            public void Read(XmlNode node)
            {
                value = (T)Convert.ChangeType(
                    node.Attributes["value"].Value, typeof(T));
            }
        }

        public string GetDefaultPath() => "XML";
        public string GetExtensionName() => ".xml";

        public IList<T> ReadData(string path)
        {
            
            string xmlStr = FileManager.Instance.FileConnector.Read(
                string.Format("{0}/{1}", GetDefaultPath(), path));

            IList<T> retval = new List<T>();

            if (xmlStr == null) return retval;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);


            if (xmlDoc == null) return retval;

            XmlNodeList nodes = xmlDoc.SelectNodes("List/Element");

            for (int i = 0; i < nodes.Count; i++)
            {
                XMLValue xmlData = new XMLValue();
                xmlData.Read(nodes[i]);

                retval.Add(xmlData.value);
            }


            return retval;
        }
        
    }
}