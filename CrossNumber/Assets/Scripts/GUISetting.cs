using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EHTool.UIKit;
using EHTool.LangKit;
using UnityEngine.Audio;
using UnityEngine.UI;
using EHTool;

public class GUISetting : GUICustomFullScreen
{
    [System.Serializable]
    struct Option {
        public string name;
        public string value;
    }

    [SerializeField] Option[] _langOpt;
    [SerializeField] Dropdown _langDropdown;
    int _nowLangIdx = 0;


    [SerializeField] Option[] _skinOpt;
    [SerializeField] Dropdown _skinDropdown;
    int _nowSkinIdx = 0;



    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _musicMasterSlider;

    public override void Open()
    {
        base.Open();
        
        _LangDropdownSetting();

        for (int i = 0; i < _langOpt.Length; i++)
        {
            if (LangManager.Instance.NowLang.CompareTo(_langOpt[i].value) == 0)
            {
                _nowLangIdx = i;
                break;
            }
        }

        _langDropdown.value = _nowLangIdx;
        _langDropdown.onValueChanged.AddListener(LangSet);

        _SkinDropdownSetting();

        for (int i = 0; i < _skinOpt.Length; i++)
        {
            if (SkinManager.Instance.NowSkin.CompareTo(_skinOpt[i].value) == 0)
            {
                _nowSkinIdx = i;
                break;
            }
        }

        _skinDropdown.value = _nowSkinIdx;
        _skinDropdown.onValueChanged.AddListener(SkinSet);

        if (!_audioMixer) return;

        _musicMasterSlider.onValueChanged.AddListener(SetMasterVolume);

        _audioMixer.GetFloat("Master", out float volume);
        _musicMasterSlider.value = Mathf.Pow(10, volume / 20);
    }

    public void LangSet(int idx)
    {
        if (_nowLangIdx == idx) return;

        _LangDropdownSetting();

        _nowLangIdx = idx;
        _langDropdown.value = idx;

        Loading(() =>
        {
            LangManager.Instance.ChangeLang(_langOpt[_langDropdown.value].value);
        });


    }

    void _LangDropdownSetting()
    {
        _langDropdown.ClearOptions();

        List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();

        for (int i = 0; i < _langOpt.Length; i++)
        {
            optionData.Add(new Dropdown.OptionData(LangManager.Instance.GetStringByKey(_langOpt[i].name), null));
        }
        _langDropdown.AddOptions(optionData);

    }

    public void SkinSet(int idx)
    {
        if (_nowSkinIdx == idx) return;

        _SkinDropdownSetting();

        _nowSkinIdx = idx;
        _skinDropdown.value = idx;

        Loading(() =>
        {
            SkinManager.Instance.ChangeSkin(_skinOpt[_skinDropdown.value].value);
        });

    }

    void _SkinDropdownSetting()
    {
        _skinDropdown.ClearOptions();

        List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();

        for (int i = 0; i < _skinOpt.Length; i++)
        {
            optionData.Add(new Dropdown.OptionData(LangManager.Instance.GetStringByKey(_skinOpt[i].name), null));
        }
        _skinDropdown.AddOptions(optionData);

    }

    public void SetMasterVolume(float volume)
    {
        if (volume <= _musicMasterSlider.minValue)
        {
            _audioMixer.SetFloat("Master", -80);
            return;

        }

        _audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }


}
