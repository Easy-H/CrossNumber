using UnityEngine;

namespace EasyH.Gaming.PathBased
{
    public class PathMap : MonoBehaviour
    {

        [SerializeField] private Plate _startPlate;

        public Plate GetStartPlate()
        {
            return _startPlate;
        }

    }
    
}