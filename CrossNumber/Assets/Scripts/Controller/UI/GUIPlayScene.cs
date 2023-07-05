using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIPlayScene : GUICustomFullScreen {

    [SerializeField] GUIAnimatedOpen _clearAnim = null;
    [SerializeField] LevelMaker _setter;

    ClearChecker _checker;

    protected override void Open()
    {
        base.Open();
        SetStage();
    }

    public void SetStage()
    {
        _setter.MakeLevel();

        _checker = new ClearChecker();

        CalculateWorld();

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
        StartCoroutine(CalculateWorldAction());

    }

    IEnumerator CalculateWorldAction()
    {

        yield return new WaitForFixedUpdate();
        if (_checker.LevelCanClear() && _state == MotionState.Idle)
        {
            _StageClear();
        }

    }

    void _StageClear()
    {
        PlayAnim(_clearAnim);
        SoundManager.Instance.PlayAudio("Clear");
        ClearDataManager.Instance.Clear(StageManager.Instance.GetStageMetaData().value);
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
        SetStage();
    }

    public void GoNextStage()
    {

        if (StageManager.Instance.StageIdx + 1 < StageManager.Instance.GetStageCount())
        {
            StageManager.Instance.StageIdx += 1;
            ReloadScene();

            return;
        }

        StageManager.WorldIdx++;
        GoToOverWorld();

    }

    public void GoToOverWorld()
    {
        Close();
    }
    public override void Close()
    {
        base.Close();
        UnitManager.Instance.DestroyAllUnit();
    }

}