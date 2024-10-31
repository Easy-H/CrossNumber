using EHTool;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class FirestoreWebGLStageConnector : MonoBehaviour, IStageConnector {

    static bool _isConnect = false;

    public FirestoreWebGLStageConnector()
    {

        if (_isConnect) return;

        FirestoreWebGLBridge.FirestoreConnect("path", AssetOpener.ReadTextAsset("FirebaseConfig"));
        _isConnect = true;

    }

    public void UploadStage(string name, StageData data)
    {
        FirestoreWebGLBridge.UploadMap(name, JsonConvert.SerializeObject(data.ToDictionary()), gameObject.name, "GetRandomStageJS", "GetRandomStageJS");

    }

    CallbackMethod<StageData> _dataCallback;
    CallbackMethod _dataFallback;

    public void GetLevelData(string key, CallbackMethod<StageData> callback, CallbackMethod fallback)
    {

        _dataCallback += callback;
        _dataFallback += fallback;

        FirestoreWebGLBridge.GetLevelData(key, gameObject.name, "GetLevelDataJS");
    }

    void GetLevelDataJS(string data)
    {
        Dictionary<string, Dictionary<string, object>> parsedData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(data);

        Dictionary<string, object> parameter = new Dictionary<string, object>();

        int i = 0;
        foreach (object d in parsedData.Values)
        {
            parameter.Add((i++).ToString(), d);
        }

        _dataCallback?.Invoke(new StageData(parameter));
        _dataCallback = null;
    }

    CallbackMethod<Dictionary<string, object>> _stageCallback;
    CallbackMethod _stageFallback;

    public void GetRandomStage(CallbackMethod<Dictionary<string, object>> callback, CallbackMethod fallback)
    {
        _stageCallback += callback;
        _stageFallback += fallback;

        FirestoreWebGLBridge.GetJSON(gameObject.name, "GetRandomStageJS", "GetRandomStageJS");
    }

    void GetRandomStageJS(string data)
    {

        Dictionary<object, Dictionary<string, object>> snapshot =
            JsonConvert.DeserializeObject<Dictionary<object, Dictionary<string, object>>>(data);

        // 무작위로 X개의 문서 선택
        int documentsToFetch = Mathf.Min(10, snapshot.Count);
        List<Dictionary<string, object>> documents = new List<Dictionary<string, object>>();

        foreach (Dictionary<string, object> documentSnapshot in snapshot.Values)
        {
            documents.Add(documentSnapshot);
        }

        Dictionary<string, object> retval = new Dictionary<string, object>();

        for (int i = 0; i < documentsToFetch; i++)
        {
            int randomIndex = Random.Range(0, documents.Count);
            retval.Add(i.ToString(), documents[randomIndex]);
            documents.RemoveAt(randomIndex);
        }

        _stageCallback?.Invoke(retval);
        _stageCallback = null;
    }
}