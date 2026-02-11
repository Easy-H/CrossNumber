using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace EasyH.Unity.SoundKit
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        private IDictionary<string, string> _dict;
        private IDictionary<string, AudioClip> _audioDict;

        private AudioMixer _mixer;
        private AudioSource[] _audio;
        private int _idx;

        protected override void OnCreate()
        {
            IDictionaryConnector<string, string> reader =
                new JsonDictionaryConnector<string, string>();

            _dict = reader.ReadData("SoundInfor");
            _audioDict = new Dictionary<string, AudioClip>();

            _audio = new AudioSource[5];
            _mixer = Resources.Load("AudioMixer") as AudioMixer;

            for (int i = 0; i < _audio.Length; i++)
            {
                _audio[i] = gameObject.AddComponent<AudioSource>();
            }
            _idx = 0;
        }

        public AudioClip GetSound(string key)
        {

            if (!_audioDict.ContainsKey(key))
            {
                if (!_dict.ContainsKey(key)) return null;

                _audioDict.Add(key, ResourceManager.Instance.
                    ResourceConnector.Import<AudioClip>(_dict[key]));
            }

            return _audioDict[key];
        }

        public void PlaySound(string v, string group = "Master")
        {
            _audio[_idx].clip = GetSound(v);
            _audio[_idx].Play();
            _audio[_idx].outputAudioMixerGroup = _mixer.FindMatchingGroups(group)[0];
            _idx = (_idx + 1) % _audio.Length;
        }

        public void PlayBGM(string v)
        {
            
        }

        public void PlaySFX(string v)
        {

        }
    }
}