using EHTool.UIKit;
using System.Collections;
using UnityEngine;

public class GUIPlay : GUICustomFullScreen {

    [SerializeField] private GUIAnimatedOpen _clear;
    [SerializeField] private StageMaker _setter;

    private string _levelName;
    private Stage _data;

    public void SetStage(Stage data)
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
        _unitMover.Undo();
        CalculateWorld();
    }

    public void MoveRedo()
    {
        _unitMover.Redo();
        CalculateWorld();

    }

    public void ReloadScene()
    {
        UIManager.Instance.OpenGUI<GUIPlay>("Play").SetStage(_data);
        Close();
    }

    public void GoNextStage()
    {
        Close();
        return;
        
        if (_data._nextStagePath == null) {
            Close();
            return;
        }

        //SetStage(new Stage(_data._nextStagePath));

    }

    public void GoToOverWorld()
    {
        Close();
    }

}