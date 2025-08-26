using EHTool.UIKit;
using UnityEngine;

public class GUICustomPopUp : GUIPopUp
{

    [SerializeField] private UIAnimSequence _openSequence;
    [SerializeField] private UIAnimSequence _closeSequence;

    public override void Open()
    {
        base.Open();
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
        gameObject.SetActive(true);
        _closeSequence.SetStart();
        _closeSequence.Action(() =>
        {
            base.Close();
        });
    }

    protected void BaseClose()
    {
        base.Close();
    }

}