using EHTool.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class GUIUploadCloud : GUICustomPopUp
{
    [SerializeField] private InputField _name;

    private Stage _stageData;

    public void SetStage(Stage stageData)
    {
        _stageData = stageData;
    }

    public void Upload()
    {

        StageManager.Instance.AddCloudStage(
            _name.text, _stageData, () => { }, (msg) => { });

    }
}
