using System.Collections;
using TMPro;
using UnityEngine;


namespace EasyH.Unity.Effect
{
    public class TextPopEffect : EffectBase
    {

        [SerializeField] TextMeshProUGUI _view;
        TextMeshProUGUI[] _targetText;

        [SerializeField] string _text;

        [SerializeField] float _maxSize = 100;
        [SerializeField] float _bojung = 10;
        [SerializeField] float _maintainTime = .2f;

        private void OnEnable()
        {
            SetText(_text);
            On(transform.position);
        }

        public override void On(Vector3 pos)
        {
            GetComponent<RectTransform>().position = pos;
            for (int i = 0; i < _targetText.Length; i++)
            {
                _targetText[i].alpha = 0;
            }
            StartCoroutine(_On());
        }

        public void SetText(string text)
        {
            _text = text;
            char[] chars = text.ToCharArray();
            float size = _maxSize * _bojung;

            if (_targetText != null)
            {
                for (int i = 1; i < _targetText.Length; i++)
                {
                    Destroy(_targetText[i].gameObject);
                }
            }

            _targetText = new TextMeshProUGUI[chars.Length];

            for (int i = 1; i < chars.Length; i++)
            {
                _targetText[i] = Instantiate(_view, transform);
                _SetText(i, chars[i].ToString(), new Vector2((i - chars.Length * .5f + 0.5f) * size, 0));

            }

            _targetText[0] = _view;
            _SetText(0, chars[0].ToString(), new Vector2((-chars.Length * .5f + 0.5f) * size, 0));
        }

        private void _SetText(int idx, string value, Vector2 pos)
        {
            _targetText[idx].text = value;

            _targetText[idx].GetComponent<RectTransform>().anchoredPosition = pos;

        }

        IEnumerator _On()
        {
            for (int i = 0; i < _targetText.Length; i++)
            {
                _targetText[i].fontSize = 0;
                _targetText[i].alpha = 1;

                float spendTime = 0;

                while (_effectTime > spendTime)
                {
                    spendTime += Time.deltaTime;
                    yield return null;

                    _targetText[i].fontSize = Mathf.Lerp(0, _maxSize, spendTime / _effectTime);

                }

            }
            yield return new WaitForSeconds(_maintainTime);

            for (int i = 0; i < _targetText.Length; i++)
            {
                float spendTime = 0;
                while (_effectTime > spendTime)
                {
                    spendTime += Time.deltaTime;
                    yield return null;

                    _targetText[i].alpha = 1 - spendTime / _effectTime;

                }

            }

            EndEffect();
        }

    }
}