using EasyH.Tool.DBKit;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;

static class FirestoreWebGLBridge
{
    [DllImport("__Internal")]
    public static extern void FirestoreAddRecord(string pathJson, string recordJson);

    [DllImport("__Internal")]
    public static extern void FirestoreUpdateRecord(string pathJson, string recordJson);
    [DllImport("__Internal")]
    public static extern void FirestoreGetAllRecord(string pathJson, string objectName, string callback, string fallback);

    [DllImport("__Internal")]
    public static extern void FirestoreDeleteRecordAt(string pathJson, string idx);
}

public class FirestoreWebGLConnector<K, T> : MonoBehaviour,
    IDatabaseConnector<K, T> 
    where T : IDictionaryable<T> {

    private static bool _isConnect = false;

    private Action<IDictionary<K, T>> _allListener;
    private Action<string> _fallbackListener;

    private string _pathJson;

    private bool _dbExist;

    public bool IsDatabaseExist()
    {
        return false;
    }

    public void Connect(string[] args)
    {
        _pathJson = JsonConvert.SerializeObject(args);

        _allListener = null;
        _fallbackListener = null;

        if (_isConnect) return;

        FirebaseManager.Instance.SetConfig();
        _isConnect = true;

    }

    public void Connect(string authName, string databaseName)
    {
        Connect(new string[2] { databaseName, authName });
    }

    public void AddRecord(T record)
    {
        if (!_isConnect) return;

        Dictionary<string, object> r
            = new()
            {
                {
                    Activator.CreateInstance<K>().ToString(),
                    record.ToDictionary()
                }
            };
            
        FirestoreWebGLBridge.FirestoreAddRecord(_pathJson,
            JsonConvert.SerializeObject(r));
    }

    public void UpdateRecordAt(K idx, T record)
    {
        UpdateRecord(new IDatabaseConnector<K, T>.UpdateLog[1]
            { new (idx, record)});
    }

    public void UpdateRecord(IDatabaseConnector<K, T>.UpdateLog[] updates)
    {
        if (!_isConnect) return;

        Dictionary<string, object> up = new Dictionary<string, object>();

        foreach (var r in updates)
        {
            if (r.Record == null)
            {
                up.Add(r.Idx.ToString(), null);
                continue;
            }
            up.Add(r.Idx.ToString(), r.Record.ToDictionary());
        }

        if (!_dbExist)
        {
            FirestoreWebGLBridge.FirestoreAddRecord(_pathJson,
                JsonConvert.SerializeObject(up));
            _dbExist = true;
            return;
        }

        FirestoreWebGLBridge.FirestoreUpdateRecord(_pathJson,
            JsonConvert.SerializeObject(up));

        _dbExist = true;
    }
    
    public void DeleteRecordAt(K idx)
    {
        FirestoreWebGLBridge.FirestoreDeleteRecordAt(_pathJson, JsonConvert.SerializeObject(idx));
    }

    public void GetAllRecord(Action<IDictionary<K, T>> callback, Action<string> fallback)
    {
        if (!_isConnect) return;

        if (_allListener != null)
        {
            _allListener += callback;
            _fallbackListener += fallback;
            return;
        }

        _allListener += callback;
        _fallbackListener += fallback;

        FirestoreWebGLBridge.FirestoreGetAllRecord(_pathJson, gameObject.name, "FSGetAllRecordCallback", "FSGetAllRecordFallback");
    }

    public void FSGetAllRecordCallback(string value) {

        _dbExist = true;

        Dictionary<string, object> snapshot = 
            JsonConvert.DeserializeObject<Dictionary<string, object>>(value);

        IDictionary<K, T> data = new Dictionary<K, T>();

        foreach (var child in snapshot)
        {
            K key = (K)Convert.ChangeType(child.Key, typeof(K));
            T temp = Activator.CreateInstance<T>();
            temp.SetValueFromDictionary(JsonConvert.DeserializeObject
                <Dictionary<string, object>>(child.Value.ToString()));

            data.Add(key, temp);
        }

        _allListener?.Invoke(data);

        _allListener = null;
        _fallbackListener = null;
    }

    public void FSGetAllRecordFallback()
    {
        _dbExist = false;

        _fallbackListener?.Invoke("Network Error");

        _allListener = null;
        _fallbackListener = null;

    }

    struct IdxAndFallback {
        public int idx;
        public Action<string> fallback;

        public IdxAndFallback(int idx, Action<string> fallback) {
            this.idx = idx;
            this.fallback = fallback;
        }
    }

    public void GetRecordAt(K idx, Action<T> callback, Action<string> fallback)
    {
        if (!_isConnect) return;

        Action<IDictionary<K, T>> thisCallback = (IDictionary<K, T> data) =>
        {
            if (data.ContainsKey(idx))
            {
                callback?.Invoke(data[idx]);
                return;
            }

            fallback?.Invoke("No Idx");
        };

        GetAllRecord(thisCallback, fallback);
    }
    
}