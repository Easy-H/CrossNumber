using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EHTool.DBKit {
    // 테스트 및 Firestore가 갖춰지지 않았을 때 사용되는 DB 연결자
    // Firestore와 연결하더라도 캐시 DB로 사용할 수 있을듯
    public class LocalDatabaseConnector<T> : IDatabaseConnector<T> where T : IDictionaryable<T> {

        DataTable _data;
        private string _path;

        ISet<CallbackMethod<IList<T>>> _allCallback;
        IDictionary<CallbackMethod<T>, ISet<int>> _recordCallback;

        IDictionary<CallbackMethod<T>, CallbackMethod<string>> _recordFallback;

        class DataTable {
            public List<T> value;
        }

        public bool IsDatabaseExist()
        {
            return File.Exists(_path);
        }

        private DataTable _GetDataTable()
        {

            if (_data == null)
            {
                string json;

                if (IsDatabaseExist())
                    json = File.ReadAllText(_path);
                else
                {
                    json = "{\"value\":[]}";
                }
                _data = JsonUtility.FromJson<DataTable>(json);
            }

            return _data;
        }

        public void Connect(string authName, string databaseName)
        {
#if UNITY_EDITOR
            _path = string.Format("{0}/{1}/{2}.json", Application.dataPath, "/Resources", databaseName);
#else
            _path = string.Format("{0}/{1}.json", Application.persistentDataPath, databaseName);
#endif
            _data = null;

            _allCallback = new HashSet<CallbackMethod<IList<T>>>();
            _recordCallback = new Dictionary<CallbackMethod<T>, ISet<int>>();
            _recordFallback = new Dictionary<CallbackMethod<T>, CallbackMethod<string>>();
        }

        public void AddRecord(T record)
        {
            DataTable table = _GetDataTable();
            table.value.Add(record);

            string json = JsonUtility.ToJson(table, true);

            File.WriteAllText(_path, json);
        }

        public void UpdateRecordAt(T record, int idx)
        {
            DataTable table = _GetDataTable();
            int removeStartIdx = Mathf.Min(table.value.Count, idx);

            table.value.RemoveRange(removeStartIdx, table.value.Count - removeStartIdx);
            table.value.Add(record);

            string json = JsonUtility.ToJson(table, true);

            File.WriteAllText(_path, json);
        }

        public void GetAllRecord(CallbackMethod<IList<T>> callback, CallbackMethod<string> fallback)
        {

            if (_allCallback.Count > 0)
            {
                _allCallback.Add(Callback);
                return;
            }

            _allCallback.Add(callback);

            IList<T> data = _GetDataTable().value;

            foreach (CallbackMethod<IList<T>> cb in _allCallback)
            {
                cb(data);
            }

            _allCallback = new HashSet<CallbackMethod<IList<T>>>();
        }

        public void GetRecordAt(CallbackMethod<T> callback, CallbackMethod<string> fallback, int idx)
        {
            if (!_recordCallback.ContainsKey(callback))
            {
                _recordCallback.Add(callback, new HashSet<int>());
                _recordFallback.Add(callback, fallback);
            }

            _recordCallback[callback].Add(idx);
            _recordFallback[callback] = fallback;

            GetAllRecord(Callback, Fallback);
        }

        public void Callback(IList<T> data)
        {
            foreach (KeyValuePair<CallbackMethod<T>, ISet<int>> callback in _recordCallback)
            {
                foreach (int idx in callback.Value)
                {
                    if (data.Count > idx)
                        callback.Key(data[idx]);
                    else
                        _recordFallback[callback.Key]("No Idx");

                }
            }

            _recordCallback = new Dictionary<CallbackMethod<T>, ISet<int>>();
            _recordFallback = new Dictionary<CallbackMethod<T>, CallbackMethod<string>>();
        }

        public void Fallback(string msg) {

            foreach (KeyValuePair<CallbackMethod<T>, ISet<int>> callback in _recordCallback)
            {
                foreach (int idx in callback.Value)
                {
                    _recordFallback[callback.Key]?.Invoke(msg);

                }
            }

            _recordCallback = new Dictionary<CallbackMethod<T>, ISet<int>>();
            _recordFallback = new Dictionary<CallbackMethod<T>, CallbackMethod<string>>();
        }
    }
}