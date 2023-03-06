using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqualUnit : CharUnit
{

    [SerializeField] Transform[] _error = new Transform[4];
    [SerializeField] DrawRedLine[] _calcResultError = new DrawRedLine[2];

    int _useError;

    private bool _errorOccurred;
    
    protected override void Start() {
        base.Start();
        UnitManager.Instance._equalUnits.Add(this);

    }

    public override void SetStateUnCalced() {
        base.SetStateUnCalced();
        _useError = 0;
        for (int i = 0; i < _error.Length; i++)
            _error[i].gameObject.SetActive(false);
    }

    // 수식의 끝이 이상할 때 표시되는 에러
    void Error(Vector3 pos) {
        _errorOccurred = true;
        _error[_useError].gameObject.SetActive(true);
        _error[_useError++].position = pos;
    }
    
    // 좌우의 수식, 상하의 수식의 값이 각각 동일한지를 확인한다.
    public bool Check() {

        bool used = false;
        _errorOccurred = false;

        Equation e1 = new Equation();
        Equation e2 = new Equation();

        //side check;
        e1.MakeEquation(transform.position, Vector3.left, true);
        e2.MakeEquation(transform.position, Vector3.right, false);

        if (e1.ContainCharCount + e2.ContainCharCount != 0) {
            CompareEquation(e1, e2, Vector3.right, 0);
            used = true;
        }
        else {
            _calcResultError[0].EraseLine();
        }
        //upside-down check;

        e1 = new Equation();
        e2 = new Equation();

        e1.MakeEquation(transform.position, Vector3.up, true);
        e2.MakeEquation(transform.position, Vector3.down, false);

        if (e1.ContainCharCount + e2.ContainCharCount != 0) {
            CompareEquation(e1, e2, Vector3.down, 1);
            used = true;
        }
        else {
            _calcResultError[1].EraseLine();
        }

        if (used) {
            Calced();
        }

        return !_errorOccurred;

    }

    // 두 식이 계산이 되는지, 계산이 된다면 그 결과가 같은지 확인한다.
    void CompareEquation(Equation e1, Equation e2, Vector3 direction, int i)
    {
        bool canCalc = true;
        
        if (!e1.TryCalc(out float e1Result)) {
            Error(transform.position - direction * (e1.ContainCharCount + 1));
            canCalc = false;
        }
        if (!e2.TryCalc(out float e2Result)) {
            Error(transform.position + direction * (e2.ContainCharCount + 1));
            canCalc = false;
        }

        if (e1Result != e2Result && canCalc) {
            _calcResultError[i].DrawLine
                (transform.position - direction * (e1.ContainCharCount - e2.ContainCharCount) * 0.5f,
                direction, e1.ContainCharCount + e2.ContainCharCount + 1);
            _errorOccurred = true;
            return;
        }

        _calcResultError[i].EraseLine();

    }

}
