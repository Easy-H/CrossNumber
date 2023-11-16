using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqualUnitController : UnitController
{

    [SerializeField] Transform[] _error = new Transform[4];
    [SerializeField] DrawRedLine[] _calcResultError = new DrawRedLine[2];

    public override void SetValue(string value, Vector3 pos)
    {
        _data = new EqualUnit(pos);
        transform.position = pos;
    }

}
