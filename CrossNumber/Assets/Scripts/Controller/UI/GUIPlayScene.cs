using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GUIPlayScene : GUICustomFullScreen {

    [SerializeField] GUIAnimatedOpen _clearAnim = null;
    [SerializeField] LevelMaker _setter;

    ClearChecker _checker;

    string _levelName;
    LevelData _data;

    protected override void Open()
    {
        base.Open();
    }

    public void SetStage(string path)
    {
        _levelName = path;
        _data = new LevelData(AssetOpener.ReadXML("StageData/" + path));

        Generate();

    }
    private void Generate() {
        _setter.MakeLevel(_data);
        _checker = new ClearChecker(_setter.Units, _setter.EqualUnits);

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
        //ClearDataManager.Instance.Clear(StageManager.Instance.GetStageMetaData().value);
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
        UIManager.OpenGUI<GUIPlayScene>("Play").SetStage(_levelName);
        Close();
    }

    public void GoNextStage()
    {
        if (_data._nextStagePath == null) {
            Close();
            return;
        }

        SetStage(_data._nextStagePath);

    }

    public void GoToOverWorld()
    {
        Close();
    }

    public override void Close()
    {
        base.Close();
    }

}