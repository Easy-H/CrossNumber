using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    [SerializeField] string _overworld = "basic";
    [SerializeField] int _thisLevel = 0;
    [SerializeField] int _addLevel = 0;

    [SerializeField] string unClear = "red";

    [SerializeField] Text txt = null;

    private void Start() {

        canvas.worldCamera = Camera.main;

        DataManager.Instance.LoadGameData(_overworld);

        if (!DataManager.Instance.gameData.GetStageClear(_thisLevel)) {
            txt.text = "<color=" + unClear + ">" + txt.text + "</color>";
        }

    }
    
    public void GoScene() {
        SceneManager.LoadScene(_thisLevel + _addLevel);
    }
    
}
