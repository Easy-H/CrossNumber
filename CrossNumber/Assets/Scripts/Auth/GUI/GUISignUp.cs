using EasyH.Unity.UI;
using UnityEngine;
using TMPro;

public class GUISignUp : GUICustomFullScreen {

    [SerializeField] private TMP_InputField _id;
    [SerializeField] private TMP_InputField _pw;

    bool _inProcess = false;

    public void SignUp()
    {
        if (_inProcess) return;

        _inProcess = true;

        //_loading.LoadingOn("msg_InTrySignUp");
        GameManager.Instance.Auth.TrySignUp(_id.text, _pw.text, () => {
            //_loading.LoadingOff();
            _inProcess = false;
            UIManager.Instance.DisplayMessage("msg_SuccessSignUp");
            Close();
            //UIManager.Instance.OpenGUI<GUIAuthEdit>("AuthEdit");

        }, (msg) => {
            //_loading.LoadingOff();
            _inProcess = false;

            UIManager.Instance.DisplayMessage(msg);

        });
    }
}