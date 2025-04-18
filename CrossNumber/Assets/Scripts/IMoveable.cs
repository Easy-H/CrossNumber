using UnityEngine;

public interface IMoveable { 
    
    public Vector2Int Pos { get; }
    
    public void SetPosition(int x, int y);
    public bool CanPlace(int x, int y);
}