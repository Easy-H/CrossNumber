using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkinSetter : MonoBehaviour, IObserver<SkinData> {

#nullable enable
    private IDisposable? _cancellation;

    [SerializeField] NeedColor _skinNeed = NeedColor.Unit;

    [SerializeField] Graphic? _source;


    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(SkinData value)
    {
        if (_source == null) return;
        _source.color = value.Get(_skinNeed);
    }

    private void OnEnable()
    {
        _cancellation = SkinManager.Instance.Subscribe(this);
    }

    private void OnDisable()
    {
        _cancellation?.Dispose();
    }

    private void OnDestroy()
    {
        _cancellation?.Dispose();
    }
}
