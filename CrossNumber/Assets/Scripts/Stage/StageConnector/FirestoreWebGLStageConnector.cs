using EHTool;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FirestoreWebGLStageConnector :
    MonoBehaviour, IStageConnector
{

    static bool _isConnect = false;

    public FirestoreWebGLStageConnector()
    {

        if (_isConnect) return;

        FirebaseManager.Instance.SetConfig();
        _isConnect = true;

    }

    public void UploadStage(string name, Stage data, Action<string> callback, Action<string> fallback)
    {
        FirestoreWebGL.UploadMap(name, JsonConvert.SerializeObject(data.ToDictionary()), gameObject.name, "GetRandomStageJS", "GetRandomStageJS");

    }

    Action<Stage> _dataCallback;
    Action _dataFallback;

    public void GetStage(string key, Action<Stage> callback, Action fallback)
    {
        _dataCallback += callback;
        _dataFallback += fallback;

        FirestoreWebGL.GetLevelData(key, gameObject.name, "GetLevelDataJS");
    }

    public void GetLevelDataJS(string data)
    {
        Dictionary<string, IDictionary<string, object>> parsedData =
            JsonConvert.DeserializeObject<Dictionary<string, IDictionary<string, object>>>(data);

        _dataCallback?.Invoke(new Stage(parsedData));
        _dataCallback = null;
    }

    private Action<StageMetaData[]> _stageCallback;
    private Action _stageFallback;

    public void GetStageList(Action<StageMetaData[]> callback, Action fallback)
    {
        _stageCallback += callback;
        _stageFallback += fallback;

        FirestoreWebGL.GetJSON(gameObject.name, "GetRandomStageJS", "GetRandomStageJS");
    }

    public void GetRandomStageJS(string data)
    {

        Dictionary<object, Dictionary<string, object>> snapshot =
            JsonConvert.DeserializeObject<Dictionary<object, Dictionary<string, object>>>(data);

        int documentsToFetch = Mathf.Min(10, snapshot.Count);
        List<Dictionary<string, object>> documents = new List<Dictionary<string, object>>();

        foreach (Dictionary<string, object> documentSnapshot in snapshot.Values)
        {
            documents.Add(documentSnapshot);
        }

        StageMetaData[] retval =
            new StageMetaData[documentsToFetch];

        for (int i = 0; i < documentsToFetch; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, documents.Count);

            retval[i] = new StageMetaData();
            retval[i].SetValueFromDictionary(
                documents[randomIndex]);
            retval[i].SetType("Cloud");

            documents.RemoveAt(randomIndex);
        }

        _stageCallback?.Invoke(retval);
        _stageCallback = null;
    }

    public void DeleteStage(string name, Action<string> callback, Action<string> fallback)
    {
        fallback?.Invoke("msg_Error");
    }
    
}