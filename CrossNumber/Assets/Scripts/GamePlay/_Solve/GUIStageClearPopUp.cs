using UnityEngine;

public class GUIStageClearPopUp : GUICustomPopUp
{
    private GUIPlay _target;

    public void SetTarget(GUIPlay target)
    {
        _target = target;
    }

    public void Clear()
    {
        _target.Close();
    }

    public void QuitStage()
    {
        _target.Close();
    }

    public void Retry()
    {
        _target.ReloadScene();
    }

}
