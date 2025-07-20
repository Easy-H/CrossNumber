using UnityEngine;

public class GUIStagePausePopUp : GUICustomPopUp
{
    private GUIStageFullScreen _target;

    public void SetTarget(GUIStageFullScreen target)
    {
        _target = target;
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
