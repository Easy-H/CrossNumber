using UnityEngine;
using UnityEngine.UI;
using EHTool.UIKit;

public class GUIBuildSavePopUp : GUICustomPopUp
{
    [SerializeField] private InputField _name;

    public void SetDefaultName(string defaultName)
    {
        _name.text = defaultName;
    }

    public void Save()
    {
        StageManager.Instance.AddCustomStage(_name.text, GetStage(),
            () =>
            {
                UIManager.Instance.DisplayMessage("msg_SuccessSave", Close);
            }, (msg) =>
            {
                UIManager.Instance.DisplayMessage(msg);
            });
    }
    
    private Stage GetStage()
    { 
        Unit[] units = FindObjectsOfType<Unit>();
        UnitInfor[] unitInfors = new UnitInfor[units.Length];

        for (int i = 0; i < units.Length; i++)
        {
            unitInfors[i] = new UnitInfor
                (units[i].Value, units[i].Pos);
        }

        return new Stage(unitInfors);
        
    }

}