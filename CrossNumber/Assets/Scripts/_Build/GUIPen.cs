using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPen : MonoBehaviour
{
    [SerializeField] Text _normal;
    [SerializeField] GameObject _eraser;

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
