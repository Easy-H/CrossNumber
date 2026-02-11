using UnityEngine;

namespace EasyH.Unity
{

    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        public IResourceConnector ResourceConnector
            { get; private set; }

        protected override void OnCreate()
        {
            ResourceConnector = gameObject.
                AddComponent<ResourcesResourceConnector>();
        }
    }

}