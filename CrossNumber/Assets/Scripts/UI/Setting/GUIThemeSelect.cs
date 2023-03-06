using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIThemeSelect : MonoBehaviour
{

    public void ChangeTheme(string theme) { 
        SkinManager.Instance.ChangeSkin(theme);
    }

}
