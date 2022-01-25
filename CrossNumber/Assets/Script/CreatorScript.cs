using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorScript : MonoBehaviour
{

    public GameObject board;
    public GameObject basicUnit;

    public string[] numUnits = null;
    public string[] charUnits = null;

    public Vector3 boardPos;
    public Vector3 numFirstPos;
    public Vector3 charFirstPos;

    private void Awake()
    {

        GameControllerScript gcs = gameObject.GetComponent<GameControllerScript>();

        gcs.unitNum = numUnits.Length + charUnits.Length;

        int i, j, k;

        Instantiate(board, boardPos, Quaternion.identity);

        for (i = 0; i < numUnits.Length; i++)
        {

            j = i / 9;
            k = i % 9;

            UnitScript setValue = Instantiate(basicUnit, numFirstPos + Vector3.down * j + Vector3.right * k, Quaternion.identity).GetComponent<UnitScript>();

            setValue.Created(numUnits[i]);

        }

        for (i = 0; i < charUnits.Length; i++)
        {

            j = i / 9;
            k = i % 9;

            UnitScript setValue = Instantiate(basicUnit, charFirstPos + Vector3.down * j + Vector3.right * k, Quaternion.identity).GetComponent<UnitScript>();

            setValue.Created(charUnits[i]);

            if (charUnits[i] == "=") {
                gcs.EqualsPos.Add(setValue);
                gcs.listLength++;
            }

        }
    }
}
