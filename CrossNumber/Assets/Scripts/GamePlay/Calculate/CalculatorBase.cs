using UnityEngine;

public struct CalculateResult
{
    public bool IsErrorOccured { get; private set; }
    public float ResultValue {get; private set;}

    public CalculateResult(bool isErrorOccured, float resultValue)
    {
        IsErrorOccured = isErrorOccured;
        ResultValue = resultValue;
    }
}

public abstract class CalculatorBase {
    
    public abstract float Calculate(string value);

    public bool CanCalcCheck(string Input)
    {
        // 수식이 존재하지 않으면 false
        if (Input.Length < 1) return false;

        string str = Input.Substring(0, 1);
        bool isNum = int.TryParse(str, out int i);

        // 첫 문자가 +, -가 아닌 문자일 경우 false
        if (!isNum && str != "+" && str != "-")
            return false;

        //마지막 문자가 숫자면 true, 아니면 false
        str = Input.Substring(Input.Length - 1);
        isNum = int.TryParse(str, out i);

        return isNum;
    }

    // 숫자와 기호를 넣으면 결과를 출력한다.
    protected static float Calc(float num1, string oper, float num2)
    {
        if (oper == "+")
            return num1 + num2;
        else if (oper == "-")
            return num1 - num2;
        else if (oper == "*")
            return num1 * num2;
        else if (oper == "^")
            return Mathf.Pow(num1, num2);
        else
            return num1 / num2;

    }
}