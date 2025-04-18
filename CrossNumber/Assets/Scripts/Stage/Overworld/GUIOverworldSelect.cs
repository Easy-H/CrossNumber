using EHTool.UIKit;

public class GUIOverworldSelect : GUICustomFullScreen
{
    private void OpenOverworld(IOverworld overworld) {

        UIManager.Instance.OpenGUI<GUIOverWorld>
            ("Overworld").SetOverworld(overworld);
    }

    public void OpenOnlineOverworld() {
        OpenOverworld(StageManager.Instance.GetFirestoreOverworld());
    }
    
    public void OpenLocalOverworld() {
        OpenOverworld(StageManager.Instance.GetLocalOverworld());
    }

}
