using EHTool;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class EqualUnit : Unit {

    [SerializeField] Transform[] _error = new Transform[4];
    [SerializeField] DrawRedLine[] _calcResultError = new DrawRedLine[2];

    int _useError;

    private bool _errorOccurred;

    public override void SetValue(string value, int x, int y)
    {
        base.SetValue(value, x, y);

        GameManager.Instance.Playground.AddEqualUnit(this);

        for (int i = 0; i < 4; i++)
        {
            _error[i] = AssetOpener.ImportGameObject("Prefabs/Error").transform;
            _error[i].SetParent(transform);
        }

        _calcResultError[0] = AssetOpener.Import<DrawRedLine>("Prefabs/RedLine");
        _calcResultError[1] = AssetOpener.Import<DrawRedLine>("Prefabs/RedLine");

        _calcResultError[0].transform.SetParent(transform);
        _calcResultError[1].transform.SetParent(transform);

    }

    public override void SetStateUnCalced()
    {
        base.SetStateUnCalced();
        _useError = 0;
        for (int i = 0; i < _error.Length; i++)
            _error[i].gameObject.SetActive(false);
    }

    // 수식의 끝이 이상할 때 표시되는 에러
    void Error(Vector2Int pos)
    {
        _errorOccurred = true;
        _error[_useError].gameObject.SetActive(true);
        _error[_useError++].position = new Vector2(pos.x, pos.y);
    }

    // 좌우의 수식, 상하의 수식의 값이 각각 동일한지를 확인한다.
    public bool Check()
    {
        bool used = false;
        _errorOccurred = false;

        EquationMaker maker = new EquationMaker();

        //side check;
        string equation1 = maker.MakeEquation(Pos, Vector2Int.left, true);
        string equation2 = maker.MakeEquation(Pos, Vector2Int.right, false);

        if (equation1.Length + equation2.Length != 0)
        {
            CompareEquation(equation1, equation2, Vector2Int.right, 0);
            used = true;
        }
        else
        {
            _calcResultError[0].EraseLine();
        }

        //upside-down check;

        equation1 = maker.MakeEquation(Pos, Vector2Int.up, true);
        equation2 = maker.MakeEquation(Pos, Vector2Int.down, false);

        if (equation1.Length + equation2.Length != 0)
        {
            CompareEquation(equation1, equation2, Vector2Int.down, 1);
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
    void CompareEquation(string e1, string e2, Vector2Int direction, int i)
    {
        bool canCalc = true;

        Equation equation1 = new Equation(e1);
        Equation equation2 = new Equation(e2);

        int e1Len = e1.Replace(" ", "").Length - Regex.Matches(e1, @"\^").Count * 2;
        int e2Len = e2.Replace(" ", "").Length - Regex.Matches(e2, @"\^").Count * 2;

        if (!equation1.CanCalc)
        {
            int freq = e1.Count(f => (f == ' '));
            Error(Pos - direction * (e1Len + 1));
            canCalc = false;
        }
        if (!equation2.CanCalc)
        {
            int freq = e2.Count(f => (f == ' '));
            Error(Pos + direction * (e2Len + 1));
            canCalc = false;
        }

        if (equation1.Value != equation2.Value && canCalc)
        {
            Vector2 dir = new Vector2(direction.x, direction.y);

            _calcResultError[i].DrawLine(Pos - (e1Len - e2Len) * 0.5f * dir,
                dir, (e1Len + e2Len) + 1);

            _errorOccurred = true;
            return;
        }

        _calcResultError[i].EraseLine();

    }
}