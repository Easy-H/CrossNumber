using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class UnitManager : MonoSingleton<UnitManager> {

    string _path = "Assets/Prefabs/Units/";

    protected override void OnCreate()
    {
        base.OnCreate();

    }

    public Unit CreateUnit(string value, Vector3 pos) {
        
        Unit created;

        if (value.Equals("="))
        {
            created = AssetOpener.Import<EqualUnit>(_path + "EqualUnit.prefab");
            created.SetValue(value);
            created.transform.position = pos;

            return created;

        }

        created = AssetOpener.Import<Unit>(_path + "Unit.prefab");
        created.SetValue(value);

        if (int.TryParse(value, out _))
        {
            created.transform.position = pos;
            return created;
        }

        created.transform.position = pos;

        return created;

    }

    public Unit CreateBuildUnit(string value, Vector3 pos) {

        Unit created = AssetOpener.Import<Unit>(_path + "BuildUnit.prefab");

        created.SetValue(value);
        created.transform.position = pos;

        return created;
    }

    public static T ObjectCheck<T>(Vector3 pos)
    {
        T unit = default;

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f);

        if (hit)
            unit = hit.transform.GetComponent<T>();

        return unit;
    }

    public static UnitData GetUnitDataAt(Vector3 pos)
    {
        UnitData unit = default;

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f, 5);

        if (hit)
            unit = hit.transform.GetComponent<Unit>().GetData();

        return unit;
    }

    public static Unit GetUnitControllerAt(Vector3 pos)
    {
        Unit unit = default;

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f);

        if (hit)
            unit = hit.transform.GetComponent<Unit>();

        return unit;
    }

}
