mergeInto(LibraryManager.library, {
    UploadMap: function(name, value, objectName, callback, fallback) {
        var parsedName = UTF8ToString(name);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);
        
        var batch = firebase.firestore().batch();

        // Set the value of 'NYC'
        var mapUnit = firebase.firestore().collection("MapData").doc();
        var mapId = firebase.firestore().collection("MapId").doc();

        batch.set(mapId, {
          "key": mapUnit.id,
          "name": parsedName
        });
        batch.set(mapUnit, JSON.parse(parsedValue));

        batch.commit();

        console.log(parsedValue);

    },
    GetLevelData: function(path, objectName, callback) {
        
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);

        var docRef = firebase.firestore().collection("MapData").doc(parsedPath);

        docRef.get().then((doc) => {
            if (doc.exists) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(doc.data()));
            
            } else {
            }
        }).catch((error) => {
        });


    },
    GetJSON: function(objectName, callback, fallback) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        firebase.firestore().collection("MapId").get().then((snapshot) => {
            var documents = [];
            snapshot.forEach((documentSnapshot) => {
                documents.push(documentSnapshot);
            });
            var documentsToFetch = Math.min(10, documents.length);
            var randomDocuments = [];
            for (let i = 0; i < documentsToFetch; i++) {
                var randomIndex = Math.floor(Math.random() * documents.length);
                randomDocuments.push(documents[randomIndex]);
                documents.splice(randomIndex, 1);
            }
            var retval = {};
            randomDocuments.forEach((ds) => {
                retval[ds.id] = ds.data();
            });
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(retval));
        });
    }
 });