using EHTool.UIKit;

public class GUIStart : GUICustomFullScreen
{
    public void OpenLocalOverworld() {
        UIManager.Instance.
            OpenGUI<GUIOverWorld>("Overworld").SetOverworld
                (StageManager.Instance.GetLocalOverworld());
        
    }

    public void OpenOverworld() {
        UIManager.Instance.
            OpenGUI<GUIOverWorld>("Overworld").SetOverworld
                (StageManager.Instance.GetFirestoreOverworld());
    }

}