using EHTool.UIKit;
using EHTool.LangKit;
using UnityEngine;
using TMPro;

public class GUIAuthData : GUICustomFullScreen {

    [SerializeField] private TextMeshProUGUI _defaultName;
    [SerializeField] private TextMeshProUGUI _id;
    [SerializeField] private TextMeshProUGUI _stageCnt;

    public override void Open()
    {
        base.Open();

        if (!GameManager.Instance.Auth.IsSignIn())
        {
            UIManager.Instance.DisplayMessage("msg_NeedSignIn");
            Close();
            return;
        }

        GameManager.Instance.Auth.GetUserData(_ShowUserData, (msg) =>
        {
            if (msg.Equals("No Idx"))
            {
                _ShowUserData(new UserData());
                return;
            }

            UIManager.Instance.DisplayMessage(msg, Close);
            
        });
        
    }

    void _ShowUserData(UserData userdata)
    {
        Loading(() =>
        {
            _id.text = GameManager.Instance.Auth.GetUserId();
            _defaultName.text = string.Format("{0}",
                GameManager.Instance.Auth.GetName());
                
            _stageCnt.text = userdata.stageCnt.ToString();

        });
        
    }

    public void SignOut()
    {
        GameManager.Instance.Auth.SignOut();
        Close();
    }

}
