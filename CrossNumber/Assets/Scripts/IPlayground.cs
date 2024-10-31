public interface IPlayground { 
    public IMoveable GetMoveableAt(int x, int y);
    public void SetMoveableAt(IMoveable target, int x, int y);

    public void AddUnit(Unit target);
    public void AddEqualUnit(EqualUnit target);
    public void RemoveUnitAt(Unit target);

    public void SetDataAt(Unit unit, int x, int y);
    Unit GetDataAt(int x, int y);

    public bool HasError();
    void Dispose();
}