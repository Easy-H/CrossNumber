using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour, IMoveable {
    public Vector2Int Pos { get; set; }

    void OnEnable()
    {
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);

        GameManager.Instance.Playground.SetMoveableAt(this, x, y);
        Pos = new Vector2Int(x, y);
    }

    public virtual bool CanPlace(int x, int y)
    {
        return GameManager.Instance.Playground.GetMoveableAt(x, y) == null;
    }

    public virtual void SetPosition(int x, int y)
    {
        if (!CanPlace(x, y)) return;

        _Setposition(x, y);
    }
    protected void _Setposition(int x, int y)
    {
        GameManager.Instance.Playground.SetMoveableAt(null, Pos.x, Pos.y);
        GameManager.Instance.Playground.SetMoveableAt(this, x, y);

        transform.position = new Vector2(x, y);
        Pos = new Vector2Int(x, y);
    }
}
