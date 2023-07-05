using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveData {
    public Transform unit { get; private set; }

    public Vector3 beforeMovePos { get; private set; }
    public Vector3 afterMovePos { get; private set; }

    public MoveData(Transform u, Vector3 origin, Vector3 moved)
    {
        unit = u;
        beforeMovePos = origin;
        afterMovePos = moved;
    }

}
