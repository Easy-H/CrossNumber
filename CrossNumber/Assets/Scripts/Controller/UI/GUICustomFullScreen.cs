using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class GUICustomFullScreen : GUIFullScreen {

    [SerializeField] Canvas _canvas;
    SceneChange _effect;

    Transform _trCamera;
    Transform _trBoard;

    protected UnitMover mover;
    Vector3 _originMouseInput;

    bool _isMoving;

    protected enum MotionState { Idle, CameraMoving, UnitMoving }
    protected MotionState _state = MotionState.Idle;

    protected override void Open()
    {
        _trCamera = Camera.main.transform;
        _trBoard = GameObject.FindWithTag("Board").transform;
        _trCamera.position = Vector3.back * 10;

        // 나중에 장면 전환 효과를 넣기
        
        _effect = UIManager.OpenGUI<SceneChange>("Capture");

        _trBoard = GameObject.FindWithTag("Board").transform;
        _state = MotionState.Idle;


        UIManager.Instance.EnrollmentGUI(this);

        mover = new UnitMover();

    }

    public void Pop() {
        _state = MotionState.Idle;
        gameObject.SetActive(true);
    }

    public override void Close() {
        base.Close();
        GameManager.Instance._pause = false;
    }

    public void OpenScene(int idx)
    {
        GameManager.Instance._pause = false;
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

        if (GameManager.Instance._pause == true)
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

        UnitController _selectedUnit = UnitManager.GetUnitControllerAt(mousePos);

        if (_selectedUnit)
        {
            _state = MotionState.UnitMoving;
            _originMouseInput = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);
            mover.StartMove(_selectedUnit.transform);

        }

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

        mover.UnitMoveTo(placePos);
        _originMouseInput = placePos;
        UnitPosChangeEvent();
    }

    virtual protected void UnitPosChangeEvent() {

    }

}
