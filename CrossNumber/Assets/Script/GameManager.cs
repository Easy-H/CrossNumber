using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] string[,] field;

    public List<Unit> equalsPos = new List<Unit>();
    public int listLength = 0;

    Vector3 originPos;

    [SerializeField] public static int maxSizeX = 7;
    [SerializeField] public static int maxSizeY = 7;

    [SerializeField] List<MoveData> moves = null;
    [SerializeField] int movesCount = 0;

    public static int unitNum = 0;
    Unit selected;

    string value;
    
    // Start is called before the first frame update
    void Start () {

        field = new string[maxSizeX, maxSizeY];

        for (int i = 0; i < maxSizeX; i++) {
            for (int j = 0; j < maxSizeY; j++)
                field[i, j] = "?";

        }
        
    }

    public void GetBack() {
        if (movesCount < 1)
            return;
        Unit unit = moves[--movesCount].GetObject().GetComponent<Unit>();
        unit.Pick(field);
        unit.Place(field, moves[movesCount].GetOriginPos(), moves[movesCount].GetMovedPos());
    }

    private void Update () {

        if (Input.GetMouseButtonDown(0)) {

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, 0.1f);

            if (hit && hit.collider.CompareTag("Unit")) {

                selected = hit.collider.GetComponent<Unit>();
                selected.Pick(field);
                originPos = selected.transform.position;
                
            }
            
        }

        if (Input.GetMouseButton(0)) {

            if (selected)
                selected.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10;

        }

        if (Input.GetMouseButtonUp(0)) {

            if (!selected)
                return;

            Vector3 result = selected.Place(field, Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10, originPos);

            if ((result - originPos).magnitude > 0.5f) {

                if (movesCount < moves.Count)
                    moves.RemoveRange(movesCount, moves.Count - movesCount);

                moves.Add(new MoveData(selected.gameObject, originPos, result));
                movesCount++;

            }

            selected = null;

        }

        if (Input.GetKeyDown(KeyCode.Z)) {

            for (int i = 0; i < maxSizeX; i++)
            {

                string printOut = "(" + i.ToString() + ") ";

                for (int j = 0; j < maxSizeY; j++)
                    printOut += field[i, j].ToString() + " ";

                Debug.Log(printOut);
            }
        }

    }
    
    // -1: 무오류, 0: 유닛 배치 덜함, 1: 수식 오류
    public int CheckClear() {

        if (unitNum != 0)
            return 0;

        for (int i = 0; i < listLength; i++)
        {
            XYPos EqualPos = new XYPos();

            EqualPos.Setting((int)equalsPos[i].transform.position.x, -(int)equalsPos[i].transform.position.y);
            
            if (EqualPos.x == -1)
                return 0;
            
            if (!CheckRight(EqualPos))
                return 1;

        }
        return -1;
        
    }

    bool CheckRight(XYPos Equals) {

        int FirstCheck = 0;
        int SecondCheck = 0;
        int CalculateTimes = 0;

        XYPos StartPos = new XYPos ();
        XYPos EndPos = new XYPos ();

        //양 옆 체크 시작

        for (int i = Equals.x - 1; i > -1; i--) {

            if (!(field[i, Equals.y] == "?" || field[i, Equals.y] == "=")) {

                StartPos.Setting(i - 1, Equals.y);
                FirstCheck++;

            }
            else break;

        }

        //기호와 숫자의 개수가 같을 경우
        if (FirstCheck % 2 != 1 && FirstCheck != 0)
            return false;

        for (int i = Equals.x + 1; i < maxSizeX; i++) {

            if (!(field[i, Equals.y] == "?" || field[i, Equals.y] == "=")) {

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

            if (!(field[Equals.x, i] == "?" || field[Equals.x, i] == "=")) {

                StartPos.Setting(Equals.x, i - 1);
                FirstCheck++;

            }
            else break;
            
        }

        if (FirstCheck % 2 != 1 && FirstCheck != 0)
            return false;

        for (int i = Equals.y + 1; i < maxSizeY; i++) {

            if (!(field[Equals.x, i] == "?" || field[Equals.x, i] == "=")) {

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
        
        Nums[NumsNum++] = int.Parse(field[Start.x + DirX * Loading, Start.y + DirY * Loading++]);

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

            Char[CharsNum++] = field[Start.x + DirX * Loading, Start.y + DirY * Loading++];
            CalLength--;

            Nums[NumsNum++] = int.Parse(field[Start.x + DirX * Loading, Start.y + DirY * Loading++]);
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
