using UnityEngine;

public class CameraMover {
    
    private Transform _cameraTr;
    private Transform _boardTr;

    private Vector3 _originMouseInput;

    public CameraMover(Transform cameraTr, Transform boardTr)
    {
        _cameraTr = cameraTr;
        _cameraTr.position = Vector3.back * 10;

        _boardTr = boardTr;

    }

    private Vector2Int Vector3ToVector2Int(Vector3 origin)
    {
        return new Vector2Int(
            Mathf.RoundToInt(origin.x), Mathf.RoundToInt(origin.y));
    }

    public void MoveStart(Vector3 pos)
    {
        _originMouseInput = pos;
    }

    public void Move(Vector3 pos)
    {
        _cameraTr.Translate(_originMouseInput - pos);

        Vector2Int mousePosInt = Vector3ToVector2Int(pos);
        _boardTr.position = new Vector3(mousePosInt.x, mousePosInt.y, 10);
    }
}