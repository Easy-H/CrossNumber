using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EHTool.UIKit;

public class Legacy_GUIOverWorldButton : MonoBehaviour
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
        GUIPlay window = UIManager.Instance.OpenGUI<GUIPlay>("Play");
        window.SetStage(new StageData(_value));
    }
    
}
