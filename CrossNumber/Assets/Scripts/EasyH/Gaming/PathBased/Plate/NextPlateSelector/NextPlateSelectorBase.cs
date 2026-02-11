using UnityEngine;

namespace EasyH.Gaming.PathBased
{
    public abstract class NextPlateSelectorBase : MonoBehaviour
    {
        public abstract Plate NextPlate(Plate from);
        public abstract int GetValue();

    }
}