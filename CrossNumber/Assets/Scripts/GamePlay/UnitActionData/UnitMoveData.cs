using UnityEngine;

[System.Serializable]
public class UnitMoveData : IUnitActionData
{

    private Vector2Int _beforePos;
    private Vector2Int _afterPos;

    public UnitMoveData(Vector2Int origin, Vector2Int moved)
    {
        _beforePos = origin;
        _afterPos = moved;
    }

    void Move(Vector2Int before, Vector2Int after)
    {
        IMoveable moveable = GameManager.Instance.Playground.
            GetMoveableAt(before.x, before.y);

        moveable.SetPosition(after.x, after.y);
        
    }

    public void Undo()
    {
        Move(_afterPos, _beforePos);
    }

    public void Redo()
    {
        Move(_beforePos, _afterPos);
    }

}