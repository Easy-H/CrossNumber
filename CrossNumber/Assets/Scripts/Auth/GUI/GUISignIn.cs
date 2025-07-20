using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EHTool.UIKit;
using UnityEngine.UI;
using TMPro;

public class GUISignIn : GUICustomFullScreen
{
    [SerializeField] private TMP_InputField _id;
    [SerializeField] private TMP_InputField _pw;

    bool _inProcess = false;

    protected override void Update()
    {
        base.Update();
        
        if (!GameManager.Instance.Auth.IsSignIn()) return;

        Close();
    }

    public void SignIn() {

        if (_inProcess) return;

        _inProcess = true;

        //_loading.LoadingOn("msg_InTrySignIn");

        GameManager.Instance.Auth.TrySignIn(_id.text, _pw.text, () => {
            
            UIManager.Instance.DisplayMessage("msg_SuccessSignIn");

            //_loading.LoadingOff();
            _inProcess = false;

            Close();

        }, (msg) => {
            //_loading.LoadingOff();
            _inProcess = false;
            UIManager.Instance.DisplayMessage(msg);
        });

    }

}