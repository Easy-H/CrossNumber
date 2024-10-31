using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBlock : MonoBehaviour, IMoveable
{
    [SerializeField] Vector2Int _start;
    [SerializeField] Vector2Int _end;

    public Vector2Int Pos { get; set; }

    public bool CanPlace(int x, int y)
    {
        return false;
    }

    public void SetPosition(int x, int y)
    {
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        for (int i = _start.x; i <= _end.x; i++) {
            for (int j = _start.y; j <= _end.y; j++) {
                GameManager.Instance.Playground.SetMoveableAt(this, i, j);
            }
        }
    }
}
