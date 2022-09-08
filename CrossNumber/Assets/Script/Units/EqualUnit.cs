using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqualUnit : CharUnit
{

    [SerializeField] Transform[] error = new Transform[4];
    [SerializeField] DrawRedLine[] calcResultError = new DrawRedLine[2];

    public static List<EqualUnit> equalUnits;

    public static bool playErrorSound;
    public static bool errorOccurred;

    int useError;

    public static bool AllCheck() {
        errorOccurred = false;
        playErrorSound = false;
        for (int i = 0; i < equalUnits.Count; i++) {
            equalUnits[i].Check();
        }
        if (playErrorSound) {
            SoundManager.instance.PlayAudio("wrongSound", false);
        }

        return !errorOccurred;
    }

    protected override void Start() {
        base.Start();
        equalUnits.Add(this);
        value = null;

    }

    protected override void SetStateUnCalced() {
        base.SetStateUnCalced();
        useError = 0;
        for (int i = 0; i < error.Length; i++)
            error[i].gameObject.SetActive(false);
    }

    // 수식의 끝이 이상할 때 표시되는 에러
    void Error(Vector3 pos) {
        errorOccurred = true;
        error[useError].gameObject.SetActive(true);
        error[useError++].position = pos;
    }
    
    // 좌우의 수식, 상하의 수식의 값이 각각 동일한지를 확인한다.
    void Check() {

        bool used = false;

        Equation e1 = new Equation();
        Equation e2 = new Equation();

        //side check;
        e1.MakeEquation(transform.position, Vector3.left, true);
        e2.MakeEquation(transform.position, Vector3.right, false);

        if (e1.num + e2.num != 0) {
            CompareEquation(e1, e2, Vector3.right, 0);
            used = true;
        }
        else {
            calcResultError[0].EraseLine();
        }
        //upside-down check;

        e1 = new Equation();
        e2 = new Equation();

        e1.MakeEquation(transform.position, Vector3.up, true);
        e2.MakeEquation(transform.position, Vector3.down, false);

        if (e1.num + e2.num != 0) {
            CompareEquation(e1, e2, Vector3.down, 1);
            used = true;
        }
        else {
            calcResultError[1].EraseLine();
        }

        if (used) {
            Calced();
        }

    }

    // 두 식이 계산이 되는지, 계산이 된다면 그 결과가 같은지 확인한다.
    void CompareEquation(Equation e1, Equation e2, Vector3 direction, int i)
    {
        bool canCalc = true;
        
        if (!e1.TryCalc(out float e1Result)) {
            Error(transform.position - direction * (e1.num + 1));
            canCalc = false;
        }
        if (!e2.TryCalc(out float e2Result)) {
            Error(transform.position + direction * (e2.num + 1));
            canCalc = false;
        }

        if (e1Result != e2Result && canCalc) {
            calcResultError[i].DrawLine(transform.position - direction * (e1.num - e2.num) * 0.5f, direction, e1.num + e2.num + 1);
            errorOccurred = true;
            return;
        }

        calcResultError[i].EraseLine();

    }

}
