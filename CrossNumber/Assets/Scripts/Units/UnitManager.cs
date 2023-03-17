using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class UnitManager : MonoSingleton<UnitManager> {

    string _path = "Assets/Prefabs/Units/";

    public UnitType SelectedUnitType { get; set; }

    public int unCalcedUnitCount { get; set; }

    public bool playErrorSound { get; set; }
    bool _canClear;

    protected override void OnCreate()
    {
        base.OnCreate();

    }

    public Unit CreateUnit(string name, Vector3 pos) {
        
        Unit created;

        if (name.Equals("="))
        {
            created = AssetOpener.Import<EqualUnit>(_path + "EqualUnit.prefab");
            created.Value = name;
            created.transform.position = pos;

            return created;

        }

        created = AssetOpener.Import<Unit>(_path + "Unit.prefab");
        created.Value = name;

        if (int.TryParse(name, out _))
        {
            created.transform.position = pos;
            return created;
        }

        created.transform.position = pos;

        return created;

    }


    public bool CanClear() {

        if (_canClear)
        {

            return true;

        }
        return false;
    }

}
