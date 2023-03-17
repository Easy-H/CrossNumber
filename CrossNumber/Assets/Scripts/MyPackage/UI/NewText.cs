using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("UI/Legacy/NewText", 100)]
public class NewText : Text
{
    // Start is called before the first frame update
    [SerializeField] protected string m_Key = String.Empty;

    protected override void Start()
    {
        base.Start();
        OnLangChanged();
    }
    override protected void OnEnable()
    {
        //SetText(m_Key);
        base.OnEnable();
        StringManager.OnLangChanged.AddListener(OnLangChanged);
    }
    override protected void OnDisable()
    {
        base.OnDisable();
        StringManager.OnLangChanged.RemoveListener(OnLangChanged);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        StringManager.OnLangChanged.RemoveListener(OnLangChanged);
    }

    public void OnLangChanged() {
        SetText(m_Key);
    }

    public void SetText(string key)
    {
        m_Key = key;

        text = StringManager.Instance.GetStringByKey(key);

        if (text == string.Empty)
            text = key;
        
    }

    
}
