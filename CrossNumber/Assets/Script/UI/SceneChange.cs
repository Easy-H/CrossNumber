using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] RawImage img;
    [SerializeField] float aniTime;

    static Texture2D texture;

    RectTransform tr;

    // Start is called before the first frame update
    void Start()
    {
        if (texture) {
            img.texture = texture;
            img.color = Color.white;
        }

        tr = gameObject.GetComponent<RectTransform>();

        StartCoroutine(Animation());
    }

    public IEnumerator Animation()
    {
        float spendTime = 0;
        float angle;
        while (spendTime < aniTime)
        {
            angle = Mathf.Lerp(0, 1, spendTime / aniTime);
            tr.anchorMin -= Vector2.one * Time.deltaTime;
            tr.anchorMax -= Vector2.one * Time.deltaTime;
            tr.eulerAngles = Vector3.forward * 45 * angle;
            spendTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        tr.anchorMax = new Vector2(Mathf.Cos(90 * Mathf.Deg2Rad), 1);
        tr.eulerAngles += Vector3.up * 90;
    }

    public IEnumerator CaptureScreen()
    {
        texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame();

        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        texture.Apply();
    }

}
