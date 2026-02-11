using System;
using UnityEngine;
using EasyH.Unity.UI;

public class GUIStageFullScreen : GUICustomFullScreen
{

    [SerializeField] protected StageMaker _setter;
    [SerializeField] private GUIStagePausePopUp _pause;

    private CustomStack<IUnitActionData> _unitActionStack;

    protected Stage _data;
    protected string _stageName;

    private Action _clearAction;

    public override void Open()
    {
        base.Open();

        _unitActionStack = new CustomStack<IUnitActionData>();
    }

    public void SetStage(Stage data, string stageName, Action clearCallback = null)
    {
        _data = data;
        _stageName = stageName;
        _clearAction = clearCallback;
        
        _pause?.SetTarget(this, _stageName);

        _captureCallback += () =>
        { 
            _setter.MakeLevel(_data);
        };

        _effectCallback += () =>
        { 
            GameManager.Instance.Playground.HasError();
        };

    }
    
    protected void PushAction(IUnitActionData data)
    {
        _unitActionStack.Push(data);
    }

    // 뒤로가기 기능
    public void MoveUndo()
    {
        IUnitActionData data = _unitActionStack.Pop();

        if (data == null) return;

        data.Undo();
        CalculateWorld();
    }

    public void MoveRedo()
    {
        IUnitActionData data = _unitActionStack.PopCancle();

        if (data == null) return;

        data.Redo();
        CalculateWorld();
    }

    public void Pause()
    {
        if (_pause == null)
        {
            _pause = UIManager.Instance.OpenGUI<GUIStagePausePopUp>("Pause");
            _pause?.SetTarget(this, _stageName);
        }
        _pause?.SetOn();
    }

    protected void CalculateWorld()
    {
        if (GameManager.Instance.Playground.HasError()) return;
        if (_motionState != MotionState.Idle) return;

        ClearEvent();
    }

    protected override void UnitPosChangeEvent()
    {
        base.UnitPosChangeEvent();
        CalculateWorld();
    }

    protected override void UnitPlace(IUnitActionData data)
    {
        base.UnitPlace(data);
        if (data == null) return;
        
        PushAction(data);
        CalculateWorld();
    }

    public virtual void Clear()
    {
        _clearAction?.Invoke();
    }

    public virtual void ReloadScene() { }

    protected virtual void ClearEvent() { }
    
}