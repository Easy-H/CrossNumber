#if !UNITY_WEBGL || UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirestoreStageConnector : IStageConnector {

    FirebaseFirestore db;

    public FirestoreStageConnector() {
        db = FirebaseFirestore.DefaultInstance;
    }

    // https://firebase.google.com/docs/firestore/manage-data/transactions?hl=ko
    // https://firebase.google.com/docs/firestore/manage-data/add-data?hl=ko#unity_7
    public void UploadStage(string name, StageData data)
    {
        WriteBatch batch = db.StartBatch();

        DocumentReference mapUnit = db.Collection("MapData").Document();
        DocumentReference mapId = db.Collection("MapId").Document();

        batch.Set(mapId, new Dictionary<string, object>
        {
            { "key", mapUnit.Id },
            { "name", name }
        });
        batch.Set(mapUnit, data.ToDictionary());


        batch.CommitAsync();
    }

    CallbackMethod<Dictionary<string, object>> stageCallback;
    CallbackMethod _stageFallback;


    public void GetRandomStage(CallbackMethod<Dictionary<string, object>> callback, CallbackMethod fallback = null)
    {

        stageCallback = callback;
        _stageFallback = fallback;

        CollectionReference collectionRef = db.Collection("MapId"); // 여기에 컬렉션 이름을 넣어주세요

        // 컬렉션의 모든 문서 가져오기
        collectionRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                QuerySnapshot snapshot = task.Result;

                // 가져온 문서 목록
                List<DocumentSnapshot> documents = new List<DocumentSnapshot>();

                foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
                {
                    documents.Add(documentSnapshot);
                }

                // 무작위로 X개의 문서 선택
                int documentsToFetch = Mathf.Min(10, documents.Count);
                List<DocumentSnapshot> randomDocuments = new List<DocumentSnapshot>();

                for (int i = 0; i < documentsToFetch; i++)
                {
                    int randomIndex = Random.Range(0, documents.Count);
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
    public void GetLevelData(string key, CallbackMethod<StageData> callback = null, CallbackMethod fallback = null)
    {

        DocumentReference docRef = db.Collection("MapData").Document(key);

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                callback?.Invoke(new StageData(snapshot.ToDictionary()));
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