using EHTool.UIKit;
using UnityEngine;
using TMPro;

public class GUIAuthReVerify : GUICustomPopUp
{
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private GUILoading _loading;

    [SerializeField] private string _verifyOpenWindow;

    public override void Open()
    {
        base.Open();
        if (GameManager.Instance.Auth.IsSignIn())
        {
            return;
        }
        UIManager.Instance.DisplayMessage("msg_NeedSignIn");
        Close();

    }

    public void ReVerify()
    {

        //_loading.LoadingOn("msg_InCheckPW");

        GameManager.Instance.Auth.ReVerify(_password.text, () =>
        {
            //_loading.LoadingOff();
            OpenWindow(_verifyOpenWindow);
            Close();

        },
        (string msg) =>
        {
            //_loading.LoadingOff();
            UIManager.Instance.DisplayMessage(msg);

        });
    }

}
