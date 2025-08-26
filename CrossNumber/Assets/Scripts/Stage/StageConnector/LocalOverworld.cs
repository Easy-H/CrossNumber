using System;
using UnityEngine;
using EHTool;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data_Overworld_", menuName = "Custom/OverworldData", order = 1)]
public class LocalOverworld :
    ScriptableObject, IStageConnector
{
    public string Name;

    public StageMetaData[] Stages;

    public string OverworldName => Name;

    public void Setting(string name, IStageConnector connector) { }

    public void GetStage(string key, Action<Stage> callback, Action fallback)
    {
        IDictionaryConnector<string, IDictionary<string, object>> dictionaryConnector =
            new JsonDictionaryConnector<string, IDictionary<string, object>>();

        Stage data = new Stage(dictionaryConnector.ReadData(string.Format("StageData/{0}", key)));

        callback?.Invoke(data);
    }

    public void GetStageList(
        Action<StageMetaData[]> callback, Action fallback)
    {
        callback?.Invoke(Stages);
    }

    public void UploadStage(string name, Stage data,
        Action<string> callback, Action<string> fallback)
    {
        fallback?.Invoke("msg_Error");
    }

    public void DeleteStage(string name, Action<string> callback, Action<string> fallback)
    {
        fallback?.Invoke("msg_Error");
    }

}
