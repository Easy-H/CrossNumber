using System.Collections.Generic;
using System;

public interface IStageConnector {

    public void GetLevelData(string key, Action<Stage> callback, Action fallback);
    public void GetRandomStage(Action<Dictionary<string, object>> callback, Action fallback);
    public void UploadStage(string name, Stage data);

}