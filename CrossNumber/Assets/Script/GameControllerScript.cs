using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{

    [SerializeField] string[,] Field = new string[9, 9];

    public List<UnitScript> EqualsPos = new List<UnitScript>();
    public int listLength = 0;

    Vector3 originPos;

    [SerializeField] int maxSizeX = 0;
    [SerializeField] int maxSizeY = 0;

    public int unitNum = 0;
    Transform Selected;

    string value;

    bool isNum = false;
    
    // Start is called before the first frame update
    void Start () {

        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++)
                Field[i, j] = "?";

        }


    }

    private void Update () {

        if (Input.GetMouseButtonDown(0)) {

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, 0.1f);

            if (hit)
            {

                Selected = hit.collider.transform;
                value = hit.collider.GetComponent<UnitScript>().value;

                hit.collider.gameObject.layer = 2;
                originPos = Selected.position;

                int FieldX = (int)Selected.position.x;
                int FieldY = -(int)Selected.position.y;

                if (FieldCheck(FieldX, FieldY))
                {

                    Field[FieldX, FieldY] = "?";

                    unitNum++;

                    if (value == "=")
                        hit.collider.GetComponent<UnitScript>().SetPos(-1, -1);

                }

                int numChk;

                isNum = int.TryParse(value, out numChk);

            }
            
        }

        if (Input.GetMouseButton(0)) {

            if (Selected)
                Selected.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10;

        }

        if (Input.GetMouseButtonUp(0)) {

            if (!Selected)
                return;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, 0.1f);

            if (hit) {
                Selected.position = originPos;
            }
            
            Vector3 FieldXY = PosCorrection(Selected.position, originPos, isNum);

            Selected.position = FieldXY;

            int FieldX = (int)FieldXY.x;
            int FieldY = -(int)FieldXY.y;

            if (FieldCheck(FieldX, FieldY)) {


                Field[FieldX, FieldY] = value;

                unitNum--;

                if (value == "=")
                    Selected.GetComponent<UnitScript>().SetPos(FieldX, FieldY);

            }

            Selected.gameObject.layer = 0;

            Selected = null;
            /*
            for (int i = 0; i < 8; i++) {

                string printOut = "(" + i.ToString () + ") ";

                for (int j = 0; j < 8; j++)
                    printOut += Field[i, j].ToString() + " ";

                Debug.Log(printOut);
            }
            */

        }


    }

    bool FieldCheck(int x, int y) {

        if (x < maxSizeX && x > -1 && y < maxSizeY && y > -1)
            return true;
        
        return false;

    }

    Vector3 PosCorrection(Vector3 MousePos, Vector3 NowPos, bool typeNum) {

        float x, y;

        x = Mathf.Abs(Mathf.Round(MousePos.x));
        y = Mathf.Abs(Mathf.Round(MousePos.y));

        if (typeNum)
        {
            //숫자인데 흰 영역이 아니면 현재 위치로 값을 넘겨준다.
            if ((x + y) % 2 == 1)
                return NowPos;

        }
        else if ((x + y) % 2 == 0)
            return NowPos;//기호인데 회색 영역이 아니면 현재 위치로 값을 넘겨준다.
        
        return new Vector3(x, -y, 0);

    }

    public bool CheckClear() {

        if (unitNum != 0)
            return false;

        for (int i = 0; i < listLength; i++)
        {
            XYPos EqualPos = new XYPos();

            EqualPos.Setting(EqualsPos[i].PosX, EqualsPos[i].PosY);
            
            if (EqualPos.x == -1)
                return false;
            
            if (!CheckRight(EqualPos))
                return false;

        }
        return true;
        
    }

    bool CheckRight(XYPos Equals) {

        int FirstCheck = 0;
        int SecondCheck = 0;
        int CalculateTimes = 0;

        XYPos StartPos = new XYPos ();
        XYPos EndPos = new XYPos ();

        //양 옆 체크 시작

        for (int i = Equals.x - 1; i > -1; i--) {

            if (!(Field[i, Equals.y] == "?" || Field[i, Equals.y] == "=")) {

                StartPos.Setting(i - 1, Equals.y);
                FirstCheck++;

            }
            else break;

        }

        //기호와 숫자의 개수가 같을 경우
        if (FirstCheck % 2 != 1 && FirstCheck != 0)
            return false;

        for (int i = Equals.x + 1; i < 9; i++) {

            if (!(Field[i, Equals.y] == "?" || Field[i, Equals.y] == "=")) {

                EndPos.Setting(i + 1, Equals.y);
                SecondCheck++;

            }
            else break;

        }

        if (SecondCheck % 2 == 1) {

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

            if (!(Field[Equals.x, i] == "?" || Field[Equals.x, i] == "=")) {

                StartPos.Setting(Equals.x, i - 1);
                FirstCheck++;

            }
            else break;
            
        }

        if (FirstCheck % 2 != 1 && FirstCheck != 0)
            return false;

        for (int i = Equals.y + 1; i < 9; i++) {

            if (!(Field[Equals.x, i] == "?" || Field[Equals.x, i] == "=")) {

                EndPos.Setting(Equals.x, i + 1);
                SecondCheck++;

            }
            else break;

        }

        if (SecondCheck % 2 == 1) {

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

    int Calculate(XYPos Start, XYPos End) {

        int[] Nums = new int [3];
        string[] Char = new string [2];

        int NumsNum = 0;
        int CharsNum = 0;
        int Loading = 1;

        int DirX = 0;
        int DirY = 0;
        int CalLength = 0;

        if (Start.y == End.y) {

            DirX = 1;
            CalLength = End.x - Start.x - 1;

        }
        else if (Start.x == End.x){

            DirY = 1;
            CalLength = End.y - Start.y - 1;

        }
        
        Nums[NumsNum++] = int.Parse(Field[Start.x + DirX * Loading, Start.y + DirY * Loading++]);

        CalLength--;

        while (true) {

            if (NumsNum == 3) {

                if (Char[1] == "*" || Char[1] == "/")
                    Nums[1] = Calc(Nums[1], Char[1], Nums[2]);
                else {

                    Nums [0] = Calc(Nums[0], Char[0], Nums[1]);

                    Char[0] = Char[1];
                    Nums[1] = Nums[2];

                }

                NumsNum--;
                CharsNum--;

            }

            if (CalLength == 0) {

                if (NumsNum == 2)
                    Nums [0] = Calc(Nums[0], Char[0], Nums[1]);

                break;

            }

            Char[CharsNum++] = Field[Start.x + DirX * Loading, Start.y + DirY * Loading++];
            CalLength--;

            Nums[NumsNum++] = int.Parse(Field[Start.x + DirX * Loading, Start.y + DirY * Loading++]);
            CalLength--;


        }

        return Nums[0];

    }

    int Calc(int Num1, string Char, int Num2) {
        
        if (Char == "+")
            return Num1 + Num2;
        else if (Char == "-")
            return Num1 - Num2;
        else if (Char == "*")
            return Num1 * Num2;
        else
            return Num1 / Num2;

    }

}
