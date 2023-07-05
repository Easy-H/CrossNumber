using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class AudioData {
    public AudioSource audio;
    public string name;
}

public class SoundManager : MonoSingleton<SoundManager> {

    class SoundData {
        public string name;
        public string path;

        public void Read(XmlNode node)
        {
            name = node.Attributes["name"].Value;
            path = node.Attributes["path"].Value;
        }
    }

    Dictionary<string, string> _dic;

    AudioSource _audio;

    protected override void OnCreate()
    {
        base.OnCreate();

        _dic = new Dictionary<string, string>();
        XmlDocument xmlDoc = AssetOpener.ReadXML("SoundInfor");

        XmlNodeList nodes = xmlDoc.SelectNodes("SoundData/Audio");

        for (int i = 0; i < nodes.Count; i++)
        {
            SoundData soundData = new SoundData();
            soundData.Read(nodes[i]);

            _dic.Add(soundData.name, soundData.path);
        }

        _audio = gameObject.AddComponent<AudioSource>();
    }

    public void PlayAudio(string audioName, bool canPlayOther = false) {

        if (_dic.TryGetValue(audioName, out string value))
        {
            _audio.clip = Resources.Load(value) as AudioClip;

        }
        else if (audioName.Equals("NoSound"))
        {
            return;
        }
        else
        {
            _audio.clip = Resources.Load(_dic["Error"]) as AudioClip;
        }

        _audio.Play();
    }

}
