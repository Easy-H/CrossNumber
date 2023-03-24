using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class UnitManager : MonoSingleton<UnitManager> {

    string _path = "Assets/Prefabs/Units/";

    public int unCalcedUnitCount { get; set; }

    public bool playErrorSound { get; set; }
    bool _canClear;

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

}
