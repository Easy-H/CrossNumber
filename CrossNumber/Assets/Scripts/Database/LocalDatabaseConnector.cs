using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace EHTool.DBKit {

    public class LocalDatabaseConnector<K, R> : IDatabaseConnector<K, R> where R : IDictionaryable<R>
    {

        Dictionary<K, R> _data;
        private string _path;

        ISet<Action<IDictionary<K, R>>> _allCallback;
        IDictionary<Action<R>, ISet<K>> _recordCallback;

        IDictionary<Action<R>, Action<string>> _recordFallback;

        class DataTable
        {
            public List<R> value;
        }

        public bool IsDatabaseExist()
        {
            return File.Exists(_path);
        }

        private Dictionary<K, R> _GetDataTable()
        {

            if (_data == null)
            {
                string json;

                if (IsDatabaseExist())
                    json = File.ReadAllText(_path);
                else
                {
                    json = "{ }";
                }
                _data = JsonConvert.DeserializeObject<Dictionary<K, R>>(json);
            }

            return _data;
        }

        public void Connect(string[] args)
        {

#if UNITY_EDITOR
            _path = string.Format("{0}/{1}/{2}.json", Application.dataPath, "/Resources", string.Join("/", args));
#else
            _path = string.Format("{0}/{1}.json", Application.persistentDataPath, string.Join("/", args));
#endif
            _data = null;

            _allCallback = new HashSet<Action<IDictionary<K, R>>>();
            _recordCallback = new Dictionary<Action<R>, ISet<K>>();
            _recordFallback = new Dictionary<Action<R>, Action<string>>();
        }

        public void Connect(string authName, string databaseName)
        {
            Connect(new string[2] { databaseName, authName });
        }

        public void AddRecord(R record)
        {
            Dictionary<K, R> table = _GetDataTable();
            table.Add(default, record);

            string json = JsonConvert.SerializeObject(table);

            File.WriteAllText(_path, json);
        }

        public void UpdateRecordAt(K idx, R record)
        {
            Dictionary<K, R> table = _GetDataTable();
            if (table.ContainsKey(idx))
            {
                table[idx] = record;
            }
            else
            {
                table.Add(idx, record);
            }

            string json = JsonConvert.SerializeObject(table);

            File.WriteAllText(_path, json);
        }

        public void UpdateRecords(UpdateData<K, R>[] updateData)
        {
            for (int i = 0; i < updateData.Length; i++)
            {
                UpdateRecordAt(updateData[i].Idx, updateData[i].Record);
            }
        }

        public void GetAllRecord(Action<IDictionary<K, R>> callback, Action<string> fallback)
        {

            if (_allCallback.Count > 0)
            {
                _allCallback.Add(Callback);
                return;
            }

            _allCallback.Add(callback);

            IDictionary<K, R> data = _GetDataTable();

            foreach (Action<IDictionary<K, R>> cb in _allCallback)
            {
                cb(data);
            }

            _allCallback = new HashSet<Action<IDictionary<K, R>>>();
        }

        public void GetRecordAt(K idx, Action<R> callback, Action<string> fallback)
        {
            if (!_recordCallback.ContainsKey(callback))
            {
                _recordCallback.Add(callback, new HashSet<K>());
                _recordFallback.Add(callback, fallback);
            }

            _recordCallback[callback].Add(idx);
            _recordFallback[callback] = fallback;

            GetAllRecord(Callback, Fallback);
        }

        public void Callback(IDictionary<K, R> data)
        {
            foreach (KeyValuePair<Action<R>, ISet<K>> callback in _recordCallback)
            {
                foreach (K idx in callback.Value)
                {
                    if (data.ContainsKey(idx))
                        callback.Key(data[idx]);
                    else
                        _recordFallback[callback.Key]("No Idx");

                }
            }

            _recordCallback = new Dictionary<Action<R>, ISet<K>>();
            _recordFallback = new Dictionary<Action<R>, Action<string>>();
        }

        public void Fallback(string msg)
        {

            foreach (KeyValuePair<Action<R>, ISet<K>> callback in _recordCallback)
            {
                foreach (K idx in callback.Value)
                {
                    _recordFallback[callback.Key]?.Invoke(msg);

                }
            }

            _recordCallback = new Dictionary<Action<R>, ISet<K>>();
            _recordFallback = new Dictionary<Action<R>, Action<string>>();
        }

        public void DeleteRecordAt(K idx)
        {
            Dictionary<K, R> table = _GetDataTable();
            table.Remove(idx);

            string json = JsonConvert.SerializeObject(table);

            File.WriteAllText(_path, json);
        }
        
    }
}