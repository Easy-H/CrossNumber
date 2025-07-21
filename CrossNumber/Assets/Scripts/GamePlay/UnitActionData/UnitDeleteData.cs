using UnityEngine;

[System.Serializable]
public class UnitDeleteData : IUnitActionData
{
    private StageMaker _creator;
    private Unit _unit;
    private string _value;
    public Vector2Int _pos;

    public UnitDeleteData(StageMaker creator, Unit unit)
    {
        _creator = creator;
        _unit = unit;
        _value = unit.Value;
        _pos = unit.Pos;
    }

    public void Undo()
    {
        _unit = _creator.CreateUnit(_value, _pos);
        SoundManager.Instance.PlayAudio("Move");
    }

    public void Redo()
    {
        _unit.Remove();
    }

}