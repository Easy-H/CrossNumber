using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawRedLine : MonoBehaviour
{
    [SerializeField] Image _animImage = null;
    [SerializeField] float _playTime = .2f;

    public void EraseLine() {
        transform.localScale = Vector3.zero;

    }

    public void DrawLine(Vector3 pos, Vector3 direct, float size) {

        pos += Vector3.forward * 2;

        if ((transform.position - pos).magnitude < 0.1f &&
            (transform.localScale - new Vector3(size, 1, 1)).magnitude < 0.1f) {
            return;
        }

        transform.position = pos;
        transform.right = direct;
        transform.localScale = new Vector3(size, 1, 1);

        StopAllCoroutines();
        StartCoroutine(_DrawLineAction());

    }

    IEnumerator _DrawLineAction()
    {
        float time = 0;

        _animImage.fillAmount = 0;

        while (time < _playTime) {
            yield return new WaitForEndOfFrame();
            _animImage.fillAmount = Mathf.Lerp(0, 1, time / _playTime);
            time += Time.deltaTime;
        }
        _animImage.fillAmount = 1;

    }
}
