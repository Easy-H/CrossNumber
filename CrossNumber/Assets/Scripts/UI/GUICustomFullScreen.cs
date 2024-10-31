using UnityEngine;
using UnityEngine.SceneManagement;
using EHTool.UIKit;
using System.Collections.Generic;
using EHTool;

public class GUICustomFullScreen : GUIFullScreen {

    [SerializeField] Transform _trContainer;
    [SerializeField] GameObject _container;
    [SerializeField] Capture _capture;

    public Canvas _canvas;

    Transform _trCamera;
    Transform _trBoard;

    protected UnitMover mover;
    Vector3 _originMouseInput;

    protected CallbackMethod _captureCallback;
    protected CallbackMethod _effectCallback;

    protected enum MotionState { Idle, CameraMoving, UnitMoving }
    protected MotionState _state = MotionState.Idle;

    protected void CaptureAndEvent(CallbackMethod callback)
    {
        _capture.CaptureScreen((texture) =>
        {
            AssetOpener.Import<SceneChange>("Prefabs/GUI/GUI_Capture").Show(texture, _effectCallback);
            callback?.Invoke();
            _effectCallback = null;
        });

    }

    public override void Open()
    {
        _isSetting = true;

        _trCamera = Camera.main.transform;
        _trCamera.position = Vector3.back * 10;

        _popupUI = new List<IGUIPopUp>();

        _trBoard = GameObject.FindWithTag("Board").transform;
        _state = MotionState.Idle;

        UnitManager.Instance.Refresh();

        mover = new UnitMover();

        _effectCallback = null;

        _container.SetActive(false);

        CaptureAndEvent(() => {
            UIManager.Instance.OpenFullScreen(this);
            _container.SetActive(true);
            _captureCallback?.Invoke();

        });

    }

    public override void SetOn()
    {
        base.SetOn();
        GameManager.Instance.Playground.Dispose();
        _container.SetActive(true);
    }

    public override void SetOff()
    {
        base.SetOff();
        _container.SetActive(false);
    }

    public override void Close()
    {
        CaptureAndEvent(() => {
            _container.SetActive(false);
            base.Close();

        });
    }

    public void Pop() {
        _state = MotionState.Idle;
        gameObject.SetActive(true);
    }

    public void OpenScene(int idx)
    {
        SceneManager.LoadScene(idx);
    }

    public override void OpenWindow(string key) {
        base.OpenWindow(key);
        _state = MotionState.Idle;
    }

    public void PlayAnim(GUIAnimatedOpen gui)
    {
        gui.Open();
    }
    
    protected virtual void Update()
    {

        if (_nowPopUp != null)
        {
            _state = MotionState.Idle;
            return;
        }

        switch (_state)
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

    void _Idle()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10;
        Unit selectedUnit = UnitManager.Instance.GetUnitControllerAt(mousePos);

        IMoveable moveable = GameManager.Instance.Playground.GetMoveableAt(
            Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));

        MoveSet(selectedUnit);
        MoveSet(moveable);
    }

    protected void MoveSet(IMoveable unit)
    {
        if (unit == null) return;

        _state = MotionState.UnitMoving;
        _originMouseInput = new Vector3(unit.Pos.x, unit.Pos.y, 0);
        mover.StartMove(unit);

    }

    void _CameraMoving()
    {

        if (Input.GetMouseButtonUp(0))
        {
            _state = MotionState.Idle;
            return;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _trCamera.position;

        _trCamera.Translate(_originMouseInput - mousePos);
        _trBoard.position = new Vector3(Mathf.Round(_trCamera.position.x), Mathf.Round(_trCamera.position.y), 10);

        _originMouseInput = mousePos;

    }

    virtual protected void _UnitMoving()
    {

        if (Input.GetMouseButtonUp(0))
        {
            _state = MotionState.Idle;
            UnitPlace();
            return;
        }

        UnitHold();
    }

    virtual protected void UnitPlace()
    {
        mover.MoveEnd();

    }

    virtual protected void UnitHold()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10;

        Vector3 placePos = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);

        if ((placePos - _originMouseInput).sqrMagnitude < 0.1f)
        {
            return;
        }

        mover.UnitMoveTo(new Vector2Int(
            Mathf.RoundToInt(placePos.x), Mathf.RoundToInt(placePos.y)));
        _originMouseInput = placePos;
        UnitPosChangeEvent();
    }

    virtual protected void UnitPosChangeEvent() {

    }

}
