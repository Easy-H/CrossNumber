using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.Events;

public class StringManager : Singleton<StringManager> {
    // Start is called before the first frame update

    public static UnityEvent OnLangChanged = new UnityEvent();
    class StringData {
        public string name;
        public string value;

        public void Read(XmlNode node)
        {
            name = node.Attributes["name"].Value;
            value = node.Attributes["value"].Value;
        }
    }

    Dictionary<string, StringData> _dic;

    string _nowLang = "Kor";

    protected override void OnCreate()
    {
        ReadStringFromXml(_nowLang);
    }

    public void ReadStringFromXml(string lang)
    {
        _dic = new Dictionary<string, StringData>();
        XmlDocument xmlDoc = AssetOpener.ReadXML(lang + "/String");

        XmlNodeList nodes = xmlDoc.SelectNodes("StringData/String");

        for (int i = 0; i < nodes.Count; i++)
        {
            StringData stringData = new StringData();
            stringData.Read(nodes[i]);

            _dic.Add(stringData.name, stringData);
        }

    }

    public void ChangeLang(string lang) {
        ReadStringFromXml(lang);
        OnLangChanged.Invoke();
    }

    public string GetStringByKey(string key) {

        if (_dic.TryGetValue(key, out StringData temp)) {
            return temp.value;
        }
        return string.Empty;

    }
}
