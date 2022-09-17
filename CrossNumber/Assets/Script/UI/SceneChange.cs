using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] RawImage _capturedImage = null;
    [SerializeField] float _playTimeSecond = 2f;

    static Texture2D _texture;

    RectTransform _tr;

    // Start is called before the first frame update
    void Start()
    {

        _tr = gameObject.GetComponent<RectTransform>();

        StartCoroutine(Animation());
    }

    public IEnumerator Animation() {

        if (_texture) {
            _capturedImage.texture = _texture;
            _capturedImage.color = Color.white;
        }

        _tr.anchorMin = Vector2.zero;
        _tr.anchorMax = Vector2.one;
        _tr.eulerAngles = Vector3.zero;


        float spendTime = 0;
        float angle;

        while (spendTime < _playTimeSecond)
        {
            angle = Mathf.Lerp(0, 1, spendTime / _playTimeSecond);

            _tr.anchorMin -= Vector2.one * Time.deltaTime;
            _tr.anchorMax -= Vector2.one * Time.deltaTime;
            _tr.eulerAngles = Vector3.forward * 45 * angle;

            spendTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
    }

    public IEnumerator CaptureScreen()
    {
        _texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame();

        _texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        _texture.Apply();
    }

}
