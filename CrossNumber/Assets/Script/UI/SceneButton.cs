﻿using System.Collections;
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
            txt.color = SkinManager.Instance.GetSkinColor(NeedColor.Unit);
        }
        else
        {
            txt.color = SkinManager.Instance.GetSkinColor(NeedColor.RedLine);

        }

    }
    
    public void GoScene() {
        UIManager.Instance.GetComponent<ButtonMethod>().GoScene(_thisLevel + _addLevel);
    }
    
}
