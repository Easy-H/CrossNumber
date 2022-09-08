using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    StageData stage;

    [SerializeField] Camera subCamera = null;
    [SerializeField] Camera traceCamera = null;

    [SerializeField] Transform trCamera = null;
    [SerializeField] Transform trBoard = null;
    
    public bool moving = false;
    [SerializeField] bool moveField = false;

    Unit selected;

    Vector3 originMouseInput;
    Vector3 originPos;

    private void Awake()
    {
        EqualUnit.equalUnits = new List<EqualUnit>();
        Unit.WhenNewSceneLoaded();
        MoveDataManager.WhenNewSceneLoaded();
        instance = this;
    }

    private void Start() {
        CheckClear();
    }

    private void Update () {
        
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            RaycastHit2D hit = Unit.ObjectCheck(mousePos, Camera.main.cullingMask);
            
            if (!hit) {
                if (moveField) {
                    moving = true;
                    originMouseInput = traceCamera.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else if (hit.collider.CompareTag("Unit")) {

                selected = hit.collider.GetComponent<Unit>();
                subCamera.cullingMask = selected.Pick();
                originPos = selected.transform.position;

            }
            return;
        }

        if (Input.GetMouseButton(0)) {

            if (moving) {
                Vector3 mousePos = traceCamera.ScreenToWorldPoint(Input.mousePosition);

                trCamera.Translate(originMouseInput - mousePos);
                trBoard.position = new Vector3(Mathf.Round(trCamera.position.x), Mathf.Round(trCamera.position.y), 1);

                originMouseInput = mousePos;
                
            }

            if (!selected)
                return;

            selected.Hold(Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10);

        }

        if (Input.GetMouseButtonUp(0)) {

            if (!selected) {
                moving = false;
                return;
            }

            Vector3 result = selected.Place();

            if ((result - originPos).magnitude > 0.5f) {
                MoveDataManager.AddData(selected.gameObject, originPos, result);

            }

            CheckClear();

            subCamera.cullingMask = 0;

            selected = null;

        }

    }

    public void CheckClear() {
        StartCoroutine(CheckClearAction());
    }

    IEnumerator CheckClearAction()
    {
        yield return new WaitForFixedUpdate();

        Unit.SetUnitsStateUncalced();

        bool canClear = EqualUnit.AllCheck();

        if (Unit.AllCalcCheck() && canClear) {
            subCamera.gameObject.SetActive(false);
            if (!selected)
            {
                stage = GameObject.FindWithTag("Data").GetComponent<StageData>();
                UIManager.instance.StartAnimation("Clear");
                DataManager.Instance.LoadGameData(stage.overworld);
                DataManager.Instance.gameData.SetStageClear(stage.level, true);
                DataManager.Instance.SaveGameData();
            }
            yield break;
        }
        
    }

}
