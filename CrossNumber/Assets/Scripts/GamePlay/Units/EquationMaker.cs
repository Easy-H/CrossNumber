using UnityEngine;

public class EquationMaker {

    private string _value;
    private bool _lastIsNum;

    public EquationMaker()
    {
        _value = "";
        _lastIsNum = false;
    }

    // 필드에 있는 유닛을 문자열 수식으로 만든다.
    public string MakeEquation(Vector2Int pos, Vector2Int dir, bool back)
    {
        _value = "";

        while (true)
        {
            pos += dir;

            Unit value = GameManager.Instance.Playground.GetDataAt(pos.x, pos.y);
            if (value == null || value.Value.Equals("=") || value.Value.Equals(""))
                break;

            _AddValue(value.Value, back);
            value.SetStateCalced();

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

    }

}