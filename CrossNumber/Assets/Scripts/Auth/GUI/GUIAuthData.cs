using EasyH.Unity.UI;
using EasyH.Tool.LangKit;
using UnityEngine;
using TMPro;

public class GUIAuthData : GUICustomFullScreen {

    [SerializeField] private TextMeshProUGUI _defaultName;
    [SerializeField] private TextMeshProUGUI _id;
    [SerializeField] private TextMeshProUGUI _stageCnt;

    [SerializeField] private GameObject _loadingField;
    [SerializeField] private GameObject _contentField;

    public override void Open()
    {
        _loadingField?.SetActive(true);
        _contentField?.SetActive(false);
        base.Open();

        if (!GameManager.Instance.Auth.IsSignIn())
        {
            UIManager.Instance.DisplayMessage("msg_NeedSignIn");
            Close();
            return;
        }
        DisplayUserData();

    }

    public override void SetOn()
    {
        base.SetOn();
        DisplayUserData();
    }

    private void DisplayUserData()
    {

        GameManager.Instance.Auth.GetUserData(_ShowUserData, (msg) =>
        {
            if (msg.Equals("No Idx"))
            {
                _ShowUserData(new UserData());
                return;
            }

            UIManager.Instance.DisplayMessage(msg);
            Close();

        });
        
    }

    void _ShowUserData(UserData userdata)
    {
        Loading(() =>
        {
            _loadingField?.SetActive(false);
            _contentField?.SetActive(true);
            
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
