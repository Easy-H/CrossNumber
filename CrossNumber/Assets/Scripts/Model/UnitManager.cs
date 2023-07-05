using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class UnitManager : Singleton<UnitManager> {

    List<Unit> _units;
    List<EqualUnit> _equalUnits;

    protected override void OnCreate()
    {
        base.OnCreate();

        _units = new List<Unit>();
        _equalUnits = new List<EqualUnit>();

    }

    public void DestroyAllUnit()
    {
        for (int i = 0; i < _equalUnits.Count; i++)
        {
            _equalUnits[i].Destroy();
        }

        _units = new List<Unit>();
        _equalUnits = new List<EqualUnit>();
    }

    public List<Unit> GetAllUnit()
    {
        return _units;
    }

    public List<EqualUnit> GetAllEqualUnit()
    {
        return _equalUnits;
    }

    public UnitController CreateUnitController(Unit unitData)
    {
        UnitController cntl = AssetOpener.ImportGameObject("Prefabs/Unit").GetComponent<UnitController>();
        cntl.SetValue(unitData);

        return cntl;
    }

    public Unit CreateUnit(string value, Vector3 pos)
    {

        Unit unitData;

        if (value.Equals("="))
        {
            unitData = new EqualUnit(pos);

            _equalUnits.Add((EqualUnit)unitData);
            _units.Add(unitData);
            return unitData;

        }

        unitData = new Unit(value, pos);
        _units.Add(unitData);

        return unitData;

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
        _units.Remove(obj.GetData());
        Object.Destroy(obj);
    }

}