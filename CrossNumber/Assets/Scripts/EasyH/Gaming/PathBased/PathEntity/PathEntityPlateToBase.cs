using System;
using UnityEngine;

namespace EasyH.Gaming.PathBased
{

    public abstract class PathEntityPlateToBase : MonoBehaviour
    {
        public abstract void MoveTo(Vector3 pos, Action callback);
    }
    
}