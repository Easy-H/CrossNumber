using EHTool;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FirestoreWebGLStageConnector : MonoBehaviour, IStageConnector {

    static bool _isConnect = false;

    public FirestoreWebGLStageConnector()
    {

        if (_isConnect) return;

        FirestoreWebGLBridge.FirestoreConnect("path", AssetOpener.ReadTextAsset("FirebaseConfig"));
        _isConnect = true;

    }

    public void UploadStage(string name, Stage data)
    {
        FirestoreWebGLBridge.UploadMap(name, JsonConvert.SerializeObject(data.ToDictionary()), gameObject.name, "GetRandomStageJS", "GetRandomStageJS");

    }

    Action<Stage> _dataCallback;
    Action _dataFallback;

    public void GetLevelData(string key, Action<Stage> callback, Action fallback)
    {

        _dataCallback += callback;
        _dataFallback += fallback;

        FirestoreWebGLBridge.GetLevelData(key, gameObject.name, "GetLevelDataJS");
    }

    public void GetLevelDataJS(string data)
    {
        Dictionary<string, IDictionary<string, object>> parsedData =
            JsonConvert.DeserializeObject<Dictionary<string, IDictionary<string, object>>>(data);

        _dataCallback?.Invoke(new Stage(parsedData));
        _dataCallback = null;
    }

    private Action<Dictionary<string, object>> _stageCallback;
    private Action _stageFallback;

    public void GetRandomStage(Action<Dictionary<string, object>> callback, Action fallback)
    {
        _stageCallback += callback;
        _stageFallback += fallback;

        FirestoreWebGLBridge.GetJSON(gameObject.name, "GetRandomStageJS", "GetRandomStageJS");
    }

    public void GetRandomStageJS(string data)
    {

        Dictionary<object, Dictionary<string, object>> snapshot =
            JsonConvert.DeserializeObject<Dictionary<object, Dictionary<string, object>>>(data);

        // �������� X���� ���� ����
        int documentsToFetch = Mathf.Min(10, snapshot.Count);
        List<Dictionary<string, object>> documents = new List<Dictionary<string, object>>();

        foreach (Dictionary<string, object> documentSnapshot in snapshot.Values)
        {
            documents.Add(documentSnapshot);
        }

        Dictionary<string, object> retval = new Dictionary<string, object>();

        for (int i = 0; i < documentsToFetch; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, documents.Count);
            retval.Add(i.ToString(), documents[randomIndex]);
            documents.RemoveAt(randomIndex);
        }

        _stageCallback?.Invoke(retval);
        _stageCallback = null;
    }
}