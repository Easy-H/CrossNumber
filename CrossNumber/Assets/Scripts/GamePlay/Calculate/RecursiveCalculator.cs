using UnityEngine;

public class RecursiveCalculator : CalculatorBase {


    public RecursiveCalculator()
    {
        
    }

    public override float Calculate(string value)
    {
        float ret = Calc(value);
        return ret;
    }

    private float Calc(string value)
    {
        string[] _words = value.Split(' ');

        float[] nums = new float[3];
        string[] ops = new string[2];

        int numsNum = 0;
        int opsNum = 0;
        int wordIdx = 0;
        int length = _words.Length;

        if (_words[wordIdx] == "-")
        {
            wordIdx++;
            nums[numsNum++] = -1;
            ops[opsNum++] = "*";
        }
        
        else if (_words[wordIdx] == "+")
        {
            wordIdx++;
        }

        while (true)
        {
            if (_words[wordIdx] == "(")
            {
                // 재귀로 실행하면 괄호가 구현될듯 함
            }

            if (!int.TryParse(_words[wordIdx++], out int tmp)) {
                return 0;
            }

            nums[numsNum++] = tmp;

            if (numsNum == 3)
            {
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

            if (length == wordIdx || _words[wordIdx].Equals(")"))
            {
                if (numsNum == 2)
                {
                    nums[0] = Calc(nums[0], ops[0], nums[1]);
                }

                return nums[0];

            }

            ops[opsNum++] = _words[wordIdx++];

        }
        
    }

}