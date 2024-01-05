using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json.Linq;

public class GUIOverWorldButton : MonoBehaviour {

    [SerializeField] TextMeshProUGUI _btnName = null;
    [SerializeField] float _mspace = 1;
    
    string _value = string.Empty;

    class Waiting : ICallback<LevelData> {
        public void Fail()
        {
        }

        public void Success(LevelData data)
        {
            UIManager.OpenGUI<GUIPlayScene>("Play").SetStage(data);
        }
    }

    public void SetButtonInfor(string name, string value) {
        _btnName.text = string.Format("<mspace=\"{0}\">{1}</mspace>", (_mspace), name.Replace(" ", ""));
        _value = value;
    }

    public void BtnClickEvent() {
        FirebaseManager.Instance.GetLevelData(_value, new Waiting());
    }
    
}
