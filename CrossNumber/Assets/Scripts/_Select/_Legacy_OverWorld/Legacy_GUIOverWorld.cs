using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using EHTool.LangKit;

public class Legacy_GUIOverWorld : GUICustomFullScreen {

    [SerializeField] Legacy_GUIOverWorldButton _button = null;

    [SerializeField] Transform _buttonContainer = null;
    [SerializeField] EHText _worldNameText = null;

    // Start is called before the first frame update
    public override void SetOn()
    {
        SetGUI();
    }
    public void SetGUI()
    {
        _buttonContainer.position = Vector3.zero;

        for (int i = 0; i < _buttonContainer.childCount; i++)
        {
            Destroy(_buttonContainer.GetChild(i).gameObject);
        }

        int length = StageManager.Instance.GetStageCount();

        _worldNameText.SetText(StageManager.Instance.GetWorldName());

        for (int i = 0; i < length; i++)
        {
            Legacy_GUIOverWorldButton newButton = Instantiate(_button, Vector3.down * i, Quaternion.identity);
            StageMetaData data = StageManager.Instance.GetStageMetaData(i);

            newButton.transform.SetParent(_buttonContainer);
            newButton.SetButtonInfor(data.name, data.value);
        }

        _buttonContainer.position = Vector3.up * (length / 2);
    }

    public void GoBeforeWorld() {
        if (StageManager.WorldIdx == 0) {
            Close();
            return;
        }

        StageManager.WorldIdx--;
        SetGUI();
    }

    public void GoNextOverWorld()
    {
        if (StageManager.WorldIdx + 1 == StageManager.WorldCount)
        {
            return;
        }

        StageManager.WorldIdx++;
        SetGUI();
    }
}
