using System.Text.RegularExpressions;
using UnityEngine;

public class OverUnit : Unit {
    Unit _overedUnit;

    [SerializeField] string _defaultValue = null;

    public override string Value {
        get {
            if (_overedUnit == null) return "";
            return _overedUnit.Value + " " + _defaultValue;
        }
    }

    public override void SetValue(string value, int x, int y)
    {
        base.SetValue(value, x, y);
        _defaultValue = value;
        _txt.text = Regex.Replace(_defaultValue, @"[^0-9]", "");
    }

    public override void SetStateUnCalced() {

        base.SetStateUnCalced();
        _overedUnit?.SetStateCalced();

    }

    // 만약 계산된다면 _overedUnit도 함께 계산된 것으로 처리한다.
    public override void SetStateCalced()
    {
        base.SetStateCalced();
        _overedUnit?.SetStateCalced();

    }

    public override void SetPosition(int x, int y)
    {
        Unit newOver = GameManager.Instance.Playground.GetDataAt(x, y);

        _SetPosition(x, y, _overedUnit);

        _overedUnit = newOver;
    }

    // 유닛이 있는지, 있다면 겹칠 수 있는 유닛인지 확인한다.
    public override bool CanPlace(int x, int y) {
        return true;
    }

}