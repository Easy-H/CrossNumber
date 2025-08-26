using System;

public interface IOverworld
{

    public string OverworldName { get; }

    public void Setting(string name, IStageConnector connector);

    public void GetStageList(
        Action<StageMetaData[]> callback, Action fallback);

}