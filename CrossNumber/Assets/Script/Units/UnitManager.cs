using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    private static UnitManager _instance;
    public static UnitManager Instance { get; private set; }
    
    private Unit[] _units;
    private EqualUnit[] _equalUnits;

    public UnitType SelectedUnitType { get; set; }

    public int unCalcedUnitCount { get; set; }

    public bool playErrorSound { get; set; }
    bool _canClear;

    public static void WhenNewSceneLoaded() {

        if (!Instance) {
            return;
        }

        Instance._units = FindObjectsOfType<Unit>();
        Instance._equalUnits = FindObjectsOfType<EqualUnit>();

        Instance.SelectedUnitType = UnitType.Null;
        Instance.CalculateWorld();

    }

    private void Awake() {
        Instance = this;
    }

    public void SetAllUnitsStateUncalced() {

        for (int i = 0; i < _units.Length; i++) {
            _units[i].SetStateUnCalced();
        }

        unCalcedUnitCount = _units.Length;

    }

    public void CalculateWorld() {

        StartCoroutine(CalculateWorldAction());

    }

    IEnumerator CalculateWorldAction() {

        playErrorSound = false;

        yield return new WaitForFixedUpdate();

        SetAllUnitsStateUncalced();

        _canClear = true;

        for (int i = 0; i < _equalUnits.Length; i++) {

            if (!_equalUnits[i].Check()) {
                _canClear = false;
            }

        }
        if (playErrorSound) {
            SoundManager.instance.PlayAudio("wrongSound", false);
        }

        if (unCalcedUnitCount != 0) {
            _canClear = false;
        }

    }

    public void CalculateCanClear() {

        StartCoroutine(CalculateCanClearAction());

    }

    IEnumerator CalculateCanClearAction() {

        SelectedUnitType = UnitType.Null;

        CalculateWorld();

        yield return new WaitForFixedUpdate();
        yield return new WaitForEndOfFrame();

        if (_canClear) {

            UIManager.Instance.StartAnimation("Clear");

            StageData stage = GameObject.FindWithTag("Data").GetComponent<StageData>();
            
            DataManager.Instance.gameData.GetOverWorld(stage.overworld).SetStageClear(stage.level, true);
            DataManager.Instance.SaveGameData();

        }
    }

}
