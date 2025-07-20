#if !UNITY_WEBGL || UNITY_EDITOR
using EHTool.DBKit;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FirestoreConnector<K, T> : IDatabaseConnector<K, T> where T : IDictionaryable<T>
{

    private DocumentReference _docRef;

    private Action<IDictionary<K, T>> _allListener;
    private Action<string> _fallbackListener;

    bool _databaseExist = false;

    public bool IsDatabaseExist()
    {
        return _databaseExist;
    }

    public void Connect(string[] args)
    {

        if (args == null || args.Length == 0 || args.Length % 2 != 0)
        {
            Debug.LogError("Invalid pathSegments. Must be an array of alternating collection and document names, with an even number of elements.");
            _docRef = null; // 유효하지 않은 경로인 경우 docRef를 null로 설정
            return;
        }

        _docRef = FirebaseFirestore.DefaultInstance.Collection(args[0]).Document(args[1]);

        for (int i = 2; i < args.Length; i += 2)
        {
            _docRef = _docRef.Collection(args[i]).Document(args[i + 1]);
        }

        _allListener = null;
        _fallbackListener = null;

    }

    public void Connect(string authName, string databaseName)
    {
        Connect(new string[2] { databaseName, authName });
    }

    public void AddRecord(T record)
    {
        // 추후 수정 필요
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "0" , record }
        };

        _docRef.UpdateAsync(updates).ContinueWithOnMainThread(task =>
        {
            Debug.Log("AddRecord");
        });
    }

    public void UpdateRecordAt(K idx, T record)
    {
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { idx.ToString(), record.ToDictionary() },
        };

        if (!_databaseExist)
        {
            _docRef.SetAsync(updates);
            _databaseExist = true;
            return;

        }

        //updates.Add((idx + 1).ToString(), FieldValue.Delete);
        _docRef.UpdateAsync(updates);
    }

    public void GetAllRecord(Action<IDictionary<K, T>> callback, Action<string> fallback)
    {
        if (_allListener != null)
        {
            _allListener += callback;
            _fallbackListener += fallback;
            return;
        }

        _allListener += callback;
        _fallbackListener += fallback;

        _docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("GetAllRecord task was canceled.");
                _fallbackListener?.Invoke("Operation canceled.");
                _allListener = null;
                _fallbackListener = null;
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError($"GetAllRecord task faulted: {task.Exception}");
                _fallbackListener?.Invoke($"Failed to retrieve data: {task.Exception.Message}");
                _allListener = null;
                _fallbackListener = null;
                return;
            }

            DocumentSnapshot snapshot = task.Result;

            _databaseExist = snapshot.Exists;

            if (snapshot.Exists)
            {
                IDictionary<string, object> snapshotData = snapshot.ToDictionary();

                IDictionary<K, T> data = new Dictionary<K, T>();

                foreach (var tuple in snapshotData)
                {
                    K key = (K)Convert.ChangeType(tuple.Key, typeof(K));
                    T value = Activator.CreateInstance<T>();

                    if (tuple.Value is IDictionary<string, object> childDataDictionary)
                    {
                        value.SetValueFromDictionary(childDataDictionary);
                    }

                    data.Add(key, value);
                }
                _allListener?.Invoke(data);

            }
            else
            {
                _fallbackListener?.Invoke("No Idx");
            }

            _allListener = null;
            _fallbackListener = null;
        });
    }

    // GetRecordAll에서 모든 레코드 받아오면 거기서 원하는걸 찾아오는 방식임
    // 비효율적인 방식이지만 이 게임에서 이걸 사용하는 건 하나밖에 없어서(GameManagerData인데 이것도 Firestore 안쓸 예정) 일단은 이렇게 둠
    public void GetRecordAt(K idx, Action<T> callback, Action<string> fallback)
    {
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

    public void DeleteRecordAt(K idx)
    {

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { idx.ToString(), FieldValue.Delete },
        };

        _docRef.UpdateAsync(updates);
    }

}
#endif