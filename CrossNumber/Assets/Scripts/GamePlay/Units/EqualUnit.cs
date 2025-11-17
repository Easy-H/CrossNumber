using UnityEngine;
using System.Text.RegularExpressions;

public class EqualUnit : Unit
{

    [SerializeField] private Transform[] _error = new Transform[4];
    [SerializeField] private DrawRedLine[] _calcResultError = new DrawRedLine[2];

    private int _useError;

    private bool _errorOccurred;

    public override void SetValue(string value, int x, int y)
    {
        base.SetValue(value, x, y);

        GameManager.Instance.Playground.AddEqualUnit(this);
    }

    public override void SetStateUnCalced()
    {
        base.SetStateUnCalced();
        _useError = 0;
        for (int i = 0; i < _error.Length; i++)
            _error[i].gameObject.SetActive(false);
    }

    public override void Remove()
    {
        GameManager.Instance.Playground.RemoveEqualUnit(this);

        base.Remove();
    }

    // 좌우의 수식, 상하의 수식의 값이 각각 동일한지를 확인한다.
    public bool Check()
    {
        _errorOccurred = false;

        EquationMaker maker = new EquationMaker();

        bool calc1 = CheckCalc(maker, Vector2Int.right, 0);
        bool calc2 = CheckCalc(maker, Vector2Int.down, 1);

        IsCalced = calc1 || calc2;

        return !_errorOccurred;

    }

    private bool CheckCalc(EquationMaker maker, Vector2Int dir, int idx)
    {

        string equation1 = maker.MakeEquation(Pos, -dir, true);
        string equation2 = maker.MakeEquation(Pos, dir, false);

        if (equation1.Length + equation2.Length == 0)
        {
            _calcResultError[idx].EraseLine();
            return false;
        }

        CompareEquation(equation1, equation2, dir, 0);

        return true;

    }

    // 두 식이 계산이 되는지, 계산이 된다면 그 결과가 같은지 확인한다.
    void CompareEquation(string e1, string e2, Vector2Int direction, int i)
    {   
        CalculatorBase calculator = new RecursiveCalculator();

        bool canCalc = true;
        
        if (!calculator.CanCalcCheck(e1))
        {
            canCalc = false;
            EquationError(e1, -direction);
        }

        if (!calculator.CanCalcCheck(e2))
        {
            canCalc = false;
            EquationError(e2, direction);
        }

        _errorOccurred = !canCalc;

        if (!canCalc || calculator.Calculate(e1)
            == calculator.Calculate(e2))
        {
            _calcResultError[i].EraseLine();
            return;
        }

        _errorOccurred = true;

        ValueMissMatchError(e1, e2, direction, i);

    }

    private int GetEquationLen(string equation)
    {
        return equation.Replace(" ", "").Length
            - Regex.Matches(equation, @"\^").Count * 2;
    }

    // 수식의 끝이 이상할 때 표시되는 에러
    private void EquationError(string equation, Vector2Int direction)
    {
        int len = GetEquationLen(equation);

        Vector2Int pos = Pos + direction * (len + 1);

        _error[_useError].gameObject.SetActive(true);
        _error[_useError++].position = new Vector2(pos.x, pos.y);

    }

    // 두 수식의 값이 다를 때 표시되는 에러
    private void ValueMissMatchError(string e1, string e2, Vector2Int direction, int i)
    {
        
        Vector2 dir = direction;

        int e1Len = GetEquationLen(e1);
        int e2Len = GetEquationLen(e2);

        _calcResultError[i].DrawLine(
            Pos - (e1Len - e2Len) * 0.5f * dir, dir, e1Len + e2Len + 1);

    }

}