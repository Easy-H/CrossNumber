using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveData {

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

    public Vector3 GetMovedPos() {
        return movedPos;
    }
}

[System.Serializable]
public class MoveDataManager
{
    static List<MoveData> moves = null;
    static int movesCount = 0;

    public static void WhenNewSceneLoaded() {
        moves = new List<MoveData>();
        movesCount = 0;
    }

    // 뒤로가기 기능
    public static void GetBack()
    {
        if (movesCount < 1)
            return;
        Unit unit = moves[--movesCount].GetObject().GetComponent<Unit>();

        unit.Pick();
        unit.Hold(moves[movesCount].GetOriginPos());
        unit.Place();
        
    }

    public static void Foward()
    {
        if (movesCount > moves.Count - 1)
            return;
        Unit unit = moves[movesCount].GetObject().GetComponent<Unit>();

        unit.Pick();
        unit.Hold(moves[movesCount++].GetMovedPos());
        unit.Place();

    }

    public static void AddData(GameObject unitData, Vector3 originPos, Vector3 resultPos) {
        if (movesCount < moves.Count)
            moves.RemoveRange(movesCount, moves.Count - movesCount);

        moves.Add(new MoveData(unitData, originPos, resultPos));
        movesCount++;
    }


}
