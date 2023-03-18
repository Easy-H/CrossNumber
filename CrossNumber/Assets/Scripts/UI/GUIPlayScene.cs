using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIPlayScene : GUICustomFullScreen
{
    [SerializeField] Transform _container = null;
    [SerializeField] GUIAnimatedOpen _clearAnim = null;
    [SerializeField] StageSetter _setter;

    MoveStack _moves;

    Unit[] _units;
    EqualUnit[] _equalUnits;

    protected override void Open()
    {
        base.Open();
        SetStage();
    }

    public void SetStage()
    {
        _moves = new MoveStack();

        _setter.SetStage();

        _units = FindObjectsOfType<Unit>();
        _equalUnits = FindObjectsOfType<EqualUnit>();

        CalculateWorld();

    }

    protected override void UnitHold()
    {
        base.UnitHold();
        CalculateWorld();
    }

    protected override void UnitPlace()
    {
        _selectedUnit.Place();
        _moves.AddMoveData(_selectedUnit, _selectedUnit.GetPeakPos(), _selectedUnit.transform.position);

        CalculateWorld();
        _selectedUnit = null;
    }

    // 뒤로가기 기능

    void SetAllUnitsStateUncalced()
    {

        for (int i = 0; i < _units.Length; i++)
        {
            _units[i].SetStateUnCalced();
        }

    }


    public void CalculateWorld()
    {

        StartCoroutine(CalculateWorldAction());

    }

    IEnumerator CalculateWorldAction()
    {

        yield return new WaitForFixedUpdate();

        SetAllUnitsStateUncalced();

        bool playErrorSound = true;
        bool canClear = true;

        for (int i = 0; i < _equalUnits.Length; i++)
        {
            if (!_equalUnits[i].Check())
            {
                canClear = false;
            }

        }
        if (playErrorSound)
        {
            SoundManager.Instance.PlayAudio("Wrong", false);
        }

        for (int i = 0; i < _units.Length; i++) {
            if (!_units[i].IsCalced())
                canClear = false;
        }

        if (canClear && _state == MotionState.Idle)
        {
            _StageClear();
        }

    }

    void _StageClear() {
        ClearDataManager.Instance.Clear(StageManager.Instance.GetStageMetaData().value);
        PlayAnim(_clearAnim);
    }

    public void MoveUndo()
    {
        _moves.Pop();
        CalculateWorld();

    }

    public void MoveRedo()
    {
        _moves.Back();
        CalculateWorld();


    }

    public void ReloadScene() {
        OpenScene(2);
    }

    public void GoNextStage() {
        if (StageManager.StageIdx + 1 < StageManager.Instance.GetStageCount()) {
            StageManager.StageIdx += 1;
            ReloadScene();
            
            return;
        }

        StageManager.WorldIdx++;
        GoToOverWorld();
            
    }

    public void GoToOverWorld() {
        OpenScene(1);
    }

}
