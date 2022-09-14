using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NeedColor {
    Background,     Unit,       RedLine,        UiPanel,    UiBoard
};

[System.Serializable]
public class SkinDataUnit {

    [SerializeField] private Color _background = Color.white;
    [SerializeField] private Color _unit = Color.black;
    [SerializeField] private Color _redLine = Color.white;
    [SerializeField] private Color _uiPanel = Color.black;
    [SerializeField] private Color _uiBoard = Color.white;

    public Color GetSkinColor(NeedColor need) {
        Color color = Color.black;

        switch (need) {
            case NeedColor.Background:
                color = _background;
                break;
            case NeedColor.Unit:
                color = _unit;
                break;
            case NeedColor.RedLine:
                color = _redLine;
                break;
            case NeedColor.UiPanel:
                color = _uiPanel;
                break;
            case NeedColor.UiBoard:
                color = _uiBoard;
                break;
        }
        return color;

    }

}

public class SkinData : MonoBehaviour {

    [SerializeField] SkinDataUnit[] _skinSet = null;

    int _idx;
    bool _idxSet = false;
    
    public SkinDataUnit Skin {
        get {
            if (!_idxSet)
                IdxSet();

            //_idx = 1;

            return _skinSet[_idx];
        }
    }

    void IdxSet() {

        if (!PlayerPrefs.HasKey("Skin")) {
            PlayerPrefs.SetInt("Skin", 0);
        }

        _idx = PlayerPrefs.GetInt("Skin");

    }

    public void SetSkinIdx(int idx) {

        _idx = idx;
        PlayerPrefs.SetInt("Skin", _idx);

        SkinSetter[] setters = FindObjectsOfType<SkinSetter>();

        for (int i = 0; i < setters.Length; i++) {
            setters[i].Setting();
        }

    }

}
