using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EHTool.LangKit {

    public interface IEHText : IObserver<IEHLangManager> {
        public void SetText(string key);

    }

    public interface IEHLangManager : IObservable<IEHLangManager>{
        public void UpdateData(bool needNotify = true);
        public void ChangeLang(string lang);
        public string GetStringByKey(string key, bool doAddKey = false);
    }

    public class LangManager : Singleton<LangManager>, IEHLangManager {
        
        private readonly ISet<IObserver<IEHLangManager>> _observers = new HashSet<IObserver<IEHLangManager>>();

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

        IDictionaryConnector<string, string> _reader;
        IDictionary<string, string> _dict;

        public string NowLang => _nowLang;

        protected override void OnCreate()
        {
            _dict = new Dictionary<string, string>();
            _reader = new JsonDictionaryConnector<string, string>();
            //_reader = new XMLLangPackReade<string, string>();
            UpdateData();
        }

        public void UpdateData(bool needNotify = true)
        {
            IDictionary<string, string> readResult = _reader.ReadData(string.Format("String/{0}", _nowLang));

            foreach (var element in readResult) {
                if (_dict.ContainsKey(element.Key)) {
                    _dict[element.Key] = element.Value;
                    continue;
                }
                _dict.Add(element.Key, element.Value);
            }

            if (needNotify)
            {
                _NotifyToObserver();
            }

        }

        public void ChangeLang(string lang)
        {
            _nowLang = lang;
            UpdateData();
        }

        public string GetStringByKey(string key, bool doAddKey=false)
        {
            if (_dict.ContainsKey(key))
            {
                return _dict[key];
            }

            if (doAddKey) {
                _dict.Add(key, null);
                AddKey();
            }

            return key;

        }

        void AddKey() {

            if (!Directory.Exists(string.Format("Assets/Resources/{0}/String", _reader.GetDefaultPath())))
            {
                return;
            }

            DirectoryInfo di = new DirectoryInfo(string.Format("Assets/Resources/{0}/String", _reader.GetDefaultPath()));
            string startLang = NowLang;

            foreach (FileInfo File in di.GetFiles())
            {
                if (File.Extension.ToLower().CompareTo(_reader.GetExtensionName()) != 0) continue;

                string name = File.Name.Substring(0, File.Name.Length - _reader.GetExtensionName().Length);

                if (name.CompareTo(NowLang) == 0) continue;

                _nowLang = name;
                UpdateData(false);
                _reader.Save(_dict, name);
            }

            _nowLang = startLang;
            UpdateData(false);
            _reader.Save(_dict, NowLang);

        }
    }

}