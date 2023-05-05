using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{

    static T _instance;

    public static T Instance {
        get {
            if (_instance == null)
            {
                if (!(_instance = FindObjectOfType<T>()))
                {
                    GameObject newInstance = new GameObject();
                    newInstance.name = "(Singleton)" + typeof(T).Name;

                    if (Application.isPlaying)
                    {
                        DontDestroyOnLoad(newInstance);
                    }

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
    protected virtual void OnCreate() { 
        
    }
}

[ExecuteInEditMode]
public class Singleton<T> where T : Singleton<T>, new() {
    static T _instance;
    static public T Instance {
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