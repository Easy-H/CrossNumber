using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIStartScene : GUICustomFullScreen
{

    public void OpenUI(string uiName) {
        UIManager.OpenGUI<GUIWindow>(uiName);
    }
}
