using UnityEngine;

namespace EHTool {
    [ExecuteInEditMode]
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour {

        static T _instance;

        public static T Instance {
            get {
                if (_instance == null && !(_instance = FindObjectOfType<T>()))
                {
                    GameObject newInstance = new GameObject();
                    newInstance.name = "(Singleton)" + typeof(T).Name;

                    if (Application.isPlaying)
                    {
                        DontDestroyOnLoad(newInstance);
                    }

                    _instance = newInstance.AddComponent<T>();
                }

                return _instance;
            }
        }

        private void Awake()
        {
            OnCreate();
        }
        protected virtual void OnCreate()
        {

        }
    }
}