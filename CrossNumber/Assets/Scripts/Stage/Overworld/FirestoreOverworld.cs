using System;
using System.Collections.Generic;

public class FirestoreOverworld : IOverworld {

    public string OverworldName { get; }

    private IStageConnector _connector;

    public void SetConnector(IStageConnector connector) {
        _connector = connector;
    }
    
    public void GetStageList(
        Action<StageMetaData[]> callback, Action fallback) {
        
        _connector.GetRandomStage((data) => {
            
            StageMetaData[] retval =
                new StageMetaData[data.Values.Count];

            int i = 0;

            foreach (object d in data.Values)
            {
                Dictionary<string, object> dd = d as Dictionary<string, object>;

                retval[i++] = new StageMetaData
                    (dd["name"].ToString(), dd["key"].ToString());

            }

            callback?.Invoke(retval);
            
        }, fallback);
    }

    public void GetStage(string value,
        Action<Stage> callback, Action fallback) {
        _connector.GetLevelData(value, callback, fallback);
    }


}