using UnityEngine;
using UnityEngine.UI;
using EasyH.Unity.UI;
using System;

public class GUIBuildSavePopUp : GUICustomPopUp
{
    [SerializeField] private InputField _name;

    private Action<string> _saveCallback;

    public void SetDefaultName(string defaultName, Action<string> saveCallback = null)
    {
        _name.text = defaultName;
        _saveCallback = saveCallback;
    }

    public void Save()
    {
        StageManager.Instance.AddCustomStage(_name.text, GetStage(),
            () =>
            {
                UIManager.Instance.DisplayMessage("msg_SuccessSave");
                _saveCallback?.Invoke(_name.text);
                Close();
            }, (msg) =>
            {
                UIManager.Instance.DisplayMessage(msg);
            });
    }
    
    private Stage GetStage()
    { 
        Unit[] units = FindObjectsByType<Unit>(FindObjectsSortMode.None);
        UnitInfor[] unitInfors = new UnitInfor[units.Length];

        for (int i = 0; i < units.Length; i++)
        {
            unitInfors[i] = new UnitInfor
                (units[i].Value, units[i].Pos);
        }

        return new Stage(unitInfors);
        
    }

}