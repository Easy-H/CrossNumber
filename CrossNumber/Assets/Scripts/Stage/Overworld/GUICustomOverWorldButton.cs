using EasyH.Unity.UI;

public class GUICustomOverWorldButton : GUIOverWorldButton {

    public override void BtnClickEvent()
    {
        UIManager.Instance.OpenGUI<GUICustomStageDataPopUp>
            ("CustomStageData").SetTarget(_stageMetaData, _parentGUI);

    }
    
}
