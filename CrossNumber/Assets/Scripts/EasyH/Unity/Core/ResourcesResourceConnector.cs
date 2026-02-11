using UnityEngine;
using System;

namespace EasyH.Unity
{
    public class ResourcesResourceConnector
        : MonoBehaviour, IResourceConnector
    {

        public T Import<T>(string path) where T : UnityEngine.Object
        {
            T source = Resources.Load(path) as T;

            return source;
        }

        public T ImportComponent<T>(string path) where T : Component
        {
            return ImportGameObject(path).GetComponent<T>();
        }

        public GameObject ImportGameObject(string path)
        {
            GameObject source = Resources.Load(path) as GameObject;
            return Instantiate(source);
        }

        public void ImportGameObjectAsync(string path, Action<GameObject> callback, Action<float> progress = null)
        {
            ResourceRequest async = Resources.LoadAsync(path);

            async.completed += (value) =>
            {
                GameObject ret = Instantiate(
                    async.asset as GameObject);

                callback?.Invoke(ret);
            };

        }

    }
}