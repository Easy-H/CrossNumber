using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Xml;



#if !UNITY_WEBGL || UNITY_EDITOR
using Firebase.Firestore;
using Firebase.Extensions;
#endif

public interface ICallback<T> {
    public void Success(T data);
    public void Fail();
}

public class FirebaseManager : MonoSingleton<FirebaseManager> {

#if !UNITY_WEBGL || UNITY_EDITOR
    FirebaseFirestore db;
#endif
    protected override void OnCreate()
    {
        base.OnCreate();
#if !UNITY_WEBGL || UNITY_EDITOR
        db = FirebaseFirestore.DefaultInstance;
#endif
    }

    // https://firebase.google.com/docs/firestore/manage-data/transactions?hl=ko
    // https://firebase.google.com/docs/firestore/manage-data/add-data?hl=ko#unity_7
    public void WriteFireStore(string name, LevelData data)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        FirebaseWebGL.Scripts.FirebaseBridge.FirebaseDatabase.UploadMap(name, JsonConvert.SerializeObject(data.ToDictionary()),  gameObject.name, "GetRandomStageJS", "GetRandomStageJS");
#else
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
#endif
    }

    ICallback<Dictionary<string, object>> stageCallback;

    void GetRandomStageJS(string data) {


#if UNITY_WEBGL && !UNITY_EDITOR
        FirebaseWebGL.Scripts.FirebaseBridge.FirebaseDatabase.WebAlert("Success");
#endif

        Dictionary<object, Dictionary<string, object>> snapshot =
            JsonConvert.DeserializeObject<Dictionary<object, Dictionary<string, object>>>(data);

        // 무작위로 X개의 문서 선택
        int documentsToFetch = Mathf.Min(10, snapshot.Count);
        List<Dictionary<string, object>> documents = new List<Dictionary<string, object>>();

        foreach (Dictionary<string, object> documentSnapshot in snapshot.Values)
        {
            documents.Add(documentSnapshot);
        }

        Dictionary<string, object> retval = new Dictionary<string, object>();

        for (int i = 0; i < documentsToFetch; i++)
        {
            int randomIndex = Random.Range(0, documents.Count);
            retval.Add(i.ToString(), documents[randomIndex]);
            documents.RemoveAt(randomIndex);
        }

        stageCallback.Success(retval);
        stageCallback = null;
    }

    public void GetRandomStage(ICallback<Dictionary<string, object>> callback)
    {

        stageCallback = callback;
#if UNITY_WEBGL && !UNITY_EDITOR
        FirebaseWebGL.Scripts.FirebaseBridge.FirebaseDatabase.GetJSON(gameObject.name, "GetRandomStageJS", "GetRandomStageJS");
#else

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

                foreach (DocumentSnapshot ds in randomDocuments) {
                    retval.Add(ds.Id, ds.ToDictionary());
                }

                callback.Success(retval);
                stageCallback = null;
            }
            else
            {
                Debug.LogError("Failed to fetch documents: " + task.Exception);
                stageCallback = null;
            }
        });
#endif

    }

    ICallback<LevelData> dataCallback;

    void GetLevelDataJS(string data)
    {
        Dictionary<string, Dictionary<string, object>> parsedData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(data);

        Dictionary<string, object> parameter = new Dictionary<string, object>();

        int i = 0;
        foreach (object d in parsedData.Values) {
            parameter.Add((i++).ToString(), d);
        }

        dataCallback.Success(new LevelData(parameter));
        dataCallback = null;
    }

    // https://firebase.google.com/docs/firestore/query-data/get-data?hl=ko
    public void GetLevelData(string key, ICallback<LevelData> callback)
    {
        if (dataCallback != null) return;
        dataCallback = callback;
#if UNITY_WEBGL && !UNITY_EDITOR
        FirebaseWebGL.Scripts.FirebaseBridge.FirebaseDatabase.GetLevelData(key, gameObject.name, "GetLevelDataJS");
        return;
#else

        DocumentReference docRef = db.Collection("MapData").Document(key);

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                callback.Success(new LevelData(snapshot.ToDictionary()));
                dataCallback = null;
            }
            else
            {
                Debug.Log("Fail");
                callback.Fail();
                dataCallback = null;
            }
        });
#endif

    }
}
