using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class UnitController : MonoBehaviour {

    protected Unit _data = new();

    public static readonly int PlaceUnitLayer = 5;
    public static readonly int AllUnitLayer = 0;

    [SerializeField] TextMeshProUGUI _txt;
    [SerializeField] GameObject _underline = null;

    //protected bool _isPeaked = false;

    public virtual void SetValue(string value, Vector3 pos)
    {
        _data = new Unit(value, pos);

        if (_data.Value.Equals("/"))
            _txt.text = "÷";
        else if (_data.Value.Equals("*"))
            _txt.text = "x";
        else
            _txt.text = _data.Value;

        transform.position = pos;

    }

    public Unit GetData()
    {
        return _data;
    }

    private void Update()
    {
        _underline.SetActive(!_data.IsCalced);
    }

}
