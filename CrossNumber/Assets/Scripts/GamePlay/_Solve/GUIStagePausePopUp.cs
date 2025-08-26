using UnityEngine;

public class GUIStagePausePopUp : GUICustomPopUp
{
    [SerializeField] private TMPro.TextMeshProUGUI _stageNameUI;
    protected string _stageName;

    private GUIStageFullScreen _target;

    public void SetTarget(GUIStageFullScreen target, string stageName)
    {
        _target = target;
        _stageName = stageName;

        if (_stageNameUI != null)
        {
            _stageNameUI.text = stageName;
        }
    }

    // Update is called once per frame
    public void Retry()
    {
        _target?.ReloadScene();
    }

    public void QuitStage()
    {
        _target?.Close();
    }
}
