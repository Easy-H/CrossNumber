using UnityEngine;

public class UnitMover {

    private IMoveable _target;
    private Vector2Int _startPos;
    private Vector2Int _beforePos;

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

    public IUnitActionData MoveEnd()
    {
        if (_target == null) return null;
        if (_startPos.Equals(_target.Pos)) return null;

        return new UnitMoveData(_startPos, _target.Pos);
    }

}