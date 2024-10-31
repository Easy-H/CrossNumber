using EHTool;
using System;
using System.Collections.Generic;
using System.Xml;

public class StageManager : MonoSingleton<StageManager> {

    #region Legacy
    public static int WorldCount { get; private set; }
    public static int WorldIdx = 0;

    class OverWorldData {
        internal string name;
        internal int size = 0;
        internal List<StageMetaData> _stages;

        public void Read(XmlNode node)
        {
            name = node.Attributes["name"].Value;
            _stages = new List<StageMetaData>();

            XmlNodeList nodes = node.SelectNodes("Stage");
            for (int i = 0; i < nodes.Count; i++)
            {
                StageMetaData stageData = new StageMetaData();
                stageData.name = nodes[i].Attributes["name"].Value;
                stageData.value = nodes[i].Attributes["value"].Value;

                _stages.Add(stageData);

            }
            size = nodes.Count;
        }

        public StageMetaData ReturnStageData(int i)
        {
            return _stages[i];
        }
    }

    public int GetStageCount()
    {
        return _dic[WorldIdx].size;
    }

    public string GetWorldName()
    {
        return _dic[WorldIdx].name;
    }

    public StageMetaData GetStageMetaData(int idx)
    {
        return _dic[WorldIdx].ReturnStageData(idx);
    }

    List<OverWorldData> _dic;
    #endregion

    IStageConnector _localStageConnector;
    IStageConnector _firestoreStageConnector;

    protected override void OnCreate()
    {
        _dic = new List<OverWorldData>();
        XmlDocument xmlDoc = AssetOpener.ReadXML("StageListData");

        XmlNodeList nodes = xmlDoc.SelectNodes("StageListData/OverWorld");

        for (int i = 0; i < nodes.Count; i++)
        {
            OverWorldData overworldData = new OverWorldData();
            overworldData.Read(nodes[i]);

            _dic.Add(overworldData);
        }

        WorldCount = nodes.Count;
        _localStageConnector = new LocalStageConnector();

#if !UNITY_WEBGL || UNITY_EDITOR
        _firestoreStageConnector = new FirestoreStageConnector();
        
#else
        _firestoreStageConnector = GetComponent<FirestoreWebGLStageConnector>();
        _firestoreStageConnector ??= gameObject.AddComponent<FirestoreWebGLStageConnector>();

#endif
    }

    internal void GetLocalStageData(string key, CallbackMethod<StageData> callback)
    {
        _localStageConnector.GetLevelData(key, callback, () => { });
    }

    internal void SaveBuildStage(StageData data)
    {
        _localStageConnector.UploadStage("Temp", data);
    }

    internal void UploadStage(string name, StageData data) { 
        _firestoreStageConnector.UploadStage(name, data);
    }

    internal void GetWorldStageData(string key, CallbackMethod<StageData> callback)
    {
        _firestoreStageConnector.GetLevelData(key, callback, () => { });
    }

    internal void GetWorldRandomStageList(CallbackMethod<Dictionary<string, object>> value)
    {
        _firestoreStageConnector.GetRandomStage(value, () => { });
    }
}