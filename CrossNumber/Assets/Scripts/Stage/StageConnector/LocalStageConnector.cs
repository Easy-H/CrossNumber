using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using EHTool;

internal class LocalStageConnector : IStageConnector {

    public LocalStageConnector()
    {

    }

    public void GetLevelData(string key, Action<Stage> callback, Action fallback)
    {

        IDictionaryConnector<string, IDictionary<string, object>> dictionaryConnector =
            new JsonDictionaryConnector<string, IDictionary<string, object>>();

        Stage data = new Stage(dictionaryConnector.ReadData(string.Format("StageData/{0}", key)));

        callback?.Invoke(data);
    }

    public void GetRandomStage(Action<Dictionary<string, object>> callback, Action fallback)
    {
    }

    public void UploadStage(string name, Stage data)
    {

    }

}