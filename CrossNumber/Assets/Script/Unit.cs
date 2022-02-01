using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    string value = "1";

    bool placed = false;
    bool isNum;

    [SerializeField] UnitDataScript unitDataSet = null;

    virtual public void Created(string inputValue) {

        value = inputValue;
        gameObject.GetComponent<SpriteRenderer>().sprite = unitDataSet.UnitSprite(value);
        int i;
        isNum = int.TryParse(value, out i);

    }

    public void Pick(string[,] field) {
        if (placed) {
            field[(int)transform.position.x, -(int)transform.position.y] = "?";
            GameManager.unitNum++;
        }
        placed = false;
        gameObject.layer = 2;
    }

    public Vector3 Place(string[,] field, Vector3 pos, Vector3 originPos) {
        Vector3 resultPosition = PosCorrection(pos);

        RaycastHit2D hit = Physics2D.Raycast(resultPosition, Vector2.down, 0.1f);

        if (hit) {
            resultPosition = originPos;
        }

        if (!FieldCheck(resultPosition.x, -resultPosition.y)) {
            resultPosition = pos;
        }
        else {
            field[(int)resultPosition.x, -(int)resultPosition.y] = value;
            placed = true;
            GameManager.unitNum--;
        }
        Debug.Log(GameManager.unitNum);
        gameObject.layer = 0;
        gameObject.transform.position = resultPosition;

        return resultPosition;

    }

    bool FieldCheck(float x, float y)
    {

        if (x < GameManager.maxSizeX && x > -1 && y < GameManager.maxSizeY && y > -1)
            return true;

        return false;

    }
    Vector3 PosCorrection(Vector3 pos)
    {

        float x, y, sin45;

        sin45 = Mathf.Sin(Mathf.Deg2Rad * 45f) * Mathf.Pow(2, 0.5f);

        x = (pos.x - pos.y) * sin45;
        y = (pos.x + pos.y) * sin45;

        if (isNum) {
            x = Mathf.Round(x * 0.5f) * 2;
            y = Mathf.Round(y * 0.5f) * 2;

        }
        else {
            x = Mathf.Round(x * 0.5f - 0.5f) * 2 + 1;
            y = Mathf.Round(y * 0.5f - 0.5f) * 2 + 1;
        }
        Vector3 v = new Vector3(x, y, 0);

        x = (v.x + v.y) * sin45 * 0.5f;
        y = (v.y - v.x) * sin45 * 0.5f;

        return new Vector3(Mathf.Round(x), Mathf.Round(y), 0);

    }


}
