using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class UnitManager : Singleton<UnitManager> {


    protected override void OnCreate()
    {
        base.OnCreate();

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

    public void DestroyUnit(UnitController obj)
    {
        Object.Destroy(obj);
    }

}