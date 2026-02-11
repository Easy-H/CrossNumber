using System.Collections;
using UnityEngine;

namespace EasyH.Unity.Effect
{
    public class PopEffect : EffectBase
    {
        [SerializeField] SpriteRenderer _targetImage;

        [SerializeField] Vector3 _minSize;
        [SerializeField] Vector3 _maxSize;

        public override void On(Vector3 pos)
        {
            StopAllCoroutines();
            transform.position = pos;
            gameObject.SetActive(true);
            _targetImage.color = new Color(_targetImage.color.r, _targetImage.color.g, _targetImage.color.b, 1);
            _targetImage.transform.localScale = _minSize;
            StartCoroutine(_On());
        }

        IEnumerator _On()
        {
            float spendTime = 0;
            while (_effectTime > spendTime)
            {
                spendTime += Time.deltaTime;
                yield return null;

                _targetImage.transform.localScale = Vector3.Lerp(_minSize, _maxSize, spendTime / _effectTime);
                _targetImage.color = new Color(_targetImage.color.r, _targetImage.color.g, _targetImage.color.b, 1 - spendTime / _effectTime);

            }
            EndEffect();

        }

    }
}