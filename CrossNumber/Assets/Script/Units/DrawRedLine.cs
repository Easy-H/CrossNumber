using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRedLine : MonoBehaviour
{
    [SerializeField] Transform coverImage = null;
    [SerializeField] float playTime = .2f;

    // 계산 결과가 틀리면 가운데에 줄을 그음
    public void DrawLine(Vector3 pos, Vector3 direct, float size) {

        pos += Vector3.forward;

        if ((transform.localScale - new Vector3(size, 1, 1)).magnitude < 0.1f) {
            return;
        }
        
        StopAllCoroutines();
        StartCoroutine(Wipe());
        EqualUnit.playErrorSound = true;

        transform.position = pos;
        transform.right = direct;
        transform.localScale = new Vector3(size, 1, 1);
    }

    public void EraseLine() {
        transform.localScale = Vector3.zero;
    }

    // 가운데에 줄을 긋는 애니메이션 코루틴
    IEnumerator Wipe()
    {
        float time = 0;
        float y = coverImage.localScale.y;

        coverImage.localScale = new Vector3(1, y);
        gameObject.layer = 9;

        while (time < playTime) {
            yield return new WaitForEndOfFrame();
            coverImage.localScale = new Vector3(1 - time / playTime, y);
            time += Time.deltaTime;
        }
        coverImage.localScale = new Vector3(0, y);
        gameObject.layer = 8;
    }
}
