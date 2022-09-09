using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Camera[] _cameras;
    
    [SerializeField] float _size = 0;

    [SerializeField] Vector2 _sizeLimit = Vector2.one;

    IEnumerator coroutine;

    // Start is called before the first frame update
    void Start() {
        _cameras = Camera.allCameras;
        CameraSizeSet(_size);

    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
            CameraSizeSet(_size - Input.GetAxis("Mouse ScrollWheel") * 4);
        }

    }

    void CameraSizeSet(float si) {
        if (si > _sizeLimit.y)
            si = _sizeLimit.y;
        else if (si < _sizeLimit.x)
            si = _sizeLimit.x;
        for (int i = 0; i < _cameras.Length; i++) {
            _cameras[i].orthographicSize = si;
        }
        _size = si;
    }

    public IEnumerator ZoomAction(float delta) {
        while (true) {
            GameManager.instance._isMoving = false;
            yield return new WaitForEndOfFrame();
            CameraSizeSet(_size + delta);
        }
    }

    
    public void ZoomStart(float delta) {
        GameManager.instance._isMoving = false;
        coroutine = ZoomAction(delta);
        StartCoroutine(coroutine);

    }

    public void ZoomEnd() {
        StopCoroutine(coroutine);
    }

}
