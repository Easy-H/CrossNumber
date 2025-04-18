using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Data_Overworld_", menuName = "Custom/OverworldData", order = 1)]
public class OverworldData : ScriptableObject, IOverworld
{
    public string Name;
    
    public StageMetaData[] Stages;
    
    public string OverworldName => Name;

    private IStageConnector _connector;

    public void SetConnector(IStageConnector connector) {
        _connector = connector;
    }
    
    public void GetStage(string value,
        Action<Stage> callback, Action fallback)
    {
        _connector.GetLevelData(value, callback, fallback);
    }

    public void GetStageList(
        Action<StageMetaData[]> callback, Action fallback)
    {
        callback?.Invoke(Stages);
    }
}
