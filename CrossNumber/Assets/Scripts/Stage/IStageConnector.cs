using System.Collections.Generic;

public interface IStageConnector {

    public void GetLevelData(string key, CallbackMethod<StageData> callback, CallbackMethod fallback);
    public void GetRandomStage(CallbackMethod<Dictionary<string, object>> callback, CallbackMethod fallback);
    public void UploadStage(string name, StageData data);

}