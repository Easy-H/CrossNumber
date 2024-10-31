using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour {

    [SerializeField] Canvas _canvas;
    [SerializeField] RectTransform _trImg;
    [SerializeField] RawImage _capturedImage = null;
    [SerializeField] float _playTimeSecond = 2f;
    [SerializeField] string _audio;

    CallbackMethod _callback;

    // Start is called before the first frame update
    static IList<SceneChange> list = new List<SceneChange>();

    public void Show(Texture2D texture, CallbackMethod callback = null)
    {

        foreach (SceneChange sc in list) {
            sc._canvas.sortingOrder += 1;
        }

        list.Add(this);

        _callback += callback;
        if (texture)
        {
            _capturedImage.texture = texture;
            _capturedImage.color = Color.white;
        }

        _trImg.anchorMin = Vector2.zero;
        _trImg.anchorMax = Vector2.one;
        _trImg.eulerAngles = Vector3.zero;

        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(Animation());
        SoundManager.Instance.PlayAudio(_audio);
    }

    private IEnumerator Animation() {

        float spendTime = 0;

        while (spendTime < _playTimeSecond)
        {
            float angle = Mathf.Lerp(0, 1, spendTime / _playTimeSecond);

            _trImg.anchorMin -= Vector2.one * Time.deltaTime;
            _trImg.anchorMax -= Vector2.one * Time.deltaTime;
            _trImg.eulerAngles = 23 * angle * Vector3.forward;

            spendTime += Time.deltaTime;
            yield return null;
        }

        list.Remove(this);
        _callback?.Invoke();
        Destroy(gameObject);

    }

}