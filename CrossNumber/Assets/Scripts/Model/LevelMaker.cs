using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Diagnostics;
using static UnityEngine.UI.CanvasScaler;

[System.Serializable]
public class LevelMaker : MonoBehaviour
{
    [SerializeField] Unit _unit;
    [SerializeField] EqualUnit _equalUnit;
    [SerializeField] OverUnit _overUnit;

    public void MakeLevel(StageData stage)
    {
        for (int i = 0; i < stage.units.Length; i++)
        {
            UnitInfor data = stage.units[i];
            CreateUnit(data.type, data.pos);
        }

    }

    private void CreateUnit(string value, Vector3 pos) {

        Unit unit;

        if (value[0] == '^') {
            unit = Instantiate(_overUnit);
        }
        else if (value.Equals("="))
        {
            unit = Instantiate(_equalUnit);
        }
        else {
            unit = Instantiate(_unit);
        }

        unit.SetValue(value, Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

    }
}
