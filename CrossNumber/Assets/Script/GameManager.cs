using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<EqualUnit> equalUnits;

    StageData stage;

    [SerializeField] AudioSource moveSound = null;
    [SerializeField] AudioSource wrongSound = null;

    [SerializeField] List<MoveData> moves = null;

    [SerializeField] Camera subCamera = null;
    [SerializeField] Camera traceCamera = null;

    [SerializeField] Transform trCamera = null;
    [SerializeField] Transform trBoard = null;
    
    [SerializeField] int movesCount = 0;
    public int unCalced = 0;

    public static bool noError;
    public static bool playWrongSound;
    public bool moving = false;
    [SerializeField] bool moveField = false;

    Unit selected;

    Vector3 originMouseInput;
    Vector3 originPos;

    private void Awake()
    {
        instance = this;
        Unit.ResetData();
        equalUnits = new List<EqualUnit>();
    }

    private void Start() {
        StartCoroutine(StartCheckClear());
    }

    // 뒤로가기 기능
    public void GetBack() {
        if (movesCount < 1)
            return;
        Unit unit = moves[--movesCount].GetObject().GetComponent<Unit>();
        unit.Pick();
        unit.Hold(moves[movesCount].GetOriginPos());
        unit.Place();
        moveSound.Stop();
        moveSound.Play();

        StartCoroutine(CheckClear());
    }

    public void Foward() {
        if (movesCount > moves.Count - 1)
            return;
        Unit unit = moves[movesCount].GetObject().GetComponent<Unit>();
        unit.Pick();
        unit.Hold(moves[movesCount++].GetMovedPos());
        unit.Place();
        moveSound.Stop();
        moveSound.Play();
        StartCoroutine(CheckClear());
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

            if (selected.Hold(Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10))
            {
                if (moveSound) {
                    moveSound.Stop();
                    moveSound.Play();

                }
                StartCoroutine(CheckClear());
            }

        }

        if (Input.GetMouseButtonUp(0)) {

            if (!selected) {
                moving = false;
                return;
            }

            Vector3 result = selected.Place();

            if ((result - originPos).magnitude > 0.5f) {

                if (movesCount < moves.Count)
                    moves.RemoveRange(movesCount, moves.Count - movesCount);

                moves.Add(new MoveData(selected.gameObject, originPos, result));
                movesCount++;

            }

            subCamera.cullingMask = 0;

            StartCoroutine(CheckClear());

            selected = null;

        }

    }

    IEnumerator StartCheckClear()
    {
        playWrongSound = false;
        subCamera.gameObject.SetActive(true);
        yield return new WaitForFixedUpdate();

        Unit.AllReset();
        noError = true;

        EqualUnit.AllCheck();

        if (unCalced != 0)
            noError = false;
        else if (noError)
        {
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

    IEnumerator CheckClear()
    {
        playWrongSound = false;
        subCamera.gameObject.SetActive(true);
        yield return new WaitForFixedUpdate();

        Unit.AllReset();
        noError = true;

        EqualUnit.AllCheck();

        if (playWrongSound) {
            moveSound.Stop();
            wrongSound.Stop();
            wrongSound.Play();
        }

        if (unCalced != 0)
            noError = false;
        else if (noError) {
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
