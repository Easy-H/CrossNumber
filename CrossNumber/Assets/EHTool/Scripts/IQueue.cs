using System;
using System.Collections;
using System.Collections.Generic;


namespace EHTool {
    public interface IQueue<T> : IEnumerable<T> where T : IComparable<T> {

        public int Count { get; }

        public void Enqueue(T item);

        public T Dequeue();

        public void Remove(T item);
    }
}