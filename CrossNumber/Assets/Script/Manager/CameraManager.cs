using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameManager gm;

    Camera[] cameras;
    
    [SerializeField] float size = 0;

    [SerializeField] Vector2 sizeLimit = Vector2.one;

    IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        
        cameras = Camera.allCameras;

        CameraSizeSet(size);

    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
            CameraSizeSet(size - Input.GetAxis("Mouse ScrollWheel") * 4);
        }

    }

    void CameraSizeSet(float si) {
        if (si > sizeLimit.y)
            si = sizeLimit.y;
        else if (si < sizeLimit.x)
            si = sizeLimit.x;
        for (int i = 0; i < cameras.Length; i++) {
            cameras[i].orthographicSize = si;
        }
        size = si;
    }

    public IEnumerator Zooming(float delta) {
        while (true) {
            gm.moving = false;
            yield return new WaitForEndOfFrame();
            CameraSizeSet(size + delta);
        }
    }

    
    public void ZoomStart(float delta) {
        gm.moving = false;
        coroutine = Zooming(delta);
        StartCoroutine(coroutine);

    }

    public void ZoomEnd() {
        StopCoroutine(coroutine);
    }

}
