using UnityEngine;

public class OpenWeb : MonoBehaviour
{
    public void OpenURL(string url) { 
        Application.OpenURL(url);
    }
}
