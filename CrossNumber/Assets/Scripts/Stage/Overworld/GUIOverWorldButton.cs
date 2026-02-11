using UnityEngine;
using TMPro;
using EasyH.Unity.UI;


public class GUIOverWorldButton : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _btnName = null;
    [SerializeField] private float _mspace = 1;

    protected StageMetaData _stageMetaData;

    protected IGUI _parentGUI;

    public void SetButtonInfor(StageMetaData stageMetaData, IGUI parent)
    {
        _stageMetaData = stageMetaData;
        _btnName.text = string.Format(
            "<mspace=\"{0}\">{1}</mspace>",
            _mspace, stageMetaData.Name.Replace(" ", ""));
        _parentGUI = parent;

    }

    public virtual void BtnClickEvent()
    {
        StageManager.Instance.GetStage(
            _stageMetaData.Type, _stageMetaData.Key,
            (data) =>
            {
                UIManager.Instance.OpenGUI<GUIPlay>("Play").
                    SetStage(data, _stageMetaData.Name);

            },
            () => { });
        
    }

}
