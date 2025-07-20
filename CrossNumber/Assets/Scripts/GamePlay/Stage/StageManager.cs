using EHTool;
using EHTool.UIKit;
using System;
using EHTool.DBKit;

public class StageManager : MonoSingleton<StageManager>
{

    private IStageConnector _localStageConnector;
    private IStageConnector _cloudStageConnector;
    private IStageConnector _buildStageConnector;

    private IOverworld _cloudOverworld;

    protected override void OnCreate()
    {

        _localStageConnector = new LocalStageConnector();
        _buildStageConnector = new CustomStageConnector();

#if !UNITY_WEBGL || UNITY_EDITOR

        _cloudStageConnector = new FirestoreStageConnector();

#else
        _cloudStageConnector = GetComponent<FirestoreWebGLStageConnector>();
        _cloudStageConnector ??= gameObject.AddComponent<FirestoreWebGLStageConnector>();
#endif

        _cloudOverworld = new FirestoreOverworld();
        _cloudOverworld.SetConnector(_cloudStageConnector);

    }

    public IOverworld GetCloudOverworld()
    {
        return _cloudOverworld;
    }

    public IOverworld GetLocalOverworld()
    {

        IOverworld retval = AssetOpener.Import<OverworldData>
            ("Data/Overworld/Data_Overworld_LocalOverworld");
        retval.SetConnector(_localStageConnector);

        return retval;
    }

    public void GetLocalStageData(string key, Action<Stage> callback)
    {
        _buildStageConnector.GetLevelData(key, callback, () => { });
    }

    public void SaveBuildStage(string name, Stage data)
    {
        _buildStageConnector.UploadStage(name, data, (value) =>
        {
            UIManager.Instance.DisplayMessage(value);
        });
    }

    public void UploadStage(string name, Stage data)
    {
        
        if (!GameManager.Instance.Auth.IsSignIn())
        {
            UnityEngine.Debug.Log("NeedSignIn");
            //UIManager.Instance.DisplayMessage("NeedSignIn");
            return;
        }

        void UploadMapLocal(UserData userData)
        {

            if (userData.stageCnt > 4)
            {
                UIManager.Instance.DisplayMessage("msg_StageOverflow");
                return;
            }

            _cloudStageConnector.UploadStage(name, data, (value) =>
            {
                userData.stageCnt++;
                userData.mapIdList.Add(value);
                GameManager.Instance.Auth.UpdateUserData(userData);
                
            });

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

            UIManager.Instance.DisplayMessage("msg_UserDataLoadError");

        });

    }

}