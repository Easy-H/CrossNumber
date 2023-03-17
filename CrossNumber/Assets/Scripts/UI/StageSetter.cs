using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Diagnostics;
using static UnityEngine.UI.CanvasScaler;

public class StageSetter : MonoBehaviour
{
    // Start is called before the first frame update
    public void SetStage() {

        StageData stage = StageManager.Instance.GetStageData();

        _CreateWorld(stage);
    }

    public void SetStage(string value)
    {

        StageData stage = StageManager.Instance.GetStageData(value);

        _CreateWorld(stage);
    }

    void _CreateWorld(StageData stage)
    {
        for (int i = 0; i < stage.units.Length; i++)
        {
            UnitData data = stage.units[i];

            Unit temp = UnitManager.Instance.CreateUnit(data.type, data.pos);
            temp.transform.SetParent(transform);
        }

    }
}
