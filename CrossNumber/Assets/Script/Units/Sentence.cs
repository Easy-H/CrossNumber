using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentence
{
    public string str;

    public int num;
    public int size;

    bool lastNum;

    public Sentence()
    {
        str = "";
        num = 0;
        size = 0;
        lastNum = false;
    }

    public Sentence(string s)
    {
        str = s;
        num = 0;
        size = 0;
        lastNum = false;
    }

    // 필드에 있는 유닛을 문자열 수식으로 만든다.
    public void MakeSentence(Vector3 pos, Vector3 dir, bool back)
    {
        while (true)
        {
            RaycastHit2D hit = Unit.ObjectCheck(pos + dir, 5);

            if (hit)
            {
                Unit touched = hit.collider.GetComponent<Unit>();
                if (touched.value == null)
                    break;

                touched.Calced();
                StrAdd(touched.value, back);

                pos += dir;
            }
            else
                break;
        }

        str = str.Trim();

    }

    public bool CalcAble()
    {
        if (num == 0)
            return false;
        string s = str.Substring(0, 1);
        bool isNum = int.TryParse(s, out int i);

        if (!isNum && s == "*")
            return false;

        s = str.Substring(str.Length - 1);
        isNum = int.TryParse(s, out i);

        return isNum;
    }

    public void StrAdd(string s, bool addback)
    {
        bool isNum = int.TryParse(s.Substring(0, 1), out int i);

        if (isNum != lastNum)
        {
            if (addback)
                str = s + " " + str;
            else
                str = str + " " + s;
            size++;
        }
        else
        {
            if (addback)
                str = s + str;
            else
                str = str + s;
        }
        lastNum = isNum;
        num++;
    }
}