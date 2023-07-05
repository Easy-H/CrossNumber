using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIOverWorldButton : MonoBehaviour
{
    [SerializeField] Canvas canvas = null;
    [SerializeField] float temp = 1;

    int _value = 0;

    [SerializeField] TextMeshProUGUI txt = null;

    public void SetButtonInfor(string name, int value) {
        txt.text = "<mspace=\""+ (temp * 2).ToString() + "\">" + name.Replace(" ", "") + "</mspace>";
        _value = value;

        canvas.worldCamera = Camera.main;
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(name.Length * temp + temp, temp * 2);
    }

    public void GotoStage() {
        StageManager.Instance.StageIdx = _value;
        UIManager.OpenGUI<GUIPlayScene>("Play");
    }
    
}
