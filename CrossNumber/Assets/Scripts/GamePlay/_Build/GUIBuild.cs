using UnityEngine;

public class GUIBuild : GUIStageFullScreen
{
    [SerializeField] private GUIUnitPenSelect[] _penBtns;

    private string _penValue = "1";
    private GUIUnitPenSelect _beforeSelect;

    public override void Open()
    {
        base.Open();

        foreach (var btn in _penBtns)
        {
            btn.SetButtonInfor(this);
        }

        _penBtns[0].Select();
    }

    public override void SetOn()
    {
        base.SetOn();
    }

    public void ChangePen(GUIUnitPenSelect select, string value)
    {
        _penValue = value;
        _beforeSelect?.DisSelect();
        _beforeSelect = select;

        SoundManager.Instance.PlayAudio("Move");
    }

    protected override void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            base.Update();
            GameManager.Instance.Playground.HasError();
            return;
        }

        if (MobileUITouchDetector.IsPointerOverUIObject()) return;

        Vector2Int targetPos =
           Vector3ToVector2Int(GetMousePos());

        Unit unit = GameManager.Instance.Playground.
            GetDataAt(targetPos.x, targetPos.y);

        if (_penValue.Equals("Erase"))
        {
            if (unit == null) return;

            PushAction(new UnitDeleteData(_setter, unit));
            unit.Remove();
            GameManager.Instance.Playground.RemoveUnit(unit);
            SoundManager.Instance.PlayAudio("Move");
            GameManager.Instance.Playground.HasError();

            return;

        }

        if (unit == null)
        {
            Unit newUnit = _setter.CreateUnit(_penValue, targetPos);
            PushAction(new UnitCreateData(_setter, newUnit));

            if (newUnit)
            {
                SoundManager.Instance.PlayAudio("Move");
                MoveSet(newUnit);
            }

            return;
        }

        MoveSet(unit);
        return;
    }

    public override void ReloadScene()
    {
        base.ReloadScene();
    }

}
