using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor.UnityLinker;
using UnityEngine;
using UnityEngine.Diagnostics;
using static UnityEngine.UI.CanvasScaler;

[System.Serializable]
public class LevelMaker : MonoBehaviour
{
    [SerializeField] Transform _parent = null;
    [SerializeField] UnitController _unit;
    [SerializeField] EqualUnitController _equalUnit;

    public List<Unit> Units { get; private set; }
    public List<EqualUnit> EqualUnits { get; private set; }

    public void MakeLevel(LevelData stage)
    {
        Units = new List<Unit>();
        EqualUnits = new List<EqualUnit>();

        for (int i = 0; i < stage.units.Length; i++)
        {
            UnitInfor data = stage.units[i];
            CreateUnit(data.type, data.pos);
        }

    }

    public void CreateUnit(string value, Vector3 pos)
    {
        if (value.Equals("="))
        {
            EqualUnitController equalUnit = Instantiate(_equalUnit);

            equalUnit.transform.SetParent(_parent);
            equalUnit.SetValue(value, pos);

            Units.Add(equalUnit.GetData());
            EqualUnits.Add((EqualUnit)equalUnit.GetData());

            return;
        }

        UnitController unit = Instantiate(_unit);

        unit.transform.SetParent(_parent);
        unit.SetValue(value, pos);

        Units.Add(unit.GetData());
    }
}
