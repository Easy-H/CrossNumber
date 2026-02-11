using EasyH.Unity.UI;
using UnityEngine;

public class GUIStart : GUICustomFullScreen
{

    [SerializeField] private string _authSettingWindowKey = "AuthSetting";
    [SerializeField] private string _signInWindowKey = "SignIn";

    public void OpenLocalOverworld() {
        UIManager.Instance.OpenGUI<GUIOverWorld>("Overworld").
            SetOverworld(StageManager.Instance.GetOverworld("Local"));
        
    }

    public void OpenOverworld()
    {
        UIManager.Instance.OpenGUI<GUIOverWorld>("Overworld").
            SetOverworld(StageManager.Instance.GetOverworld("Cloud"));
    }

    public void OpenCustomOverworld()
    { 
        UIManager.Instance.OpenGUI<GUIOverWorld>("CustomOverworld").
            SetOverworld(StageManager.Instance.GetOverworld("Custom"));
    }

    public void OpenAuth()
    {
        if (GameManager.Instance.Auth.IsSignIn())
        {
            OpenWindow(_authSettingWindowKey);
            return;
        }
        OpenWindow(_signInWindowKey);
    }

}