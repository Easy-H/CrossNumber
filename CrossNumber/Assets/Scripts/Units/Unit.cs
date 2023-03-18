using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public enum UnitType {
    NumUnit,            NumZero,
    UseCalcAndSignChar, UseOnlyCalcChar,    EqualUnit,
    OverUnit,           Null
};

public class Unit : MonoBehaviour {

    [SerializeField] public UnitType _unitType = UnitType.Null;
    [SerializeField] TextMeshProUGUI _txt;

    [SerializeField] Protector[] _protector = null;
    [SerializeField] GameObject _underline = null;
    
    private int _defaultLayer;

    private Vector3 posWhenPeak;

    [SerializeField] protected string _value = "1";
    
    public string Value {
        get {
            return _value;
        }
        set {
            _value = value;
        }
    }

    protected bool _isPeaked = false;
    bool _isCalced = true;

    protected virtual void Start() {
        _defaultLayer = gameObject.layer;

        if (Value.Equals("/"))
            _txt.text = "÷";
        else if (Value.Equals("*"))
            _txt.text = "x";
        else
            _txt.text = Value;
    }

    public virtual void SetStateUnCalced() {

        _isCalced = false;

        SetProtector();

    }
    public virtual void SetStateCalced() {

        if (!_isCalced) {
            _underline.SetActive(false);
        }

        _isCalced = true;

    }

    public bool IsCalced()
    {
        _underline.SetActive(!_isCalced);
        return _isCalced;
    }

    public void Pick() {

        posWhenPeak = transform.position;

        gameObject.layer = 2;
        _isPeaked = true;

        UnitManager.Instance.SelectedUnitType = _unitType;
        ClearProtector();
        
    }

    public Vector3 GetPeakPos() { 
        return posWhenPeak;
    }

    protected virtual bool CanPlace(Vector3 pos) {

        if (Physics2D.Raycast(pos, Vector2.down, 0.1f))
            return false;

        return true;
    }

    public void SetPos(Vector3 pos, out bool changed) {

        changed = false;

        if (!CanPlace(pos))
            return;

        // 직전의 위치와 이동하고자 하는 위치를 비교했을 때 변화가 있는지 확인한다.
        Vector3 resultPos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 0);

        if ((transform.position - resultPos).magnitude < 0.1f)
            return;

        changed = true;
        transform.position = resultPos;

    }

    public void Place() {

        SetProtector();

        gameObject.layer = _defaultLayer;
        _isPeaked = false;

    }

    void ClearProtector() {

        for (int i = 0; i < _protector.Length; i++) {
            _protector[i].Clear();
        }

    }

    protected void SetProtector() {

        if (_isPeaked) {
            return;
        }

        for (int i = 0; i < _protector.Length; i++) {
            _protector[i].SetProtectorApear();
        }

    }

    public static T ObjectCheck<T>(Vector3 pos) {
        T unit = default;

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f);

        if (hit)
            unit = hit.transform.GetComponent<T>();

        return unit;
    }

    public static Unit ObjectCheck(Vector3 pos, int layerValue) {
        Unit unit = default;

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f, layerValue);

        if (hit)
            unit = hit.transform.GetComponent<Unit>();

        return unit;
    }

}
