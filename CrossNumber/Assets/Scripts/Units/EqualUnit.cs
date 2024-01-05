using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class EqualUnit : Unit {

    [SerializeField] Transform[] _error = new Transform[4];
    [SerializeField] DrawRedLine[] _calcResultError = new DrawRedLine[2];

    int _useError;

    private bool _errorOccurred;

    public EqualUnit(Vector3 pos)
    {
        Value = "=";
        Pos = pos;

        IsCalced = true;

        for (int i = 0; i < 4; i++)
        {
            _error[i] = AssetOpener.ImportGameObject("Prefabs/Error").transform;
            _error[i].SetParent(UnitManager.Instance.Container);
        }

        _calcResultError[0] = AssetOpener.ImportGameObject("Prefabs/RedLine").GetComponent<DrawRedLine>();
        _calcResultError[1] = AssetOpener.ImportGameObject("Prefabs/RedLine").GetComponent<DrawRedLine>();


        _calcResultError[0].transform.SetParent(UnitManager.Instance.Container);
        _calcResultError[1].transform.SetParent(UnitManager.Instance.Container);

    }
    public override void SetStateUnCalced()
    {
        base.SetStateUnCalced();
        _useError = 0;
        for (int i = 0; i < _error.Length; i++)
            _error[i].gameObject.SetActive(false);
    }

    // 수식의 끝이 이상할 때 표시되는 에러
    void Error(Vector3 pos)
    {
        _errorOccurred = true;
        _error[_useError].gameObject.SetActive(true);
        _error[_useError++].position = pos;
    }

    // 좌우의 수식, 상하의 수식의 값이 각각 동일한지를 확인한다.
    public bool Check()
    {

        bool used = false;
        _errorOccurred = false;

        EquationMaker maker = new EquationMaker();

        //side check;
        string equation1 = maker.MakeEquation(Pos, Vector3.left, true);
        string equation2 = maker.MakeEquation(Pos, Vector3.right, false);

        if (equation1.Length + equation2.Length != 0)
        {
            CompareEquation(equation1, equation2, Vector3.right, 0);
            used = true;
        }
        else
        {
            _calcResultError[0].EraseLine();
        }

        //upside-down check;

        equation1 = maker.MakeEquation(Pos, Vector3.up, true);
        equation2 = maker.MakeEquation(Pos, Vector3.down, false);

        if (equation1.Length + equation2.Length != 0)
        {
            CompareEquation(equation1, equation2, Vector3.down, 1);
            used = true;
        }
        else
        {
            _calcResultError[1].EraseLine();
        }

        if (used)
        {
            IsCalced = true;
        }

        return !_errorOccurred;

    }

    // 두 식이 계산이 되는지, 계산이 된다면 그 결과가 같은지 확인한다.
    void CompareEquation(string e1, string e2, Vector3 direction, int i)
    {
        bool canCalc = true;

        Equation equation1 = new Equation(e1);
        Equation equation2 = new Equation(e2);

        if (!equation1.CanCalc)
        {
            int freq = e1.Count(f => (f == ' '));
            Error(Pos - direction * (e1.Length - freq + 1));
            canCalc = false;
        }
        if (!equation2.CanCalc)
        {
            int freq = e2.Count(f => (f == ' '));
            Error(Pos + direction * (e2.Length - freq + 1));
            canCalc = false;
        }

        if (equation1.Value != equation2.Value && canCalc)
        {
            int factor1 = e1.Length / 2 + 1;
            int factor2 = e2.Length / 2 + 1;

            _calcResultError[i].DrawLine(Pos - direction * (factor1 - factor2) * 0.5f, direction, (factor1 + factor2) + 1);

            _errorOccurred = true;
            return;
        }

        _calcResultError[i].EraseLine();

    }
}