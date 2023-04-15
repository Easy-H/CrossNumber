using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class Singleton<T> where T : new() {
    T _instance;
    public T Instance {
        get {
            if (_instance == null)
                _instance = new T();

            return _instance;
        }
    }
}

public class FirebaseManager : MonoSingleton<FirebaseManager>
{

    public void WriteFireStore()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("users").Document("alovelace");
        Dictionary<string, object> user = new Dictionary<string, object>
{
        { "First", "Ada" },
        { "Last", "Lovelace" },
        { "Born", 1815 },
};
        docRef.SetAsync(user).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the alovelace document in the users collection.");
        });
    }

    public void ReadFireStore() { 
        
    }
}
