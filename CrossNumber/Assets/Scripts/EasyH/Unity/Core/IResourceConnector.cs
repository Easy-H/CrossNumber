using UnityEngine;
using System;

namespace EasyH.Unity {
    public interface IResourceConnector
    {
        public T Import<T>(string path) where T : UnityEngine.Object;
        public T ImportComponent<T>(string path) where T : Component;
        public GameObject ImportGameObject(string path);

        public void ImportGameObjectAsync(
            string path,
            Action<GameObject> callback,
            Action<float> progress = null);

    }
}