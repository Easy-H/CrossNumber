using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace EHTool.DBKit {

    public class LocalDatabaseConnector<K, R> : IDatabaseConnector<K, R> where R : IDictionaryable<R>
    {

        private Dictionary<K, R> _data;
        private string _path;

        private Action<IDictionary<K, R>> _allCallback;
        private IDictionary<K, Action<R>> _recordCallback;

        private IDictionary<K, Action<string>> _recordFallback;

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

            _allCallback = null;
            _recordCallback = new Dictionary<K, Action<R>>();
            _recordFallback = new Dictionary<K, Action<string>>();
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

            if (_allCallback != null)
            {
                _allCallback += callback;
                return;
            }

            _allCallback = callback;

            IDictionary<K, R> data = _GetDataTable();

            Action<IDictionary<K, R>> c = _allCallback;
            _allCallback = null;

            c.Invoke(data);
        }

        public void GetRecordAt(K idx, Action<R> callback, Action<string> fallback)
        {
            if (_recordCallback.ContainsKey(idx))
            {
                _recordCallback[idx] += callback;
                _recordFallback[idx] += fallback;
            }

            _recordCallback[idx] = callback;
            _recordFallback[idx] = fallback;

            GetAllRecord(Callback, Fallback);
        }

        public void Callback(IDictionary<K, R> data)
        {

            foreach (KeyValuePair<K, Action<R>> callback in _recordCallback)
            {
                if (data.ContainsKey(callback.Key))
                    callback.Value?.Invoke(data[callback.Key]);
                else
                    _recordFallback[callback.Key]("No Idx");
            }

            _recordCallback = new Dictionary<K, Action<R>>();
            _recordFallback = new Dictionary<K, Action<string>>();
        }

        public void Fallback(string msg)
        {

            foreach (KeyValuePair<K, Action<R>> callback in _recordCallback)
            {
                _recordFallback[callback.Key]?.Invoke(msg);
            }

            _recordCallback = new Dictionary<K, Action<R>>();
            _recordFallback = new Dictionary<K, Action<string>>();
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