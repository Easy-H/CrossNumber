using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveData {
    public IMoveable unit { get; private set; }

    public Vector2Int beforeMovePos { get; private set; }
    public Vector2Int afterMovePos { get; private set; }

    public MoveData(IMoveable u, Vector2Int origin, Vector2Int moved)
    {
        unit = u;
        beforeMovePos = origin;
        afterMovePos = moved;
    }

}
