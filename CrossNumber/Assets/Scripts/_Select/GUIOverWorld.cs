using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GUIOverWorld : GUICustomFullScreen {

    [SerializeField] GUIOverWorldButton[] _buttons = null;

    [SerializeField] Transform _buttonContainer = null;

    [SerializeField] Canvas _btnCanvas;

    public override void Open()
    {
        base.Open();

        _btnCanvas.worldCamera = Camera.main;
        StageManager.Instance.GetWorldRandomStageList(Success);
    }

    void Success(Dictionary<string, object> data)
    {

        CaptureAndEvent(() => {

            _buttonContainer.position = Vector3.zero;

            int i = 0;

            foreach (object d in data.Values)
            {
                if (i >= _buttons.Length) break;

                Dictionary<string, object> dd = d as Dictionary<string, object>;
                _buttons[i].gameObject.SetActive(true);
                _buttons[i++].SetButtonInfor(dd["name"].ToString(), dd["key"].ToString());

            }

            for (; i < _buttons.Length; i++)
            {
                _buttons[i].gameObject.SetActive(false);

            }

        });

    }
}
