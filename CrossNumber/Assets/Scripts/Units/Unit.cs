using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit {

    public string Value { get; set; }
    public bool IsCalced { get; set; }

    public Vector3 Pos { get; set; }

    public Unit()
    {
        Value = "1";
        IsCalced = false;
    }

    public Unit(string value, Vector3 pos = default)
    {
        Value = value;
        IsCalced = true;
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
}