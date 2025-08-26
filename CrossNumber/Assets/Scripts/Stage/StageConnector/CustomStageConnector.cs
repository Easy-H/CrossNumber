using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using UnityEngine;
using EHTool.DBKit;

internal class CustomStageConnector : IStageConnector
{

    private string _path;
    private IDatabaseConnector<string, StageMetaData> _dbConnector;

    public CustomStageConnector()
    {
        _dbConnector =
            new LocalDatabaseConnector<string, StageMetaData>();

        _dbConnector.Connect(new string[1] { "CustomStages" });

#if UNITY_EDITOR
        _path = string.Format("{0}/{1}", Application.dataPath, "Resources/Json/StageData");
#else
        _path = string.Format("{0}", Application.persistentDataPath);
#endif
    }

    public void GetStage(string key, Action<Stage> callback, Action fallback)
    {
        string path = string.Format("{0}/{1}.json", _path, key);

        if (!File.Exists(path))
        {
            callback?.Invoke(new Stage());
            return;
        }

        string json = File.ReadAllText(path);

        Dictionary<string, IDictionary<string, object>> parsedData =
            JsonConvert.DeserializeObject<Dictionary<string, IDictionary<string, object>>>(json);

        Stage data = new Stage(parsedData);

        callback?.Invoke(data);
    }

    public void GetStageList(
        Action<StageMetaData[]> callback, Action fallback)
    {
        _dbConnector.GetAllRecord((data) =>
        {
            StageMetaData[] retval =
                data.Values.ToArray();

            for (int i = 0; i < retval.Length; i++)
            {
                retval[i].SetType("Custom");
            }
            callback?.Invoke(retval);

        }, (msg) => { fallback?.Invoke(); });

    }

    public void UploadStage(string name, Stage data, Action<string> callback, Action<string> fallback)
    {
        _dbConnector.UpdateRecordAt(
            name, new StageMetaData(name, name, false));

        File.WriteAllText(GetPath(name),
            JsonConvert.SerializeObject(data.ToDictionary()));
;
        callback?.Invoke(name);
    }

    public void DeleteStage(string name, Action<string> callback, Action<string> fallback)
    {
        _dbConnector.DeleteRecordAt(name);

        File.Delete(GetPath(name));

        callback?.Invoke(name);
    }

    string GetPath(string name)
    {
        return string.Format("{0}/{1}.json", _path, name);
    }

}