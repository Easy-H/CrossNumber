using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioData {
    public AudioSource audio;
    public string name;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource sceneStartSound = null;
    [SerializeField] AudioData[] audios = null;

    public static SoundManager instance;

    private void Start()
    {
        instance = this;
    }

    public void PlayAudio(string audioName, bool canPlayOther) {
        if (sceneStartSound.isPlaying)
            return;

        for (int i = 0; i < audios.Length; i++) {

            if (audios[i].name.Equals(audioName))
            {
                if (canPlayOther || !audios[i].audio.isPlaying)
                    audios[i].audio.Play();
            }
            else if (!canPlayOther) {
                audios[i].audio.Stop();
            }

        }
    }

}
