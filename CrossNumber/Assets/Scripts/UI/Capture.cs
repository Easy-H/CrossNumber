using System.Collections;
using System;
using UnityEngine;

public class Capture : MonoBehaviour {

    private Action<Texture2D> _callback;

    public void CaptureScreen(Action<Texture2D> callback)
    {
        gameObject.SetActive(true);
        StartCoroutine(_CaptureScreen());
        _callback += callback;

    }

    public IEnumerator _CaptureScreen()
    {

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        yield return new WaitForEndOfFrame();

        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        texture.Apply();

        _callback?.Invoke(texture);
        _callback = null;
    }

}
