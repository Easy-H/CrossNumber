using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringInt {
    public string str;
    public int num;
    public int size;

    bool lastNum = false;

    public StringInt() {
        str = "";
        num = 0;
        size = 0;
    }

    public StringInt(string s)
    {
        str = s;
        num = 0;
        size = 0;
    }

    public void StrAdd(string s, bool addback) {
        bool isNum = int.TryParse(s, out int i);

        if (isNum != lastNum) {
            if (addback)
                str = s + " " + str;
            else
                str = str + " " + s;
            size++;
        }
        else if (isNum) {
            if (addback)
                str = s + str;
            else
                str = str + s;
        }
        else {
            return;
        }
        lastNum = isNum;
        num++;
    }
}

public class EqualUnit : CharUnit
{
    [SerializeField] Transform[] error = new Transform[2];
    int useError;

    protected override void Awake()
    {
        base.Awake();
        gm.equals.Add(this);
    }

    //수식이 틀리면 수식 전체에 빨갛게
    //한쪽이 비면 그쪽 한칸만 빨갛게
    //숫자가 있어야 하면 그쪽한칸만 빨갛게
    //모든 쪽에 수식이 없으면 유닛이 빨갛게
    void Error(Vector3 pos, Vector3 size) {
        error[useError].gameObject.SetActive(true);
        error[useError].position = pos;
        error[useError++].localScale = size;
        GameManager.noError = false;
    }

    public void Check() {
        useError = 0;

        error[0].gameObject.SetActive(false);
        error[1].gameObject.SetActive(false);

        StringInt si1 = new StringInt();
        StringInt si2 = new StringInt();

        bool noEquation = true;
        //side check;
        si1 = MakeSentence(transform.position, Vector3.left, si1, true);
        si2 = MakeSentence(transform.position, Vector3.right, si2, false);
        
        if (si1.num + si2.num != 0) {
            CheckCalc(si1, si2, Vector3.right);
            noEquation = false;
        }
        //upside-down check;
        si1 = new StringInt();
        si2 = new StringInt();

        si1 = MakeSentence(transform.position, Vector3.up, si1, true);
        si2 = MakeSentence(transform.position, Vector3.down, si2, false);

        if (si1.num + si2.num != 0) {
            CheckCalc(si1, si2, Vector3.down);
            noEquation = false;
        }

        if (noEquation)
            Error(transform.position, Vector3.one);

    }

    void CheckCalc(StringInt s1, StringInt s2, Vector3 direction)
    {
        if (s1.num == 0) {
            Error(transform.position - direction, Vector3.one);
        }
        else if (s2.num == 0) {
            Error(transform.position + direction, Vector3.one);
        }
        else if (s1.size % 2 == 0) {
            Error(transform.position - direction * (s1.num + 1), Vector3.one);
            return;
        }
        else if (s2.size % 2 == 0) {
            Error(transform.position + direction * (s2.num + 1), Vector3.one);
            return;
        }
        else if (Calculate(s1) != Calculate(s2)) {
            Error(transform.position - direction * (s1.num - s2.num) * 0.5f, Vector3.one + direction * (s1.num + s2.num));
        }

    }

    // 필드에 있는 유닛을 문자열 수식으로 만든다.
    StringInt MakeSentence(Vector3 pos, Vector3 dir, StringInt si, bool back) {

        RaycastHit2D hit = ObjectCheck(pos + dir, "Char");

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

    int Calculate(StringInt si)
    {
        string equation = si.str.Trim();
        string[] words = equation.Split(' ');

        int[] Nums = new int[3];
        string[] Char = new string[2];

        int NumsNum = 0;
        int CharsNum = 0;
        int i = 0;

        int CalLength = words.Length;

        Nums[NumsNum++] = int.Parse(words[i++]);

        while (true) {

            if (NumsNum == 3) {

                if (Char[1] == "*" || Char[1] == "/")
                    Nums[1] = Calc(Nums[1], Char[1], Nums[2]);
                else {
                    Nums[0] = Calc(Nums[0], Char[0], Nums[1]);

                    Char[0] = Char[1];
                    Nums[1] = Nums[2];

                }

                NumsNum--;
                CharsNum--;

            }

            if (CalLength == i) {

                if (NumsNum == 2)
                    Nums[0] = Calc(Nums[0], Char[0], Nums[1]);

                break;

            }

            Char[CharsNum++] = words[i++];

            Nums[NumsNum++] = int.Parse(words[i++]);
            
        }

        return Nums[0];

    }

    int Calc(int Num1, string Char, int Num2)
    {

        if (Char == "+")
            return Num1 + Num2;
        else if (Char == "-")
            return Num1 - Num2;
        else if (Char == "*")
            return Num1 * Num2;
        else
            return Num1 / Num2;

    }

    /*

    bool CheckRight()
    {
        //양 옆 체크 시작

        for (int i = Equals.x - 1; i > -1; i--)
        {

            if (!(field[i, Equals.y] == "?" || field[i, Equals.y] == "="))
            {

                StartPos.Setting(i - 1, Equals.y);
                FirstCheck++;

            }
            else break;

        }

        //기호와 숫자의 개수가 같을 경우
        if (FirstCheck % 2 != 1 && FirstCheck != 0)
            return false;

        for (int i = Equals.x + 1; i < maxSizeX; i++)
        {

            if (!(field[i, Equals.y] == "?" || field[i, Equals.y] == "="))
            {

                EndPos.Setting(i + 1, Equals.y);
                SecondCheck++;

            }
            else break;

        }

        if (SecondCheck % 2 == 1)
        {

            int a = Calculate(StartPos, Equals);
            int b = Calculate(Equals, EndPos);

            if (a != b)
                return false;

            CalculateTimes++;

        }
        else if (!(FirstCheck == 0 && SecondCheck == 0))
            return false;   //둘중의 한쪽만 수식이 존재하는 경우

        //양 옆 체크 종료

        //위 아래 체크 시작

        FirstCheck = 0;
        SecondCheck = 0;

        for (int i = Equals.y - 1; i > -1; i--)
        {

            if (!(field[Equals.x, i] == "?" || field[Equals.x, i] == "="))
            {

                StartPos.Setting(Equals.x, i - 1);
                FirstCheck++;

            }
            else break;

        }

        if (FirstCheck % 2 != 1 && FirstCheck != 0)
            return false;

        for (int i = Equals.y + 1; i < maxSizeY; i++)
        {

            if (!(field[Equals.x, i] == "?" || field[Equals.x, i] == "="))
            {

                EndPos.Setting(Equals.x, i + 1);
                SecondCheck++;

            }
            else break;

        }

        if (SecondCheck % 2 == 1)
        {

            int a = Calculate(StartPos, Equals);
            int b = Calculate(Equals, EndPos);

            if (a != b)
                return false;

            CalculateTimes++;

        }
        else if (!(FirstCheck == 0 && SecondCheck == 0))
            return false;

        if (CalculateTimes == 0)
            return false;

        return true;

    }

    int Calculate(XYPos Start, XYPos End)
    {

        int[] Nums = new int[3];
        string[] Char = new string[2];

        int NumsNum = 0;
        int CharsNum = 0;
        int Loading = 1;

        int DirX = 0;
        int DirY = 0;
        int CalLength = 0;

        if (Start.y == End.y)
        {

            DirX = 1;
            CalLength = End.x - Start.x - 1;
            Nums[0] = Calc(Nums[0], Char[0], Nums[1]);

        }
        else if (Start.x == End.x)
        {

            DirY = 1;
            CalLength = End.y - Start.y - 1;

        }

        Nums[NumsNum++] = int.Parse(field[Start.x + DirX * Loading, Start.y + DirY * Loading++]);

        CalLength--;

        while (true)
        {

            if (NumsNum == 3)
            {

                if (Char[1] == "*" || Char[1] == "/")
                    Nums[1] = Calc(Nums[1], Char[1], Nums[2]);
                else
                {


                    Char[0] = Char[1];
                    Nums[1] = Nums[2];

                }

                NumsNum--;
                CharsNum--;

            }

            if (CalLength == 0)
            {

                if (NumsNum == 2)
                    Nums[0] = Calc(Nums[0], Char[0], Nums[1]);

                break;

            }

            Char[CharsNum++] = field[Start.x + DirX * Loading, Start.y + DirY * Loading++];
            CalLength--;

            Nums[NumsNum++] = int.Parse(field[Start.x + DirX * Loading, Start.y + DirY * Loading++]);
            CalLength--;


        }

        return Nums[0];

    }

    int Calc(int Num1, string Char, int Num2)
    {

        if (Char == "+")
            return Num1 + Num2;
        else if (Char == "-")
            return Num1 - Num2;
        else if (Char == "*")
            return Num1 * Num2;
        else
            return Num1 / Num2;

    }
    */
}
