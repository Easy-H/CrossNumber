using EHTool.UIKit;
using System.Xml;
using UnityEngine;

public class GUIBuild : GUICustomFullScreen {
    
    [SerializeField] private StageMaker _setter;
    [SerializeField] private GUIUnitPenSelect[] _penBtns;

    private string _penValue = "1";
    private GUIUnitPenSelect _beforeSelect;

    public override void Open()
    {
        base.Open();

        foreach (var btn in _penBtns) {
            btn.SetButtonInfor(this);
        }

        _penBtns[0].Select();
    }

    public override void SetOn()
    {
        base.SetOn();
        StageManager.Instance.GetLocalStageData("Temp", (data) =>
        {
            _setter.MakeLevel(data);
        });
    }

    public void ChangePen(GUIUnitPenSelect select, string value)
    {
        _penValue = value;
        if (_beforeSelect != null) {
            _beforeSelect.DisSelect();
        }
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

        Vector3 pos = Camera.main.ScreenToWorldPoint
            (Input.mousePosition);
        Vector2Int targetPos =
            new Vector2Int(Mathf.RoundToInt(pos.x),
                Mathf.RoundToInt(pos.y));

        Unit unit = GameManager.Instance.Playground.
            GetDataAt(targetPos.x, targetPos.y);

        if (unit == null)
        {
            if (_penValue.Equals("Erase")) return;

            Unit newUnit = _setter.CreateUnit(_penValue, targetPos);

            if (newUnit)
            {
                SoundManager.Instance.PlayAudio("Move");
                MoveSet(newUnit);
            }
            GameManager.Instance.Playground.HasError();

            return;
        }

        if (!_penValue.Equals("Erase")) {
            MoveSet(unit);
            GameManager.Instance.Playground.HasError();
            return;
        }

        GameManager.Instance.Playground.RemoveUnitAt(unit);
        SoundManager.Instance.PlayAudio("Move");
        GameManager.Instance.Playground.HasError();
    }

    public void GenerateWorld(string name)
    {

        Unit[] units = FindObjectsOfType<Unit>();
        UnitInfor[] unitInfors = new UnitInfor[units.Length];

        for (int i = 0; i < units.Length; i++) {
            unitInfors[i] = new UnitInfor
                (units[i].Value, units[i].Pos);
        }

        Stage newStage = new Stage(unitInfors);
        StageManager.Instance.SaveBuildStage("Temp", newStage);

        UIManager.Instance.OpenGUI<GUIPlay>
            ("BuildTest").SetStage(newStage);
        
    }

}
