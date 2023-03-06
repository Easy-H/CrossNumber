using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveData {
    public Unit unit { get; private set; }

    public Vector3 beforeMovePos { get; private set; }
    public Vector3 afterMovePos { get; private set; }

    public MoveData(Unit u, Vector3 origin, Vector3 moved) {
        unit = u;
        beforeMovePos = origin;
        afterMovePos = moved;
    }

}

public class MoveStack {

    public List<MoveData> _moveData;
    int _moveIdx;

    public MoveStack() { 
        _moveData = new List<MoveData>();
        _moveIdx = 0;

    }

    public void AddMoveData(Unit unitData, Vector3 originPos, Vector3 resultPos)
    {
        if (_moveIdx < _moveData.Count)
            _moveData.RemoveRange(_moveIdx, _moveData.Count - _moveIdx);

        _moveData.Add(new MoveData(unitData, originPos, resultPos));
        _moveIdx++;
    }

    public void Pop()
    {
        if (_moveIdx < 1)
            return;

        Unit unit = _moveData[--_moveIdx].unit;
        unit.SetPos(_moveData[_moveIdx].beforeMovePos, out bool isChanged);

    }

    public void Back()
    {
        if (_moveIdx > _moveData.Count - 1)
            return;

        Unit unit = _moveData[_moveIdx].unit;

        unit.SetPos(_moveData[_moveIdx].beforeMovePos, out bool isChanged);
        
    }
}