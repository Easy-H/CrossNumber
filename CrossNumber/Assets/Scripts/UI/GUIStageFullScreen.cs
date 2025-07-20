using UnityEngine;
using EHTool.UIKit;

public class GUIStageFullScreen : GUICustomFullScreen
{

    [SerializeField] protected StageMaker _setter;
    [SerializeField] private GUIStagePausePopUp _pause;

    private CustomStack<IUnitActionData> _unitActionStack;

    protected Stage _data;

    public override void Open()
    {
        base.Open();
        _pause?.SetTarget(this);

        _unitActionStack = new CustomStack<IUnitActionData>();
    }

    public void SetStage(Stage data)
    {
        //_levelName = path;
        _data = data;

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
        _unitActionStack.Pop()?.Undo();
        CalculateWorld();
    }

    public void MoveRedo()
    {
        _unitActionStack.PopCancle().Redo();
        CalculateWorld();
    }

    public void Pause()
    {
        if (_pause == null)
        {
            _pause = UIManager.Instance.OpenGUI<GUIStagePausePopUp>("Pause");
            _pause?.SetTarget(this);
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

    public virtual void ReloadScene()
    {

    }

    protected virtual void ClearEvent() { }
    
}