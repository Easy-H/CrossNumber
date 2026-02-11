using System;
using System.Collections.Generic;


namespace EasyH {
    public interface IQueue<T> : IEnumerable<T> {

        public int Count { get; }

        public void Enqueue(T item);

        public T Dequeue();

        public void Remove(T item);
    }
}