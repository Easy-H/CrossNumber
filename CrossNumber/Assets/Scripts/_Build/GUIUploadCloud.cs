using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIUploadCloud : MonoBehaviour
{
    StageData _data;
    
    public void Upload() {
        StageManager.Instance.UploadStage("2 + 3", _data);
    }

    public void SetData(StageData data) {
        _data = data;
    }
}
