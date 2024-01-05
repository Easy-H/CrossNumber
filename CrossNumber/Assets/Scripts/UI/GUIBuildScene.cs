using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.XR;
public class GUIBuildScene : GUICustomFullScreen {

    [SerializeField] LevelMaker _setter;
    [SerializeField] GUIAnimatedOpen _editToolContainer;
    [SerializeField] GUIPen _pen;

    string _penValue = "1";

    protected override void Open()
    {
        base.Open();
        ChangePen("1");
        //_setter.MakeLevel("Temp");
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

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);
        pos.z = 0;

        UnitController unit = UnitManager.Instance.GetUnitControllerAt(pos);

        if (unit == null)
        {
            if (_penValue.Equals("Erase")) return;

            UnitController newUnit = UnitManager.Instance.BuilderCreateUnit(_penValue, pos);
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

        UnitManager.Instance.DestroyUnit(unit);
    }

    public void GenerateWorld(string name)
    {

        XmlDocument Document = new XmlDocument();
        XmlElement FList = Document.CreateElement("StageData");
        Document.AppendChild(FList);

        UnitController[] units = FindObjectsOfType<UnitController>();

        for (int i = 0; i < units.Length; i++)
        {
            XmlElement FElement = Document.CreateElement("Unit");
            FElement.SetAttribute("value", units[i].GetData().Value);
            FElement.SetAttribute("xPos", units[i].transform.position.x.ToString());
            FElement.SetAttribute("yPos", units[i].transform.position.y.ToString());

            FList.AppendChild(FElement);
        }

        //Document.Save("Assets/Resources/XML/StageData/" + name + ".xml");
        //Debug.Log(name + " Saved");

        GUIPlayScene temp = UIManager.OpenGUI<GUIPlayScene>("BuildTest");

        temp.SetStage(new LevelData(Document));
        temp.GetComponent<GUIUploadCloud>().SetData(new LevelData(Document));


    }

}
