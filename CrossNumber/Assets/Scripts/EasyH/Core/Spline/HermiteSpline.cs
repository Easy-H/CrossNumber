using UnityEngine;

namespace EasyH {
    public class HermiteSpline : ISpline
    {
        public float Get(float p1, float p2,
            float v1, float v2, float t)
        {
            float ret = 0;

            float s = 1 - t;

            ret = s * s * (1 + 2 * t) * p1
                + t * t * (1 + 2 * s) * p2
                + s * s * t * v1
                - s * t * t * v2;

            return ret;

        }
    }
}