using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitMover {

    IMoveable _target;
    Vector2Int startPos;

    CustomStack<MoveData> _moves = new CustomStack<MoveData>();

    public void StartMove(IMoveable target)
    {
        _target = target;
        startPos = target.Pos;
    }

    public void UnitMoveTo(Vector2Int pos)
    {
        if (!_target.CanPlace(pos.x, pos.y)) return;

        _target?.SetPosition(pos.x, pos.y);

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
        if (_target == null) return;
        _moves?.Push(new MoveData(_target, startPos, _target.Pos));
        _target = null;
    }


}