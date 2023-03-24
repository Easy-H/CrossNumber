using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class BasicUnitController : MonoBehaviour {

    [SerializeField] TextMeshProUGUI _txt;

    [SerializeField] Protector[] _protector = null;
    [SerializeField] GameObject _underline = null;

    private int _defaultLayer;

    private Vector3 posWhenPeak;

    //protected bool _isPeaked = false;
    bool _isCalced = true;
    bool _isPeaked = false;

    protected virtual void Start()
    {
        _defaultLayer = gameObject.layer;
    }

    public void SetValue(string value)
    {
        GetComponent<Unit>().SetValue(value);

        if (value.Equals("/"))
            _txt.text = "÷";
        else if (value.Equals("*"))
            _txt.text = "x";
        else
            _txt.text = value;

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
        _underline.SetActive(!_isCalced);
        return _isCalced;
    }
    public virtual void SetStateUnCalced()
    {

        _isCalced = false;

    }
    public virtual void SetStateCalced()
    {

        if (!_isCalced)
        {
            _underline.SetActive(false);
        }

        _isCalced = true;

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

    public void Pick()
    {

        posWhenPeak = transform.position;

        gameObject.layer = 2;
        _isPeaked = true;

    }
    public void Place()
    {

        gameObject.layer = _defaultLayer;
        _isPeaked = false;

    }
    public virtual bool CanPlace(Vector3 pos)
    {

        if (Physics2D.Raycast(pos, Vector2.down, 0.1f))
            return false;

        return true;
    }

    public static T ObjectCheck<T>(Vector3 pos)
    {
        T unit = default;

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f);

        if (hit)
            unit = hit.transform.GetComponent<T>();

        return unit;
    }
    public static Unit ObjectCheck(Vector3 pos, int layerValue)
    {
        Unit unit = default;

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f, layerValue);

        if (hit)
            unit = hit.transform.GetComponent<Unit>();

        return unit;
    }
    public static Unit ObjectCheck(Vector3 pos)
    {
        Unit unit = default;

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f);

        if (hit)
            unit = hit.transform.GetComponent<Unit>();

        return unit;
    }

}
