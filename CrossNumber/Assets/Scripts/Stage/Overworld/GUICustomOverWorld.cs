using EasyH.Unity.UI;

public class GUICustomOverWorld : GUIOverWorld
{

    public void OpenTempBuild()
    {
        StageManager.Instance.GetStage("Custom", "Temp",
        (data) =>
        {
            UIManager.Instance.OpenGUI<GUIStageFullScreen>
                ("Build").SetStage(data, "Temp");

        }, () => { });
    }

    public override void ClosePopUp(IGUIPopUp popUp)
    {
        base.ClosePopUp(popUp);
    }
    
}