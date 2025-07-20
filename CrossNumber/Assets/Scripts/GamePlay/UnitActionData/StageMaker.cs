using UnityEngine;

public class StageMaker : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private EqualUnit _equalUnit;
    [SerializeField] private OverUnit _overUnit;

    public void MakeLevel(Stage stage)
    {
        for (int i = 0; i < stage.Units.Length; i++)
        {
            UnitInfor data = stage.Units[i];
            CreateUnit(data.type, data.pos);
        }

    }

    public Unit CreateUnit(string value) {
        
        if (value[0] == '^') {
            return Instantiate(_overUnit);
        }
        else if (value.Equals("="))
        {
            return Instantiate(_equalUnit);
        }

        return Instantiate(_unit);
    }

    public Unit CreateUnit(string value, Vector2Int pos) {

        Unit unit = CreateUnit(value);
        unit.SetValue(value, pos.x, pos.y);

        return unit;
    }
    
}
