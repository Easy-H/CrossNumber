using System;
using System.Collections.Generic;

namespace EasyH {
    public class Unsubscriber<T> : IDisposable {
        private readonly ISet<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;

        public Unsubscriber(ISet<IObserver<T>> observers, IObserver<T> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}