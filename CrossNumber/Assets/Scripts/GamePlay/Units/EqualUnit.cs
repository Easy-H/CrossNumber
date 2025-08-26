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

    bool CheckCalc(EquationMaker maker, Vector2Int dir, int idx)
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

        Equation equation1 = new Equation(e1);
        Equation equation2 = new Equation(e2);

        int e1Len = e1.Replace(" ", "").Length - Regex.Matches(e1, @"\^").Count * 2;
        int e2Len = e2.Replace(" ", "").Length - Regex.Matches(e2, @"\^").Count * 2;

        bool canCalc = Error(equation1, Pos - direction * (e1Len + 1))
            && Error(equation2, Pos + direction * (e2Len + 1));

        if (equation1.Value == equation2.Value || !canCalc)
        {
            _calcResultError[i].EraseLine();
            return;
        }
        
        Vector2 dir = new Vector2(direction.x, direction.y);

        _calcResultError[i].DrawLine(
            Pos - (e1Len - e2Len) * 0.5f * dir, dir, e1Len + e2Len + 1);

        _errorOccurred = true;


    }

    // 수식의 끝이 이상할 때 표시되는 에러
    bool Error(Equation equation, Vector2Int pos)
    {
        if (equation.CanCalc) return true;

        _errorOccurred = true;
        _error[_useError].gameObject.SetActive(true);
        _error[_useError++].position = new Vector2(pos.x, pos.y);

        return false;
    }

}