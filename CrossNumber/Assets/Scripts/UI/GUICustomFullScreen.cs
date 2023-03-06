using UnityEngine;
using UnityEngine.SceneManagement;

public class GUICustomFullScreen : GUIFullScreen {

    [SerializeField] Canvas _canvas;
    SceneChange _effect;

    Transform _trCamera;
    Transform _trBoard;

    protected Unit _selectedUnit;
    Vector3 _originMouseInput;

    bool _isMoving;

    enum MotionState { Idle, CameraMoving, UnitMoving }
    MotionState _state = MotionState.Idle;

    protected override void Open()
    {
        _trCamera = Camera.main.transform;
        _trBoard = GameObject.FindWithTag("Board").transform;
        _trCamera.position = Vector3.back * 10;

        _effect = UIManager.OpenGUI<SceneChange>("Capture");

        _trBoard = GameObject.FindWithTag("Board").transform;
        _state = MotionState.Idle;

        _effect.GetComponent<RectTransform>().SetParent(_canvas.transform);
        _effect.Show();

    }

    public void OpenScene(int idx)
    {
        GameManager.Instance._pause = false;
        SceneManager.LoadScene(idx);
    }
    private void Update()
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

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _selectedUnit = Unit.ObjectCheck(mousePos, Camera.main.cullingMask);

        if (_selectedUnit)
        {
            _state = MotionState.UnitMoving;
            _selectedUnit.Pick();

        }
        else
        {
            _state = MotionState.CameraMoving;
            _originMouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _trCamera.position;

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

        if (!_selectedUnit)
        {
            _state = MotionState.Idle;
            return;
        }

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
        _selectedUnit.Place();
        _selectedUnit = null;

    }

    virtual protected void UnitHold()
    {
        _selectedUnit.SetPos(Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10, out bool changed);

        if (changed)
        {
            SoundManager.Instance.PlayAudio("Move", false);
            UnitManager.Instance.CalculateWorld();

        }
    }

}
