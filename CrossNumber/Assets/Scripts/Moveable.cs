using UnityEngine;

public class Moveable : MonoBehaviour, IMoveable
{

    public Vector2Int Pos { get; set; }

    void OnEnable()
    {
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);

        Pos = new Vector2Int(x, y);
        SetMoveable(this);
    }

    public virtual bool CanPlace(int x, int y)
    {
        return GameManager.Instance.Playground.GetMoveableAt(x, y) == null;
    }

    public virtual void SetPosition(int x, int y)
    {
        if (!CanPlace(x, y)) return;

        SetMoveable(null);

        transform.position = new Vector2(x, y);
        Pos = new Vector2Int(x, y);

        SetMoveable(this);
    }

    protected void SetMoveable(IMoveable moveable)
    {
        GameManager.Instance.Playground.SetMoveableAt
            (moveable, Pos.x, Pos.y);

    }

    public void Remove()
    {
        Destroy(gameObject);
    }

}
