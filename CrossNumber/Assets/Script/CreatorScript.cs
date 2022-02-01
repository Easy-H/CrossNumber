using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorScript : MonoBehaviour
{

    public GameObject board;
    public GameObject basicUnit;
    
    public string[] numUnits = null;
    public string[] charUnits = null;

    public int boardSize = 9;

    public Vector3 boardPos;
    public Vector3 numFirstPos;
    public Vector3 charFirstPos;

    private void Awake()
    {

        GameManager gcs = gameObject.GetComponent<GameManager>();

        GameManager.unitNum = numUnits.Length + charUnits.Length;

        int i, j, k;

        board = Instantiate(board, boardPos, Quaternion.identity);

        board.GetComponent<SpriteRenderer>().size = Vector2.one * boardSize;

        Camera.main.orthographicSize = boardSize + 1;
        Camera.main.transform.position = new Vector3(boardPos.x, boardPos.y * 2 + 1f, -10);
        GameManager.maxSizeX = boardSize;
        GameManager.maxSizeY = boardSize;

        for (i = 0; i < numUnits.Length; i++)
        {

            j = i / 9;
            k = i % 9;

            Unit setValue = Instantiate(basicUnit, numFirstPos + Vector3.down * j + Vector3.right * k, Quaternion.identity).GetComponent<Unit>();

            setValue.Created(numUnits[i]);

        }

        for (i = 0; i < charUnits.Length; i++)
        {

            j = i / 9;
            k = i % 9;

            Unit setValue = Instantiate(basicUnit, charFirstPos + Vector3.down * j + Vector3.right * k, Quaternion.identity).GetComponent<Unit>();

            setValue.Created(charUnits[i]);

            if (charUnits[i] == "=") {
                gcs.equalsPos.Add(setValue);
                gcs.listLength++;
            }

        }
    }
}
