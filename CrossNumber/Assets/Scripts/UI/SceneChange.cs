using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] RawImage _capturedImage = null;
    [SerializeField] float _playTimeSecond = 2f;

    RectTransform _trImg;

    static Texture2D _texture;

    // Start is called before the first frame update

    public void Show()
    {
        _texture = null;
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(_Show());
    }

    IEnumerator _Show()
    {
        StartCoroutine(CaptureScreen());
        yield return null;
        gameObject.SetActive(true);
        StartCoroutine(Animation());
    }

    public void Capture()
    {
        StartCoroutine(CaptureScreen());

    }

    public IEnumerator Animation() {

        _trImg = _capturedImage.GetComponent<RectTransform>();

        if (_texture) {
            _capturedImage.texture = _texture;
            _capturedImage.color = Color.white;
        }

        _trImg.anchorMin = Vector2.zero;
        _trImg.anchorMax = Vector2.one;

        float spendTime = 0;

        while (spendTime < _playTimeSecond)
        {
            float angle = Mathf.Lerp(0, 1, spendTime / _playTimeSecond);

            _trImg.anchorMin -= Vector2.one * Time.deltaTime;
            _trImg.anchorMax -= Vector2.one * Time.deltaTime;
            _trImg.eulerAngles = Vector3.forward * 45 * angle;

            spendTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }

    public IEnumerator CaptureScreen()
    {
        _capturedImage.color = Color.clear;
        _texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        yield return new WaitForEndOfFrame();

        _texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        _texture.Apply();
    }

}
