using UnityEngine;

namespace EHTool {

    [ExecuteInEditMode]
    public class Singleton<T> where T : Singleton<T>, new() {
        static T _instance;
        public static T Instance {
            get {
                if (_instance == null)
                {
                    _instance = new T();
                    _instance.OnCreate();
                }

                return _instance;
            }
        }
        protected virtual void OnCreate()
        {

        }
    }
}