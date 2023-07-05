using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMover {

    Transform _target;
    Unit _targetUnit;
    Vector3 startPos;

    CustomStack<MoveData> _moves = new CustomStack<MoveData>();

    public void StartMove(Transform target)
    {
        _target = target;
        startPos = target.position;
        _targetUnit = UnitManager.GetUnitDataAt(target.position);
    }

    public void UnitMoveTo(Vector3 pos)
    {

        if (Physics2D.Raycast(pos, Vector2.down, 0.1f))
            return;

        _target.position = pos;
        _targetUnit.Pos = pos;

        SoundManager.Instance.PlayAudio("Move");
    }

    public void Undo()
    {

        MoveData temp = _moves.Pop();
        if (temp == null) return;

        _target = temp.unit;
        UnitMoveTo(temp.beforeMovePos);
    }

    public void Redo()
    {

        MoveData temp = _moves.PopCancle();
        if (temp == null) return;

        _target = temp.unit;
        UnitMoveTo(temp.afterMovePos);
    }

    public void MoveEnd()
    {
        _moves.Push(new MoveData(_target, startPos, _target.position));
        _target = null;
    }


}