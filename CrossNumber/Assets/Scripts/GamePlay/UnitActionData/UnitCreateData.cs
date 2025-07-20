using UnityEngine;

[System.Serializable]
public class UnitCreateData : IUnitActionData
{
    private StageMaker _creator;
    private Unit _unit;
    private string _value;
    public Vector2Int _pos;

    public UnitCreateData(StageMaker creator, Unit unit)
    {
        _creator = creator;
        _unit = unit;
        _value = unit.Value;
        _pos = unit.Pos;
    }

    public void Undo()
    {
        GameManager.Instance.Playground.RemoveUnit(_unit);
    }

    public void Redo()
    {
        _creator.CreateUnit(_value, _pos);
        SoundManager.Instance.PlayAudio("Move");
    }

}