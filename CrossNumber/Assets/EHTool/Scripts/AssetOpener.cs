using UnityEngine;
using System;

namespace EHTool {
    public class AssetOpener : MonoBehaviour {

        public static T Import<T>(string path) where T : UnityEngine.Object
        {
            T source = Resources.Load(path) as T;

            return source;
        }

        public static T ImportComponent<T>(string path) where T : Component
        {
            return ImportGameObject(path).GetComponent<T>();

        }

        public static GameObject ImportGameObject(string path)
        {
            GameObject source = Resources.Load(path) as GameObject;
            return Instantiate(source);
        }

        public static ResourceRequest ImportGameObjectAsync(string path, Action<GameObject> callback, Action<float> progress = null) {
            ResourceRequest async = Resources.LoadAsync(path);

            async.completed += (value) => {
                callback?.Invoke(async.asset as GameObject);
            };

            return async;

        }

        public static string ReadTextAsset(string path) { 
            TextAsset retval = (TextAsset)Resources.Load(path, typeof(TextAsset));

            return retval.text;
        }

    }

}