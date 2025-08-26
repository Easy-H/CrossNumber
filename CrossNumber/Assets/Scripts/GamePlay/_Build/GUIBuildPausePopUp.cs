using UnityEngine;
using UnityEngine.UI;
using EHTool.UIKit;

public class GUIBuildPausePopUp : GUIStagePausePopUp
{
    public void Save()
    {
        UIManager.Instance.OpenGUI<GUIBuildSavePopUp>
            ("Save").SetDefaultName(_stageName);
    }

}