using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

namespace EHTool.LangKit {

    public class XMLLangPackReader : ILangPackReader {

        class StringData : XMLNodeReader {
            public string key;
            public string value;

            public void Read(XmlNode node)
            {
                key = node.Attributes["key"].Value;
                value = node.Attributes["value"].Value;
            }
        }

        IDictionary<string, string> _dic;
        string _path;

        public XMLLangPackReader() {
            _dic = new Dictionary<string, string>();
        }

        public void AddKey(string key)
        {
            if (key != null && !_dic.ContainsKey(key))
            {
                _dic[key] = null;
            }

            if (Directory.Exists("Assets/Resources/XML/String"))
            {
                DirectoryInfo di = new DirectoryInfo("Assets/Resources/XML/String");
                foreach (FileInfo File in di.GetFiles())
                {
                    if (File.Extension.ToLower().CompareTo(".json") != 0) continue;

                    string name = File.Name.Substring(0, File.Name.Length - 4);

                    if (name.CompareTo(_path) == 0) continue;

                    ReadData(name);
                    Save(name);
                }
            }
            ReadData(_path);
            Save(_path);
        }

        public void ConverFromDictionary(IDictionary<string, string> dict)
        {
            _dic = dict;
        }

        public void ConvertType(ILangPackReader target)
        {
            if (Directory.Exists("Assets/Resources/XML/String"))
            {
                DirectoryInfo di = new DirectoryInfo("Assets/Resources/XML/String");
                foreach (FileInfo File in di.GetFiles())
                {
                    if (File.Extension.ToLower().CompareTo(".xml") != 0) continue;

                    string name = File.Name.Substring(0, File.Name.Length - 4);

                    if (name.CompareTo(_path) == 0) continue;

                    ReadData(name);
                    target.ConverFromDictionary(_dic);
                    target.Save(name);
                }
            }
            ReadData(_path);
            target.ConverFromDictionary(_dic);
            target.Save(_path);
        }

        public string GetStringByKey(string key)
        {
            if (_dic.TryGetValue(key, out string value))
            {
                return value;
            }
            return null;
        }

        public void ReadData(string path)
        {
            _path = path;
            XmlDocument xmlDoc = AssetOpener.ReadXML("String/" + path);

            if (xmlDoc == null) return;

            XmlNodeList nodes = xmlDoc.SelectNodes("List/Element");

            for (int i = 0; i < nodes.Count; i++)
            {
                StringData stringData = new StringData();
                stringData.Read(nodes[i]);

                _dic[stringData.key] = stringData.value;
            }

        }

        public void Save(string path) {

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode root = xmlDoc.CreateElement("List");
            xmlDoc.AppendChild(root);

            //ÃâÃ³: https://blog.naver.com/kmc7468/220660088517
            foreach (KeyValuePair<string, string> obj in _dic)
            {
                XmlNode node = xmlDoc.CreateElement("Element");

                XmlAttribute key = xmlDoc.CreateAttribute("key");
                key.Value = obj.Key;
                XmlAttribute value = xmlDoc.CreateAttribute("value");
                value.Value = obj.Value;

                node.Attributes.SetNamedItem(key);
                node.Attributes.SetNamedItem(value);
                root.AppendChild(node);
            }

            if (!Directory.Exists("Assets/Resources/XML/Tmp"))
            {
                Directory.CreateDirectory("Assets/Resources/XML/Tmp");
            }

            xmlDoc.Save(string.Format("Assets/Resources/XML/Tmp/{0}-1.xml", path));
        }
    }
}