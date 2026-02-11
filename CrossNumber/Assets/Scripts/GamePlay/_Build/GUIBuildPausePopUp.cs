using EasyH.Unity.UI;

public class GUIBuildPausePopUp : GUIStagePausePopUp
{

    public void Save()
    {
        UIManager.Instance.OpenGUI<GUIBuildSavePopUp>
            ("Save").SetDefaultName(_stageName, (name) =>
            {
                _stageNameUI.text = name;
            });
    }

}