using EHTool.UIKit;
using UnityEngine;

public class GUIStart : GUICustomFullScreen
{

    [SerializeField] private string _authSettingWindowKey = "AuthSetting";
    [SerializeField] private string _signInWindowKey = "SignIn";

    public void OpenLocalOverworld() {
        UIManager.Instance.
            OpenGUI<GUIOverWorld>("Overworld").SetOverworld
                (StageManager.Instance.GetLocalOverworld());
        
    }

    public void OpenOverworld()
    {
        UIManager.Instance.
            OpenGUI<GUIOverWorld>("Overworld").SetOverworld
                (StageManager.Instance.GetCloudOverworld());
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