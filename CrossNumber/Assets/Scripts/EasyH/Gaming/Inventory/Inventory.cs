using UnityEngine;
using System.Collections.Generic;
using System;

namespace EasyH.Gaming.Inventory
{
    public class Inventory : MonoBehaviour, IObservable<Inventory>
    {

        public IList<string> ItemList { get; private set; }
            = new List<string>();

        [SerializeField] private int _budget = 5;

        private ISet<IObserver<Inventory>> _observers
            = new HashSet<IObserver<Inventory>>();

        public IDisposable Subscribe(IObserver<Inventory> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);

                observer.OnNext(this);
            }

            return new Unsubscriber<Inventory>(_observers, observer);
        }


        public bool IsFull()
        {
            return ItemList.Count >= _budget;
        }

        public void AddItem(string newItem)
        {
            if (IsFull()) return;
            ItemList.Add(newItem);
            Notify();
        }

        public void RemoveItem(int idx)
        {
            ItemList.RemoveAt(idx);
            Notify();
        }

        private void Notify()
        {
            foreach (var obs in _observers)
            {
                obs.OnNext(this);
            }
        }

    }
}