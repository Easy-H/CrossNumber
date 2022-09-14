using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    [SerializeField] Canvas canvas = null;

    [SerializeField] OverWorldName _overworld = OverWorldName.Beginner;
    [SerializeField] int _thisLevel = 0;
    [SerializeField] int _addLevel = 0;
    
    [SerializeField] Text txt = null;

    private void Start() {

        canvas.worldCamera = Camera.main;
        
        if (DataManager.Instance.gameData.GetOverWorld(_overworld).GetStageClear(_thisLevel)) {
            txt.color = GameManager.Instance.SkinInfor.Skin.GetSkinColor(NeedColor.Unit);
        }
        else {
            txt.color = GameManager.Instance.SkinInfor.Skin.GetSkinColor(NeedColor.RedLine);

        }

    }
    
    public void GoScene() {
        SceneManager.LoadScene(_thisLevel + _addLevel);
    }
    
}
