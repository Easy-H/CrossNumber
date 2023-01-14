using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.XR;

public class StringManager : MonoSingleton<StringManager> {
    // Start is called before the first frame update

    class StringData {
        public string name;
        public string value;

        public void Read(XmlNode node)
        {
            name = node.SelectSingleNode("name").InnerText;
            value = node.SelectSingleNode("value").InnerText.Trim();
        }
    }

    Dictionary<string, StringData> _dic;
    public List<NewText> texts = new List<NewText>();

    string _nowLang = "Kor";
    string _fileName = "String.xml";

    protected override void OnCreate()
    {
        _dic = new Dictionary<string, StringData>();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("Assets/KOG/XML/" + _nowLang + "/" + _fileName);

        XmlNodeList nodes = xmlDoc.SelectNodes("StringData/String");

        for (int i = 0; i < nodes.Count; i++)
        {
            StringData stringData = new StringData();
            stringData.Read(nodes[i]);

            _dic.Add(stringData.name, stringData);
        }
    }

    public string GetStringByKey(string key)
    {

        if (_dic.TryGetValue(key, out StringData temp))
        {
            return temp.value;
        }
        return string.Empty;

    }

    public void LangSet(string lang) {
        _nowLang = lang;
        OnCreate();
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].SetText();
        }
    }
}
