using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Camera _traceCamera = null;

    [SerializeField] Transform _trCamera = null;
    [SerializeField] Transform _trBoard = null;
    
    public bool _isMoving = false;

    Unit _selectedUnit;

    Vector3 _originMouseInput;
    Vector3 _originUnitPos;

    bool canClear;

    private void Awake()
    {
    }

    private void Start() {
        UnitManager.WhenNewSceneLoaded();
        MoveData.Instance.WhenNewSceneLoaded();

        Instance = this;
    }

    private void Update () {
        
        if (Input.GetMouseButtonDown(0)) {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            _selectedUnit = Unit.ObjectCheck(mousePos, Camera.main.cullingMask);

            if (!_selectedUnit) {
                _isMoving = true;
                _originMouseInput = _traceCamera.ScreenToWorldPoint(Input.mousePosition);

            }
            else {
                _selectedUnit.Pick();

            }
            return;

        }

        if (Input.GetMouseButton(0)) {

            if (_isMoving) {
                Vector3 mousePos = _traceCamera.ScreenToWorldPoint(Input.mousePosition);

                _trCamera.Translate(_originMouseInput - mousePos);
                _trBoard.position = new Vector3(Mathf.Round(_trCamera.position.x), Mathf.Round(_trCamera.position.y), 1);

                _originMouseInput = mousePos;
                
            }

            if (!_selectedUnit)
                return;

            _selectedUnit.Hold(Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10);

        }

        if (Input.GetMouseButtonUp(0)) {

            if (!_selectedUnit) {
                _isMoving = false;
                return;
            }

            _selectedUnit.Place();
            _selectedUnit = null;

        }

    }


}
