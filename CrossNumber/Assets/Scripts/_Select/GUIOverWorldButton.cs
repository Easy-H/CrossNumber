using UnityEngine;
using TMPro;
using EHTool.UIKit;

public class GUIOverWorldButton : MonoBehaviour {

    [SerializeField] TextMeshProUGUI _btnName = null;
    [SerializeField] float _mspace = 1;
    
    string _value = string.Empty;

    public void SetButtonInfor(string name, string value) {
        _btnName.text = string.Format("<mspace=\"{0}\">{1}</mspace>", _mspace, name.Replace(" ", ""));
        _value = value;
    }

    public void BtnClickEvent() {
        GUIFullScreen loading = UIManager.Instance.OpenGUI<GUIFullScreen>("Loading");

        StageManager.Instance.GetWorldStageData(_value, (StageData data) => {
            loading.Close();
            OpenLevel(data);

        });
    }

    void OpenLevel(StageData data) {
        UIManager.Instance.OpenGUI<GUIPlayScene>("Play").SetStage(data);
    }
    
}
