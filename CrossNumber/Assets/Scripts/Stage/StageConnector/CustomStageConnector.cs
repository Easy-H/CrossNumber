using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

internal class CustomStageConnector : IStageConnector {

    private string _path;

    public CustomStageConnector()
    {
#if UNITY_EDITOR
        _path = string.Format("{0}/{1}", Application.dataPath, "Resources/Json/StageData");
#else
        _path = string.Format("{0}", Application.persistentDataPath);
#endif
    }

    public void GetLevelData(string key, Action<Stage> callback, Action fallback)
    {
        string path = string.Format("{0}/{1}.json", _path, key);

        if (!File.Exists(path)) {
            callback?.Invoke(new Stage());
            return;
        }

        string json = File.ReadAllText(path);

        Dictionary<string, IDictionary<string, object>> parsedData =
            JsonConvert.DeserializeObject<Dictionary<string, IDictionary<string, object>>>(json);

        Stage data = new Stage(parsedData);

        callback?.Invoke(data);
    }

    public void GetRandomStage(Action<Dictionary<string, object>> callback, Action fallback)
    {
    }

    public void UploadStage(string name, Stage data)
    {
        File.WriteAllText(string.Format("{0}/{1}.json", _path, name),
            JsonConvert.SerializeObject(data.ToDictionary()));
    }

}