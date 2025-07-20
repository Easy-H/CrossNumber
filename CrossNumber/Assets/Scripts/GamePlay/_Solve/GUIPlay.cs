using EHTool.UIKit;
using UnityEngine;

public class GUIPlay : GUIStageFullScreen
{
    [SerializeField] private GUIStageClearPopUp _clear;

    private string _levelName;

    protected override void ClearEvent()
    {
        if (_clear == null)
        {
            _clear = UIManager.Instance.OpenGUI<GUIStageClearPopUp>("StageClear");
        }

        _clear.SetTarget(this);
        _clear.SetOn();

        SoundManager.Instance.PlayAudio("Clear");

    }


    public override void ReloadScene()
    {
        UIManager.Instance.OpenGUI<GUIPlay>("Play").
            SetStage(_data);
        Close();
    }

    public void GoNextStage()
    {
        Close();

        if (_data._nextStagePath == null)
        {
            Close();
            return;
        }

        //SetStage(new Stage(_data._nextStagePath));

    }

}