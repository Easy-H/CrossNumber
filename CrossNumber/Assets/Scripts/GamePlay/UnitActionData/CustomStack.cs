using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomStack<T> {

    public List<T> _elementList;
    int _idx;

    public CustomStack() { 
        _elementList = new List<T>();
        _idx = 0;

    }

    public void Push(T data)
    {
        if (_idx < _elementList.Count)
            _elementList.RemoveRange(_idx, _elementList.Count - _idx);

        _elementList.Add(data);
        _idx++;
    }

    public T Pop()
    {
        if (_idx < 1)
            return default;

        return _elementList[--_idx];

    }

    public T PopCancle()
    {
        if (_idx > _elementList.Count - 1)
            return default;

        return _elementList[_idx++];
        
    }
}