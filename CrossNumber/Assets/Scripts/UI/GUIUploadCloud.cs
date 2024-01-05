using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIUploadCloud : MonoBehaviour
{
    LevelData _data;
    
    public void Upload() {
        FirebaseManager.Instance.WriteFireStore("2 + 3", _data);
    }

    public void SetData(LevelData data) {
        _data = data;
    }
}
