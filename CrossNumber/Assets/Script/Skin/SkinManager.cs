using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public enum NeedColor {
    Background,     Unit,       RedLine,        UiPanel,    UiBoard,    UiBoardLine
};


public class SkinManager: MonoSingleton<SkinManager> {
    class SkinData {
        public string name;

        Color back;
        Color unit;
        Color red;
        Color panel;
        Color board;
        Color line;

        public void Read(XmlNode node)
        {
            name = node.Attributes["name"].Value;
            back = XmlToColor(node.SelectSingleNode("Background"));
            unit = XmlToColor(node.SelectSingleNode("Unit"));
            red = XmlToColor(node.SelectSingleNode("Redline"));
            panel = XmlToColor(node.SelectSingleNode("UIPanel"));
            board = XmlToColor(node.SelectSingleNode("UIBoard"));
            line = XmlToColor(node.SelectSingleNode("UIBoardLine"));

        }

        Color XmlToColor(XmlNode node)
        {
            float r = float.Parse(node.Attributes["red"].Value) / 255;
            float g = float.Parse(node.Attributes["green"].Value) / 255;
            float b = float.Parse(node.Attributes["blue"].Value) / 255;
            float a = float.Parse(node.Attributes["alpha"].Value) / 255;
            Color color = new Color(r, g, b, a);
            return color;
        }

        public Color Get(NeedColor type)
        {
            switch (type)
            {
                case NeedColor.Background:
                    return back;
                case NeedColor.Unit:
                    return unit;
                case NeedColor.RedLine:
                    return red;
                case NeedColor.UiPanel:
                    return panel;
                case NeedColor.UiBoard:
                    return board;
                case NeedColor.UiBoardLine:
                    return line;
                default:
                    return Color.white;
            }
        }
    }

    Dictionary<string, SkinData> _dic;

    bool _idxSet = false;

    public string nowSkin;
    protected override void OnCreate()
    {
        _dic = new Dictionary<string, SkinData>();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("Assets/KOG/XML/Skin.xml");

        XmlNodeList nodes = xmlDoc.SelectNodes("SkinData/Skin");

        for (int i = 0; i < nodes.Count; i++)
        {
            SkinData skinData = new SkinData();
            skinData.Read(nodes[i]);

            _dic.Add(skinData.name, skinData);
        }

        IdxSet();
    }

    void IdxSet() {

        if (!PlayerPrefs.HasKey("Skin")) {
            PlayerPrefs.SetString("Skin", "Basic");
            Debug.Log("??");
        }

        Debug.Log("Why");
        nowSkin = PlayerPrefs.GetString("Skin");

    }

    public Color GetSkinColor(NeedColor need) {
        SkinData skin = _dic[nowSkin];
        return skin.Get(need);
    }

    public void SetSkinIdx(string key) {

        nowSkin = key;
        PlayerPrefs.SetString("Skin", nowSkin);

        SkinSetter[] setters = FindObjectsOfType<SkinSetter>();

        for (int i = 0; i < setters.Length; i++) {
            setters[i].Setting();
        }

    }

}
