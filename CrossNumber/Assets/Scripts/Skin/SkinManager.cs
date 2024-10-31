using EHTool;
using EHTool.LangKit;
using System;
using System.Collections.Generic;
using System.Linq;
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

public interface ISkinSetter : IObserver<SkinData> {
    public void SkinChange(SkinData data);

}

public class SkinManager: Singleton<SkinManager>, IObservable<SkinData> {

    private readonly ISet<IObserver<SkinData>> _observers = 
        new HashSet<IObserver<SkinData>>();

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

    public string NowSkin { get; private set; }

    protected override void OnCreate()
    {
        _dic = new Dictionary<string, SkinData>();
        XmlDocument xmlDoc = AssetOpener.ReadXML("Skin");

        XmlNodeList nodes = xmlDoc.SelectNodes("SkinData/Skin");

        for (int i = 0; i < nodes.Count; i++)
        {
            SkinDataReader skinData = new SkinDataReader();
            skinData.Read(nodes[i]);

            _dic.Add(skinData.name, skinData.data);
        }

        IdxSet();
    }

    public IDisposable Subscribe(IObserver<SkinData> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);

            observer.OnNext(_dic[NowSkin]);
        }

        return new Unsubscriber<SkinData>(_observers, observer);
    }

    void IdxSet() {

        if (!PlayerPrefs.HasKey("Skin")) {
            PlayerPrefs.SetString("Skin", "Basic");
        }

        NowSkin = PlayerPrefs.GetString("Skin");

    }

    public SkinData GetNowSkinData() {
        return _dic[NowSkin];
    }

    public void ChangeSkin(string skinType)
    {

        NowSkin = skinType;

        _NotifyToObserver();

        PlayerPrefs.SetString("Skin", NowSkin);

    }

    private void _NotifyToObserver()
    {
        foreach (IObserver<SkinData> target in _observers)
        {
            target.OnNext(GetNowSkinData());
        }
    }

}
