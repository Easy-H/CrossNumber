using System.Collections;
using System.Collections.Generic;
using System.IO;
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

public class DataManager : MonoBehaviour {
    // ---싱글톤으로 선언--- 
    static GameObject _container;
    static GameObject Container {
        get {
            return _container;
        }
    }
    static DataManager _instance;
    public static DataManager Instance {
        get {
            if (!_instance) {
                _container = new GameObject();
                _container.name = "DataController";
                _instance = _container.AddComponent(typeof(DataManager)) as DataManager;
                _instance.LoadGameData();
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }

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