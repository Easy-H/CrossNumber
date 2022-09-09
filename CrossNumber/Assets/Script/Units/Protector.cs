using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protector : MonoBehaviour
{
    [SerializeField] int[] _carefulLayer = null;
    [SerializeField] protected int _careful;

    protected virtual void Start()
    {
        _careful = 0;
        for (int i = 0; i < _carefulLayer.Length; i++) {
            _careful = _careful | (1 << _carefulLayer[i]);
        }
    }

    public void Clear() {
        gameObject.SetActive(false);
    }

    public virtual void Set() {
        if (Unit.ObjectCheck(transform.position, _careful)) {
            gameObject.SetActive(false);
            if (_careful == 0)
                Debug.Log(Unit.ObjectCheck(transform.position, _careful).collider.gameObject);
            return;
        }

        gameObject.SetActive(true);
    }

}
