using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;

public enum NeedColor {
    Background,     Unit,       RedLine,        UiPanel,    UiBoard,    UiBoardLine
};

public class SkinData {

    public Color back;
    public Color unit;
    public Color red;
    public Color panel;
    public Color board;
    public Color line;

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


public class SkinChangedEvent : UnityEvent<SkinData> { }

public class SkinManager: MonoSingleton<SkinManager> {

    public static SkinChangedEvent OnSkinChanged = new SkinChangedEvent();

    public class SkinDataReader {
        internal string name;
        internal SkinData data;

        public void Read(XmlNode node)
        {
            data = new SkinData();
            name = node.Attributes["name"].Value;
            data.back = XmlToColor(node.SelectSingleNode("Background"));
            data.unit = XmlToColor(node.SelectSingleNode("Unit"));
            data.red = XmlToColor(node.SelectSingleNode("Redline"));
            data.panel = XmlToColor(node.SelectSingleNode("UIPanel"));
            data.board = XmlToColor(node.SelectSingleNode("UIBoard"));
            data.line = XmlToColor(node.SelectSingleNode("UIBoardLine"));

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

    }
    Dictionary<string, SkinData> _dic;

    public string nowSkin;
    protected override void OnCreate()
    {
        _dic = new Dictionary<string, SkinData>();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("Assets/XML/Skin.xml");

        XmlNodeList nodes = xmlDoc.SelectNodes("SkinData/Skin");

        for (int i = 0; i < nodes.Count; i++)
        {
            SkinDataReader skinData = new SkinDataReader();
            skinData.Read(nodes[i]);

            _dic.Add(skinData.name, skinData.data);
        }

        IdxSet();
    }

    void IdxSet() {

        if (!PlayerPrefs.HasKey("Skin")) {
            PlayerPrefs.SetString("Skin", "Basic");
        }

        nowSkin = PlayerPrefs.GetString("Skin");

    }

    public SkinData GetNowSkinData() {
        return _dic[nowSkin];
    }

    public void ChangeSkin(string skinType)
    {

        nowSkin = skinType;
        SkinData skin = _dic[nowSkin];

        OnSkinChanged.Invoke(skin);

        PlayerPrefs.SetString("Skin", nowSkin);

    }

}
