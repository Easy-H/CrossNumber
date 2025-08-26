using System;

public interface IStageConnector
{
    public void GetStage(string key,
        Action<Stage> callback, Action fallback);
    public void GetStageList(Action<StageMetaData[]> callback,
        Action fallback);
    public void UploadStage(string name, Stage data,
        Action<string> callback, Action<string> fallback);
    public void DeleteStage(string name, Action<string> callback, Action<string> fallback);

}