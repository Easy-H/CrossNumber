using UnityEngine;
using UnityEngine.UI;

public class GUIUnitPen : MonoBehaviour
{
    [SerializeField] private Text _normal;
    [SerializeField] private GameObject _eraser;

    public void SetPen(string value) {
        if (value.Equals("Erase")) {
            _eraser.SetActive(true);
            _normal.gameObject.SetActive(false);
            return;
        }
        _eraser.SetActive(false);
        _normal.gameObject.SetActive(true);
        _normal.text = value;
    }
}
