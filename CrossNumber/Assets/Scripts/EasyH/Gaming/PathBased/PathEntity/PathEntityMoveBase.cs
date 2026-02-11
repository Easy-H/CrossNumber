using UnityEngine;
using System;

namespace EasyH.Gaming.PathBased
{


    public abstract class PathEntityMoveBase : MonoBehaviour
    {

        public abstract void MoveTo(Plate startPos, PathEntity target, int amount, Action arrive, Action<Plate> moveEnd);

    }
}