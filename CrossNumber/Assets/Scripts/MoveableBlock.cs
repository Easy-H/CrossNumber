using UnityEngine;
using System.Collections;

public class MoveableBlock : MonoBehaviour, IMoveable
{

    public Vector2Int Pos { get; set; }

    public bool CanPlace(int x, int y)
    {
        return false;
    }

    public void SetPosition(int x, int y)
    {
        SetMoveable(null);

        transform.position = new Vector2(x, y);
        Pos = new Vector2Int(x, y);

        SetMoveable(this);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);

        Pos = new Vector2Int(x, y);

        StartCoroutine(WaitForLayout());
    }
    
    IEnumerator WaitForLayout()
    {
        yield return new WaitForEndOfFrame();

        SetMoveable(this);
    }

    private int GetStartXPos() {
        return Mathf.RoundToInt(transform.position.x
            - (((RectTransform)transform).sizeDelta.x * .5f));
    }
    
    private int GetEndXPos() {
        return Mathf.RoundToInt(transform.position.x
            + (((RectTransform)transform).sizeDelta.x * .5f));
    }
    
    private int GetStartYPos() {
        return Mathf.RoundToInt(transform.position.y
            - (((RectTransform)transform).sizeDelta.y * .5f));
    }
    
    private int GetEndYPos() {
        return Mathf.RoundToInt(transform.position.y
            + (((RectTransform)transform).sizeDelta.y * .5f));
    }

    private void SetMoveable(IMoveable moveable) {
        
        Vector2Int min = new Vector2Int(GetStartXPos(), GetStartYPos());
        Vector2Int max = new Vector2Int(GetEndXPos(), GetEndYPos());

        for (int i = min.x; i <= max.x; i++) {
            for (int j = min.y; j <= max.y; j++) {
                GameManager.Instance.Playground.
                    SetMoveableAt(moveable, i, j);
            }
        }
    }
}
