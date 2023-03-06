using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

public class GUIPlayScene : GUICustomFullScreen
{
    [SerializeField] Transform _container = null;
    [SerializeField] GUIAnimatedOpen _clearAnim = null;

    MoveStack _moves;

    protected override void Open()
    {
        base.Open();
        SetStage();
    }

    public void SetStage() {

        UnitManager.Instance.SetInitial();

        _moves = new MoveStack();

        string stage = StageManager.Instance.GetStageData().value;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("Assets/XML/StageData/" + stage + ".xml");

        XmlNodeList nodes = xmlDoc.SelectNodes("StageData/Unit");

        for (int i = 0; i < nodes.Count; i++) {
            string value = nodes[i].Attributes["value"].Value;
            int x = int.Parse(nodes[i].Attributes["xPos"].Value);
            int y = int.Parse(nodes[i].Attributes["yPos"].Value);

            Transform temp = UnitManager.Instance.CreateUnit(value, new Vector3(x, y)).transform;

            temp.SetParent(_container);
        }

        UnitManager.Instance.CalculateWorld();
    }

    public void PlayAnim(GUIAnimatedOpen gui) {
        gui.Open();
    }
    public override void Close()
    {
        base.Close();

    }

    protected override void UnitPlace()
    {
        _selectedUnit.Place();
        _moves.AddMoveData(_selectedUnit, _selectedUnit.GetPeakPos(), _selectedUnit.transform.position);

        if (UnitManager.Instance.CanClear())
        {
            ClearDataManager.Instance.Clear(StageManager.Instance.GetStageData().value);
            _clearAnim.Open();
        }

        _selectedUnit = null;
    }

    // 뒤로가기 기능

    public void MoveUndo()
    {
        _moves.Pop();
        UnitManager.Instance.CalculateWorld();

    }

    public void MoveRedo()
    {
        _moves.Back();
        UnitManager.Instance.CalculateWorld();


    }

    public void ReloadScene() {
        OpenScene(2);
    }

    public void GoNextStage() {
        if (StageManager.StageIdx + 1 < StageManager.Instance.GetStageCount()) {
            StageManager.StageIdx += 1;
            ReloadScene();
            
            return;
        }

        StageManager.WorldIdx++;
        GoToOverWorld();
            
    }

    public void GoToOverWorld() {
        OpenScene(1);
    }

}
