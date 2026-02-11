using System;
using UnityEngine;

namespace EasyH.Gaming.PathBased
{

    public class PathEntity : MonoBehaviour
    {

        [SerializeField] private PathEntityMoveBase _moveOnMap;
        [SerializeField] private PathEntityPlateToBase _initialMoveOnMap;
        [SerializeField] protected Plate _nowPlate;

        public virtual void Move(int amount)
        {

            if (_nowPlate != null)
            {
                PawnMove(amount);
                return;
            }

            GoStartPlate(() =>
            {
                PawnMove(amount);
            });

        }

        protected virtual void GoStartPlate(Action callback)
        { 
            _nowPlate = PathManager.
                Instance.Map.GetStartPlate();
            _initialMoveOnMap.MoveTo(
                _nowPlate.transform.position, callback);
        }

        private void PawnMove(int amount)
        {
            _moveOnMap.MoveTo(_nowPlate, this, amount, ArriveGoal, (value) =>
            {
                ArrivePlate(value);
            });
        }

        public virtual void ArrivePlate(Plate value)
        {
            _nowPlate = value;
        }

        public virtual void ArriveGoal()
        {

        }   

        public void Dispose(Vector3 pos)
        {
            _initialMoveOnMap.MoveTo(pos, null);
        }

    }
}