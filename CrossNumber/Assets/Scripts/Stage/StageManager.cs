using EHTool;
using System;

public class StageManager : MonoSingleton<StageManager> {

    private IStageConnector _localStageConnector;
    private IStageConnector _firestoreStageConnector;

    private IOverworld _firestoreOverworld;

    protected override void OnCreate()
    {
        
        _localStageConnector = new LocalStageConnector();

#if !UNITY_WEBGL || UNITY_EDITOR
        _firestoreStageConnector = new FirestoreStageConnector();
        
#else
        _firestoreStageConnector = GetComponent<FirestoreWebGLStageConnector>();
        _firestoreStageConnector ??= gameObject.AddComponent<FirestoreWebGLStageConnector>();
#endif

        _firestoreOverworld = new FirestoreOverworld();
        _firestoreOverworld.SetConnector(_firestoreStageConnector);

    }

    public IOverworld GetFirestoreOverworld() {
        return _firestoreOverworld;
    }

    public IOverworld GetLocalOverworld() {
        
        IOverworld retval = AssetOpener.Import<OverworldData>
            ("Data/Overworld/Data_Overworld_LocalOverworld");
        retval.SetConnector(_localStageConnector);

        return retval;
    }

    public void GetLocalStageData(string key, Action<Stage> callback)
    {
        _localStageConnector.GetLevelData(key, callback, () => { });
    }

    public void SaveBuildStage(string name, Stage data)
    {
        _localStageConnector.UploadStage(name, data);
    }

    public void UploadStage(string name, Stage data) { 
        _firestoreStageConnector.UploadStage(name, data);
    }

}