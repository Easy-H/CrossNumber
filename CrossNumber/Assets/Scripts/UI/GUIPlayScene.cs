using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveData {
    public Unit unit { get; private set; }

    public Vector3 beforeMovePos { get; private set; }
    public Vector3 afterMovePos { get; private set; }

    public MoveData(Unit u, Vector3 origin, Vector3 moved)
    {
        unit = u;
        beforeMovePos = origin;
        afterMovePos = moved;
    }

}

public class GUIPlayScene : GUICustomFullScreen
{

    [SerializeField] GUIAnimatedOpen _clearAnim = null;
    [SerializeField] StageSetter _setter;

    CustomStack<MoveData> _moves;

    Unit[] _units;
    EqualUnit[] _equalUnits;

    protected override void Open()
    {
        base.Open();
        SetStage();
    }

    public void SetStage()
    {
        _moves = new CustomStack<MoveData>();

        _setter.SetStage();

        _units = FindObjectsOfType<Unit>();
        _equalUnits = FindObjectsOfType<EqualUnit>();

        CalculateWorld();

    }

    protected override void UnitPosChangeEvent()
    {
        base.UnitPosChangeEvent();
        CalculateWorld();
    }

    protected override void UnitPlace()
    {
        _selectedUnit.Place();
        _moves.AddData(new (_selectedUnit, _selectedUnit.GetPeakPos(), _selectedUnit.transform.position));

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

        bool canClear = true;

        for (int i = 0; i < _equalUnits.Length; i++)
        {
            if (!_equalUnits[i].Check())
            {
                canClear = false;
            }

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
        PlayAnim(_clearAnim);
        SoundManager.Instance.PlayAudio("Clear");
        ClearDataManager.Instance.Clear(StageManager.Instance.GetStageMetaData().value);
    }

    public void MoveUndo()
    {
        MoveData temp = _moves.Pop();
        if (temp == null) return;

        temp.unit.SetPos(temp.beforeMovePos);
        CalculateWorld();

    }

    public void MoveRedo()
    {
        MoveData temp = _moves.Back();
        if (temp == null) return;

        temp.unit.SetPos(temp.afterMovePos);
        CalculateWorld();


    }

    public void ReloadScene() {
        OpenScene(2);
    }

    public void GoNextStage() {

        if (StageManager.Instance.StageIdx + 1 < StageManager.Instance.GetStageCount()) {
            StageManager.Instance.StageIdx += 1;
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
