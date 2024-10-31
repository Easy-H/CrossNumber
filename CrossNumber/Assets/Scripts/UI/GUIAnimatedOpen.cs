using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIAnimatedOpen : GUIPopUp {

    [SerializeField] UIAnimSequence _openSequence;
    [SerializeField] UIAnimSequence _closeSequence;

    public override void Open()
    {
        base.Open();
        _openSequence.Action();
    }

    public override void SetOn()
    {
        base.SetOn();
        _openSequence.Action();
    }

    public override void SetOff()
    {
        _closeSequence.Action(base.SetOff);
    }

    public override void Close()
    {
        _closeSequence.Action(base.Close);

    }

    public void Toggle() {
        if (gameObject.activeSelf) {
            SetOff();
            return;
        }
        SetOn();

    }

}