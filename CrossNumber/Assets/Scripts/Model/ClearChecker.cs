using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearChecker {

    List<Unit> _units;
    List<EqualUnit> _equalUnits;

    public ClearChecker(List<Unit> units, List<EqualUnit> equalUnits)
    {
        _units = units;
        _equalUnits = equalUnits;
    }

    public bool LevelCanClear()
    {

        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].SetStateUnCalced();
        }

        bool canClear = true;


        for (int i = 0; i < _equalUnits.Count; i++)
        {
            if (!_equalUnits[i].Check())
            {
                canClear = false;
            }

        }

        for (int i = 0; i < _units.Count; i++)
        {
            if (!_units[i].IsCalced)
                canClear = false;
        }

        return canClear;

    }

}