using System;

public class Overworld : IOverworld
{

    public string OverworldName { get; private set; }

    private IStageConnector _connector;

    public void Setting(string name, IStageConnector connector)
    {

        OverworldName = name;
        _connector = connector;
    }

    public void GetStageList(
        Action<StageMetaData[]> callback, Action fallback)
    {
        _connector.GetStageList(callback, fallback);

    }

}