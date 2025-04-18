using EHTool;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager> {

    private IDictionary<string, string> _dic;

    private AudioSource _audio;

    protected override void OnCreate()
    {
        base.OnCreate();
        IDictionaryConnector<string, string> dictionaryConnector
            = new XMLDictionaryConnector<string, string>
            ("name", "path", "SoundData", "Audio");
            
        _dic = dictionaryConnector.ReadData("SoundInfor");

        _audio = gameObject.AddComponent<AudioSource>();
    }

    public void PlayAudio(string audioName, bool canPlayOther = false) {

        if (_dic.TryGetValue(audioName, out string value))
        {
            _audio.clip = AssetOpener.Import<AudioClip>(value);

        }
        else
        {
            return;
            //_audio.clip = Resources.Load(_dic["Error"]) as AudioClip;
        }

        _audio.Play();
    }

}
