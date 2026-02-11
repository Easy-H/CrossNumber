using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace EasyH {

    public class XMLDictionaryConnector<K, V> : IDictionaryConnector<K, V> {

        class XMLKeyValue {
            public K key;
            public V value;

            public void Read(XmlNode node, string keyAttributeName, string valueAttributeName)
            {
                key = (K)Convert.ChangeType(node.Attributes[keyAttributeName].Value, typeof(K));
                value = (V)Convert.ChangeType(node.Attributes[valueAttributeName].Value, typeof(V));
            }
        }

        private string _keyAttributeName;
        private string _valueAttributeName;
        private string _listName;
        private string _nodeName;

        public XMLDictionaryConnector()
        {
            _keyAttributeName = "key";
            _valueAttributeName = "value";
            _listName = "List";
            _nodeName = "Element";
        }

        public XMLDictionaryConnector(string keyAttributeName, string valueAttributeName, string listName, string nodeName) {
            
            _keyAttributeName = keyAttributeName;
            _valueAttributeName = valueAttributeName;
            _listName = listName;
            _nodeName = nodeName;
        }

        public string GetDefaultPath() => "XML";
        public string GetExtensionName() => ".xml";

        public IDictionary<K, V> ReadData(string path)
        {
            string xmlStr = FileManager.Instance.FileConnector.Read(
                string.Format("{0}/{1}", GetDefaultPath(),path));

            IDictionary<K, V> retval = new Dictionary<K, V>();

            if (xmlStr == null) return retval;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);

            if (xmlDoc == null) return retval;

            XmlNodeList nodes = xmlDoc.SelectNodes(
                string.Format("{0}/{1}", _listName, _nodeName));

            for (int i = 0; i < nodes.Count; i++)
            {
                XMLKeyValue xmlData = new XMLKeyValue();
                xmlData.Read(nodes[i], _keyAttributeName, _valueAttributeName);

                retval.Add(xmlData.key, xmlData.value);
            }

            return retval;

        }

        public void Save(IDictionary<K, V> data, string path)
        {
#if !UNITY_EDITOR
            return;
#endif

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode root = xmlDoc.CreateElement("List");
            xmlDoc.AppendChild(root);

            //참고: https://blog.naver.com/kmc7468/220660088517
            foreach (KeyValuePair<K, V> obj in data)
            {
                XmlNode node = xmlDoc.CreateElement("Element");

                XmlAttribute key = xmlDoc.CreateAttribute("key");
                key.Value = obj.Key.ToString();
                XmlAttribute value = xmlDoc.CreateAttribute("value");
                value.Value = obj.Value.ToString();

                node.Attributes.SetNamedItem(key);
                node.Attributes.SetNamedItem(value);
                root.AppendChild(node);
            }

            if (!Directory.Exists("Assets/Resources/XML"))
            {
                Directory.CreateDirectory("Assets/Resources/XML");
            }

            xmlDoc.Save(string.Format("Assets/Resources/XML/{0}.xml", path));
        }
    }
}