using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqualUnit : CharUnit
{

    [SerializeField] Transform[] error = new Transform[4];
    [SerializeField] Transform[] calcError = new Transform[2];
    [SerializeField] Transform[] calcErrorHelper = new Transform[2];

    [SerializeField] float calcHelperTime = 0.2f;

    int useError;

    IEnumerator[] calcErrorHelp;

    public static void AllCheck() {
        for (int i = 0; i < GameManager.instance.equalUnits.Count; i++) {
            GameManager.instance.equalUnits[i].Check();
        }
    }

    protected override void Start() {
        base.Start();
        GameManager.instance.equalUnits.Add(this);
        calcErrorHelp = new IEnumerator[2];
        calcErrorHelp[0] = CalcHelper(0);
        calcErrorHelp[1] = CalcHelper(1);
        value = null;

    }

    protected override void ResetValue()
    {
        base.ResetValue();
        useError = 0;
        for (int i = 0; i < error.Length; i++)
            error[i].gameObject.SetActive(false);
    }

    // 수식의 끝이 이상할 때 표시되는 에러
    void Error(Vector3 pos, int i) {
        GameManager.noError = false;
        error[useError].gameObject.SetActive(true);
        error[useError++].position = pos;
        calcError[i].gameObject.SetActive(false);
    }

    // 계산 결과가 틀리면 가운데에 줄을 그음
    void CalcError(Vector3 pos, Vector3 direct, float size, int i) {
        GameManager.noError = false;
        pos += Vector3.forward;
        if (calcError[i].gameObject.activeSelf && (calcError[i].localScale - new Vector3(size, 1, 1)).magnitude < 0.1f && !peaked) {
            return;
        }
        GameManager.playWrongSound = true;
        StopCoroutine(calcErrorHelp[i]);
        calcErrorHelp[i] = CalcHelper(i);
        StartCoroutine(calcErrorHelp[i]);

        calcError[i].gameObject.SetActive(true);
        calcError[i].position = pos;
        calcError[i].right = direct;
        calcError[i].localScale = new Vector3(size, 1, 1);
    }

    // 가운데에 줄을 긋는 애니메이션 코루틴
    IEnumerator CalcHelper(int i) {
        float time = 0;
        float y = calcErrorHelper[i].localScale.y;
        calcErrorHelper[i].localScale = new Vector3(1, y);
        calcError[i].gameObject.layer = 9;
        while (time < calcHelperTime) {
            yield return new WaitForEndOfFrame();
            calcErrorHelper[i].localScale = new Vector3(1 - time / calcHelperTime, y);
            time += Time.deltaTime;
        }
        calcErrorHelper[i].localScale = new Vector3(0, y);
        calcError[i].gameObject.layer = 8;
    }

    // 좌우의 수식, 상하의 수식의 값이 각각 동일한지를 확인한다.
    void Check() {

        Sentence si1 = new Sentence();
        Sentence si2 = new Sentence();

        //side check;
        si1.MakeSentence(transform.position, Vector3.left, true);
        si2.MakeSentence(transform.position, Vector3.right, false);

        if (si1.num + si2.num != 0) {
            CheckCalc(si1, si2, Vector3.right, 0);
        }
        else {

            calcError[0].gameObject.SetActive(false);
        }
        //upside-down check;

        si1 = new Sentence();
        si2 = new Sentence();

        si1.MakeSentence(transform.position, Vector3.up, true);
        si2.MakeSentence(transform.position, Vector3.down, false);

        if (si1.num + si2.num != 0) {
            CheckCalc(si1, si2, Vector3.down, 1);
        }
        else {
            calcError[1].gameObject.SetActive(false);
        }

        Calced();

    }

    // 계산이 가능한 상황인지 확인한다.
    void CheckCalc(Sentence s1, Sentence s2, Vector3 direction, int i)
    {
        bool canCalc = true;

        if (!s1.CalcAble()) {
            Error(transform.position - direction * (s1.num + 1), i);
            canCalc = false;
        }
        if (!s2.CalcAble())
        {
            Error(transform.position + direction * (s2.num + 1), i);
            canCalc = false;
        }

        if (!canCalc) {
            calcError[i].gameObject.SetActive(false);
            return;
        }

        if (Calculate(s1) != Calculate(s2)) {
            CalcError(transform.position - direction * (s1.num - s2.num) * 0.5f, direction, s1.num + s2.num + 1, i);
            return;
        }
        calcError[i].gameObject.SetActive(false);

    }

    // 필드에 있는 유닛을 문자열 수식으로 만든다.
    Sentence MakeSentence(Vector3 pos, Vector3 dir, Sentence si, bool back) {

        RaycastHit2D hit = ObjectCheck(pos + dir, 5);

        if (hit) {
            
            Unit touched = hit.collider.GetComponent<Unit>();
            string getvalue = touched.value;

            if (getvalue != "=") {
                si.StrAdd(getvalue, back);
                touched.Calced();
                si = MakeSentence(pos + dir, dir, si, back);
            }
        }

        return si;
    }

    // Sentence를 기반으로 연산한다.
    /*
    float Calculate(Sentence si)
    {
        string[] words = si.str.Split(' ');

        float[] nums = new float[3];
        string[] ops = new string[2];

        int numsNum = 0;
        int opsNum = 0;
        int i = 0;

        int CalLength = words.Length;

        if (words[i] == "-") {
            words[++i] = "-" + words[i];
        }
        else if (words[i] == "+") {
            i++;
        }

        nums[numsNum++] = int.Parse(words[i++]);

        while (true) {

            if (numsNum == 3) {
                if (ops[0] == "^")
                {
                    nums[0] = Calc(nums[0], ops[0], nums[1]);

                    ops[0] = ops[1];
                    nums[1] = nums[2];
                }
                else if (ops[1] != "+" && ops[1] != "-")
                {
                    nums[1] = Calc(nums[1], ops[1], nums[2]);
                }
                else
                {
                    nums[0] = Calc(nums[0], ops[0], nums[1]);

                    ops[0] = ops[1];
                    nums[1] = nums[2];

                }

                numsNum--;
                opsNum--;

            }

            if (CalLength == i) {

                if (numsNum == 2)
                    nums[0] = Calc(nums[0], ops[0], nums[1]);

                break;

            }

            ops[opsNum++] = words[i++];

            nums[numsNum++] = int.Parse(words[i++]);

        }

        return nums[0];

    }*/
    string[] words;
    int calLength;

    // Sentence를 기반으로 연산한다.
    float Calculate(Sentence si) {
        words = si.str.Split(' ');
        calLength = words.Length;

        return CalculateUnit();
    }

    float CalculateUnit()
    {

        float[] nums = new float[3];
        string[] ops = new string[2];

        int numsNum = 0;
        int opsNum = 0;
        int i = 0;
        
        if (words[i] == "-") {
            i++;
            nums[numsNum++] = -1;
            ops[opsNum++] = "*";
        }
        else if (words[i] == "+") {
            i++;
        }

        while (true) {

            if (words[i] == "(") {

            }
            nums[numsNum++] = int.Parse(words[i++]);

            if (numsNum == 3) {
                if (ops[0] == "^") {
                    nums[0] = Calc(nums[0], ops[0], nums[1]);

                    ops[0] = ops[1];
                    nums[1] = nums[2];
                }
                else if (ops[1] != "+" && ops[1] != "-") {
                    nums[1] = Calc(nums[1], ops[1], nums[2]);
                }
                else
                {
                    nums[0] = Calc(nums[0], ops[0], nums[1]);

                    ops[0] = ops[1];
                    nums[1] = nums[2];

                }

                numsNum--;
                opsNum--;

            }

            if (calLength == i || words[i] == ")")
            {
                if (numsNum == 2)
                    nums[0] = Calc(nums[0], ops[0], nums[1]);

                return nums[0];
            }

            ops[opsNum++] = words[i++];

        }

    }

    // 숫자와 기호를 넣으면 결과를 출력한다.
    float Calc(float Num1, string Char, float Num2)
    {

        if (Char == "+")
            return Num1 + Num2;
        else if (Char == "-")
            return Num1 - Num2;
        else if (Char == "*")
            return Num1 * Num2;
        else if (Char == "^")
            return Mathf.Pow(Num1, Num2);
        else
            return Num1 / Num2;

    }
}
