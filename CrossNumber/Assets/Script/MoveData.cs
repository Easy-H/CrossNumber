using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDataUnit {
    public Unit unit { get; private set; }

    public Vector3 beforeMovePos { get; private set; }
    public Vector3 afterMovePos { get; private set; }

    public MoveDataUnit(Unit u, Vector3 origin, Vector3 moved) {
        unit = u;
        beforeMovePos = origin;
        afterMovePos = moved;
    }

}

[System.Serializable]
public class MoveData {
    List<MoveDataUnit> _data;

    static MoveData _instance;
    public static MoveData Instance {
        get {
            if (_instance == null)
                _instance = new MoveData();
            return _instance;
        }
    }

    int _idx;

    public void WhenNewSceneLoaded() {
        _data = new List<MoveDataUnit>();
    }

    public void AddData(Unit unitData, Vector3 originPos, Vector3 resultPos) {
        if (_idx < _data.Count)
            _data.RemoveRange(_idx, _data.Count - _idx);

        _data.Add(new MoveDataUnit(unitData, originPos, resultPos));
        _idx++;
    }


    // 뒤로가기 기능
    public void Undo() {
        if (_idx < 1)
            return;
        Unit unit = _data[--_idx].unit;

        unit.Pick();
        unit.Hold(_data[_idx].beforeMovePos);
        unit.Place(false);

    }

    public void Redo() {
        if (_idx > _data.Count - 1)
            return;
        Unit unit = _data[_idx].unit;

        unit.Pick();
        unit.Hold(_data[_idx++].afterMovePos);
        unit.Place(false);


    }


}
