public interface IUnit { 
    
    public string Value { get; }
    public bool IsCalced { get; }

    public void SetStateCalced();
    public void SetStateUnCalced();

}