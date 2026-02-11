using EasyH.Unity.UI;
using UnityEngine.UI;
using UnityEngine;

public class GUICustomStageDataPopUp : GUICustomPopUp
{
    private IGUI _parent;
    private StageMetaData _stageMetaData;
    [SerializeField] private TMPro.TextMeshProUGUI _stageName;

    public void SetTarget(StageMetaData stageMetaData, IGUI parent)
    {
        _stageMetaData = stageMetaData;
        _stageName.text = stageMetaData.Name;
        _parent = parent;
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
        StageManager.Instance.DeleteCustomStage(_stageMetaData.Name,
        () =>
        {
            _parent.SetOn();
            Close();
        }, (msg)=>
        {
            Debug.Log(msg);
        });
    }
    
}