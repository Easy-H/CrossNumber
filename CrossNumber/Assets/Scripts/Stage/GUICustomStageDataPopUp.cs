using EHTool.UIKit;

public class GUICustomStageDataPopUp : GUICustomPopUp
{
    private StageMetaData _stageMetaData;

    public void SetTarget(StageMetaData stageMetaData)
    {
        _stageMetaData = stageMetaData;
    }

    public void Edit()
    {
        StageManager.Instance.GetStage("Custom", _stageMetaData.Key,
            (data) =>
            {
                UIManager.Instance.OpenGUI<GUIBuild>("Build").
                    SetStage(data, _stageMetaData.Name);
                BaseClose();

            }, () => { });

    }

    public void Upload()
    {
        StageManager.Instance.GetStage("Custom", _stageMetaData.Key,
            (data) =>
            {
                UIManager.Instance.OpenGUI<GUIPlay>("BuildTest").
                    SetStage(data, _stageMetaData.Name,
                    () =>
                    {
                        UIManager.Instance.OpenGUI<GUIUploadCloud>("Upload").SetStage(data);
                    });
                BaseClose();

            }, () => { });

    }

    public void Delete()
    {
        StageManager.Instance.DeleteCustomStage(_stageMetaData.Key,
        () =>
        {
            UnityEngine.Debug.Log("DeleteCustomStage");
            Close();
        }, (msg)=> { });
    }
    
}