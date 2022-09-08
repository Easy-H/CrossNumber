using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    static Unit[] units = null;
    static int unitCount;
    static int uncalcedUnitCount;

    public bool overed = false;

    [SerializeField] Protector[] protector = null;
    [SerializeField] GameObject underline = null;

    protected int defaultLayer;
    [SerializeField] int[] carefulLayer = null;
    int careful;

    public string value = "1";

    protected bool peaked = false;
    bool calced = true;

    public static void WhenNewSceneLoaded() {
        unitCount = 0;
        units = null;
    }

    public static void SetUnitsStateUncalced() {
        if (unitCount == 0)
            return;

        if (units == null) {
            GameObject[] finder = GameObject.FindGameObjectsWithTag("Unit");
            units = new Unit[finder.Length];
            for (int i = 0; i < units.Length; i++) {
                units[i] = finder[i].GetComponent<Unit>();
                units[i].SetStateUnCalced();
            }
        }
        else {
            for (int i = 0; i < units.Length; i++) {
                units[i].SetStateUnCalced();
            }
        }

        uncalcedUnitCount = unitCount;

    }

    public static bool AllCalcCheck() {
        if (uncalcedUnitCount > 0)
            return false;
        return true;
    }

    protected virtual void Start() {
        unitCount++;
        careful = 0;
        defaultLayer = gameObject.layer;

        // carefulLayer를 계산한다
        for (int i = 0; i < carefulLayer.Length; i++) {
            careful = careful | (1 << carefulLayer[i]);
        }
    }

    protected virtual void SetStateUnCalced() {
        if (!overed) {
            calced = false;
            StartCoroutine(DrawUnderline());
        }
        SetProtector();
    }

    public void Overed() {
        Calced();
        gameObject.GetComponent<Collider2D>().enabled = false;
        enabled = false;
        overed = true;
    }

    public void BreakOvered() {
        overed = false;
        enabled = true;
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public void Calced() {
        if (!calced) {
            uncalcedUnitCount--;
            underline.SetActive(false);
        }

        calced = true;
    }

    IEnumerator DrawUnderline()
    {
        yield return new WaitForEndOfFrame();

        if (!calced) {
            underline.SetActive(true);
        }

    }

    public int Pick() {
        gameObject.layer = 2;
        peaked = true;
        ClearProtector();
        return careful;
    }

    public bool Hold(Vector3 pos) {

        if (ObjectCheck(pos, careful))
            return false;

        Vector3 resultPos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 0);

        if ((transform.position - resultPos).magnitude < 0.1f)
            return false;

        GameManager.instance.CheckClear();
        SoundManager.instance.PlayAudio("moveSound", true);
        transform.position = resultPos;


        return true;
        
    }

    public Vector3 Place() {
        gameObject.layer = defaultLayer;
        peaked = false;
        return transform.position;
    }

    void ClearProtector() {
        for (int i = 0; i < protector.Length; i++) {
            protector[i].Clear();
        }
    }

    protected void SetProtector() {
        if (peaked)
            return;
        
        for (int i = 0; i < protector.Length; i++) {
            protector[i].Set();
        }
    }

    public static RaycastHit2D ObjectCheck(Vector3 pos, int layerValue) {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f, layerValue);

        return hit;
    }
    
}
