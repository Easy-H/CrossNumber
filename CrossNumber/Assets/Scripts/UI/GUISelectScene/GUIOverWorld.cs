using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GUIOverWorld : GUICustomFullScreen, ICallback<Dictionary<string, object>> {

    [SerializeField] GUIOverWorldButton[] _buttons = null;

    [SerializeField] Transform _buttonContainer = null;

    [SerializeField] Canvas _btnCanvas;

    protected override void Open()
    {
        base.Open();
        _btnCanvas.worldCamera = Camera.main;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        FirebaseManager.Instance.GetRandomStage(this);
    }


    void ICallback<Dictionary<string, object>>.Success(Dictionary<string, object> data)
    {

        _buttonContainer.position = Vector3.zero;

        int i = 0;

        foreach (object d in data.Values)
        {
            Dictionary<string, object> dd = d as Dictionary<string, object>;
            _buttons[i].gameObject.SetActive(true);
            _buttons[i++].SetButtonInfor(dd["name"].ToString(), dd["key"].ToString());

        }

        for (; i < _buttons.Length; i++) {
            _buttons[i].gameObject.SetActive(false); ;
            
        }

    }

    void ICallback<Dictionary<string, object>>.Fail()
    {
        throw new System.NotImplementedException();
    }
}
