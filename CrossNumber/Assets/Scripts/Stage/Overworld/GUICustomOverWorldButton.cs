using EHTool.UIKit;

public class GUICustomOverWorldButton : GUIOverWorldButton {

    public override void BtnClickEvent()
    {
        UIManager.Instance.OpenGUI<GUICustomStageDataPopUp>
            ("CustomStageData").SetTarget(_stageMetaData);

    }
    
}
