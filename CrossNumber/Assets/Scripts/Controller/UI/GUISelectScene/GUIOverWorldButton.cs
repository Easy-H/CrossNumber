using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json.Linq;

public class GUIOverWorldButton : MonoBehaviour
{
    [SerializeField] Canvas canvas = null;
    [SerializeField] float temp = 1;

    [SerializeField] string _value = string.Empty;

    [SerializeField] TextMeshProUGUI txt = null;

    public void SetButtonInfor(string name, string value) {
        txt.text = "<mspace=\""+ (temp * 2).ToString() + "\">" + name.Replace(" ", "") + "</mspace>";
        _value = value;

        canvas.worldCamera = Camera.main;
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(name.Length * temp + temp, temp * 2);
    }

    public void GotoStage() {
        GUIPlayScene window = UIManager.OpenGUI<GUIPlayScene>("Play");
        window.SetStage(_value);
    }
    
}
