using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour, IMoveable, IUnit {

    public static readonly int PlaceUnitLayer = 5;
    public static readonly int AllUnitLayer = 0;

    [SerializeField] protected TextMeshProUGUI _txt;
    [SerializeField] GameObject _underline = null;

    public virtual string Value { get; private set; }
    public bool IsCalced { get; protected set; }

    public Vector2Int Pos { get; set; }

    public virtual void SetStateCalced()
    {
        IsCalced = true;
    }

    public virtual void SetStateUnCalced()
    {
        IsCalced = false;
    }

    public virtual void SetPosition(int x, int y)
    {
        if (!CanPlace(x, y)) return;

        _SetPosition(x, y);
    }

    protected void _SetPosition(int x, int y, Unit defaultUnit = null) {

        GameManager.Instance.Playground.SetMoveableAt(defaultUnit, Pos.x, Pos.y);
        GameManager.Instance.Playground.SetDataAt(defaultUnit, Pos.x, Pos.y);

        GameManager.Instance.Playground.SetMoveableAt(this, x, y);
        GameManager.Instance.Playground.SetDataAt(this, x, y);

        transform.position = new Vector2(x, y);
        Pos = new Vector2Int(x, y);
    }

    public virtual bool CanPlace(int x, int y) {
        return GameManager.Instance.Playground.GetMoveableAt(x, y) == null;
    }

    public virtual void SetValue(string value, int x, int y)
    {

        Value = value;
        IsCalced = true;

        if (Value.Equals("/"))
            _txt.text = "÷";
        else if (Value.Equals("*"))
            _txt.text = "x";
        else
            _txt.text = Value;

        transform.position = new Vector2(x, y);
        Pos = new Vector2Int(x, y);

        GameManager.Instance.Playground.AddUnit(this);
        GameManager.Instance.Playground.
            SetMoveableAt(this, x, y);
        GameManager.Instance.Playground.SetDataAt(this, x, y);
    }

    public void Remove()
    {
        GameManager.Instance.Playground.SetMoveableAt(null, Pos.x, Pos.y);
        GameManager.Instance.Playground.SetDataAt(null, Pos.x, Pos.y);
        Destroy(gameObject);
    }

    private void Update()
    {
        _underline.SetActive(!IsCalced);
    }

}
