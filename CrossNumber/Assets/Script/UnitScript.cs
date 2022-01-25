using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public string value = "1";

    [SerializeField] UnitDataScript unitDataSet = null;

    XYPos unitPos = null;

    virtual public void Created(string inputValue) {

        value = inputValue;

        gameObject.GetComponent<SpriteRenderer>().sprite = unitDataSet.UnitSprite(value);

    }

    public void SetPos(int x, int y) {
        PosX = x;
        PosY = y;
    }

    public int PosX = -1;
    public int PosY = -1;


}
