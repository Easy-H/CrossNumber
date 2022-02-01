using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MoveData
{
    [SerializeField] GameObject unit;

    [SerializeField] Vector3 originPos;
    [SerializeField] Vector3 movedPos;

    public MoveData(GameObject u, Vector3 origin, Vector3 moved) {
        unit = u;
        originPos = origin;
        movedPos = moved;
    }

    public GameObject GetObject() {
        return unit;
    }

    public Vector3 GetOriginPos() {
        return originPos;
    }

    public Vector3 GetMovedPos()
    {
        return movedPos;
    }

}
