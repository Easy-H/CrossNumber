using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkinSetter : MonoBehaviour
{
    [SerializeField] NeedColor _skinNeed = NeedColor.Unit;
    [SerializeField] Graphic _source;

    private void Start()
    {
        OnSkinChanged(SkinManager.Instance.GetNowSkinData());
    }
    private void OnEnable()
    {
        SkinManager.OnSkinChanged.AddListener(OnSkinChanged);
    }

    private void OnDisable()
    {
        SkinManager.OnSkinChanged.RemoveListener(OnSkinChanged);
    }

    private void OnSkinChanged(SkinData skinType)
    {
        _source.color = skinType.Get(_skinNeed);
    }

    private void OnDestroy()
    {
        SkinManager.OnSkinChanged.RemoveListener(OnSkinChanged);
    }

}
