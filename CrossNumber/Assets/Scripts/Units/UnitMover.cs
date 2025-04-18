using UnityEngine;

public class UnitMover {

    private IMoveable _target;
    private Vector2Int _startPos;
    private Vector2Int _beforePos;

    private CustomStack<MoveData> _moves
        = new CustomStack<MoveData>();

    public void StartMove(IMoveable target)
    {
        _target = target;
        _startPos = target.Pos;
        _beforePos = _startPos;
    }

    public void UnitMoveTo(Vector2Int pos)
    {
        if ((_beforePos - pos).sqrMagnitude < 0.1f) return;
        _beforePos = pos;
        
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
        _moves?.Push(new MoveData(_target, _startPos, _target.Pos));
        _target = null;
    }


}