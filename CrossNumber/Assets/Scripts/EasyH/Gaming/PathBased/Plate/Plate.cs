using System;
using UnityEngine;

namespace EasyH.Gaming.PathBased
{

    public class Plate : MonoBehaviour
    {

        [SerializeField] private NextPlateSelectorBase _nextPlateSelector;

        public void Leave(PathEntity target, Action<Plate> callback)
        {
            callback?.Invoke(_nextPlateSelector.NextPlate(null));
        }

        public void NextPlate(Plate from, Action<Plate> callback)
        {
            callback?.Invoke(_nextPlateSelector.NextPlate(from));
        }

        public int GetValue()
        {
            return _nextPlateSelector.GetValue();
        }

    }

}