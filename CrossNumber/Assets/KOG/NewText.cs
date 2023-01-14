using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewText : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] string _key;
    Text _text;

    void OnEnable()
    {
        _text = GetComponent<Text>();

        if (_text == null)
            _text = gameObject.AddComponent<Text>();

        StringManager.Instance.texts.Add(this);
        SetText();
    }

    public void SetText()
    {
        _text.text = StringManager.Instance.GetStringByKey(_key);

    }
}
