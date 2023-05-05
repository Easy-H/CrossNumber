using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirebaseManager : Singleton<FirebaseManager>
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
