using EasyH;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitManager : Singleton<UnitManager> {

    public List<Unit> Units { get; private set; }
    public List<EqualUnit> EqualUnits { get; private set; }

    public Transform Container { get; set; }

    public Unit BuilderCreateUnit(string value, Vector3 pos)
    {

        if (EventSystem.current.IsPointerOverGameObject()) return null;

        Unit unit = AssetOpener.ImportGameObject("Prefabs/Unit").GetComponent<Unit>();

        unit.transform.SetParent(Container);
        unit.SetValue(value, Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

        return unit;
    }

    public void Refresh() {

        Units = new List<Unit>();
        EqualUnits = new List<EqualUnit>();
    }

    public Unit GetUnitControllerAt(Vector3 pos)
    {
        Unit unit = default;

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f, -1);

        if (hit)
            unit = hit.transform.GetComponent<Unit>();

        return unit;
    }

    public void DestroyUnit(Unit obj)
    {
        Units.Remove(obj);
    }

}