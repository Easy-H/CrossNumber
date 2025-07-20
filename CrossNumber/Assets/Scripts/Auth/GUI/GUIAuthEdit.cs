using EHTool.UIKit;
using UnityEngine;
using TMPro;
using System;

public class GUIAuthEdit : GUICustomFullScreen {

    private Action _callback;

    [SerializeField] private TMP_InputField _newName;
    [SerializeField] private TMP_InputField _newPW;
    [SerializeField] private TextMeshProUGUI _defaultName;

    public override void Open()
    {
        base.Open();

        if (!GameManager.Instance.Auth.IsSignIn())
        {
            UIManager.Instance.DisplayMessage("msg_NeedSignIn");
            Close();
            return;
        }

        _defaultName.text = GameManager.Instance.Auth.GetName();
        
    }
    
    public void Setting()
    {

        if (_newName.text.CompareTo(_defaultName.text) != 0 && _newName.text.CompareTo("") != 0)
        {
            //_loading.LoadingOn("msg_InChangeName");
            GameManager.Instance.Auth.DisplayNameChange(_newName.text, () =>
            {
                UIManager.Instance.DisplayMessage("msg_Applied");
                _defaultName.text = GameManager.Instance.Auth.GetName();
                _newName.text = "";
                _callback?.Invoke();
                //_loading.LoadingOff();
            },
            (string msg) =>
            {
                UIManager.Instance.DisplayMessage(msg);
                //_loading.LoadingOff();

            });
        }

        if (_newPW.text.CompareTo("") == 0) return;

        //_loading.LoadingOn("msg_InChangePW");

        GameManager.Instance.Auth.PasswordChange(_newPW.text, () =>
        {
            UIManager.Instance.DisplayMessage("msg_Applied");
            _newPW.text = string.Empty;
            _callback?.Invoke();
            //_loading.LoadingOff();
        },
        (string msg) =>
        {
            UIManager.Instance.DisplayMessage(msg);
            //_loading.LoadingOff();

        });

    }

    public void Delete() {


        string[] btnName = { "btn_Proceed", "btn_Cancle" };

        Action[] callback = new Action[2]{ () => {

            //_loading.LoadingOn("msg_InDeleteAuth");

            GameManager.Instance.Auth.DeleteUser(() =>
            {
                //_loading.LoadingOff();
                UIManager.Instance.DisplayMessage("msg_DeleteAuth");
                Close();
            },
            (string msg) =>
            {
                //_loading.LoadingOff();
                UIManager.Instance.DisplayMessage(msg);
            });

        },
        () => {
            return;
        } };

        /*
        UIManager.Instance.OpenGUI<GUIChoose>("DoubleChoose")
            .Set("label_DeleteAuth", "msg_ProceedDeleteAuth", btnName, callback);
        */
    }

}
