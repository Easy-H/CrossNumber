using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

[System.Serializable]
public enum OverWorldName {
    Beginner,
    Intermediate,
    Advanced
}

[System.Serializable]
public class OverWorldData {
    public bool[] stageClear = new bool[0];

    public bool GetStageClear(int idx) {
        if (stageClear.Length <= idx) {
            bool[] clearData = new bool[idx + 1];
            for (int i = 0; i < stageClear.Length; i++) {
                clearData[i] = stageClear[i];
            }
            for (int i = stageClear.Length; i < idx; i++) {
                clearData[i] = false;
            }
            stageClear = clearData;
        }

        return stageClear[idx];
    }

    public void SetStageClear(int idx, bool clear) {
        if (stageClear == null) {
            stageClear = new bool[1];
            stageClear[0] = false;
        }
        else if (stageClear.Length <= idx) {
            bool[] clearData = new bool[idx + 1];
            for (int i = 0; i < stageClear.Length; i++) {
                clearData[i] = stageClear[i];
            }
            for (int i = stageClear.Length; i < idx; i++) {
                clearData[i] = false;
            }
            stageClear = clearData;
        }
        stageClear[idx] = clear;

    }
}

[System.Serializable]
public class GameData {
    [SerializeField] OverWorldData _beginner = new OverWorldData();
    [SerializeField] OverWorldData _intermediate = new OverWorldData();
    [SerializeField] OverWorldData _advanced = new OverWorldData();

    public OverWorldData GetOverWorld(OverWorldName worldName) {
        OverWorldData result = null;

        switch (worldName) {
            case OverWorldName.Beginner:
                result = _beginner;
                break;
            case OverWorldName.Intermediate:
                result = _intermediate;
                break;
            case OverWorldName.Advanced:
                result = _advanced;
                break;

        }

        return result;
    }
}

public class DataManager : MonoSingleton<DataManager> {
    // ---싱글톤으로 선언--- 

    // --- 게임 데이터 파일이름 설정 ---
    // "원하는 이름(영문).json"
    public string GameDataFileName = "GameData.json";

    public GameData _gameData;
    public GameData gameData {
        get {
            // 게임이 시작되면 자동으로 실행되도록
            if (_gameData == null) {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }

    // 저장된 게임 불러오기
    public void LoadGameData() {
        string filePath = Application.persistentDataPath + GameDataFileName;

        //Debug.Log(filePath);

        // 저장된 게임이 있다면
        if (File.Exists(filePath)) {
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }

        // 저장된 게임이 없다면
        else {
            print("새로운 파일 생성");
            _gameData = new GameData();
        }
    }

    // 게임 저장하기
    public void SaveGameData() {
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        // 이미 저장된 파일이 있다면 덮어쓰기
        File.WriteAllText(filePath, ToJsonData);

    }

    // 게임을 종료하면 자동저장되도록
    private void OnApplicationQuit() {
        SaveGameData();
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

public class UnitSaveManager : MonoSingleton<UnitSaveManager> {

    List<Unit> _units;

    string _path = "Assets/Resources/UnitData.xml";

    public void AddUnit(Unit data)
    {
        _units.Add(data);
        SaveFurnitureXML();
    }

    public void Null()
    {
        return;
    }
    public void SaveFurnitureXML()
    {
        Write(_path);
    }

    protected override void OnCreate()
    {
        _units = new List<Unit>();

        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.Load(_path);
        }
        catch (FileNotFoundException)
        {
            return;
        }

        XmlNodeList nodes = xmlDoc.SelectNodes("UnitData/Unit");

        for (int i = 0; i < nodes.Count; i++)
        {
            string unitCode = nodes[i].Attributes["unitCode"].Value;
            Unit newUnit = UnitDataManager.CreateUnit(unitCode);

            Vector3 pos = new Vector3();
            pos.x = float.Parse(nodes[i].Attributes["posX"].Value);
            pos.y = float.Parse(nodes[i].Attributes["posY"].Value);
            pos.z = float.Parse(nodes[i].Attributes["posZ"].Value);

            Vector3 dir = new Vector3();
            dir.x = float.Parse(nodes[i].Attributes["dirX"].Value);
            dir.y = float.Parse(nodes[i].Attributes["dirY"].Value);
            dir.z = float.Parse(nodes[i].Attributes["dirZ"].Value);

            newUnit.transform.position = pos;
            newUnit.transform.up = dir;

            newUnit.UnitCode = unitCode;

            _units.Add(newUnit);
        }
    }

    public void Write(string filePath)
    {

        XmlDocument Document = new XmlDocument();
        XmlElement FList = Document.CreateElement("UnitData");
        Document.AppendChild(FList);

        foreach (Unit infor in _units)
        {
            XmlElement FElement = Document.CreateElement("Unit");
            FElement.SetAttribute("unitCode", infor.UnitCode);
            FElement.SetAttribute("posX", infor.transform.position.x.ToString());
            FElement.SetAttribute("posY", infor.transform.position.y.ToString());
            FElement.SetAttribute("posZ", infor.transform.position.z.ToString());
            FElement.SetAttribute("dirX", infor.transform.up.x.ToString());
            FElement.SetAttribute("dirY", infor.transform.up.y.ToString());
            FElement.SetAttribute("dirZ", infor.transform.up.z.ToString());
            FList.AppendChild(FElement);
        }
        Document.Save(filePath);
    }

}

*/