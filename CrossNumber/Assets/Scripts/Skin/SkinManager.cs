using EHTool;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager: Singleton<SkinManager>, IObservable<SkinData> {

    private readonly ISet<IObserver<SkinData>> _observers = 
        new HashSet<IObserver<SkinData>>();

    private IDictionary<string, SkinData> _dic;

    public string NowSkin { get; private set; }

    protected override void OnCreate()
    {
        IDictionaryConnector<string, string> dictionaryConnector
            = new JsonDictionaryConnector<string, string>();
            
        IDictionary<string, string> data = dictionaryConnector.ReadData("SkinInfor");

        _dic = new Dictionary<string, SkinData>();

        foreach (var d in data)
        {
            _dic.Add(d.Key, AssetOpener.Import<SkinData>(d.Value));
        }

        IdxSet();
    }

    public IDisposable Subscribe(IObserver<SkinData> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);

            observer.OnNext(GetNowSkinData());
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
