using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    [SerializeField] string overworld = "basic";
    [SerializeField] int thisLevel = 0;
    [SerializeField] int addLevel = 0;

    [SerializeField] string unClear = "red";

    [SerializeField] Text txt = null;

    private void Start()
    {
        canvas.worldCamera = Camera.main;

        DataManager.Instance.LoadGameData(overworld);
        if (!DataManager.Instance.gameData.GetStageClear(thisLevel)) {
            txt.text = "<color=" + unClear + ">" + txt.text + "</color>";
        }
    }
    
    public void GoScene()
    {
        Debug.Log(1);
        SceneManager.LoadScene(thisLevel + addLevel);
    }
    
}
