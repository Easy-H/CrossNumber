using UnityEngine;

public class WorldUIButtonContainer : MonoBehaviour
{

    [SerializeField] private Canvas _btnCanvas;

    void Start()
    {
        _btnCanvas.worldCamera = Camera.main;
    }
}