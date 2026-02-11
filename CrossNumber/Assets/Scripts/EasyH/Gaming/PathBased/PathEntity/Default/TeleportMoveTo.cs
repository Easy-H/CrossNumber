using System.Collections;
using UnityEngine;
using System;

namespace EasyH.Gaming.PathBased
{
    public class TeleportMoveTo : PathEntityPlateToBase
    {

        [SerializeField] private float _stall = 0.1f;
        [SerializeField] private float _delay = 0.1f;

        public override void MoveTo(Vector3 pos, Action callback)
        {
            StartCoroutine(_MoveTo(pos, callback));
        }

        IEnumerator _MoveTo(Vector3 pos, Action callback)
        {
            yield return new WaitForSeconds(_stall);

            gameObject.transform.localScale = Vector3.zero;

            yield return new WaitForSeconds(_delay);

            gameObject.transform.position = pos;
            gameObject.transform.localScale = Vector3.one;

            yield return new WaitForSeconds(_stall);

            callback?.Invoke();
        }
    }
}