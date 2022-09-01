using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    static Unit[] units = null;
    static int beforeUnitNum = 0;
    static int realUnitNum = 0;

    public bool overed = false;

    [SerializeField] Protector[] protector = null;
    [SerializeField] GameObject underline = null;

    protected int defaultLayer;
    [SerializeField] int[] carefulLayer = null;
    [SerializeField] int careful;

    public string value = "1";

    public bool peaked = false;
    bool calced = true;

    public static void ResetData() {
        beforeUnitNum = 0;
        realUnitNum = 0;
    }

    protected virtual void Start() {
        realUnitNum++;
        calced = true;
        careful = 0;
        defaultLayer = gameObject.layer;
        for (int i = 0; i < carefulLayer.Length; i++)
        {
            careful = careful | (1 << carefulLayer[i]);
        }
    }

    public static void AllReset() {
        if (realUnitNum == 0)
            return;

        if (realUnitNum != beforeUnitNum) {
            GameObject[] finder = GameObject.FindGameObjectsWithTag("Unit");
            units = new Unit[finder.Length];
            for (int i = 0; i < units.Length; i++) {
                units[i] = finder[i].GetComponent<Unit>();
                units[i].ResetValue();
            }
            beforeUnitNum = realUnitNum;
        }
        else {
            for (int i = 0; i < units.Length; i++) {
                units[i].ResetValue();
            }
        }

    }

    protected virtual void ResetValue() {
        if (!overed)
            BreakCalc();
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
            GameManager.instance.unCalced--;
            underline.SetActive(false);
        }

        calced = true;
    }

    public void BreakCalc()
    {
        if (calced) {
            GameManager.instance.unCalced++;
            calced = false;
        }

        StartCoroutine(DrawUnderline());
    }

    IEnumerator DrawUnderline()
    {
        yield return new WaitForEndOfFrame();

        if (!calced)
        {
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
        
        transform.position = resultPos;

        return true;
        
    }

    public Vector3 Place() {
        gameObject.layer = defaultLayer;
        peaked = false;
        return transform.position;
    }

    void ClearProtector() {
        for (int i = 0; i < protector.Length; i++)
        {
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
