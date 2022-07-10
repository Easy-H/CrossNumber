using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protector : MonoBehaviour
{
    [SerializeField] int[] carefulLayer = null;
    [SerializeField] protected int careful;

    protected virtual void Start()
    {
        careful = 0;
        for (int i = 0; i < carefulLayer.Length; i++)
        {
            careful = careful | (1 << carefulLayer[i]);
        }
    }

    public void Clear() {
        gameObject.SetActive(false);
    }

    public virtual void Set() {
        if (Unit.ObjectCheck(transform.position, careful)) {
            gameObject.SetActive(false);
            if (careful == 0)
                Debug.Log(Unit.ObjectCheck(transform.position, careful).collider.gameObject);
            return;
        }

        gameObject.SetActive(true);
    }

}
