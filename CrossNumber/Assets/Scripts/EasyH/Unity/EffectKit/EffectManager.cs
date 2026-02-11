using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyH;


namespace EasyH.Unity.Gaming.Effect
{
    public class EffectManager : MonoSingleton<EffectManager>
    {

        [SerializeField] private List<GameObject> ImpactVfx;
        private float impactVfxLifetime = 3f;
        private GameObject target;

        public void SetTarget(GameObject target)
        {
            this.target = target;
        }

        public void ShowImpactVfxHandler(int impactCode)
        {
            StartCoroutine(ShowImpactVfx(impactCode));
        }

        public IEnumerator ShowImpactVfx(int impactCode)
        {
            yield return new WaitForSeconds(0.2f);
            if (ImpactVfx.Count > 0)
            {
                GameObject impactVfxInstance = Instantiate(ImpactVfx[impactCode], target.transform.position + target.transform.forward,
                    Quaternion.Euler(target.transform.localEulerAngles));
                if (impactVfxLifetime > 0)
                {
                    Destroy(impactVfxInstance, impactVfxLifetime);
                }
            }
        }
    }

}