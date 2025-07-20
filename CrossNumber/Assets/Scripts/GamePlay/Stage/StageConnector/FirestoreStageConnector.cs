#if !UNITY_WEBGL || UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirestoreStageConnector : IStageConnector
{

    private FirebaseFirestore _db;

    public FirestoreStageConnector()
    {
        _db = FirebaseFirestore.DefaultInstance;
    }

    // https://firebase.google.com/docs/firestore/manage-data/transactions?hl=ko
    // https://firebase.google.com/docs/firestore/manage-data/add-data?hl=ko#unity_7
    public void UploadStage(string name, Stage data, Action<string> callback)
    {
        DocumentReference mapUnit = _db.Collection("MapData").Document();
        DocumentReference mapId = _db.Collection("MapId").Document();

        WriteBatch batch = _db.StartBatch();

        batch.Set(mapId, new Dictionary<string, object>
            {
                { "key", mapUnit.Id },
                { "name", name }
            });

        batch.Set(mapUnit, data.ToDictionary());
        batch.CommitAsync();

        callback?.Invoke(mapUnit.Id);

    }

    Action<Dictionary<string, object>> stageCallback;
    Action _stageFallback;

    public void GetRandomStage(Action<Dictionary<string, object>> callback, Action fallback = null)
    {

        stageCallback = callback;
        _stageFallback = fallback;

        CollectionReference collectionRef = _db.Collection("MapId");

        collectionRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                QuerySnapshot snapshot = task.Result;

                List<DocumentSnapshot> documents = new List<DocumentSnapshot>();

                foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
                {
                    documents.Add(documentSnapshot);
                }

                int documentsToFetch = Mathf.Min(10, documents.Count);
                List<DocumentSnapshot> randomDocuments = new List<DocumentSnapshot>();

                for (int i = 0; i < documentsToFetch; i++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, documents.Count);
                    randomDocuments.Add(documents[randomIndex]);
                    documents.RemoveAt(randomIndex);
                }

                Dictionary<string, object> retval = new Dictionary<string, object>();

                foreach (DocumentSnapshot ds in randomDocuments)
                {
                    retval.Add(ds.Id, ds.ToDictionary());
                }

                callback?.Invoke(retval);
                stageCallback = null;
            }
            else
            {
                Debug.LogError("Failed to fetch documents: " + task.Exception);
                stageCallback = null;
            }
        });

    }

    // https://firebase.google.com/docs/firestore/query-data/get-data?hl=ko
    public void GetLevelData(string key, Action<Stage> callback = null, Action fallback = null)
    {

        DocumentReference docRef = _db.Collection("MapData").Document(key);

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;

            if (snapshot.Exists)
            {
                IDictionary<string, IDictionary<string, object>> ret
                    = new Dictionary<string, IDictionary<string, object>>();

                foreach (var d in snapshot.ToDictionary())
                {
                    ret.Add(d.Key, d.Value as Dictionary<string, object>);
                }

                callback?.Invoke(new Stage(ret));
            }
            else
            {
                Debug.Log("Fail");
                fallback?.Invoke();
            }
        });

    }
}

#endif