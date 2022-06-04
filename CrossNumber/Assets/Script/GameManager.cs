using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<EqualUnit> equals = new List<EqualUnit>();
    [SerializeField] Camera subCamera = null;

    Vector3 originPos;

    [SerializeField] List<MoveData> moves = null;
    [SerializeField] int movesCount = 0;

    public int unCalced = 0;
    public static bool noError;

    Unit selected;
    
    // Start is called before the first frame update
    void Start () {
        CheckClear();
    }

    // 뒤로가기 기능
    public void GetBack() {
        if (movesCount < 1)
            return;
        Unit unit = moves[--movesCount].GetObject().GetComponent<Unit>();
        unit.Pick();
        unit.Hold(moves[movesCount].GetOriginPos());
        unit.Place();
    }

    private void Update () {

        if (Input.GetMouseButtonDown(0)) {

            int layerMask = ~((1 << 2) | (1 << LayerMask.NameToLayer("Char")));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, 0.1f, layerMask);

            if (hit && hit.collider.CompareTag("Unit")) {

                selected = hit.collider.GetComponent<Unit>();
                selected.Pick();
                originPos = selected.transform.position;

                if (selected.GetComponent<CharUnit>())
                    subCamera.gameObject.SetActive(true);
                
            }
            
        }

        if (Input.GetMouseButton(0)) {

            if (!selected)
                return;

            selected.Hold(Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10);
            CheckClear();

        }

        if (Input.GetMouseButtonUp(0)) {
            if (!selected)
                return;

            Vector3 result = selected.Place();

            if ((result - originPos).magnitude > 0.5f) {

                if (movesCount < moves.Count)
                    moves.RemoveRange(movesCount, moves.Count - movesCount);

                moves.Add(new MoveData(selected.gameObject, originPos, result));
                movesCount++;

            }

            subCamera.gameObject.SetActive(false);

            if (noError)
            {
                UIManager.instance.Clear();
                Debug.Log("Clear");
            }

            selected = null;

        }
        

    }
    
    // -1: 무오류, 0: 유닛 배치 덜함, 1: 수식 오류
    public void CheckClear() {
        noError = true;
        for (int i = 0; i < equals.Count; i++)
        {
            equals[i].Check();

        }

        if (unCalced - equals.Count != 0)
            noError = false;

    }

}
