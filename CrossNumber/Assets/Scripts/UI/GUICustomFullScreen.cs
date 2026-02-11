using UnityEngine;
using EasyH.Unity;
using EasyH.Unity.UI;
using System.Collections.Generic;
using System;

public class GUICustomFullScreen : GUIFullScreen {

    [SerializeField] private GameObject _container;
    [SerializeField] private Capture _capture;

    protected UnitMover _unitMover;
    protected CameraMover _cameraMover;

    protected Action _captureCallback;
    protected Action _effectCallback;

    protected enum MotionState { Idle, CameraMoving, UnitMoving }
    protected MotionState _motionState = MotionState.Idle;

    protected void Loading(Action callback)
    {
        _capture.CaptureScreen((texture) =>
        {
            ResourceManager.Instance.ResourceConnector.ImportComponent<SceneChange>
                ("Prefabs/GUI/GUI_Capture").Show(texture, _effectCallback);
            callback?.Invoke();
            _effectCallback = null;
        });

    }

    public override void Open()
    {
        _isSetting = true;
        _popupUI = new List<IGUIPopUp>();

        _motionState = MotionState.Idle;

        UnitManager.Instance.Refresh();

        _unitMover = new UnitMover();
        _cameraMover = new CameraMover(Camera.main.transform,
            GameObject.FindWithTag("Board").transform);

        _effectCallback = null;

        _container.SetActive(false);

        Loading(() =>
        {
            UIManager.Instance.OpenFullScreen(this);
            _container.SetActive(true);
            _captureCallback?.Invoke();
            _cameraMover.Reset();

        });

    }

    public override void SetOn()
    {
        base.SetOn();
        GameManager.Instance.Playground.Dispose();

        _motionState = MotionState.Idle;
        _container.SetActive(true);
    }

    public override void SetOff()
    {
        base.SetOff();
        _container.SetActive(false);
    }

    public override void Close()
    {
        Loading(() => {
            _container.SetActive(false);
            _cameraMover.Reset();
            base.Close();

        });
    }
    
    protected virtual void Update()
    {
        if (_nowPopUp != null)
        {
            _motionState = MotionState.Idle;
            return;
        }

        if (MobileUITouchDetector.IsPointerOverUIObject()) 
            return;

        switch (_motionState)
        {
            case MotionState.Idle:
                _Idle();
                break;
            case MotionState.UnitMoving:
                _UnitMoving();
                break;
            case MotionState.CameraMoving:
                _CameraMoving();
                break;
        }
    }

    protected Vector3 GetMousePos()
    {
        Vector3 mousePos =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return new Vector3(mousePos.x, mousePos.y, 0);
    }

    protected Vector2Int Vector3ToVector2Int(Vector3 origin)
    {
        return new Vector2Int(
            Mathf.RoundToInt(origin.x),
            Mathf.RoundToInt(origin.y));
    }

    private void _Idle()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Vector3 mousePos = GetMousePos();
        Vector2Int targetPos = Vector3ToVector2Int(mousePos);

        IMoveable moveable = GameManager.Instance.Playground.       
            GetMoveableAt(targetPos.x, targetPos.y);

        if (moveable == null) {
            _cameraMover.MoveStart(mousePos);
            _motionState = MotionState.CameraMoving;
            return;
        }

        MoveSet(moveable);
    }

    protected void MoveSet(IMoveable moveable)
    {
        if (moveable == null) return;

        _motionState = MotionState.UnitMoving;
        _unitMover.StartMove(moveable);

    }

    private void _CameraMoving()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _motionState = MotionState.Idle;
            return;
        }

        _cameraMover.Move(GetMousePos());

    }

    virtual protected void _UnitMoving()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _motionState = MotionState.Idle;
            UnitPlace(_unitMover.MoveEnd());
            return;
        }

        UnitHold();
    }

    virtual protected void UnitPlace(IUnitActionData data)
    {
        
    }

    virtual protected void UnitHold()
    {
        _unitMover.UnitMoveTo(Vector3ToVector2Int(GetMousePos()));
        UnitPosChangeEvent();
    }

    virtual protected void UnitPosChangeEvent() { }

}