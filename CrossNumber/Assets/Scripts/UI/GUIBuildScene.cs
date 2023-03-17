using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.XR;
public class GUIBuildScene : GUICustomFullScreen {

    [SerializeField] StageSetter _setter;
    enum BuildState {
        Idle, Create, Erase
    }

    BuildState _buildState = BuildState.Idle;

    string _createValue = string.Empty;

    protected override void Open()
    {
        base.Open();
        _setter.SetStage("Temp");
    }

    protected override void Update()
    {
        switch(_buildState)
        {
            case BuildState.Idle:
                base.Update();
                return;
            case BuildState.Create:
                _CreateRoutine();
                break;
            case BuildState.Erase:
                _EraseRoutine();
                break;
        }
    }

    void _CreateRoutine() {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);

        pos.z = 0;

        _selectedUnit = UnitManager.Instance.CreateUnit(_createValue, pos);

        _state = MotionState.UnitMoving;
        _buildState = BuildState.Idle;
    }

    void _EraseRoutine() {

        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Unit selectedUnit = Unit.ObjectCheck(pos, Camera.main.cullingMask);

        if (selectedUnit)
            Destroy(selectedUnit.gameObject);

        _state = MotionState.Idle;
        _buildState = BuildState.Idle;

    }

    public void CreateSet(string value) {

        _buildState = BuildState.Create;
        _createValue = value;

    }

    public void EraseSet() {
        _buildState = BuildState.Erase;
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
        Document.Save("Assets/XML/StageData/" + name + ".xml");

        Debug.Log(name + " Saved");

    }

}
