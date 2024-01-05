using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Carousel : MonoBehaviour {


    [SerializeField] Scrollbar _scrollbar;

    float _scroll_pos = 0;
    float[] _pos;
    float _distance;

    // Start is called before the first frame update
    void Start() {

        _pos = new float[transform.childCount];
        _distance = 1f / (_pos.Length - 1);

        for (int i = 0; i < _pos.Length; i++)
        {
            _pos[i] = _distance * i;
        }
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetMouseButton(0)) {
            _scroll_pos = _scrollbar.value;
        }
        else {
            for (int i = 0; i < _pos.Length; i++) {
                if (_scroll_pos >= _pos[i] + (_distance / 2) || _scroll_pos <= _pos[i] - (_distance / 2)) continue;
                _scrollbar.value = Mathf.Lerp(_scrollbar.value, _pos[i], 0.1f);
            }
        }

        for (int i = 0; i < _pos.Length; i++) {
            if (_scroll_pos >= _pos[i] + (_distance / 2) || _scroll_pos <= _pos[i] - (_distance / 2)) continue;

            transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
            for (int j = 0; j < _pos.Length; j++)
            {
                if (j == i) continue;

                transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
            }
        }
    }
}