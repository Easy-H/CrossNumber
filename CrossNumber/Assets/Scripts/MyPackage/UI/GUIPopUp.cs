using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GUIPopUp : GUIWindow {

    [SerializeField] UnityEvent _openEvent;
    protected override void Open()
    {
        if (UIManager.Instance.NowPopUp == null)
        {
            base.Open();
            return;
        }

        _openEvent.Invoke();

        RectTransform rect = gameObject.GetComponent<RectTransform>();

        rect.SetParent(UIManager.Instance.NowPopUp._canvas.transform);
        rect.offsetMax = Vector3.zero;
        rect.offsetMin = Vector3.zero;
    }
}
