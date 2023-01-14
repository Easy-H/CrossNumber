using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour {

    static T _instance;

    public static T Instance {
        get {
            if (_instance == null)
            {
                if (!(_instance = FindObjectOfType<T>()))
                {
                    GameObject newInstance = new GameObject();
                    newInstance.name = "(Singleton)" + typeof(T).Name;
                    DontDestroyOnLoad(newInstance);

                    _instance = newInstance.AddComponent<T>();
                }
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
