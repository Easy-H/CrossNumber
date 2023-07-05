using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearChecker {

    public ClearChecker()
    {

    }

    public bool LevelCanClear()
    {

        List<Unit> units = UnitManager.Instance.GetAllUnit();
        List<EqualUnit> equalUnits = UnitManager.Instance.GetAllEqualUnit();

        for (int i = 0; i < units.Count; i++)
        {
            units[i].SetStateUnCalced();
        }

        bool canClear = true;


        for (int i = 0; i < equalUnits.Count; i++)
        {
            if (!equalUnits[i].Check())
            {
                canClear = false;
            }

        }

        for (int i = 0; i < units.Count; i++)
        {
            if (!units[i].IsCalced)
                canClear = false;
        }

        return canClear;

    }

}