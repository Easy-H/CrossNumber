using EHTool.UIKit;
using UnityEngine;

public class GUICustomMessageBox : GUIMessageBox
{
    [SerializeField] TMPro.TextMeshProUGUI _text;
    [SerializeField] private UIAnimSequence _openSequence;
    [SerializeField] private UIAnimSequence _closeSequence;

    protected override void ShowMessage(string key)
    {
        _text.text = key;
    }

    public override void Open()
    {
        _openSequence.SetStart();
        base.Open();
        _openSequence.Action();
    }

    public override void SetOn()
    {
        _openSequence.SetStart();
        base.SetOn();
        _openSequence.Action();
    }

    public override void SetOff()
    {
        _closeSequence.SetStart();
        _closeSequence.Action(() =>
        {
            base.SetOff();
        });
    }

    public override void Close()
    {
        _closeSequence.SetStart();
        _closeSequence.Action(() =>
        {
            base.Close();
        });
    }

}
