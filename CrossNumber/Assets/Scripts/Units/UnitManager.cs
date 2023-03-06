using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class UnitManager : MonoSingleton<UnitManager> {
    
    public List<Unit> _units;
    public List<EqualUnit> _equalUnits;

    string _path = "Assets/Prefabs/Units/";

    public UnitType SelectedUnitType { get; set; }

    public int unCalcedUnitCount { get; set; }

    public bool playErrorSound { get; set; }
    bool _canClear;

    protected override void OnCreate()
    {
        base.OnCreate();

        _units = new List<Unit>();
        _equalUnits = new List<EqualUnit>();
    }

    public void SetInitial()
    {
        _units.Clear();
        _equalUnits.Clear();

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

    public void SetAllUnitsStateUncalced() {

        for (int i = 0; i < _units.Count; i++) {
            _units[i].SetStateUnCalced();
        }

        unCalcedUnitCount = _units.Count;

    }

    public void CalculateWorld() {

        StartCoroutine(CalculateWorldAction());

    }

    IEnumerator CalculateWorldAction() {

        playErrorSound = false;

        yield return new WaitForFixedUpdate();

        SetAllUnitsStateUncalced();

        _canClear = true;

        for (int i = 0; i < _equalUnits.Count; i++) {

            if (!_equalUnits[i].Check()) {
                _canClear = false;
            }

        }
        if (playErrorSound) {
            SoundManager.Instance.PlayAudio("Wrong", false);
        }

        if (unCalcedUnitCount != 0) {
            _canClear = false;
        }

    }

    public bool CanClear() {

        if (_canClear)
        {

            return true;

        }
        return false;
    }

}
