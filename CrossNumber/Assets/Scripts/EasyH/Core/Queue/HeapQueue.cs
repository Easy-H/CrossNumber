using System;
using System.Collections;
using System.Collections.Generic;

namespace EasyH
{
    public class HeapQueue<T> : IQueue<T>
    {

        private readonly IList<T> _list;

        public int Count => _list.Count;
        private Func<T, T, int> _comparator;

        public HeapQueue()
        {
            _list = new List<T>();
        }

        public HeapQueue(Func<T, T, int> comparator)
        {
            _list = new List<T>();
            SetComparator(comparator);
        }

        public void SetComparator(Func<T, T, int> comparator)
        {
            _comparator = comparator;
        }

        public void Enqueue(T item)
        {
            _list.Add(item);

            SiftUp(_list.Count - 1);
        }

        private void SiftUp(int index)
        {
            int childIndex = index;
            
            while (childIndex > 0)
            {
                int parentIndex = (childIndex - 1) / 2;

                if (_comparator(_list[parentIndex],
                    _list[childIndex]) <= 0)
                    break;
                
                (_list[parentIndex], _list[childIndex])
                    = (_list[childIndex], _list[parentIndex]);
                
                childIndex = parentIndex;
            }
        }

        public T Dequeue()
        {
            if (_list.Count == 0) return default;
            
            T retval = _list[0];

            int lastIndex = _list.Count - 1;
            _list[0] = _list[lastIndex];
            _list.RemoveAt(lastIndex); 

            if (_list.Count > 0)
                SiftDown(0);

            return retval;
        }
        
        private void SiftDown(int index)
        {
            int parentIndex = index;
            int count = _list.Count;

            while (true)
            {
                int leftChildIndex = 2 * parentIndex + 1;
                int rightChildIndex = 2 * parentIndex + 2;
                int swapIndex = parentIndex;
                
                if (leftChildIndex < count &&
                    _comparator(_list[leftChildIndex], _list[swapIndex]) < 0)
                {
                    swapIndex = leftChildIndex;
                }

                if (rightChildIndex < count &&
                    _comparator(_list[rightChildIndex], _list[swapIndex]) < 0)
                {
                    swapIndex = rightChildIndex;
                }

                if (swapIndex == parentIndex)
                    break;

                (_list[parentIndex], _list[swapIndex])
                    = (_list[swapIndex], _list[parentIndex]);

                parentIndex = swapIndex;
            }
        }

        private void Heapify()
        {
            for (int i = (_list.Count / 2) - 1; i >= 0; i--)
            {
                SiftDown(i);
            }
        }
        
        public void Remove(T item)
        {
            if (!_list.Contains(item)) return;

            _list.Remove(item);
            
            if (_list.Count > 0)
                Heapify();
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