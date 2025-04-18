using UnityEngine;

public class GUIOverWorld : GUICustomFullScreen {

    [SerializeField] private GUIOverWorldButton[] _buttons = null;
    [SerializeField] private Transform _buttonContainer = null;

    private IOverworld _overworld;

    public override void Open()
    {
        base.Open();
    }

    public void SetOverworld(IOverworld overworld) {
        _overworld = overworld;
        _overworld.GetStageList(Success, null);
    }

    void Success(StageMetaData[] data)
    {

        CaptureAndEvent(() => {

            _buttonContainer.position = Vector3.zero;

            int i = 0;

            foreach (StageMetaData d in data)
            {
                if (i >= _buttons.Length) break;

                _buttons[i].gameObject.SetActive(true);
                _buttons[i++].SetButtonInfor(d.name, d.key, _overworld);

            }

            for (; i < _buttons.Length; i++)
            {
                _buttons[i].gameObject.SetActive(false);

            }

        });

    }
}
