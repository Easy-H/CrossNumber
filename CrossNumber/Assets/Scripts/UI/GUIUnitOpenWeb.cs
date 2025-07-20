using UnityEngine;

public class GUIUnitOpenWeb : MonoBehaviour
{
    public void OpenURL(string url) { 
        Application.OpenURL(url);
    }
}
