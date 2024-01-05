using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using static UnityEngine.UI.CanvasScaler;

[System.Serializable]
public class LevelMaker : MonoBehaviour
{
    [SerializeField] UnitController _unit;
    [SerializeField] EqualUnitController _equalUnit;

    public void MakeLevel(LevelData stage)
    {

        for (int i = 0; i < stage.units.Length; i++)
        {
            UnitInfor data = stage.units[i];
            UnitManager.Instance.CreateUnit(data.type, data.pos);
        }

    }
}
