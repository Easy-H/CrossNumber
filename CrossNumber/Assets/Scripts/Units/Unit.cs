using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class UnitData {

    public UnitData() {
        Value = "1";
        IsCalced = false;
    }

    public string Value { get; set; }
    public bool IsCalced;

    public void SetStateCalced() {
        IsCalced = true;
    }
}

public class Unit : MonoBehaviour {

    UnitData _data = new UnitData();

    public static readonly int PlaceUnitLayer = 5;
    public static readonly int AllUnitLayer = 0;

    [SerializeField] TextMeshProUGUI _txt;

    [SerializeField] Protector[] _protector = null;
    [SerializeField] GameObject _underline = null;
    
    private int _defaultLayer;

    private Vector3 posWhenPeak;

    //protected bool _isPeaked = false;
    bool _isPeaked = false;

    protected virtual void Start() {
        _defaultLayer = gameObject.layer;

    }

    public void SetValue(string value)
    {
        _data.Value = value;

        if (_data.Value.Equals("/"))
            _txt.text = "÷";
        else if (_data.Value.Equals("*"))
            _txt.text = "x";
        else
            _txt.text = _data.Value;

    }

    public UnitData GetData() {
        return _data;
    }

    // 요건 GUIPlayScene에서 유닛을 선택했을 때 작동하도록 함
    protected void SetProtector()
    {
        if (_isPeaked)
        {
            return;
        }

        for (int i = 0; i < _protector.Length; i++)
        {
            _protector[i].SetProtectorApear();
        }

    }

    public bool IsCalced()
    {
        _underline.SetActive(!_data.IsCalced);
        return _data.IsCalced;
    }

    public virtual void SetStateUnCalced() {

        _data.IsCalced = false;

    }
    public virtual void SetStateCalced() {

        if (!_data.IsCalced) {
            _underline.SetActive(false);
        }

        _data.IsCalced = true;

    }


    public Vector3 GetPos()
    {
        return transform.position;
    }
    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }
    public Vector3 GetPeakPos()
    {
        return posWhenPeak;
    }

    public void Pick() {

        posWhenPeak = transform.position;

        gameObject.layer = 2;
        _isPeaked = true;

    }
    public void Place() {

        gameObject.layer = _defaultLayer;
        _isPeaked = false;

    }
    public virtual bool CanPlace(Vector3 pos)
    {

        if (Physics2D.Raycast(pos, Vector2.down, 0.1f))
            return false;

        return true;
    }

}
