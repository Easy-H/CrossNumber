using EHTool.UIKit;

public class GUIOverworldSelect : GUICustomFullScreen
{
    private void OpenOverworld(IOverworld overworld) {

        UIManager.Instance.OpenGUI<GUIOverWorld>
            ("Overworld").SetOverworld(overworld);
    }

    public void OpenOnlineOverworld() {
        OpenOverworld(StageManager.Instance.
            GetOverworld("Cloud"));
    }
    
    public void OpenLocalOverworld() {
        OpenOverworld(StageManager.Instance.
            GetOverworld("Local"));
    }

}
