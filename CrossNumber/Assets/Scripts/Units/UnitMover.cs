using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitMover {

    Transform _target;
    Vector3 startPos;

    CustomStack<MoveData> _moves = new CustomStack<MoveData>();

    public void StartMove(Transform target)
    {
        _target = target;
        startPos = target.position;
    }

    public void UnitMoveTo(Vector3 pos)
    {

        if (Physics2D.Raycast(pos, Vector2.down, 0.1f))
            return;

        UnitManager.Instance.GetUnitDataAt(_target.position).Pos = pos;
        _target.position = pos;

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