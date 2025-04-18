using System;
using UnityEngine;

public class SkinSetter : MonoBehaviour, IObserver<SkinData> {

#nullable enable
    private IDisposable? _cancellation;

    [SerializeField] SkinData.Type _skinNeed = SkinData.Type.Unit;


    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(SkinData value)
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = value.Get(_skinNeed);
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
