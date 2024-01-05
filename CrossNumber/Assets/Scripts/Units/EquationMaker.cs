using UnityEngine;

public class EquationMaker {
    public int ContainCharCount { get; private set; }

    private string _value;
    private bool _lastIsNum;

    public EquationMaker()
    {
        _value = "";
        ContainCharCount = 0;
        _lastIsNum = false;
    }

    public EquationMaker(string s)
    {
        _value = s;
        ContainCharCount = 0;
        _lastIsNum = false;
    }

    // 필드에 있는 유닛을 문자열 수식으로 만든다.
    public string MakeEquation(Vector3 pos, Vector3 dir, bool back)
    {
        _value = "";

        for (Unit unit = UnitManager.Instance.GetUnitDataAt(pos + dir); unit != null; unit = UnitManager.Instance.GetUnitDataAt(pos + dir))
        {
            string value = unit.Value;
            if (value == null)
                break;

            unit.IsCalced = true;
            _AddValue(value, back);

            pos += dir;
        }

        _value = _value.Trim();

        return _value;
    }

    void _AddValue(string str, bool addback)
    {
        bool isNum = int.TryParse(str.Substring(0, 1), out int i);

        if (isNum != _lastIsNum)
        {
            if (addback)
                _value = str + " " + _value;
            else
                _value = _value + " " + str;
        }
        else
        {
            if (addback)
                _value = str + _value;
            else
                _value = _value + str;
        }
        _lastIsNum = isNum;
        ContainCharCount++;

    }

}