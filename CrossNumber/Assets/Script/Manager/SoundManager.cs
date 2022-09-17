using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioData {
    public AudioSource audio;
    public string name;
}

public class SoundManager : MonoBehaviour {
    [SerializeField] AudioSource _sceneStartSound = null;
    [SerializeField] AudioData[] _audios = null;

    public static SoundManager instance { get; private set; }

    private void Start() {
        instance = this;
    }

    public void PlayAudio(string audioName, bool canPlayOther) {
        if (_sceneStartSound.isPlaying)
            return;

        for (int i = 0; i < _audios.Length; i++) {

            if (_audios[i].name.Equals(audioName)) {
                _audios[i].audio.Stop();
                _audios[i].audio.Play();
            }
            else if (!canPlayOther) {
                _audios[i].audio.Stop();
            }

        }
    }

}
