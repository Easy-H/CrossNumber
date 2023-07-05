using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
public class UnitController : MonoBehaviour {

    Unit _data = new Unit();

    public static readonly int PlaceUnitLayer = 5;
    public static readonly int AllUnitLayer = 0;

    [SerializeField] TextMeshProUGUI _txt;
    [SerializeField] GameObject _underline = null;

    //protected bool _isPeaked = false;

    public void SetValue(Unit unitData)
    {
        _data = unitData;

        if (_data.Value.Equals("/"))
            _txt.text = "÷";
        else if (_data.Value.Equals("*"))
            _txt.text = "x";
        else
            _txt.text = _data.Value;

        transform.position = unitData.Pos;

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
