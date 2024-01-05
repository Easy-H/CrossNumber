using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearChecker {

    public bool LevelCanClear()
    {

        for (int i = 0; i < UnitManager.Instance.Units.Count; i++)
        {
            UnitManager.Instance.Units[i].SetStateUnCalced();
        }

        bool canClear = true;


        for (int i = 0; i < UnitManager.Instance.EqualUnits.Count; i++)
        {
            if (!UnitManager.Instance.EqualUnits[i].Check())
            {
                canClear = false;
            }

        }

        for (int i = 0; i < UnitManager.Instance.Units.Count; i++)
        {
            if (!UnitManager.Instance.Units[i].IsCalced)
                canClear = false;
        }

        return canClear;

    }

}