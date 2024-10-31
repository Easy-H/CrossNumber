using EHTool.UIKit;

public class GUIStartScene : GUICustomFullScreen
{

    public void OpenUI(string uiName) {
        UIManager.Instance.OpenGUI<GUIWindow>(uiName);
    }
}
