using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace EasyH.Gaming.PathBased
{

    public class CoroutinePawnMove : PathEntityMoveBase
    {

        [SerializeField] private UnityEvent _moveEvent;
        [SerializeField] private float _startStall;
        [SerializeField] private float _moveStall;
        [SerializeField] private float _moveTime = 0.5f;

        //[SerializeField] private ParticleSystem _moveEffect;

        private Action _arriveCallback;
        private Action<Plate> _moveEndCallback;

        private Plate _beforePlate;
        private Plate _nowPlate;
        private int _movePoint;

        public override void MoveTo(Plate startPos, PathEntity target, int amount, Action arrive, Action<Plate> moveEnd)
        {
            _movePoint = amount;

            _arriveCallback = arrive;
            _moveEndCallback = moveEnd;

            //startPos.NextPlate(null, MoveTo);
            startPos.Leave(target, MoveTo);
        }

        private void MoveTo(Plate plate)
        {
            if (plate == null)
            {
                _arriveCallback?.Invoke();
                return;
            }

            _movePoint--;

            StartCoroutine(_MoveTo(plate.transform.position, _moveTime, _moveStall, () =>
            {
                _beforePlate = _nowPlate;
                _nowPlate = plate;

                if (_movePoint < 1)
                {
                    _moveEndCallback?.Invoke(plate);
                    return;
                }

                plate.NextPlate(_beforePlate, MoveTo);

            }));
        }

        IEnumerator _MoveTo(Vector3 goalPos, float moveTime, float stopTime, Action callback)
        {
            float spendTime = 0;
            Vector3 originPos = transform.position;

            while (spendTime < moveTime)
            {
                yield return null;
                spendTime += Time.deltaTime;
                transform.position = Vector3.Lerp(originPos, goalPos, spendTime / moveTime);
            }

            //_moveEffect.Play();
            _moveEvent.Invoke();
            //SFXManager.Instance.PlaySFX("FootStep");
            yield return new WaitForSeconds(stopTime);

            transform.position = goalPos;
            callback?.Invoke();
        }
    }
}