using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSetter : MonoBehaviour {

    [SerializeField] NeedColor _skinNeed = NeedColor.Unit;

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
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = skinType.Get(_skinNeed);
    }
    private void OnDestroy()
    {
        SkinManager.OnSkinChanged.RemoveListener(OnSkinChanged);
    }

}
