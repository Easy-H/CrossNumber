using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance { get; private set; }

    private Unit[] _units;
    private EqualUnit[] _equalUnits;

    public int unCalcedUnitCount { get; set; }

    public bool _playErrorSound { get; set; }
    bool _canClear;

    private void Awake()
    {
        instance = this;
    }

    public static void WhenNewSceneLoaded() {

        if (!instance)
            return;

        instance._units = FindObjectsOfType<Unit>();
        instance._equalUnits = FindObjectsOfType<EqualUnit>();

        instance.CalculateWorld();

    }

    public void SetUnitsStateUncalced()
    {
        for (int i = 0; i < _units.Length; i++) {
            _units[i].SetStateUnCalced();
        }

        unCalcedUnitCount = _units.Length;

    }

    public void CalculateWorld() {

        StartCoroutine(CalculateWorldAction());

    }

    IEnumerator CalculateWorldAction(){

        _playErrorSound = false;

        yield return new WaitForFixedUpdate();

        SetUnitsStateUncalced();
        
        _canClear = true;

        for (int i = 0; i < _equalUnits.Length; i++) {
            if(!_equalUnits[i].Check())
                _canClear = false;
        }
        if (_playErrorSound) {
            SoundManager.instance.PlayAudio("wrongSound", false);
        }

        if (unCalcedUnitCount != 0)
            _canClear = false;
    }

    public void CheckClear()
    {
        StartCoroutine(CheckClearAction());
    }

    IEnumerator CheckClearAction()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForEndOfFrame();

        if (_canClear)
        {

            StageData stage = GameObject.FindWithTag("Data").GetComponent<StageData>();
            UIManager.instance.StartAnimation("Clear");

            DataManager.Instance.LoadGameData(stage.overworld);
            DataManager.Instance.gameData.SetStageClear(stage.level, true);
            DataManager.Instance.SaveGameData();

        }

    }
}
