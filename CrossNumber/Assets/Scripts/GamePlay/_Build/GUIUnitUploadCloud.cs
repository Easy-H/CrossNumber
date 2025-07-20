using EHTool.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class GUIUnitUploadCloud : MonoBehaviour
{
    [SerializeField] private InputField _name;
    
    public void Upload() {
        StageManager.Instance.GetLocalStageData("Temp", (data) => {
            
            StageManager.Instance.UploadStage(_name.text, data);
            StageManager.Instance.SaveBuildStage("Temp", new Stage());
            UIManager.Instance.NowDisplay.Close();

        });
    }
}
