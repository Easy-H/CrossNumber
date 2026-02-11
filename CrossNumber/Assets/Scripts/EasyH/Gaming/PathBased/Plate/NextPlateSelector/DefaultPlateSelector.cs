using UnityEngine;

namespace EasyH.Gaming.PathBased
{

    public class DefaultPlateSelector : NextPlateSelectorBase
    {

        [SerializeField] Plate _nextPlate;

        public override Plate NextPlate(Plate from)
        {
            return _nextPlate;
        }

        public override int GetValue()
        {
            return 0;
        }

    }
}