using System.Collections.Generic;
using UnityEngine;

public class SolvePlayground : IPlayground {

    IDictionary<Vector2Int, IMoveable> _moveableMap;
    IDictionary<Vector2Int, Unit> _valueMap;

    IList<Unit> _units;
    IList<EqualUnit> _equalUnits;

    public SolvePlayground() { 
        _moveableMap = new Dictionary<Vector2Int, IMoveable>();
        _valueMap = new Dictionary<Vector2Int, Unit>();

        _units = new List<Unit>();
        _equalUnits = new List<EqualUnit>();
    }

    public IMoveable GetMoveableAt(int x, int y)
    {
        if (_moveableMap == null) return null;

        Vector2Int targetPos = new Vector2Int(x, y);

        if (!_moveableMap.ContainsKey(targetPos)) return null;

        return _moveableMap[targetPos];
    }

    public void SetMoveableAt(IMoveable target, int x, int y)
    {
        if (_moveableMap == null) return;

        Vector2Int newKey = new Vector2Int(x, y);

        if (_moveableMap.ContainsKey(newKey))
        {
            _moveableMap[newKey] = target;
            return;

        }

        _moveableMap.Add(newKey, target);

    }

    public void AddUnit(Unit target)
    {
        _units.Add(target);
    }

    public void AddEqualUnit(EqualUnit target)
    {
        _equalUnits.Add(target);
    }

    public void RemoveUnit(Unit target)
    {
        _units.Remove(target);
        target.Remove();
    }

    public void SetDataAt(Unit unit, int x, int y)
    {
        Vector2Int newKey = new Vector2Int(x, y);

        if (_valueMap.ContainsKey(newKey)) {
            _valueMap[newKey] = unit;
            return;
        }
        _valueMap.Add(newKey, unit);
    }

    public bool HasError()
    {
        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].SetStateUnCalced();
        }

        bool hasError = false;

        for (int i = 0; i < _equalUnits.Count; i++)
        {
            if (!_equalUnits[i].Check())
            {
                hasError = true;
            }

        }

        for (int i = 0; i < _units.Count; i++)
        {
            if (!_units[i].IsCalced)
                hasError = true;
        }

        return hasError;
    }

    public Unit GetDataAt(int x, int y)
    {
        Vector2Int target = new Vector2Int(x, y);

        if (_valueMap.ContainsKey(target)) {
            return _valueMap[target];
        }
        return null;
    }

    public void Dispose() {

        for (int i = 0; i < _units.Count; i++) {
            _units[i].Remove();
        }
        
        _moveableMap = new Dictionary<Vector2Int, IMoveable>();
        _valueMap = new Dictionary<Vector2Int, Unit>();

        _units.Clear();
        _equalUnits.Clear();
    }
}