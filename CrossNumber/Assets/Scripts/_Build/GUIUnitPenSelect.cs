using UnityEngine;
using UnityEngine.UI;

public class GUIUnitPenSelect : MonoBehaviour
{
    [SerializeField] private string _value;
    [SerializeField] private Text _valueTxt;
    [SerializeField] private GameObject _selectGO;

    private GUIBuild _target;

    public void SetButtonInfor(GUIBuild target)
    {
        _target = target;
        _selectGO.SetActive(false);

        if (_value.Equals("Erase")) return;

        if (_value.Equals("*")) {
            _valueTxt.text = "x";
            return;
        }
        if (_value.Equals("/")) {
            _valueTxt.text = "÷";
            return;
        }
        _valueTxt.text = _value;
    }

    public void Select()
    {
        _target.ChangePen(this, _value);
        _selectGO.SetActive(true);
    }

    public void DisSelect()
    {
        _selectGO.SetActive(false);
    }
}
