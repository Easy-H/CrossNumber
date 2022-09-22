using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NeedColor {
    Background,     Unit,       RedLine,        UiPanel,    UiBoard,    UiBoardLine
};

[System.Serializable]
public class SkinDataUnit {

    [SerializeField] private Color _background = Color.white;
    [SerializeField] private Color _unit = Color.black;
    [SerializeField] private Color _redLine = Color.white;
    [SerializeField] private Color _uiPanel = Color.black;
    [SerializeField] private Color _uiBoard = Color.white;
    [SerializeField] private Color _uiBoardLine = Color.white;

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
            case NeedColor.UiBoardLine:
                color = _uiBoardLine;
                break;
        }
        return color;

    }

}

public class SkinData : MonoBehaviour {

    [SerializeField] SkinDataUnit[] _skinSet = null;

    public int Idx { get; set; }
    bool _idxSet = false;
    
    public SkinDataUnit Skin {
        get {
            if (!_idxSet)
                IdxSet();

            //_idx = 1;

            return _skinSet[Idx];
        }
    }

    void IdxSet() {

        if (!PlayerPrefs.HasKey("Skin")) {
            PlayerPrefs.SetInt("Skin", 0);
        }

        Idx = PlayerPrefs.GetInt("Skin");

    }

    public void SetSkinIdx(int idx) {

        Idx = idx;
        PlayerPrefs.SetInt("Skin", Idx);

        SkinSetter[] setters = FindObjectsOfType<SkinSetter>();

        for (int i = 0; i < setters.Length; i++) {
            setters[i].Setting();
        }

    }

}
