using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GUIOverWorldButton : MonoBehaviour
{
    [SerializeField] Canvas canvas = null;

    int _value = 0;

    [SerializeField] TextMeshProUGUI txt = null;

    public void SetButtonInfor(string name, int value) {
        txt.text = "<mspace=\"50\">" + name + "</mspace>";
        _value = value;

        canvas.worldCamera = Camera.main;
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(name.Length * 50 + 50, 100);
    }

    public void GotoStage() {
        StageManager.StageIdx = _value;
        SceneManager.LoadScene(2);
    }
    
}
