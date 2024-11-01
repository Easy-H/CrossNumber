using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIUploadCloud : MonoBehaviour
{

    [SerializeField] InputField _name;
    
    public void Upload() {
        StageManager.Instance.GetLocalStageData("Temp", (data) => {
            StageManager.Instance.UploadStage(_name.text, data);
            StageManager.Instance.SaveBuildStage(new StageData());
            UIManager.Instance.NowDisplay.Close();

        });
    }
}
