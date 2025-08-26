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
    public void GetStageList(Action<StageMetaData[]> callback,
        Action fallback = null)
    {

        CollectionReference collectionRef = _db.Collection("MapId");

        collectionRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogError("Failed to fetch documents: " + task.Exception);
                fallback?.Invoke();
                return;
            }
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

            StageMetaData[] retval =
                new StageMetaData[randomDocuments.Count];

            for (int i = 0; i < randomDocuments.Count; i++)
            {
                retval[i] = new StageMetaData();
                retval[i].SetValueFromDictionary(
                    randomDocuments[i].ToDictionary());
                retval[i].SetType("Cloud");
            }

            callback?.Invoke(retval);
        });

    }

    // https://firebase.google.com/docs/firestore/query-data/get-data?hl=ko
    public void GetStage(string key, Action<Stage> callback = null, Action fallback = null)
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

    // https://firebase.google.com/docs/firestore/manage-data/transactions?hl=ko
    // https://firebase.google.com/docs/firestore/manage-data/add-data?hl=ko#unity_7
    public void UploadStage(string name, Stage data, Action<string> callback, Action<string> fallback)
    {
        DocumentReference mapUnit = _db.Collection("MapData").Document();
        DocumentReference mapId = _db.Collection("MapId").Document(mapUnit.Id);

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

    public void DeleteStage(string name,
        Action<string> callback, Action<string> fallback)
    {
        DocumentReference mapUnit = _db.Collection("MapData").Document(name);
        DocumentReference mapId = _db.Collection("MapId").Document(name);

        mapUnit.DeleteAsync();
        mapId.DeleteAsync();

        callback?.Invoke(mapUnit.Id);
    }

}

#endif