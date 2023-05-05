using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomStack<T> {

    public List<T> _moveData;
    int _moveIdx;

    public CustomStack() { 
        _moveData = new List<T>();
        _moveIdx = 0;

    }

    public void AddData(T data)
    {
        if (_moveIdx < _moveData.Count)
            _moveData.RemoveRange(_moveIdx, _moveData.Count - _moveIdx);

        _moveData.Add(data);
        _moveIdx++;
    }

    public T Pop()
    {
        if (_moveIdx < 1)
            return default;

        return _moveData[--_moveIdx];

    }

    public T Back()
    {
        if (_moveIdx > _moveData.Count - 1)
            return default;

        return _moveData[_moveIdx++];
        
    }
}