using System;
using System.Collections.Generic;
using UnityEngine;

namespace EHTool.LangKit {

    public interface IEHText : IObserver<IEHLangManager> {
        public void SetText(string key);

    }

    public interface IEHLangManager { 
        
    }

    public class LangManager : Singleton<LangManager>, IEHLangManager, IObservable<IEHLangManager> {
        
        private readonly ISet<IObserver<IEHLangManager>> _observers =
            new HashSet<IObserver<IEHLangManager>>();

        public IDisposable Subscribe(IObserver<IEHLangManager> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);

                observer.OnNext(this);
            }

            return new Unsubscriber<IEHLangManager>(_observers, observer);
        }

        private void _NotifyToObserver()
        {
            foreach (IObserver<IEHLangManager> target in _observers) {
                target.OnNext(this);
            } 
        }

        string _nowLang = "Kor";
        ILangPackReader _reader;

        public string NowLang => _nowLang;

        protected override void OnCreate()
        {
            _reader = new XMLLangPackReader();
            //_reader = new XMLLangPackReader();
            _reader.ReadData(_nowLang);
            //_reader.ConvertType(new JsonLangPackReader());
        }

        public void UpdateData()
        {
            _reader.ReadData(_nowLang);
            _NotifyToObserver();

        }

        public void ChangeLang(string lang)
        {
            _nowLang = lang;
            UpdateData();
        }

        public string GetStringByKey(string key, bool doAddKey=false)
        {
            string retval = _reader.GetStringByKey(key);

            if (retval != null)
            {
                return retval;
            }

            if (doAddKey) {
                _reader.AddKey(key);
                Debug.Log("New Key!!: " + key);
            }

            return key;

        }
    }

}