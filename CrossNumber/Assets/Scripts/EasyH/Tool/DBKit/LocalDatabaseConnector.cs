using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace EasyH.Tool.DBKit
{

    public class LocalDatabaseConnector<K, T> : IDatabaseConnector<K, T> where T : IDictionaryable<T>
    {

        private Dictionary<K, Dictionary<string, object>> _data;
        private string _path;

        private ISet<Action<IDictionary<K, T>>> _allCallback;
        private IDictionary<Action<T>, ISet<K>> _recordCallback;

        private IDictionary<Action<T>, Action<string>> _recordFallback;

        public bool IsDatabaseExist()
        {
            return File.Exists(_path);
        }

        private Dictionary<K, Dictionary<string, object>> _GetDataTable()
        {

            if (_data == null)
            {
                _data = new Dictionary<K, Dictionary<string, object>>();

                string json;

                if (IsDatabaseExist())
                    json = File.ReadAllText(_path);
                else
                {
                    json = "{ }";
                }

                _data = JsonConvert.DeserializeObject
                    <Dictionary<K, Dictionary<string, object>>>(json);

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

            _allCallback = new HashSet<Action<IDictionary<K, T>>>();
            _recordCallback = new Dictionary<Action<T>, ISet<K>>();
            _recordFallback = new Dictionary<Action<T>, Action<string>>();
        }

        public void Connect(string authName, string databaseName)
        {
            Connect(new string[2] { databaseName, authName });
        }

        public void AddRecord(T record)
        {
            Dictionary<K, Dictionary<string, object>> table
                = _GetDataTable();

            table.Add(default, new Dictionary<string, object>(
                    record.ToDictionary()));

            string json = JsonConvert.SerializeObject(table);

            File.WriteAllText(_path, json);
        }

        public void UpdateRecordAt(K idx, T record)
        {
            UpdateRecord(new IDatabaseConnector<K, T>.UpdateLog[1] { new(idx, record) });
        }

        public void UpdateRecord(IDatabaseConnector<K, T>.UpdateLog[] updates)
        {
            IDictionary<K, Dictionary<string, object>> target = _GetDataTable();

            foreach (var r in updates)
            {
                if (r.Record == null)
                {
                    if (target.ContainsKey(r.Idx))
                        target.Remove(r.Idx);
                    continue;
                }
                if (target.ContainsKey(r.Idx))
                {
                    target[r.Idx] = new Dictionary<string, object>(
                        r.Record.ToDictionary());
                    continue;
                }

                target.Add(r.Idx, new Dictionary<string, object>(
                        r.Record.ToDictionary()));
            }

            string json = JsonConvert.SerializeObject(target);

            // Get the directory path from the full file path
            string directoryPath = Path.GetDirectoryName(_path);

            // Check if the directory exists and create it if it doesn't
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(_path, json);

        }

        public void GetAllRecord(Action<IDictionary<K, T>> callback, Action<string> fallback)
        {

            if (_allCallback.Count > 0)
            {
                _allCallback.Add(Callback);
                return;
            }

            _allCallback.Add(callback);

            IDictionary<K, T> data = new Dictionary<K, T>();

            foreach (var childSnapshot in _GetDataTable())
            {

                K key = (K)Convert.ChangeType(childSnapshot.Key, typeof(K));
                T value = Activator.CreateInstance<T>();

                if (childSnapshot.Value is IDictionary<string, object> childDataDictionary)
                {
                    value.SetValueFromDictionary(childDataDictionary);
                }

                data.Add(key, value);
            }

            foreach (Action<IDictionary<K, T>> cb in _allCallback)
            {
                cb(data);
            }

            _allCallback = new HashSet<Action<IDictionary<K, T>>>();

        }

        public void GetRecordAt(K idx, Action<T> callback, Action<string> fallback)
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

        public void Callback(IDictionary<K, T> data)
        {
            foreach (KeyValuePair<Action<T>, ISet<K>> callback in _recordCallback)
            {
                foreach (K idx in callback.Value)
                {
                    if (data.ContainsKey(idx))
                        callback.Key(data[idx]);
                    else
                        _recordFallback[callback.Key]("No Idx");

                }
            }

            _recordCallback = new Dictionary<Action<T>, ISet<K>>();
            _recordFallback = new Dictionary<Action<T>, Action<string>>();
        }

        public void Fallback(string msg)
        {

            foreach (KeyValuePair<Action<T>, ISet<K>> callback in _recordCallback)
            {
                foreach (K idx in callback.Value)
                {
                    _recordFallback[callback.Key]?.Invoke(msg);

                }
            }

            _recordCallback = new Dictionary<Action<T>, ISet<K>>();
            _recordFallback = new Dictionary<Action<T>, Action<string>>();
        }

        public void DeleteRecordAt(K idx)
        {

        }
    }
}