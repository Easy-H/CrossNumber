using EHTool;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

internal class LocalStageConnector : IStageConnector {

    string _path;

    public LocalStageConnector()
    {
#if UNITY_EDITOR
        _path = string.Format("{0}/{1}", Application.dataPath, "Resources/Json/StageData");
#else
        _path = string.Format("{0}", Application.persistentDataPath);
#endif
    }

    public void GetLevelData(string key, CallbackMethod<StageData> callback, CallbackMethod fallback)
    {
        string path = string.Format("{0}/{1}.json", _path, key);

        if (!File.Exists(path)) {
            callback?.Invoke(new StageData());
            return;
        }

        string json = File.ReadAllText(path);

        Dictionary<string, Dictionary<string, object>> parsedData =
            JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(json);

        Dictionary<string, object> parameter = new Dictionary<string, object>();

        int i = 0;
        foreach (object d in parsedData.Values)
        {
            parameter.Add((i++).ToString(), d);
        }

        StageData data = new StageData(parameter);

        callback?.Invoke(data);
    }

    public void GetRandomStage(CallbackMethod<Dictionary<string, object>> callback, CallbackMethod fallback)
    {
    }

    public void UploadStage(string name, StageData data)
    {
        File.WriteAllText(string.Format("{0}/{1}.json", _path, name),
            JsonConvert.SerializeObject(data.ToDictionary()));
    }

}