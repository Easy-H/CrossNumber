using UnityEngine;
using TMPro;
using EHTool.UIKit;

public class GUIOverWorldButton : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI _btnName = null;
    [SerializeField] private float _mspace = 1;
    
    private IOverworld _overworld;
    private string _key = string.Empty;

    public void SetButtonInfor(string name, string key,
        IOverworld overworld) {
        _btnName.text = string.Format("<mspace=\"{0}\">{1}</mspace>", _mspace, name.Replace(" ", ""));
        _key = key;
        _overworld = overworld;
    }

    public void BtnClickEvent() {
        GUIFullScreen loading = UIManager.Instance.
            OpenGUI<GUIFullScreen>("Loading");

        _overworld.GetStage(_key, (data) => {
            loading.Close();
            OpenLevel(data);

        }, () => {});
    }

    void OpenLevel(Stage data) {
        UIManager.Instance.OpenGUI<GUIPlay>("Play").SetStage(data);
    }
    
}
