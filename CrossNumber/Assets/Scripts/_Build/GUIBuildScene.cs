using EHTool;
using EHTool.UIKit;
using System.Xml;
using UnityEngine;
public class GUIBuildScene : GUICustomFullScreen {

    [SerializeField] LevelMaker _setter;
    [SerializeField] GUIAnimatedOpen _editToolContainer;
    [SerializeField] GUIPen _pen;

    string _penValue = "1";

    public override void SetOn()
    {
        base.SetOn();
        StageManager.Instance.GetLocalStageData("Temp", (data) =>
        {
            _setter.MakeLevel(data);
        });
        ChangePen("1");
    }

    public void ChangePen(string value)
    {
        _penValue = value;
        _pen.SetPen(_penValue);
    }

    protected override void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            base.Update();
            return;
        }

        if (MobileUITouchDetector.IsPointerOverUIObject()) return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Unit unit = GameManager.Instance.Playground.GetDataAt(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

        if (unit == null)
        {
            if (_penValue.Equals("Erase")) return;

            Unit newUnit = _setter.CreateUnit(_penValue, pos);

            if (newUnit)
            {
                SoundManager.Instance.PlayAudio("Move");
                MoveSet(newUnit);
            }

            return;
        }

        if (!_penValue.Equals("Erase")) {
            MoveSet(unit);
            return;
        }

        GameManager.Instance.Playground.RemoveUnitAt(unit);
    }

    public void GenerateWorld(string name)
    {

        XmlDocument Document = new XmlDocument();
        XmlElement FList = Document.CreateElement("StageData");
        Document.AppendChild(FList);

        Unit[] units = FindObjectsOfType<Unit>();

        for (int i = 0; i < units.Length; i++)
        {
            XmlElement FElement = Document.CreateElement("Unit");
            FElement.SetAttribute("value", units[i].Value);
            FElement.SetAttribute("xPos", units[i].transform.position.x.ToString());
            FElement.SetAttribute("yPos", units[i].transform.position.y.ToString());

            FList.AppendChild(FElement);
        }

        //Document.Save("Assets/Resources/XML/StageData/" + name + ".xml");
        //Debug.Log(name + " Saved");

        StageData newStage = new StageData(Document);
        StageManager.Instance.SaveBuildStage(newStage);

        GUIPlayScene temp = UIManager.Instance.OpenGUI<GUIPlayScene>("BuildTest");

        temp.SetStage(newStage);
        temp.GetComponent<GUIUploadCloud>().SetData(newStage);

    }

}
