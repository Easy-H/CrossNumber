using EHTool.UIKit;

public class GUIStart : GUICustomFullScreen
{

    public void OpenUI(string uiName) {
        UIManager.Instance.OpenGUI<GUIWindow>(uiName);
    }
}
