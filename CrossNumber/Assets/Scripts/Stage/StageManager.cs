using EHTool;
using System;
using System.Collections.Generic;

public class StageManager : MonoSingleton<StageManager>
{

    private IDictionary<string, IStageConnector> _stageConnectorDict;

    protected override void OnCreate()
    {
        _stageConnectorDict = new Dictionary<string, IStageConnector>()
        {
            { "Custom", new CustomStageConnector() }
        };

        _stageConnectorDict.Add("Local",
            AssetOpener.Import<LocalOverworld>
            ("Data/Overworld/Data_Overworld_LocalOverworld"));

        IStageConnector cloudConnector;

#if !UNITY_WEBGL || UNITY_EDITOR
        cloudConnector = new FirestoreStageConnector();
#else
        cloudConnector = GameManager.Instance.
            GetComponent<FirestoreWebGLStageConnector>()
        cloudConnector ??= GameManager.Instance.
            AddComponent<FirestoreWebGLStageConnector>();
#endif
        _stageConnectorDict.Add("Cloud", cloudConnector);

    }

    public void GetStage(string type, string value,
        Action<Stage> callback, Action fallback)
    {
        _stageConnectorDict[type].GetStage(
            value, callback, fallback);
    }
    
    //public IOverworld GetOverworld(string name) => _stageConnectorDict[name];

    public IOverworld GetOverworld(string name)
    {
        IOverworld ret = new Overworld();
        ret.Setting(name, _stageConnectorDict[name]);

        return ret;
    }

    public void AddCustomStage(string name, Stage data,
        Action callback, Action<string> fallback)
    {

        _stageConnectorDict["Custom"].UploadStage(name, data,
            (msg) => { callback?.Invoke(); }, fallback);

    }

    public void DeleteCustomStage(string name,
        Action callback, Action<string> fallback)
    {
        _stageConnectorDict["Custom"].DeleteStage(
            name, (msg) => { callback?.Invoke(); }, fallback);
    }

    public void AddCloudStage(string name, Stage data,
        Action callback, Action<string> fallback)
    {

        if (!GameManager.Instance.Auth.IsSignIn())
        {
            fallback?.Invoke("NeedSignIn");
            UnityEngine.Debug.Log("NeedSignIn");
            //UIManager.Instance.DisplayMessage("NeedSignIn");
            return;
        }

        void UploadMapLocal(UserData userData)
        {

            if (userData.stageCnt > 4)
            {
                fallback?.Invoke("msg_StageOverflow");
                return;
            }

            _stageConnectorDict["Cloud"].UploadStage(name, data,
            (value) =>
            {
                userData.stageCnt++;
                userData.mapIdList.Add(value);
                GameManager.Instance.Auth.UpdateUserData(userData);
                callback?.Invoke();

            }, fallback);

        }

        GameManager.Instance.Auth.GetUserData((userData) =>
        {
            UploadMapLocal(userData);

        }, (error) =>
        {
            if (error.Equals("No Idx"))
            {
                UserData userData = new UserData();
                UploadMapLocal(userData);

                return;
            }

            fallback?.Invoke("msg_UserDataLoadError");

        });
    }
    
}