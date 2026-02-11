using UnityEngine;

namespace EasyH.Unity {
    
    [ExecuteInEditMode]
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour {

        static T _instance;

        public static T Instance {
            get {

                if (_instance == null && !(_instance = FindAnyObjectByType<T>()))
                {
                    GameObject newInstance = new GameObject();
                    newInstance.name = "(MonoSingleton)" + typeof(T).Name;

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