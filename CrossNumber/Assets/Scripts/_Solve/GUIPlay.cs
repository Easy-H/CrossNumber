using EHTool.UIKit;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIPlay : GUICustomFullScreen {

    [SerializeField] GUIAnimatedOpen _clear;
    [SerializeField] LevelMaker _setter;

    string _levelName;
    StageData _data;

    public void SetStage(StageData data)
    {
        //_levelName = path;
        _data = data;
        _captureCallback += Generate;
        _effectCallback += CalculateWorld;

    }

    private void Generate() {
        _setter.MakeLevel(_data);
    }

    protected override void UnitPosChangeEvent()
    {
        base.UnitPosChangeEvent();
        CalculateWorld();
    }

    protected override void UnitPlace()
    {
        base.UnitPlace();
        CalculateWorld();
    }

    // 뒤로가기 기능
    public void CalculateWorld()
    {
        if (!GameManager.Instance.Playground.HasError() && _state == MotionState.Idle)
        {
            _StageClear();
        }

    }

    IEnumerator CalculateWorldAction()
    {

        yield return new WaitForFixedUpdate();
        if (!GameManager.Instance.Playground.HasError() && _state == MotionState.Idle)
        {
            _StageClear();
        }

    }

    void _StageClear()
    {
        _clear.SetOn();
        SoundManager.Instance.PlayAudio("Clear");
    }

    public void MoveUndo()
    {
        mover.Undo();
        CalculateWorld();
    }

    public void MoveRedo()
    {
        mover.Redo();
        CalculateWorld();

    }

    public void ReloadScene()
    {
        UIManager.Instance.OpenGUI<GUIPlay>("Play").SetStage(_data);
        Close();
    }

    public void GoNextStage()
    {
        if (_data._nextStagePath == null) {
            Close();
            return;
        }

        SetStage(new StageData(_data._nextStagePath));

    }

    public void GoToOverWorld()
    {
        Close();
    }

}