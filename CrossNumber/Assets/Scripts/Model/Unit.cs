using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit {

    public string Value { get; set; }
    public bool IsCalced { get; private set; }

    public Vector3 Pos { get; set; }

    public Unit()
    {
        Value = "1";
        IsCalced = false;
    }

    public Unit(string value, Vector3 pos = default)
    {
        Value = value;
        IsCalced = false;
        Pos = pos;
    }

    public void SetStateCalced()
    {
        IsCalced = true;
    }

    public virtual void SetStateUnCalced()
    {
        IsCalced = false;
    }

    public static Unit GetUnitDataAt(Vector3 pos)
    {
        UnitController cntl = GetUnitControllerAt(pos);

        if (cntl == null)
            return null;

        return cntl.GetData();
    }

    public static UnitController GetUnitControllerAt(Vector3 pos)
    {
        UnitController unit = default;

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f);

        if (hit)
            unit = hit.transform.GetComponent<UnitController>();

        return unit;
    }
}