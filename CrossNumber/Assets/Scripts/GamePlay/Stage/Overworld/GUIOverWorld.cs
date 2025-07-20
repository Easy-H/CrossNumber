using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GUIOverWorld : GUICustomFullScreen {

    [SerializeField] private TextMeshProUGUI _overworldName;
    [SerializeField] private List<GUIOverWorldButton> _buttons = null;
    [SerializeField] private Transform _buttonContainer = null;

    private IOverworld _overworld;

    public void SetOverworld(IOverworld overworld) {
        _overworldName.text = overworld.OverworldName;
        _overworld = overworld;
        _overworld.GetStageList(_Success, null);
    }

    private void _Success(StageMetaData[] data)
    {

        Loading(() => {
            
            _buttonContainer.position = Vector3.zero;

            int i = 0;

            foreach (StageMetaData d in data)
            {
                if (i >= _buttons.Count) {
                    _buttons.Add(Instantiate(_buttons[0], _buttonContainer));
                }

                _buttons[i].gameObject.SetActive(true);
                _buttons[i++].SetButtonInfor(d.name, d.key, _overworld);

            }

            for (; i < _buttons.Count; i++)
            {
                _buttons[i].gameObject.SetActive(false);

            }

        });

    }
}
