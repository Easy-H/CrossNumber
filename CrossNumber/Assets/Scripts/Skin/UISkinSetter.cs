using System;
using UnityEngine;
using UnityEngine.UI;

public class UISkinSetter : MonoBehaviour, IObserver<SkinData> {

#nullable enable
    private IDisposable? _cancellation;

    [SerializeField] private SkinData.Type _skinNeed = SkinData.Type.Unit;

    [SerializeField] private Graphic? _source;

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
