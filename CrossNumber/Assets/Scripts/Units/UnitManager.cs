using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.CanvasScaler;

public class UnitManager : Singleton<UnitManager> {

    public List<Unit> Units { get; private set; }
    public List<EqualUnit> EqualUnits { get; private set; }

    public Transform Container { get; set; }

    protected override void OnCreate()
    {
        base.OnCreate();

    }

    public UnitController BuilderCreateUnit(string value, Vector3 pos)
    {

        if (EventSystem.current.IsPointerOverGameObject()) return null;

        UnitController unit = AssetOpener.ImportGameObject("Prefabs/Unit").GetComponent<UnitController>();

        unit.transform.SetParent(Container);
        unit.SetValue(value, pos);

        Units.Add(unit.GetData());

        if (value.Equals("="))
        {
            EqualUnits.Add((EqualUnit)unit.GetData());

        }

        return unit;
    }

    public UnitController CreateUnit(string value, Vector3 pos)
    {

        UnitController unit = AssetOpener.ImportGameObject("Prefabs/Unit").GetComponent<UnitController>();

        unit.transform.SetParent(Container);
        unit.SetValue(value, pos);

        Units.Add(unit.GetData());

        if (value.Equals("="))
        {
            EqualUnits.Add((EqualUnit)unit.GetData());

        }

        return unit;
    }

    public void Refresh() {

        Units = new List<Unit>();
        EqualUnits = new List<EqualUnit>();
    }

    public Unit GetUnitDataAt(Vector3 pos)
    {
        UnitController cntl = GetUnitControllerAt(pos);

        if (cntl == null)
            return null;

        return cntl.GetData();
    }

    public UnitController GetUnitControllerAt(Vector3 pos)
    {
        UnitController unit = default;

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f, -1);

        if (hit)
            unit = hit.transform.GetComponent<UnitController>();

        return unit;
    }

    public void DestroyUnit(UnitController obj)
    {
        Units.Remove(obj.GetData());
        Object.Destroy(obj.gameObject);
    }

}