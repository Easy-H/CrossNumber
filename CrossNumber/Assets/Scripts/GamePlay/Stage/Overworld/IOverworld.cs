using System;

[Serializable]
public class StageMetaData {
    public string name;
    public string key;

    public StageMetaData(string n, string v) {
        name = n;
        key = v;
    }

}

public interface IOverworld {

    public string OverworldName { get; }

    public void SetConnector(IStageConnector connector);

    public void GetStageList(
        Action<StageMetaData[]> callback, Action fallback);

    public void GetStage(string value,
        Action<Stage> callback, Action fallback);

}