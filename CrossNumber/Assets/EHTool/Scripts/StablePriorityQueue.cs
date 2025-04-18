using EHTool.UIKit;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EHTool {
public class StablePriorityQueue<T> : IQueue<T>  where T : IComparable<T> {
    
    private readonly IList<T> _list;

    public int Count => _list.Count;

    public StablePriorityQueue() { 
        _list = new List<T>();
    }

    public void Enqueue(T item) {
        for (int i = 0; i < _list.Count; i++) {
            if (_list[i].CompareTo(item) < 0) {
                continue;
            }
            _list.Insert(i, item);
            return;
        }
        _list.Add(item);
    }
    public T Dequeue() {
        if (_list.Count < 1) return default;

        T retval = _list[0];
        _list.RemoveAt(0);
        
        return retval;
    }

    public void Remove(T item)
    {
        if (!_list.Contains(item)) return;
        _list.Remove(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _list.GetEnumerator();
    }
}
}