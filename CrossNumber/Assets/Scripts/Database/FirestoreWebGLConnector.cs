using EHTool;
using EHTool.DBKit;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;

static class FirestoreWebGLBridge
{

    [DllImport("__Internal")]
    public static extern void FirestoreConnect(string firebaseConfigValue);

    [DllImport("__Internal")]
    public static extern void FirestoreAddRecord(string pathJson, string recordJson, string idx);

    [DllImport("__Internal")]
    public static extern void FirestoreUpdateRecordAt(string pathJson, string recordJson, string idx);
    [DllImport("__Internal")]
    public static extern void FirestoreGetAllRecord(string pathJson, string objectName, string callback, string fallback);

    [DllImport("__Internal")]
    public static extern void FirestoreDeleteRecordAt(string pathJson, string idx);
}

public class FirestoreWebGLConnector<K, T> : MonoBehaviour, IDatabaseConnector<K, T> where T : IDictionaryable<T> {

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

        FirestoreWebGLBridge.FirestoreConnect(AssetOpener.ReadTextAsset("FirebaseConfig"));
        _isConnect = true;

    }

    public void Connect(string authName, string databaseName)
    {
        Connect(new string[2] { databaseName, authName });
    }

    public void AddRecord(T record)
    {
        if (!_isConnect) return;
        FirestoreWebGLBridge.FirestoreAddRecord(_pathJson,
            JsonConvert.SerializeObject(record.ToDictionary()),
            JsonConvert.SerializeObject(Activator.CreateInstance<K>()));
    }

    public void UpdateRecordAt(K idx, T record)
    {
        if (!_isConnect) return;

        if (_dbExist)
        {
            FirestoreWebGLBridge.FirestoreUpdateRecordAt(_pathJson,
                JsonConvert.SerializeObject(record.ToDictionary()),
                JsonConvert.SerializeObject(idx));
            return;
        }
        FirestoreWebGLBridge.FirestoreAddRecord(_pathJson,
            JsonConvert.SerializeObject(record.ToDictionary()),
            JsonConvert.SerializeObject(idx));
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

        Dictionary<string, object> snapshot = JsonConvert.DeserializeObject<Dictionary<string, object>>(value);

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